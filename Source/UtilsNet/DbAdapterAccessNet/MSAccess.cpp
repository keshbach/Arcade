/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2007-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "LoggedInUser.h"
#include "MSAccess.h"

#define CAccessProvider_ACCDB L"Provider=Microsoft.ACE.OLEDB.12.0;"
#define CAccessProvider_MDB L"Provider=Microsoft.Jet.OLEDB.4.0;"

#define CDataSource L"Data Source="

#define CEnglishLCID MAKELCID(MAKELANGID(LANG_ENGLISH, SUBLANG_ENGLISH_US), SORT_DEFAULT)

#define CMdbFileExt L".mdb"
#define CAccdbFileExt L".accdb"

#define CLoggedInUsersGuid L"{947bb102-5d43-11d1-bdbf-00c04fb92675}"

typedef struct tagTLoggedInUserData
{
    LPWSTR pszComputerName;
	LPWSTR pszUserName;
    BOOL bConnected;
	BOOL bSuspectState;
} TLoggedInUserData;

typedef struct tagTLoggedInUsersData
{
    LPCWSTR pszDatabaseFile;
	ULONG ulTotalLoggedInUserData;
	TLoggedInUserData* pLoggedInUserData;
} TLoggedInUsersData;

enum EColumnType
{
    ectUnknown,
    ectCharacter,
    ectInteger,
    ectDateTime,
    ectBoolean
};

typedef struct tagTTableColumnData
{
    LPWSTR pszColumnName;
    ULONG ulColumnLength;
    EColumnType ColumnType;
} TTableColumnData;

typedef struct tagTTableSchemaData
{
    LPCWSTR pszDatabaseFile;
    LPCWSTR pszTableName;
    ULONG ulTotalTableColumnData;
    TTableColumnData* pTableColumnData;
} TTableSchemaData;

#pragma unmanaged

static HANDLE l_hHeap = NULL;

static BOOL lInitHeap(VOID)
{
    if (l_hHeap == NULL)
    {
        l_hHeap = ::HeapCreate(0, 0, 0);

        if (l_hHeap)
        {
            return TRUE;
        }
    }

    return FALSE;
}

static BOOL lUninitHeap(VOID)
{
    BOOL bResult = FALSE;

    if (l_hHeap)
    {
        bResult = ::HeapDestroy(l_hHeap);

        l_hHeap = NULL;
    }

    return bResult;
}

static LPVOID lAllocMem(
  ULONG ulMemLen)
{
    if (l_hHeap)
    {
        return ::HeapAlloc(l_hHeap, HEAP_ZERO_MEMORY, ulMemLen);
    }

    return NULL;
}

static LPVOID lReAllocMem(
  LPVOID pvMem,
  ULONG ulMemLen)
{
    if (l_hHeap)
    {
        return ::HeapReAlloc(l_hHeap, HEAP_ZERO_MEMORY, pvMem, ulMemLen);
    }

    return NULL;
}

static BOOL lFreeMem(
  LPVOID pvMem)
{
    if (l_hHeap)
    {
        return ::HeapFree(l_hHeap, 0, pvMem);
    }

    return FALSE;
}

static LPCWSTR lAllocConnectionString(
  LPCWSTR pszDatabaseFile)
{
    LPCWSTR pszProvider, pszExtension;
    LPWSTR pszConnection;
    LONG lConnectionLen;

    pszExtension = ::PathFindExtensionW(pszDatabaseFile);

    if (0 == ::lstrcmpiW(pszExtension, CMdbFileExt))
    {
        pszProvider = CAccessProvider_MDB;
    }
    else if (0 == ::lstrcmpiW(pszExtension, CAccdbFileExt))
    {
        pszProvider = CAccessProvider_ACCDB;
    }
    else
    {
        return NULL;
    }

    lConnectionLen = ::lstrlenW(pszProvider) +
                     ::lstrlenW(CDataSource) +
                     ::lstrlenW(pszDatabaseFile) + 4;

    pszConnection = (LPWSTR)lAllocMem(sizeof(WCHAR) * lConnectionLen);

    ::StringCchCopyW(pszConnection, lConnectionLen, pszProvider);
    ::StringCchCatW(pszConnection, lConnectionLen, CDataSource);
    ::StringCchCatW(pszConnection, lConnectionLen, L"\"");
    ::StringCchCatW(pszConnection, lConnectionLen, pszDatabaseFile);
    ::StringCchCatW(pszConnection, lConnectionLen, L"\";");

    return pszConnection;
}

