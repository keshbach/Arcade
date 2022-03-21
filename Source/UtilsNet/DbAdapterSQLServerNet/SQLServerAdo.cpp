/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "SQLServerAdo.h"

#define SQLServer2012

#if defined(SQLServer2008)
#define CProviderName L"SQLNCLI10"
#elif defined(SQLServer2012)
#define CProviderName L"SQLNCLI11"
#else
#error SQL Server native client provider name has not been defined.
#endif

#define CProvider L"Provider="
#define CServer L"Server="
#define CInitialCatalog L"Initial Catalog="
#define CUserId L"Uid="
#define CPassword L"Password="
#define CDisablePooling L"Pooling=false;"

#define CSelectTableSchemaQueryStart \
    L"SELECT c.name AS column_name, " \
    L"       t.name AS type_name, " \
    L"       c.max_length " \
    L"FROM sys.columns AS c " \
    L"JOIN sys.types AS t ON c.user_type_id = t.user_type_id " \
    L"WHERE c.object_id = OBJECT_ID('dbo."

#define CSelectTableSchemaQueryEnd \
    L"');"

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
    LPCWSTR pszServer;
    LPCWSTR pszPort;
    LPCWSTR pszCatalog;
    LPCWSTR pszUserName;
    LPCWSTR pszPassword;
    LPCWSTR pszTableName;
    ULONG ulTotalTableColumnData;
    TTableColumnData* pTableColumnData;
} TTableSchemaData;

typedef struct tagTSnapshotIsolationData
{
    LPCWSTR pszServer;
    LPCWSTR pszPort;
    LPCWSTR pszCatalog;
    LPCWSTR pszUserName;
    LPCWSTR pszPassword;
    BOOL bSnapshotIsolation;
} TSnapshotIsolationData;

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
  LPCWSTR pszServer,
  LPCWSTR pszPort,
  LPCWSTR pszCatalog,
  LPCWSTR pszUserName,
  LPCWSTR pszPassword)
{
    LPWSTR pszConnection;
    LONG lConnectionLen;

	lConnectionLen = ::lstrlenW(CProvider) + ::lstrlenW(CProviderName) + 1;
	lConnectionLen += ::lstrlenW(CServer) + ::lstrlenW(pszServer) +
		              ::lstrlenW(pszPort) + 2;
	lConnectionLen += ::lstrlenW(CInitialCatalog) + ::lstrlenW(pszCatalog) + 1;
	lConnectionLen += ::lstrlenW(CUserId) + ::lstrlenW(pszUserName) + 1;
	lConnectionLen += ::lstrlenW(CPassword) + ::lstrlenW(pszPassword) + 1;
    lConnectionLen += ::lstrlenW(CDisablePooling);
	lConnectionLen += 1;

    pszConnection = (LPWSTR)lAllocMem(sizeof(WCHAR) * lConnectionLen);

    ::StringCchCopyW(pszConnection, lConnectionLen, CProvider);
    ::StringCchCatW(pszConnection, lConnectionLen, CProviderName);
    ::StringCchCatW(pszConnection, lConnectionLen, L";");
    ::StringCchCatW(pszConnection, lConnectionLen, CServer);
	::StringCchCatW(pszConnection, lConnectionLen, pszServer);
	::StringCchCatW(pszConnection, lConnectionLen, L",");
	::StringCchCatW(pszConnection, lConnectionLen, pszPort);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
	::StringCchCatW(pszConnection, lConnectionLen, CInitialCatalog);
	::StringCchCatW(pszConnection, lConnectionLen, pszCatalog);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
    ::StringCchCatW(pszConnection, lConnectionLen, CUserId);
	::StringCchCatW(pszConnection, lConnectionLen, pszUserName);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
	::StringCchCatW(pszConnection, lConnectionLen, CPassword);
	::StringCchCatW(pszConnection, lConnectionLen, pszPassword);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
    ::StringCchCatW(pszConnection, lConnectionLen, CDisablePooling);

    return pszConnection;
}

static VOID lFreeConnectionString(
  LPCWSTR pszConnection)
{
    lFreeMem((LPVOID)pszConnection);
}

