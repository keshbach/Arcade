/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "SQLServerOdbc.h"

#define CDriverName L"{SQL Server Native Client 11.0}"

#define CDriver L"Driver="
#define CServer L"Server="
#define CDatabase L"Database="
#define CUserId L"Uid="
#define CPassword L"Pwd="

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

	lConnectionLen = ::lstrlenW(CDriver) + ::lstrlenW(CDriverName) + 1;
	lConnectionLen += ::lstrlenW(CServer) + ::lstrlenW(pszServer) +
		              ::lstrlenW(pszPort) + 2;
	lConnectionLen += ::lstrlenW(CDatabase) + ::lstrlenW(pszCatalog) + 1;
	lConnectionLen += ::lstrlenW(CUserId) + ::lstrlenW(pszUserName) + 1;
	lConnectionLen += ::lstrlenW(CPassword) + ::lstrlenW(pszPassword) + 1;
	lConnectionLen += 1;

    pszConnection = (LPWSTR)lAllocMem(sizeof(WCHAR) * lConnectionLen);

    ::StringCchCopyW(pszConnection, lConnectionLen, CDriver);
    ::StringCchCatW(pszConnection, lConnectionLen, CDriverName);
    ::StringCchCatW(pszConnection, lConnectionLen, L";");
    ::StringCchCatW(pszConnection, lConnectionLen, CServer);
	::StringCchCatW(pszConnection, lConnectionLen, pszServer);
	::StringCchCatW(pszConnection, lConnectionLen, L",");
	::StringCchCatW(pszConnection, lConnectionLen, pszPort);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
	::StringCchCatW(pszConnection, lConnectionLen, CDatabase);
	::StringCchCatW(pszConnection, lConnectionLen, pszCatalog);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
    ::StringCchCatW(pszConnection, lConnectionLen, CUserId);
	::StringCchCatW(pszConnection, lConnectionLen, pszUserName);
	::StringCchCatW(pszConnection, lConnectionLen, L";");
	::StringCchCatW(pszConnection, lConnectionLen, CPassword);
	::StringCchCatW(pszConnection, lConnectionLen, pszPassword);
	::StringCchCatW(pszConnection, lConnectionLen, L";");

    return pszConnection;
}

static VOID lFreeConnectionString(
  LPCWSTR pszConnection)
{
    lFreeMem((LPVOID)pszConnection);
}

static BOOL lAllocateODBCConnection(
  LPCWSTR pszServer,
  LPCWSTR pszPort,
  LPCWSTR pszCatalog,
  LPCWSTR pszUserName,
  LPCWSTR pszPassword,
  SQLHANDLE* phConnection)
{
    SQLHANDLE hEnvironment;
    LPCWSTR pszConnectionString;
    SQLSMALLINT nStringLen;
    SQLRETURN SqlReturnCode;

    *phConnection = NULL;

    pszConnectionString = lAllocConnectionString(pszServer, pszPort, pszCatalog, pszUserName, pszPassword);

    if (pszConnectionString == NULL)
    {
        return FALSE;
    }

    if (SQL_SUCCESS != ::SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &hEnvironment))
    {
        lFreeConnectionString(pszConnectionString);

        return FALSE;
    }

    if (SQL_SUCCESS != ::SQLSetEnvAttr(hEnvironment, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)SQL_OV_ODBC3, 0) ||
        SQL_SUCCESS != ::SQLSetEnvAttr(hEnvironment, SQL_ATTR_CONNECTION_POOLING, SQL_CP_OFF, 0))
    {
        ::SQLFreeHandle(SQL_HANDLE_ENV, hEnvironment);

        lFreeConnectionString(pszConnectionString);

        return FALSE;
    }

    if (SQL_SUCCESS != ::SQLAllocHandle(SQL_HANDLE_DBC, hEnvironment, phConnection))
    {
        ::SQLFreeHandle(SQL_HANDLE_ENV, hEnvironment);

        lFreeConnectionString(pszConnectionString);

        return FALSE;
    }

    SqlReturnCode = ::SQLDriverConnect(*phConnection, NULL, (SQLWCHAR*)pszConnectionString,
                                       SQL_NTS, NULL, 0, &nStringLen, SQL_DRIVER_NOPROMPT);

    if (SqlReturnCode != SQL_SUCCESS && SqlReturnCode != SQL_SUCCESS_WITH_INFO)
    {
        ::SQLFreeHandle(SQL_HANDLE_DBC, *phConnection);
        ::SQLFreeHandle(SQL_HANDLE_ENV, hEnvironment);

        lFreeConnectionString(pszConnectionString);

        *phConnection = NULL;

        return FALSE;
    }

    ::SQLFreeHandle(SQL_HANDLE_ENV, hEnvironment);

    lFreeConnectionString(pszConnectionString);

    return TRUE;
}