static VOID lFreeConnectionString(
  LPCWSTR pszConnection)
{
    lFreeMem((LPVOID)pszConnection);
}

static LPCWSTR lAllocTmpDatabaseFile(
  LPCWSTR pszDatabaseFile)
{
    LONG nTmpDatabaseFileLen = ::lstrlenW(pszDatabaseFile) + 2;
    LPWSTR pszTmpDatabaseFile, pszFileName;

    pszTmpDatabaseFile = (LPWSTR)lAllocMem(nTmpDatabaseFileLen * sizeof(WCHAR));

    ::StringCchCopyW(pszTmpDatabaseFile, nTmpDatabaseFileLen, pszDatabaseFile);

    pszFileName = ::PathFindFileNameW(pszTmpDatabaseFile);

    *pszFileName = 0;

    ::StringCchCatW(pszTmpDatabaseFile, nTmpDatabaseFileLen, L"_");

    pszFileName = ::PathFindFileNameW(pszDatabaseFile);

    ::StringCchCatW(pszTmpDatabaseFile, nTmpDatabaseFileLen, pszFileName);

    return pszTmpDatabaseFile;
}

static VOID lFreeTmpDatabaseFile(
  LPCWSTR pszTmpDatabaseFile)
{
    lFreeMem((LPVOID)pszTmpDatabaseFile);
}

static BOOL lAllocateADOConnection(
  LPCWSTR pszDatabaseFile,
  ADOConnection** ppConnection)
{
    BOOL bResult = FALSE;
    LPCWSTR pszConnection = lAllocConnectionString(pszDatabaseFile);
    BSTR bstrConnection = ::SysAllocString(pszConnection);
    IUnknown* pUnknown = NULL;
    CLSID clsid;

    if (S_OK == ::CLSIDFromProgID(OLESTR("ADODB.Connection"), &clsid) &&
        S_OK == ::CoCreateInstance(clsid, NULL, CLSCTX_INPROC_SERVER, IID_IUnknown, (LPVOID*)&pUnknown) &&
        S_OK == pUnknown->QueryInterface(__uuidof(ADOConnection), (LPVOID*)ppConnection))
    {
        if (S_OK == (*ppConnection)->Open(bstrConnection))
        {
            bResult = TRUE;
        }
        else
        {
            (*ppConnection)->Release();

            *ppConnection = NULL;
        }
    }

    if (pUnknown)
    {
        pUnknown->Release();
    }

    ::SysFreeString(bstrConnection);

    lFreeConnectionString(pszConnection);

    return bResult;
}

static BOOL lOpenColumnsSchema(
  ADOConnection* pConnection,
  LPCWSTR pszTableName,
  ADORecordset** ppRecordset)
{
    BOOL bResult = FALSE;
    VARIANT RestrictionsVariant, SchemaIDVariant, TmpVariant;
    LONG lIndices;

    ::VariantInit(&RestrictionsVariant);
    ::VariantInit(&SchemaIDVariant);
    ::VariantInit(&TmpVariant);

    RestrictionsVariant.vt = VT_ARRAY | VT_VARIANT;
    RestrictionsVariant.parray = ::SafeArrayCreateVector(VT_VARIANT, 0, 3);

    lIndices = 0;

    ::SafeArrayPutElement(RestrictionsVariant.parray, &lIndices, &TmpVariant);

    ++lIndices;

    ::SafeArrayPutElement(RestrictionsVariant.parray, &lIndices, &TmpVariant);

    ++lIndices;

    TmpVariant.vt = VT_BSTR;
    TmpVariant.bstrVal = ::SysAllocString(pszTableName);

    ::SafeArrayPutElement(RestrictionsVariant.parray, &lIndices, &TmpVariant);

    ::VariantClear(&TmpVariant);

    SchemaIDVariant.vt = VT_ERROR;
    SchemaIDVariant.scode = DISP_E_PARAMNOTFOUND;

    if (S_OK == pConnection->OpenSchema(adSchemaColumns, RestrictionsVariant,
                                        SchemaIDVariant, ppRecordset))
    {
        bResult = TRUE;
    }

    ::VariantClear(&SchemaIDVariant);
    ::VariantClear(&RestrictionsVariant);

    return bResult;
}

