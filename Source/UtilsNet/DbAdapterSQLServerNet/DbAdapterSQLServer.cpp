/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbAdapterSQLServer.h"

#include "SQLServerOdbc.h"
#include "SQLServerAdo.h"

#include <UtilsNet/Includes/UtDbAdapterMacros.h>

#pragma region "Constants"

#define CSQLServerRegKeyName L"SQLServer"

#define CServerRegValueName L"Server"
#define CPortRegValueName L"Port"
#define CCatalogRegValueName L"Catalog"
#define CUserNameRegValueName L"UserName"
#define CPasswordRegValueName L"Password"
#define CConnectionModeRegValueName L"ConnectionMode"

#define COleDbConnectionMode L"oledb"
#define CODBCConnectionMode L"odbc"

#pragma endregion

Common::Data::DbAdapterSQLServer::DbAdapterSQLServer()
{
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
    System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict;
    System::Collections::IDictionaryEnumerator^ DictEnum;
    System::String^ sErrorMessage;

	sServer = L"";
	nPort = 0;
    sCatalog = L"";
    sUserName = L"";
    sPassword = L"";
    sConnectionMode = L"";

    if (!ReadSettings(RegKey, SettingsDict, sErrorMessage))
    {
        return false;
    }

    DictEnum = SettingsDict->GetEnumerator();

    while (DictEnum->MoveNext())
    {
        if ((System::String^)DictEnum->Key == CServerRegValueName)
        {
            sServer = (System::String^)DictEnum->Value;
        }
        else if ((System::String^)DictEnum->Key == CPortRegValueName)
        {
            nPort = (System::UInt16)DictEnum->Value;
        }
        else if ((System::String^)DictEnum->Key == CCatalogRegValueName)
        {
            sCatalog = (System::String^)DictEnum->Value;
        }
        else if ((System::String^)DictEnum->Key == CUserNameRegValueName)
        {
            sUserName = (System::String^)DictEnum->Value;
        }
        else if ((System::String^)DictEnum->Key == CPasswordRegValueName)
        {
            sPassword = (System::String^)DictEnum->Value;
        }
        else if ((System::String^)DictEnum->Key == CConnectionModeRegValueName)
        {
            sConnectionMode = (System::String^)DictEnum->Value;
        }
    }

	return true;
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

System::Boolean Common::Data::DbAdapterSQLServer::ProvideReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^% SettingsDict,
  System::String^% sErrorMessage)
{
    SettingsDict = gcnew System::Collections::Generic::Dictionary<System::String^, System::Object^>();

    sErrorMessage = "";

    MDatabaseAdapterReadDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CServerRegValueName, "");
    MDatabaseAdapterReadDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CCatalogRegValueName, "");
    MDatabaseAdapterReadDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CPortRegValueName, 0);
    MDatabaseAdapterReadDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CUserNameRegValueName, "");
    MDatabaseAdapterReadEncryptedDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CPasswordRegValueName, "");
    MDatabaseAdapterReadDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CConnectionModeRegValueName, CODBCConnectionMode);

    return true;
}

System::Boolean Common::Data::DbAdapterSQLServer::ProvideWriteSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict,
  System::String^% sErrorMessage)
{
    System::String^ Value;

    sErrorMessage = "";

    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CServerRegValueName, System::String, sErrorMessage)
    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CCatalogRegValueName, System::String, sErrorMessage)
    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CPortRegValueName, System::UInt16, sErrorMessage)
    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CUserNameRegValueName, System::String, sErrorMessage)
    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CPasswordRegValueName, System::String, sErrorMessage)
    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CConnectionModeRegValueName, System::String, sErrorMessage)

    Value = (System::String^)SettingsDict[CConnectionModeRegValueName];

    if (Value != COleDbConnectionMode && Value != CODBCConnectionMode)
    {
        sErrorMessage = System::String::Format("The \"{0}\" setting must contain the value of either \"{1}\" or \"{2}\"",
                                               CConnectionModeRegValueName, COleDbConnectionMode,
                                               CODBCConnectionMode);

        return false;
    }

    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CServerRegValueName, sErrorMessage)
    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CCatalogRegValueName, sErrorMessage)
    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CPortRegValueName, sErrorMessage)
    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CUserNameRegValueName, sErrorMessage)
    MDatabaseAdapterWriteEncryptedDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CPasswordRegValueName, sErrorMessage)
    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CSQLServerRegKeyName, RegKey, CConnectionModeRegValueName, sErrorMessage)

    return true;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