static BOOL lAllocateADOConnection(
  LPCWSTR pszServer,
  LPCWSTR pszPort,
  LPCWSTR pszCatalog,
  LPCWSTR pszUserName,
  LPCWSTR pszPassword,
  ADOConnection** ppConnection)
{
    BOOL bResult = FALSE;
    LPCWSTR pszConnection = lAllocConnectionString(pszServer, pszPort,
		                                           pszCatalog, pszUserName,
												   pszPassword);
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
	LPWSTR pszCommandText;
	LONG lCommandTextLen;
	BSTR CommandText;
    VARIANT RecsAffectedVariant;

	::VariantInit(&RecsAffectedVariant);

	lCommandTextLen = ::lstrlenW(CSelectTableSchemaQueryStart) +
		              ::lstrlenW(pszTableName) +
		              ::lstrlenW(CSelectTableSchemaQueryEnd) + 1;

	pszCommandText = (LPWSTR)lAllocMem(lCommandTextLen * sizeof(WCHAR));

	::StringCchCatW(pszCommandText, lCommandTextLen, CSelectTableSchemaQueryStart);
	::StringCchCatW(pszCommandText, lCommandTextLen, pszTableName);
	::StringCchCatW(pszCommandText, lCommandTextLen, CSelectTableSchemaQueryEnd);

	CommandText = ::SysAllocString(pszCommandText);

	lFreeMem(pszCommandText);

	if (S_OK == pConnection->Execute(CommandText, &RecsAffectedVariant,
		                             adCmdText, ppRecordset))
	{
		bResult = TRUE;
	}

	::SysFreeString(CommandText);

    ::VariantClear(&RecsAffectedVariant);

    return bResult;
}

static BOOL lOpenDatabaseOptions(
  ADOConnection* pConnection,
  ADORecordset** ppRecordset)
{
    BOOL bResult = FALSE;
	BSTR CommandText;
    VARIANT RecsAffectedVariant;

	::VariantInit(&RecsAffectedVariant);

	CommandText = ::SysAllocString(OLESTR("DBCC USEROPTIONS"));

	if (S_OK == pConnection->Execute(CommandText, &RecsAffectedVariant,
		                             adCmdText, ppRecordset))
	{
		bResult = TRUE;
	}

	::SysFreeString(CommandText);

    ::VariantClear(&RecsAffectedVariant);

    return bResult;
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

	if (lAllocateADOConnection(pTableSchemaData->pszServer,
		                       pTableSchemaData->pszPort,
		                       pTableSchemaData->pszCatalog,
							   pTableSchemaData->pszUserName,
		                       pTableSchemaData->pszPassword, &pConnection))
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

                                if (0 == ::lstrcmpiW(bstrName, L"column_name"))
                                {
                                    if (ValueVariant.vt == VT_BSTR)
                                    {
                                        bstrColumnName = ValueVariant.bstrVal;

                                        ValueVariant.vt = VT_EMPTY;
                                    }
                                }
                                else if (0 == ::lstrcmpiW(bstrName, L"type_name"))
                                {
                                    if (ValueVariant.vt == VT_BSTR)
									{
										if (0 == ::lstrcmpiW(ValueVariant.bstrVal, L"int"))
										{
                                            ColumnType = ectInteger;
										}
										else if (0 == ::lstrcmpiW(ValueVariant.bstrVal, L"nvarchar"))
										{
                                            ColumnType = ectCharacter;
										}
										else if (0 == ::lstrcmpiW(ValueVariant.bstrVal, L"bit"))
										{
                                            ColumnType = ectBoolean;
										}
									}
                                }
                                else if (0 == ::lstrcmpiW(bstrName, L"max_length"))
                                {
									if (S_OK == ::VariantChangeType(&ValueVariant, &ValueVariant, 0, VT_I4))
                                    {
                                        lColumnLen = ValueVariant.lVal;

										if (lColumnLen == -1)
										{
				                            lColumnLen = 0x7FFFFFFF;
										}
                                    }
                                }

                                ::VariantClear(&ValueVariant);

                                ::SysFreeString(bstrName);

                                pField->Release();
                            }

                            ::VariantClear(&IndexVariant);
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

                        ::StringCchCopyW(pTableColumnData->pszColumnName,
                                         lColumnNameLen, bstrColumnName);

                        if (pTableColumnData->ColumnType == ectCharacter)
                        {
                            // Convert bytes to characters (assuming all text
                            // is in Unicode)

                            pTableColumnData->ulColumnLength /= 2;
                        }

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