static BOOL lOpenLoggedInUsersSchema(
  ADOConnection* pConnection,
  ADORecordset** ppRecordset)
{
    BOOL bResult = FALSE;
    VARIANT RestrictionsVariant, SchemaIDVariant;

    ::VariantInit(&RestrictionsVariant);
    ::VariantInit(&SchemaIDVariant);

    RestrictionsVariant.vt = VT_ERROR;
    RestrictionsVariant.scode = DISP_E_PARAMNOTFOUND;

    SchemaIDVariant.vt = VT_BSTR;
    SchemaIDVariant.bstrVal = ::SysAllocString(CLoggedInUsersGuid);

    if (S_OK == pConnection->OpenSchema(adSchemaProviderSpecific,
                                        RestrictionsVariant,
                                        SchemaIDVariant, ppRecordset))
    {
        bResult = TRUE;
    }

    ::VariantClear(&SchemaIDVariant);
    ::VariantClear(&RestrictionsVariant);

    return bResult;
}

static DWORD WINAPI lLoggedInUsersThreadProc(
  LPVOID pvParameter)
{
    DWORD dwResult = FALSE;
    TLoggedInUsersData* pLoggedInUsersData = (TLoggedInUsersData*)pvParameter;
	TLoggedInUserData* pLoggedInUserData;
    ADOConnection* pConnection = NULL;
    ADORecordset* pRecordset = NULL;
    ADOFields* pFields = NULL;
    ADOField* pField;
    VARIANT_BOOL vbEOF;
	LONG lCount, lValueLen;
    VARIANT IndexVariant, ValueVariant;
	BSTR bstrName;

    pLoggedInUsersData->ulTotalLoggedInUserData = 0;
    pLoggedInUsersData->pLoggedInUserData = NULL;

    if (S_OK != ::CoInitializeEx(NULL, COINIT_MULTITHREADED))
    {
        return FALSE;
    }

    if (lAllocateADOConnection(pLoggedInUsersData->pszDatabaseFile, &pConnection))
    {
        if (lOpenLoggedInUsersSchema(pConnection, &pRecordset))
        {
			pLoggedInUsersData->pLoggedInUserData = (TLoggedInUserData*)lAllocMem(0);

			while (S_OK == pRecordset->get_EOF(&vbEOF) &&
                   vbEOF == VARIANT_FALSE)
            {
                if (S_OK == pRecordset->get_Fields(&pFields))
                {
                    if (S_OK == pFields->get_Count(&lCount))
					{
						pLoggedInUsersData->pLoggedInUserData =
							(TLoggedInUserData*)lReAllocMem(
								pLoggedInUsersData->pLoggedInUserData,
								sizeof(TLoggedInUserData) *
									(pLoggedInUsersData->ulTotalLoggedInUserData + 1));

						pLoggedInUserData = &pLoggedInUsersData->pLoggedInUserData[pLoggedInUsersData->ulTotalLoggedInUserData];

						pLoggedInUsersData->ulTotalLoggedInUserData += 1;

                        for (long lIndex = 0; lIndex < lCount; ++lIndex)
                        {
                            ::VariantInit(&IndexVariant);

                            IndexVariant.vt = VT_I4;
                            IndexVariant.lVal = lIndex;

                            if (S_OK == pFields->get_Item(IndexVariant, &pField))
							{
								pField->get_Name(&bstrName);

                                ::VariantInit(&ValueVariant);

                                pField->get_Value(&ValueVariant);

                                if (0 == ::lstrcmpiW(bstrName, L"COMPUTER_NAME"))
								{
									if (ValueVariant.vt == VT_BSTR)
									{
										lValueLen = ::SysStringLen(ValueVariant.bstrVal) + 1;

										pLoggedInUserData->pszComputerName = (LPWSTR)lAllocMem(lValueLen * sizeof(WCHAR));

										::StringCchCopy(pLoggedInUserData->pszComputerName,
											            lValueLen, ValueVariant.bstrVal);
									}
									else
									{
										pLoggedInUserData->pszComputerName = NULL;
									}
								}
                                else if (0 == ::lstrcmpiW(bstrName, L"LOGIN_NAME"))
								{
									if (ValueVariant.vt == VT_BSTR)
									{
										lValueLen = ::SysStringLen(ValueVariant.bstrVal) + 1;

										pLoggedInUserData->pszUserName = (LPWSTR)lAllocMem(lValueLen * sizeof(WCHAR));

										::StringCchCopy(pLoggedInUserData->pszUserName,
											            lValueLen, ValueVariant.bstrVal);
									}
									else
									{
										pLoggedInUserData->pszUserName = NULL;
									}
								}
                                else if (0 == ::lstrcmpiW(bstrName, L"CONNECTED"))
								{
									if (ValueVariant.vt == VT_BOOL)
									{
										pLoggedInUserData->bConnected = (ValueVariant.boolVal == VARIANT_TRUE) ? TRUE : FALSE;
									}
									else
									{
										pLoggedInUserData->bConnected = FALSE;
									}
								}
                                else if (0 == ::lstrcmpiW(bstrName, L"SUSPECT_STATE"))
								{
									if (ValueVariant.vt == VT_NULL)
									{
										pLoggedInUserData->bSuspectState = FALSE;
									}
									else
									{
										pLoggedInUserData->bSuspectState = TRUE;
									}
								}

								::SysFreeString(bstrName);

								::VariantClear(&ValueVariant);

								pField->Release();
							}

							::VariantClear(&IndexVariant);
						}
					}

					pFields->Release();
				}		

                pRecordset->MoveNext();
            }

            pRecordset->Close();
            pRecordset->Release();

			if (pLoggedInUsersData->ulTotalLoggedInUserData == 0)
			{
				lFreeMem(pLoggedInUsersData->pLoggedInUserData);

                pLoggedInUsersData->pLoggedInUserData = NULL;
			}

            dwResult = TRUE;
        }

        pConnection->Close();
        pConnection->Release();
    }

    ::CoUninitialize();

    return dwResult;
}

