/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbAdapterAccess.h"

#include "LoggedInUser.h"
#include "MSAccess.h"

#define CDatabaseRegValueName L"Database"

Common::Data::DbAdapterAccess::DbAdapterAccess()
{
}

System::Boolean Common::Data::DbAdapterAccess::SaveSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sDatabaseFile)
{
	System::Boolean bResult = false;

	try
	{
		RegKey->SetValue(CDatabaseRegValueName, sDatabaseFile,
			             Microsoft::Win32::RegistryValueKind::String);

		bResult = true;
	}
	catch (System::Exception^)
	{
	}

	return bResult;
}

System::Boolean Common::Data::DbAdapterAccess::ReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^% sDatabaseFile)
{
	System::Boolean bResult = false;

	sDatabaseFile = L"";

	try
	{
		sDatabaseFile = (System::String^)RegKey->GetValue(CDatabaseRegValueName);

		bResult = true;
	}
	catch (System::Exception^)
	{
	}

	return bResult;
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

System::Boolean Common::Data::DbAdapterAccess::ProvideAddCommandParameter(
  System::Data::Common::DbCommand^ Command,
  System::String^ sParameterName,
  System::Object^ Value)
{
    System::Data::OleDb::OleDbCommand^ TempCommand = (System::Data::OleDb::OleDbCommand^)Command;

    TempCommand->Parameters->AddWithValue(sParameterName, Value);

    return true;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