static BOOL lFreeODBCConnection(
  SQLHANDLE hConnection)
{
    ::SQLDisconnect(hConnection);

    ::SQLFreeHandle(SQL_HANDLE_DBC, hConnection);

    return TRUE;
}

static BOOL lAllocateODBCStatement(
  SQLHANDLE hConnection,
  LPCWSTR pszStatement,
  SQLHANDLE* phStatement)
{
    BOOL bResult(FALSE);

    *phStatement = NULL;

    if (SQL_SUCCESS != ::SQLAllocHandle(SQL_HANDLE_STMT, hConnection, phStatement))
    {
        return FALSE;
    }

    switch (::SQLExecDirect(*phStatement, (SQLWCHAR*)pszStatement, SQL_NTS))
    {
        case SQL_SUCCESS:
        case SQL_SUCCESS_WITH_INFO:
            bResult = TRUE;
            break;
        default:
            ::SQLFreeHandle(SQL_HANDLE_STMT, *phStatement);

            *phStatement = NULL;
            break;
    }

    return bResult;
}

static BOOL lFreeODBCStatement(
  SQLHANDLE hStatement)
{
    ::SQLCancel(hStatement);
    ::SQLFreeHandle(SQL_HANDLE_STMT, hStatement);

    return TRUE;
}

static BOOL lGetDatabaseSnapshotIsolation(
  SQLHANDLE hStatement,
  LPBOOL pbSupported)
{
    BOOL bQuitLoop(FALSE);
    SQLRETURN SqlReturnCode;
    SQLCHAR cName[50], cValue[50];

    *pbSupported = FALSE;

    if (SQL_SUCCESS != ::SQLBindCol(hStatement, 1, SQL_C_WCHAR, cName, sizeof(cName), NULL) ||
        SQL_SUCCESS != ::SQLBindCol(hStatement, 2, SQL_C_WCHAR, cValue, sizeof(cValue), NULL))
    {
        ::SQLFreeStmt(hStatement, SQL_UNBIND);

        return FALSE;
    }

    while (bQuitLoop == FALSE)
    {
        SqlReturnCode = ::SQLFetch(hStatement);

        if (SqlReturnCode == SQL_SUCCESS || SqlReturnCode == SQL_SUCCESS_WITH_INFO)
        {
            if (0 == ::lstrcmpi((LPTSTR)cName, TEXT("isolation level")) &&
                0 == ::lstrcmpi((LPTSTR)cValue, TEXT("read committed snapshot")))
            {
                *pbSupported = TRUE;
            }
        }
        else
        {
            bQuitLoop = TRUE;
        }
    }

    ::SQLFreeStmt(hStatement, SQL_UNBIND);

    return TRUE;
}

static BOOL lGetDatabaseTableSchema(
  SQLHANDLE hStatement,
  TTableColumnData** ppTableColumnData,
  LPINT pnTableColumnDataLen)
{
    BOOL bQuitLoop(FALSE);
    SQLRETURN SqlReturnCode;
    SQLCHAR cColumnName[50], cTypeName[50];
    SQLINTEGER nMaxLen;
    INT nColumnNameLen, nTypeNameLen;
    TTableColumnData* pCurTableColumnData;

    *ppTableColumnData = NULL;
    *pnTableColumnDataLen = 0;

    if (SQL_SUCCESS != ::SQLBindCol(hStatement, 1, SQL_C_WCHAR, cColumnName, sizeof(cColumnName), NULL) ||
        SQL_SUCCESS != ::SQLBindCol(hStatement, 2, SQL_C_WCHAR, cTypeName, sizeof(cTypeName), NULL) ||
        SQL_SUCCESS != ::SQLBindCol(hStatement, 3, SQL_C_LONG, &nMaxLen, sizeof(nMaxLen), NULL))
    {
        ::SQLFreeStmt(hStatement, SQL_UNBIND);

        return FALSE;
    }

    while (bQuitLoop == FALSE)
    {
        SqlReturnCode = ::SQLFetch(hStatement);

        if (SqlReturnCode == SQL_SUCCESS || SqlReturnCode == SQL_SUCCESS_WITH_INFO)
        {
            nColumnNameLen = ::lstrlen((LPTSTR)cColumnName) + 1;
            nTypeNameLen = ::lstrlen((LPTSTR)cTypeName) + 1;

            if (*pnTableColumnDataLen > 0)
            {
                *ppTableColumnData = (TTableColumnData*)lReAllocMem(*ppTableColumnData, 
                                                                    (*pnTableColumnDataLen + 1) * sizeof(TTableColumnData));
            }
            else
            {
                *ppTableColumnData = (TTableColumnData*)lAllocMem(sizeof(TTableColumnData));
            }

            pCurTableColumnData = &(*ppTableColumnData)[*pnTableColumnDataLen];

            pCurTableColumnData->pszColumnName = (LPTSTR)lAllocMem(nColumnNameLen * sizeof(TCHAR));

            ::StringCchCopy(pCurTableColumnData->pszColumnName, nColumnNameLen, (LPTSTR)cColumnName);

            if (nMaxLen != -1)
            {
                pCurTableColumnData->ulColumnLength = nMaxLen;
            }
            else
            {
                pCurTableColumnData->ulColumnLength = INT_MAX;
            }

            if (0 == ::lstrcmpi((LPTSTR)cTypeName, TEXT("int")))
            {
                pCurTableColumnData->ColumnType = ectInteger;
            }
            else if (0 == ::lstrcmpi((LPTSTR)cTypeName, TEXT("nvarchar")))
            {
                pCurTableColumnData->ColumnType = ectCharacter;
            }
            else if (0 == ::lstrcmpi((LPTSTR)cTypeName, TEXT("bit")))
            {
                pCurTableColumnData->ColumnType = ectBoolean;
            }

            *pnTableColumnDataLen += 1;
        }
        else
        {
            bQuitLoop = TRUE;
        }
    }

    ::SQLFreeStmt(hStatement, SQL_UNBIND);

    return TRUE;
}