static DWORD WINAPI lCompactDatabaseThreadProc(
  LPVOID pvParameter)
{
    DWORD dwResult = FALSE;
    LPCWSTR pszDatabaseFile = (LPCWSTR)pvParameter;
    IUnknown* pUnknown = NULL;
    IJetEngine* pJetEngine = NULL;
    CLSID clsid;
    LPCWSTR pszTmpDatabaseFile, pszSourceConnection, pszDestConnection;
    BSTR bstrSourceConnection, bstrDestConnection;

    if (S_OK != ::CoInitializeEx(NULL, COINIT_MULTITHREADED))
    {
        return FALSE;
    }

    if (S_OK == ::CLSIDFromProgID(OLESTR("JRO.JetEngine"), &clsid) &&
        S_OK == ::CoCreateInstance(clsid, NULL, CLSCTX_INPROC_SERVER,
                                   IID_IUnknown, (LPVOID*)&pUnknown) &&
        S_OK == pUnknown->QueryInterface(__uuidof(IJetEngine), (LPVOID*)&pJetEngine))
    {
        pszTmpDatabaseFile = lAllocTmpDatabaseFile(pszDatabaseFile);

        pszSourceConnection = lAllocConnectionString(pszDatabaseFile);
        pszDestConnection = lAllocConnectionString(pszTmpDatabaseFile);

        bstrSourceConnection = ::SysAllocString(pszSourceConnection);
        bstrDestConnection = ::SysAllocString(pszDestConnection);

        lFreeConnectionString(pszDestConnection);
        lFreeConnectionString(pszSourceConnection);

        if (S_OK == pJetEngine->CompactDatabase(bstrSourceConnection,
                                                bstrDestConnection))
        {
            if (::MoveFileExW(pszTmpDatabaseFile, pszDatabaseFile,
                              MOVEFILE_REPLACE_EXISTING))
            {
                dwResult = TRUE;
            }
            else
            {
                ::DeleteFileW(pszTmpDatabaseFile);
            }
        }

        lFreeTmpDatabaseFile(pszTmpDatabaseFile);

        ::SysFreeString(bstrDestConnection);
        ::SysFreeString(bstrSourceConnection);
    }

    if (pJetEngine)
    {
        pJetEngine->Release();
    }

    if (pUnknown)
    {
        pUnknown->Release();
    }

    ::CoUninitialize();

    return dwResult;
}

