/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbAdapterSQLServer.h"

#include "SQLServerOdbc.h"
#include "SQLServerAdo.h"

#define CServerRegValueName L"Server"
#define CPortRegValueName L"Port"
#define CCatalogRegValueName L"Catalog"
#define CUserNameRegValueName L"UserName"
#define CPasswordRegValueName L"Password"
#define CConnectionModeRegValueName L"ConnectionMode"

#define COleDbConnectionMode L"oledb"
#define CODBCConnectionMode L"odbc"

Common::Data::DbAdapterSQLServer::DbAdapterSQLServer()
{
}

System::Boolean Common::Data::DbAdapterSQLServer::SaveSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sServer,
  System::Int32 nPort,
  System::String^ sCatalog,
  System::String^ sUserName,
  System::String^ sPassword,
  System::String^ sConnectionMode)
{
	System::Boolean bResult = false;
	System::String^ sTmpPassword = L"";
    System::Char nValue;

	for (System::Int32 nIndex = 0; nIndex < sPassword->Length; ++nIndex)
	{
		nValue = sPassword[nIndex] + 1;

		sTmpPassword += nValue.ToString();
	}

	try
	{
		RegKey->SetValue(CServerRegValueName, sServer,
			             Microsoft::Win32::RegistryValueKind::String);

		RegKey->SetValue(CPortRegValueName, nPort,
			             Microsoft::Win32::RegistryValueKind::DWord);

		RegKey->SetValue(CCatalogRegValueName, sCatalog,
			             Microsoft::Win32::RegistryValueKind::String);

		RegKey->SetValue(CUserNameRegValueName, sUserName,
			             Microsoft::Win32::RegistryValueKind::String);

		RegKey->SetValue(CPasswordRegValueName, sTmpPassword,
			             Microsoft::Win32::RegistryValueKind::String);

        RegKey->SetValue(CConnectionModeRegValueName, sConnectionMode,
                         Microsoft::Win32::RegistryValueKind::String);

		bResult = true;
    }
	catch (System::Exception^)
	{
	}

	return bResult;
}

System::Boolean Common::Data::DbAdapterSQLServer::ReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^% sServer,
  System::Int32% nPort,
  System::String^% sCatalog,
  System::String^% sUserName,
  System::String^% sPassword,
  System::String^% sConnectionMode)
{
	System::Boolean bResult = false;
	System::String^ sTmpPassword = L"";
    System::Char nValue;

	sServer = L"";
	nPort = 0;
    sCatalog = L"";
    sUserName = L"";
    sPassword = L"";
    sConnectionMode = L"";

	try
	{
		sServer = (System::String^)RegKey->GetValue(CServerRegValueName);
		nPort = (System::Int32)RegKey->GetValue(CPortRegValueName);
		sCatalog = (System::String^)RegKey->GetValue(CCatalogRegValueName);
		sUserName = (System::String^)RegKey->GetValue(CUserNameRegValueName);
		sTmpPassword = (System::String^)RegKey->GetValue(CPasswordRegValueName);
        sConnectionMode = (System::String^)RegKey->GetValue(CConnectionModeRegValueName);

		for (System::Int32 nIndex = 0; nIndex < sTmpPassword->Length; ++nIndex)
		{
			nValue = sTmpPassword[nIndex] - 1;

			sPassword += nValue.ToString();
		}

		bResult = true;
	}
	catch (System::Exception^)
	{
	}

    sTmpPassword = nullptr;

	return bResult;
}

System::Boolean Common::Data::DbAdapterSQLServer::InitDatabase(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^% sErrorMessage)
{
	sErrorMessage = L"";

    if (false == ReadSettings(RegKey, m_sServer, m_nPort, m_sCatalog,
                              m_sUserName, m_sPassword, m_sConnectionMode))
	{
		sErrorMessage = L"Could not read the registry settings.";

		return false;
	}

	return true;
}

System::Boolean Common::Data::DbAdapterSQLServer::UninitDatabase(
  System::String^% sErrorMessage)
{
	sErrorMessage = L"";

    m_sServer = nullptr;
    m_nPort = 0;
    m_sCatalog = nullptr;
    m_sUserName = nullptr;
    m_sPassword = nullptr;
    m_sConnectionMode = nullptr;

	return true;
}