#pragma managed

System::String^ Common::Data::SQLServerOdbc::BuildConnectionString(
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

System::Boolean Common::Data::SQLServerOdbc::GetDatabaseTableSchema(
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
    SQLHANDLE hConnection, hStatement;
    LPTSTR pszCommandText;
    LONG lCommandTextLen;
    TTableColumnData* pTableColumnData;
    INT nTableColumnDataLen;
    Common::Data::DbTableColumn::EColumnType ColumnType;

    TableColumnList = nullptr;

    if (lInitHeap() == FALSE)
    {
        return false;
    }

    lCommandTextLen = ::lstrlen(CSelectTableSchemaQueryStart) +
                      ::lstrlen(pszTableName) +
                      ::lstrlen(CSelectTableSchemaQueryEnd) + 1;

    pszCommandText = (LPTSTR)lAllocMem(lCommandTextLen * sizeof(TCHAR));

    if (pszCommandText == NULL)
    {
        lInitHeap();

        return false;
    }

    ::StringCchCat(pszCommandText, lCommandTextLen, CSelectTableSchemaQueryStart);
    ::StringCchCat(pszCommandText, lCommandTextLen, pszTableName);
    ::StringCchCat(pszCommandText, lCommandTextLen, CSelectTableSchemaQueryEnd);

    TableColumnList = gcnew System::Collections::Generic::List<Common::Data::DbTableColumn^>();

    if (lAllocateODBCConnection(pszServer, pszPort, pszCatalog, pszUserName, pszPassword, &hConnection))
    {
        if (lAllocateODBCStatement(hConnection, pszCommandText, &hStatement))
        {
            if (lGetDatabaseTableSchema(hStatement, &pTableColumnData, &nTableColumnDataLen))
            {
                for (INT nIndex = 0; nIndex < nTableColumnDataLen; ++nIndex)
                {
                    switch (pTableColumnData[nIndex].ColumnType)
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
                            gcnew System::String(pTableColumnData[nIndex].pszColumnName),
                            pTableColumnData[nIndex].ulColumnLength,
                            ColumnType));

                    lFreeMem(pTableColumnData[nIndex].pszColumnName);
                }

                lFreeMem(pTableColumnData);

                bResult = true;
            }

            lFreeODBCStatement(hStatement);
        }

        lFreeODBCConnection(hConnection);
    }

    lFreeMem(pszCommandText);

    if (lUninitHeap() == FALSE)
    {
        return false;
    }

    return bResult;
}

System::Boolean Common::Data::SQLServerOdbc::GetProvideSnapshotIsolationSupported(
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
    SQLHANDLE hConnection, hStatement;
    BOOL bSupported;

    if (lInitHeap() == FALSE)
    {
        return false;
    }

    if (lAllocateODBCConnection(pszServer, pszPort, pszCatalog, pszUserName, pszPassword, &hConnection))
    {
        if (lAllocateODBCStatement(hConnection, TEXT("DBCC USEROPTIONS"), &hStatement))
        {
            if (lGetDatabaseSnapshotIsolation(hStatement, &bSupported))
            {
                bSnapshotSupported = bSupported ? true : false;

                bResult = true;
            }

            lFreeODBCStatement(hStatement);
        }

        lFreeODBCConnection(hConnection);
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