static DWORD WINAPI lGetDatabaseTableSchemaThreadProc(
  LPVOID pvParameter)
{
    DWORD dwResult = FALSE;
    TTableSchemaData* pTableSchemaData = (TTableSchemaData*)pvParameter;
    ADOConnection* pConnection = NULL;
    ADORecordset* pRecordset = NULL;
    ADOFields* pFields = NULL;
    ADOField* pField = NULL;
    TTableColumnData* pTableColumnData;
    VARIANT IndexVariant, ValueVariant;
    VARIANT_BOOL vbEOF;
    BSTR bstrName, bstrColumnName;
    long lCount, lColumnLen, lColumnNameLen, lColumnFlags;
    EColumnType ColumnType;

    if (S_OK != ::CoInitializeEx(NULL, COINIT_MULTITHREADED))
    {
        return FALSE;
    }

    if (lAllocateADOConnection(pTableSchemaData->pszDatabaseFile, &pConnection))
    {
        if (lOpenColumnsSchema(pConnection, pTableSchemaData->pszTableName,
                               &pRecordset))
        {
            pTableSchemaData->pTableColumnData = (TTableColumnData*)lAllocMem(0);

            while (S_OK == pRecordset->get_EOF(&vbEOF) &&
                   vbEOF == VARIANT_FALSE)
            {
                if (S_OK == pRecordset->get_Fields(&pFields))
                {
                    if (S_OK == pFields->get_Count(&lCount))
                    {
                        bstrColumnName = NULL;
                        ColumnType = ectUnknown;
                        lColumnLen = 0;
                        lColumnFlags = 0;

                        for (long lIndex = 0; lIndex < lCount; ++lIndex)
                        {
                            ::VariantInit(&IndexVariant);

                            IndexVariant.vt = VT_I4;
                            IndexVariant.lVal = lIndex;

                            if (S_OK == pFields->get_Item(IndexVariant, &pField))
                            {
                                pField->get_Name(&bstrName);

                                ::VariantInit(&ValueVariant);

                                pField->get_Value(&ValueVariant);

                                if (0 == ::lstrcmpiW(bstrName, L"COLUMN_NAME"))
                                {
                                    if (ValueVariant.vt == VT_BSTR)
                                    {
                                        bstrColumnName = ValueVariant.bstrVal;

                                        ValueVariant.vt = VT_EMPTY;
                                    }
                                }
                                else if (0 == ::lstrcmpiW(bstrName, L"DATA_TYPE"))
                                {
                                    if (S_OK == ::VariantChangeType(&ValueVariant, &ValueVariant, 0, VT_I4))
                                    {
                                        switch (ValueVariant.lVal)
                                        {
                                            case adInteger:
                                                ColumnType = ectInteger;
                                                break;
                                            case adBoolean:
                                                ColumnType = ectBoolean;
                                                break;
                                            case adDate:
                                            case adDBDate:
                                            case adDBTime:
                                            case adDBTimeStamp:
                                                ColumnType = ectDateTime;
                                                break;
                                            case adWChar:
                                            case adVarWChar:
                                                ColumnType = ectCharacter;
                                                break;
                                        }
                                    }
                                }
                                else if (0 == ::lstrcmpiW(bstrName, L"CHARACTER_MAXIMUM_LENGTH"))
                                {
                                    if (S_OK == ::VariantChangeType(&ValueVariant, &ValueVariant, 0, VT_I4))
                                    {
                                        lColumnLen = ValueVariant.lVal;
                                    }
                                }
                                else if (0 == ::lstrcmpiW(bstrName, L"COLUMN_FLAGS"))
                                {
                                    if (S_OK == ::VariantChangeType(&ValueVariant, &ValueVariant, 0, VT_I4))
                                    {
                                        lColumnFlags = ValueVariant.lVal;
                                    }
                                }

                                ::VariantClear(&ValueVariant);

                                ::SysFreeString(bstrName);

                                pField->Release();
                            }

                            ::VariantClear(&IndexVariant);
                        }

                        // Check for a memo field

                        if (ColumnType == ectCharacter && lColumnFlags == 234)
                        {
                            lColumnLen = 0x7FFFFFFF;
                        }

                        pTableSchemaData->pTableColumnData =
                            (TTableColumnData*)lReAllocMem(
                                pTableSchemaData->pTableColumnData,
                                sizeof(TTableColumnData) *
                                    (pTableSchemaData->ulTotalTableColumnData + 1));

                        pTableColumnData = &pTableSchemaData->pTableColumnData[pTableSchemaData->ulTotalTableColumnData];

                        pTableSchemaData->ulTotalTableColumnData += 1;

                        lColumnNameLen = ::lstrlenW(bstrColumnName) + 1;

                        pTableColumnData->pszColumnName = (LPWSTR)lAllocMem(sizeof(WCHAR) * lColumnNameLen);
                        pTableColumnData->ulColumnLength = lColumnLen;
                        pTableColumnData->ColumnType = ColumnType;

                        ::StringCchCopy(pTableColumnData->pszColumnName,
                                        lColumnNameLen, bstrColumnName);

                        ::SysFreeString(bstrColumnName);
                    }

                    pFields->Release();
                }

                pRecordset->MoveNext();
            }

            if (pTableSchemaData->ulTotalTableColumnData > 0)
            {
                dwResult = TRUE;
            }
            else
            {
                lFreeMem(pTableSchemaData->pTableColumnData);

                pTableSchemaData->pTableColumnData = NULL;
            }

            pRecordset->Close();
            pRecordset->Release();
        }

        pConnection->Close();
        pConnection->Release();
    }

    ::CoUninitialize();

    return dwResult;
}