static DWORD WINAPI lGetDatabaseSnapshotIsolationThreadProc(
  LPVOID pvParameter)
{
    DWORD dwResult = FALSE;
    TSnapshotIsolationData* pSnapshotIsolationData = (TSnapshotIsolationData*)pvParameter;
    ADOConnection* pConnection = NULL;
    ADORecordset* pRecordset = NULL;
    ADOFields* pFields = NULL;
    ADOField* pField = NULL;
    VARIANT IndexVariant, ValueVariant;
    VARIANT_BOOL vbEOF;
    BSTR bstrName;
    long lCount;
    BOOL bRowFound;

    pSnapshotIsolationData->bSnapshotIsolation = FALSE;

    if (S_OK != ::CoInitializeEx(NULL, COINIT_MULTITHREADED))
    {
        return FALSE;
    }

    if (lAllocateADOConnection(pSnapshotIsolationData->pszServer,
		                       pSnapshotIsolationData->pszPort,
		                       pSnapshotIsolationData->pszCatalog,
							   pSnapshotIsolationData->pszUserName,
		                       pSnapshotIsolationData->pszPassword,
                               &pConnection))
    {
        if (lOpenDatabaseOptions(pConnection, &pRecordset))
        {
            bRowFound = FALSE;

            while (S_OK == pRecordset->get_EOF(&vbEOF) &&
                   vbEOF == VARIANT_FALSE &&
                   bRowFound == FALSE)
            {
                if (S_OK == pRecordset->get_Fields(&pFields))
                {
                    if (S_OK == pFields->get_Count(&lCount) && lCount == 2)
                    {
                        ::VariantInit(&IndexVariant);

                        IndexVariant.vt = VT_I4;
                        IndexVariant.lVal = 0;

                        if (S_OK == pFields->get_Item(IndexVariant, &pField))
                        {
                            pField->get_Name(&bstrName);

                            if (0 == ::lstrcmpiW(bstrName, L"Set Option"))
                            {
                                ::VariantInit(&ValueVariant);

                                pField->get_Value(&ValueVariant);

                                if (ValueVariant.vt == VT_BSTR)
                                {
									if (0 == ::lstrcmpiW(ValueVariant.bstrVal, L"isolation level"))
                                    {
                                        bRowFound = TRUE;
                                    }
                                }

                                ::VariantClear(&ValueVariant);
                            }

                            ::SysFreeString(bstrName);

                            pField->Release();
                        }

                        ::VariantClear(&IndexVariant);

                        if (bRowFound)
                        {
                            ::VariantInit(&IndexVariant);

                            IndexVariant.vt = VT_I4;
                            IndexVariant.lVal = 1;

                            if (S_OK == pFields->get_Item(IndexVariant, &pField))
                            {
                                ::VariantInit(&ValueVariant);

                                pField->get_Value(&ValueVariant);

                                if (ValueVariant.vt == VT_BSTR)
                                {
									if (0 == ::lstrcmpiW(ValueVariant.bstrVal, L"read committed snapshot"))
                                    {
                                        pSnapshotIsolationData->bSnapshotIsolation = TRUE;

                                        dwResult = TRUE;
                                    }
                                }

                                ::VariantClear(&ValueVariant);
                            }

                            pField->Release();

                            ::VariantClear(&IndexVariant);
                        }
                    }

                    pFields->Release();
                }

                pRecordset->MoveNext();
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

#pragma managed

System::String^ Common::Data::SQLServerAdo::BuildConnectionString(
  System::String^ sServer,
  System::Int32 nPort,
  System::String^ sCatalog,
  System::String^ sUserName,
  System::String^ sPassword)
{
    pin_ptr<const wchar_t> pszServer = PtrToStringChars(sServer);
	pin_ptr<const wchar_t> pszPort = PtrToStringChars(nPort.ToString());
	pin_ptr<const wchar_t> pszCatalog = PtrToStringChars(sCatalog);
	pin_ptr<const wchar_t> pszUserName = PtrToStringChars(sUserName);
	pin_ptr<const wchar_t> pszPassword = PtrToStringChars(sPassword);
    LPCWSTR pszConnection;
    System::String^ sConnection;

    if (lInitHeap() == FALSE)
    {
        return nullptr;
    }

    pszConnection = lAllocConnectionString(pszServer, pszPort, pszCatalog,
		                                   pszUserName, pszPassword);

    sConnection = gcnew System::String(pszConnection);

    lFreeConnectionString(pszConnection);

    if (lUninitHeap() == FALSE)
    {
        return nullptr;
    }

    return sConnection;
}

System::Boolean Common::Data::SQLServerAdo::GetDatabaseTableSchema(
  System::String^ sServer,
  System::Int32 nPort,
  System::String^ sCatalog,
  System::String^ sUserName,
  System::String^ sPassword,
  System::String^ sTableName,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList)
{
    System::Boolean bResult = false;
    pin_ptr<const wchar_t> pszServer = PtrToStringChars(sServer);
	pin_ptr<const wchar_t> pszPort = PtrToStringChars(nPort.ToString());
	pin_ptr<const wchar_t> pszCatalog = PtrToStringChars(sCatalog);
	pin_ptr<const wchar_t> pszUserName = PtrToStringChars(sUserName);
	pin_ptr<const wchar_t> pszPassword = PtrToStringChars(sPassword);
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

    TableSchemaData.pszServer = pszServer;
    TableSchemaData.pszPort = pszPort;
    TableSchemaData.pszCatalog = pszCatalog;
    TableSchemaData.pszUserName = pszUserName;
    TableSchemaData.pszPassword = pszPassword;
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
                                             pTableColumnData->ulColumnLength,
                                             ColumnType));

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

System::Boolean Common::Data::SQLServerAdo::GetProvideSnapshotIsolationSupported(
  System::String^ sServer,
  System::Int32 nPort,
  System::String^ sCatalog,
  System::String^ sUserName,
  System::String^ sPassword,
  System::Boolean% bSnapshotSupported)
{
    System::Boolean bResult = false;
    pin_ptr<const wchar_t> pszServer = PtrToStringChars(sServer);
	pin_ptr<const wchar_t> pszPort = PtrToStringChars(nPort.ToString());
	pin_ptr<const wchar_t> pszCatalog = PtrToStringChars(sCatalog);
	pin_ptr<const wchar_t> pszUserName = PtrToStringChars(sUserName);
	pin_ptr<const wchar_t> pszPassword = PtrToStringChars(sPassword);
    TSnapshotIsolationData SnapshotIsolationData;
    HANDLE hThread;
    DWORD dwExitCode;

    bSnapshotSupported = false;

    if (lInitHeap() == FALSE)
    {
        return false;
    }

    SnapshotIsolationData.pszServer = pszServer;
    SnapshotIsolationData.pszPort = pszPort;
    SnapshotIsolationData.pszCatalog = pszCatalog;
    SnapshotIsolationData.pszUserName = pszUserName;
    SnapshotIsolationData.pszPassword = pszPassword;
    SnapshotIsolationData.bSnapshotIsolation = FALSE;

    hThread = ::CreateThread(NULL, 0, lGetDatabaseSnapshotIsolationThreadProc,
                             (LPVOID)&SnapshotIsolationData, 0, NULL);

    if (hThread)
    {
        ::WaitForSingleObject(hThread, INFINITE);

        if (::GetExitCodeThread(hThread, &dwExitCode) && dwExitCode == TRUE)
        {
            bSnapshotSupported = SnapshotIsolationData.bSnapshotIsolation ? true : false;

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
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
