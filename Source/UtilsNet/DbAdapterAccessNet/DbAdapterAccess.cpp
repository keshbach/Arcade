/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbAdapterAccess.h"

#include "LoggedInUser.h"
#include "MSAccess.h"

#include <UtilsNet/Includes/UtDbAdapterMacros.h>

#pragma region "Constants"

#define CAccessRegKeyName L"Access"

#define CDatabaseRegValueName L"Database"

#pragma endregion

Common::Data::DbAdapterAccess::DbAdapterAccess()
{
}

System::Boolean Common::Data::DbAdapterAccess::InitDatabase(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^% sErrorMessage)
{
	sErrorMessage = L"";

	if (false == ReadSettings(RegKey, m_sDatabaseFile))
	{
		sErrorMessage = L"Could not read the registry settings.";

		return false;
	}

	return true;
}

System::Boolean Common::Data::DbAdapterAccess::UninitDatabase(
  System::String^% sErrorMessage)
{
    System::Boolean bResult = true;
    System::Collections::Generic::List<LoggedInUser^>^ LoggedInUserList;

	sErrorMessage = L"";

	if (MSAccess::GetLoggedInUsers(m_sDatabaseFile,
                                   LoggedInUserList) &&
        LoggedInUserList->Count == 1)
    {
        if (false == MSAccess::CompactDatabase(m_sDatabaseFile))
        {
			sErrorMessage = L"The database could not be compacted.";

			bResult = false;
        }
    }

    if (LoggedInUserList != nullptr)
    {
        delete LoggedInUserList;

        LoggedInUserList = nullptr;
    }

    delete m_sDatabaseFile;

    m_sDatabaseFile = nullptr;

	return bResult;
}

System::String^ Common::Data::DbAdapterAccess::GetIdentityQueryStatement(void)
{
    return L"SELECT @@Identity;";
}

System::String^ Common::Data::DbAdapterAccess::BuildConnectionString(
  System::String^% sErrorMessage)
{
    sErrorMessage = L"";

    return MSAccess::BuildConnectionString(m_sDatabaseFile);
}

System::Boolean Common::Data::DbAdapterAccess::ProvideTableSchema(
  System::String^ sTableName,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
  System::String^% sErrorMessage)
{
    sErrorMessage = L"";

    return MSAccess::GetDatabaseTableSchema(m_sDatabaseFile,
                                            sTableName,
                                            TableColumnList);
}

System::String^ Common::Data::DbAdapterAccess::ProvideSQLBooleanValue(
  System::Boolean bValue)
{
	return bValue ? L"Yes" : L"No";
}

System::String^ Common::Data::DbAdapterAccess::ProvideSQLSumInt32Function(
  System::String^ sColumnName)
{
    return System::String::Format(L"IIF(IsNull(SUM({0})), 0, CLNG(SUM({0})))", sColumnName);
}

System::Boolean Common::Data::DbAdapterAccess::ProvideSnapshotIsolationSupported(
  System::Boolean% bSnapshotSupported,
  System::String^% sErrorMessage)
{
    bSnapshotSupported = true;
    sErrorMessage = L"";

    return true;
}

System::Boolean Common::Data::DbAdapterAccess::ProvideUpdateWithParameterizedSubQuerySupported()
{
    return false;
}

System::Data::Common::DbConnection^ Common::Data::DbAdapterAccess::ProvideConnection(void)
{
    return gcnew System::Data::OleDb::OleDbConnection();
}

System::Boolean Common::Data::DbAdapterAccess::ProvideConnectionActive(
  System::Data::Common::DbConnection^ Connection)
{
    // Need to implement and test where the database is on a network share or a
    // removable drive and then the resource is not available.

    Connection;

    return true;
}

System::Boolean Common::Data::DbAdapterAccess::ProvideAddCommandParameter(
  System::Data::Common::DbCommand^ Command,
  System::String^ sParameterName,
  System::Object^ Value)
{
    System::Data::OleDb::OleDbCommand^ TempCommand = (System::Data::OleDb::OleDbCommand^)Command;

    TempCommand->Parameters->AddWithValue(sParameterName, Value);

    return true;
}

System::Boolean Common::Data::DbAdapterAccess::ProvideReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^% SettingsDict,
  System::String^% sErrorMessage)
{
    SettingsDict = gcnew System::Collections::Generic::Dictionary<System::String^, System::Object^>();

    sErrorMessage = "";

    MDatabaseAdapterReadDictionarySetting(SettingsDict, CAccessRegKeyName, RegKey, CDatabaseRegValueName, "");

    return true;
}

System::Boolean Common::Data::DbAdapterAccess::ProvideWriteSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict,
  System::String^% sErrorMessage)
{
    sErrorMessage = "";

    MDatabaseAdapterVerifyWriteDictionarySetting(SettingsDict, CDatabaseRegValueName, System::String, sErrorMessage)

    MDatabaseAdapterWriteDictionarySetting(SettingsDict, CAccessRegKeyName, RegKey, CDatabaseRegValueName, sErrorMessage)

    return true;
}

System::Boolean Common::Data::DbAdapterAccess::ReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^% sDatabaseFile)
{
    System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict;
    System::Collections::IDictionaryEnumerator^ DictEnum;
    System::String^ sErrorMessage;

    sDatabaseFile = L"";

    if (!ReadSettings(RegKey, SettingsDict, sErrorMessage))
    {
        return false;
    }

    DictEnum = SettingsDict->GetEnumerator();

    while (DictEnum->MoveNext())
    {
        if ((System::String^)DictEnum->Key == CDatabaseRegValueName)
        {
            sDatabaseFile = (System::String^)DictEnum->Value;
        }
    }

    return true;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