System::String^ Common::Data::DbAdapterSQLServer::GetIdentityQueryStatement(void)
{
    if (m_sConnectionMode == COleDbConnectionMode)
    {
        return L"SELECT CAST(@@IDENTITY AS INT);";
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        return L"SELECT SCOPE_IDENTITY();";
    }

    return L"";
}

System::String^ Common::Data::DbAdapterSQLServer::BuildConnectionString(
  System::String^% sErrorMessage)
{
    sErrorMessage = L"";

    if (m_sConnectionMode == COleDbConnectionMode)
    {
        return SQLServerAdo::BuildConnectionString(m_sServer, m_nPort, m_sCatalog,
                                                   m_sUserName, m_sPassword);
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        return SQLServerOdbc::BuildConnectionString(m_sServer, m_nPort, m_sCatalog,
                                                    m_sUserName, m_sPassword);
    }

    return L"";
}

System::Boolean Common::Data::DbAdapterSQLServer::ProvideTableSchema(
  System::String^ sTableName,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
  System::String^% sErrorMessage)
{
    sErrorMessage = L"";

    if (m_sConnectionMode == COleDbConnectionMode)
    {
        return SQLServerAdo::GetDatabaseTableSchema(m_sServer, m_nPort,
                                                    m_sCatalog, m_sUserName,
                                                    m_sPassword, sTableName,
                                                    TableColumnList);
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        return SQLServerOdbc::GetDatabaseTableSchema(m_sServer, m_nPort,
                                                     m_sCatalog, m_sUserName,
                                                     m_sPassword, sTableName,
                                                     TableColumnList);
    }

    return false;
}

System::String^ Common::Data::DbAdapterSQLServer::ProvideSQLBooleanValue(
  System::Boolean bValue)
{
	return bValue ? L"1" : L"0";
}

System::Boolean Common::Data::DbAdapterSQLServer::ProvideSnapshotIsolationSupported(
  System::Boolean% bSnapshotSupported,
  System::String^% sErrorMessage)
{
    bSnapshotSupported = false;
    sErrorMessage = L"";

    if (m_sConnectionMode == COleDbConnectionMode)
    {
        return SQLServerAdo::GetProvideSnapshotIsolationSupported(m_sServer, m_nPort,
                                                                   m_sCatalog, m_sUserName,
                                                                   m_sPassword,
                                                                   bSnapshotSupported);
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        return SQLServerOdbc::GetProvideSnapshotIsolationSupported(m_sServer, m_nPort,
                                                                   m_sCatalog, m_sUserName,
                                                                   m_sPassword,
                                                                   bSnapshotSupported);
    }

    return false;
}

System::Boolean Common::Data::DbAdapterSQLServer::ProvideUpdateWithParameterizedSubQuerySupported()
{
    return true;
}

System::Data::Common::DbConnection^ Common::Data::DbAdapterSQLServer::ProvideConnection(void)
{
    if (m_sConnectionMode == COleDbConnectionMode)
    {
        return gcnew System::Data::OleDb::OleDbConnection();
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        return gcnew System::Data::Odbc::OdbcConnection();
    }

    return nullptr;
}

System::Boolean Common::Data::DbAdapterSQLServer::ProvideAddCommandParameter(
    System::Data::Common::DbCommand^ Command,
    System::String^ sParameterName,
    System::Object^ Value)
{
    System::Boolean bResult(false);

    if (m_sConnectionMode == COleDbConnectionMode)
    {
        System::Data::OleDb::OleDbCommand^ TempCommand = (System::Data::OleDb::OleDbCommand^)Command;

        TempCommand->Parameters->AddWithValue(sParameterName, Value);

        bResult = true;
    }
    else if (m_sConnectionMode == CODBCConnectionMode)
    {
        System::Data::Odbc::OdbcCommand^ TempCommand = (System::Data::Odbc::OdbcCommand^)Command;

        TempCommand->Parameters->AddWithValue(sParameterName, Value);

        bResult = true;
    }

    return bResult;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