static BOOL lCompactDatabase(
  LPCWSTR pszDatabaseFile)
{
    BOOL bResult = FALSE;
    HANDLE hThread;
    DWORD dwExitCode;

    if (lInitHeap() == FALSE)
    {
        return FALSE;
    }

    hThread = ::CreateThread(NULL, 0, lCompactDatabaseThreadProc,
                             (LPVOID)pszDatabaseFile, 0, NULL);

    if (hThread)
    {
        ::WaitForSingleObject(hThread, INFINITE);

        if (::GetExitCodeThread(hThread, &dwExitCode) && dwExitCode == TRUE)
        {
            bResult = TRUE;
        }

        ::CloseHandle(hThread);
    }

    if (lUninitHeap() == FALSE)
    {
        return FALSE;
    }

    return bResult;
}

#pragma managed

System::String^ Common::Data::MSAccess::BuildConnectionString(
  System::String^ sDatabaseFile)
{
    pin_ptr<const wchar_t> pszDatabaseFile = PtrToStringChars(sDatabaseFile);
    LPCWSTR pszConnection;
    System::String^ sConnection;

    if (lInitHeap() == FALSE)
    {
        return nullptr;
    }

    pszConnection = lAllocConnectionString(pszDatabaseFile);

    sConnection = gcnew System::String(pszConnection);

    lFreeConnectionString(pszConnection);

    if (lUninitHeap() == FALSE)
    {
        return nullptr;
    }

    return sConnection;
}

System::Boolean Common::Data::MSAccess::GetLoggedInUsers(
  System::String^ sDatabaseFile,
  System::Collections::Generic::List<LoggedInUser^>^% LoggedInUserList)
{
	System::Boolean bResult = false;
    pin_ptr<const wchar_t> pszDatabaseFile = PtrToStringChars(sDatabaseFile);
    TLoggedInUsersData LoggedInUsersData;
	TLoggedInUserData* pLoggedInUserData;
    HANDLE hThread;
    DWORD dwExitCode;

    if (lInitHeap() == FALSE)
    {
        return false;
    }

    LoggedInUserList = gcnew System::Collections::Generic::List<LoggedInUser^>();

    LoggedInUsersData.pszDatabaseFile = pszDatabaseFile;

    hThread = ::CreateThread(NULL, 0, lLoggedInUsersThreadProc,
                             &LoggedInUsersData, 0, NULL);

    if (hThread)
    {
        ::WaitForSingleObject(hThread, INFINITE);

        if (::GetExitCodeThread(hThread, &dwExitCode) && dwExitCode == TRUE)
        {
            bResult = true;
        }

		if (LoggedInUsersData.pLoggedInUserData)
		{
			for (ULONG ulIndex = 0; ulIndex < LoggedInUsersData.ulTotalLoggedInUserData;
				 ++ulIndex)
			{
				pLoggedInUserData = &LoggedInUsersData.pLoggedInUserData[ulIndex];

				if (pLoggedInUserData->pszComputerName && pLoggedInUserData->pszUserName)
				{
					LoggedInUserList->Add(
                        gcnew Common::Data::LoggedInUser(
						    gcnew System::String(pLoggedInUserData->pszComputerName),
							gcnew System::String(pLoggedInUserData->pszUserName),
							pLoggedInUserData->bConnected ? true : false,
							pLoggedInUserData->bSuspectState ? true : false));
				}

				if (pLoggedInUserData->pszComputerName)
				{
					lFreeMem(pLoggedInUserData->pszComputerName);
				}

				if (pLoggedInUserData->pszUserName)
				{
					lFreeMem(pLoggedInUserData->pszUserName);
				}
			}

			lFreeMem(LoggedInUsersData.pLoggedInUserData);
		}

        ::CloseHandle(hThread);
    }

    if (lUninitHeap() == FALSE)
    {
        return false;
    }

    return bResult;
}

System::Boolean Common::Data::MSAccess::CompactDatabase(
  System::String^ sDatabaseFile)
{
    pin_ptr<const wchar_t> pszDatabaseFile = PtrToStringChars(sDatabaseFile);

    if (lCompactDatabase(pszDatabaseFile))
    {
        return true;
    }

    return false;
}

System::Boolean Common::Data::MSAccess::GetDatabaseTableSchema(
  System::String^ sDatabaseFile,
  System::String^ sTableName,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList)
{
    System::Boolean bResult = false;
    pin_ptr<const wchar_t> pszDatabaseFile = PtrToStringChars(sDatabaseFile);
    pin_ptr<const wchar_t> pszTableName = PtrToStringChars(sTableName);
    TTableSchemaData TableSchemaData;
    HANDLE hThread;
    DWORD dwExitCode;
    TTableColumnData* pTableColumnData;
    Common::Data::DbTableColumn::EColumnType ColumnType;

    TableColumnList = nullptr;

    if (lInitHeap() == FALSE)
    {
        return false;
    }

    TableColumnList = gcnew System::Collections::Generic::List<Common::Data::DbTableColumn^>();

    TableSchemaData.pszDatabaseFile = pszDatabaseFile;
    TableSchemaData.pszTableName = pszTableName;
    TableSchemaData.ulTotalTableColumnData = 0;
    TableSchemaData.pTableColumnData = NULL;

    hThread = ::CreateThread(NULL, 0, lGetDatabaseTableSchemaThreadProc,
                             (LPVOID)&TableSchemaData, 0, NULL);

    if (hThread)
    {
        ::WaitForSingleObject(hThread, INFINITE);

        if (::GetExitCodeThread(hThread, &dwExitCode) && dwExitCode == TRUE)
        {
            for (ULONG ulIndex = 0; ulIndex < TableSchemaData.ulTotalTableColumnData;
                 ++ulIndex)
            {
                pTableColumnData = &TableSchemaData.pTableColumnData[ulIndex];

                switch (pTableColumnData->ColumnType)
                {
                    case ectCharacter:
                        ColumnType = Common::Data::DbTableColumn::EColumnType::Character;
                        break;
                    case ectInteger:
                        ColumnType = Common::Data::DbTableColumn::EColumnType::Integer;
                        break;
                    case ectDateTime:
                        ColumnType = Common::Data::DbTableColumn::EColumnType::DateTime;
                        break;
                    case ectBoolean:
                        ColumnType = Common::Data::DbTableColumn::EColumnType::Boolean;
                        break;
                    default:
                        ColumnType = Common::Data::DbTableColumn::EColumnType::Unknown;
                        break;
                }

                TableColumnList->Add(
                    gcnew Common::Data::DbTableColumn(
                        gcnew System::String(pTableColumnData->pszColumnName),
                                             pTableColumnData->ulColumnLength, ColumnType));

                lFreeMem(pTableColumnData->pszColumnName);
            }

            lFreeMem(TableSchemaData.pTableColumnData);

            bResult = true;
        }

        ::CloseHandle(hThread);
    }

    if (lUninitHeap() == FALSE)
    {
        return false;
    }

    return bResult;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2007-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
