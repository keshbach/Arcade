/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbTableColumn.h"
#include "IDbLogging.h"
#include "IDbAdapter.h"
#include "IDbProvider.h"

#include "DbAdapterBase.h"

#include <UtilsNet/Includes/UtDbAdapterMacros.h>

#define CMaxConnectionsAllowed 4

static array<System::Byte>^ lGetEncryptionKey()
{
    array<System::Byte>^ Key = {0x06, 0x66, 0xba, 0x9a, 0xad, 0x7c, 0xf2, 0x72,
                                0x3d, 0xab, 0x34, 0xc3, 0xb2, 0xa1, 0x57, 0x0f};

    return Key;
}

static array<System::Byte>^ lGetEncryptionIV() 
{
    array<System::Byte>^ IV = {0x09, 0x07, 0x12, 0x8a, 0x76, 0x69, 0xda, 0x82,
                               0x1a, 0xe3, 0xbe, 0xcd, 0x02, 0x9a, 0x3c, 0xbb};

    return IV;
}

Common::Data::DbAdapterBase::DbAdapterBase()
{
    m_ConnectionPoolMutex = gcnew System::Threading::Mutex(false);
    m_TableSchemaMutex = gcnew System::Threading::Mutex(false);

    m_ConnectionPool = gcnew TConnectionPool();

    m_ConnectionPool->FreeList = gcnew System::Collections::Generic::List<System::Data::Common::DbConnection^>();
    m_ConnectionPool->UsedList = gcnew System::Collections::Generic::List<System::Data::Common::DbConnection^>();

    m_TableSchemaDictionary = gcnew System::Collections::Generic::Dictionary<System::String^, System::Collections::Generic::List<Common::Data::DbTableColumn^>^>();
}

Common::Data::DbAdapterBase::~DbAdapterBase()
{
    System::String^ sErrorMessage = nullptr;

    Close(sErrorMessage);
}

System::Boolean Common::Data::DbAdapterBase::Initialize(
  Microsoft::Win32::RegistryKey^ RegKey,
  Common::Data::IDbLogging^ Logging,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    m_Logging = Logging;

    m_Logging->DatabaseMessage("Initializing the database adapter");

    return pProvider->InitDatabase(RegKey, sErrorMessage);
}

System::Boolean Common::Data::DbAdapterBase::Uninitialize(
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult;

    m_Logging->DatabaseMessage("Uninitializing the database adapter");

    bResult = Close(sErrorMessage);

    if (false == pProvider->UninitDatabase(sErrorMessage))
    {
	    bResult = false;
    }

    m_Logging = nullptr;

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::GetIdentityValue(
  System::Data::Common::DbCommand^ Command,
  System::Int32% nIdentity,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult = false;
    System::Data::Common::DbDataReader^ DataReader = nullptr;

    nIdentity = -1;
    sErrorMessage = L"";

    try
    {
        Command->CommandText = pProvider->GetIdentityQueryStatement();

        DataReader = Command->ExecuteReader();

        if (DataReader->Read())
        {
            if (!DataReader->IsDBNull(0))
            {
                if (DataReader->GetDataTypeName(0) == L"DBTYPE_I4")
                {
                    nIdentity = DataReader->GetInt32(0);
                    bResult = true;
                }
                else if (DataReader->GetDataTypeName(0) == L"numeric")
                {
                    System::Decimal dValue = DataReader->GetDecimal(0);

                    nIdentity = System::Convert::ToInt32(dValue);
                    bResult = true;
                }
                else
                {
                    sErrorMessage = L"Unknown database data type of \"";
                    sErrorMessage += DataReader->GetDataTypeName(0);
                    sErrorMessage += L"\" found when trying to retrieve the identity value.";
                }
            }
            else
            {
                sErrorMessage = L"The identify value is null.";
            }
        }

        DataReader->Close();
    }

    catch (System::Data::Common::DbException^ Exception)
    {
        try
        {
            if (DataReader != nullptr)
            {
                DataReader->Close();
            }
        }
        catch (System::Data::Common::DbException^ Exception)
        {
            m_Logging->DatabaseMessage(System::String::Format(L"Exception in GetIdentityValue while closing the Data Reader ({0})", Exception->Message));
        }

        sErrorMessage = Exception->Message;
    }

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::AllocConnection(
  System::Data::Common::DbConnection^% Connection,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult = false;
    System::Data::Common::DbConnection^ TmpConnection = nullptr;
    System::String^ sTmpLogMessage;

    Connection = nullptr;
    sErrorMessage = L"";

    m_ConnectionPoolMutex->WaitOne();

    while (m_ConnectionPool->FreeList->Count > 0)
    {
        TmpConnection = m_ConnectionPool->FreeList[0];

        m_ConnectionPool->FreeList->RemoveAt(0);

        if (pProvider->ProvideConnectionActive(TmpConnection))
        {
            Connection = TmpConnection;

            m_ConnectionPool->UsedList->Add(Connection);

            m_ConnectionPoolMutex->ReleaseMutex();

            return true;
        }

        TmpConnection->Close();
    }

    if (CreateConnection(TmpConnection, sErrorMessage))
    {
        Connection = TmpConnection;

        m_ConnectionPool->UsedList->Add(Connection);

        if (m_ConnectionPool->FreeList->Count + m_ConnectionPool->UsedList->Count <= CMaxConnectionsAllowed)
        {
            sTmpLogMessage = nullptr;
        }
        else
        {
            sTmpLogMessage = L"Warning: Maximum database connections reached.";
        }

        m_ConnectionPoolMutex->ReleaseMutex();

        if (sTmpLogMessage != nullptr)
        {
            m_Logging->DatabaseMessage(sTmpLogMessage);
        }

        bResult = true;
    }
    else
    {
        m_ConnectionPoolMutex->ReleaseMutex();

        sErrorMessage = L"Could not allocate a database connection.";
    }

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::FreeConnection(
  System::Data::Common::DbConnection^ Connection,
  System::String^% sErrorMessage)
{
    sErrorMessage = L"";

    m_ConnectionPoolMutex->WaitOne();

    m_ConnectionPool->UsedList->Remove(Connection);
    m_ConnectionPool->FreeList->Add(Connection);

    m_ConnectionPoolMutex->ReleaseMutex();

    return true;
}

System::Boolean Common::Data::DbAdapterBase::GetTableSchema(
  System::String^ sTableName,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult = false;
    System::Collections::Generic::List<Common::Data::DbTableColumn^>^ TmpTableColumnList = nullptr;

    TableColumnList = nullptr;
    sErrorMessage = L"";

    m_TableSchemaMutex->WaitOne();

    if (m_TableSchemaDictionary->ContainsKey(sTableName) == false)
    {
        if (pProvider->ProvideTableSchema(sTableName, TmpTableColumnList,
                                          sErrorMessage))
        {
            m_TableSchemaDictionary->Add(sTableName, TmpTableColumnList);

            bResult = true;
        }
    }
    else
    {
        bResult = true;
    }

    if (bResult)
    {
        CopySchema(m_TableSchemaDictionary[sTableName], TableColumnList);
    }

    m_TableSchemaMutex->ReleaseMutex();

    return bResult;
}

System::String^ Common::Data::DbAdapterBase::GetSQLBooleanValue(
  System::Boolean bValue)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideSQLBooleanValue(bValue);
}

System::String^ Common::Data::DbAdapterBase::GetSQLSumInt32Function(
  System::String^ sColumnName)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideSQLSumInt32Function(sColumnName);
}

System::Boolean Common::Data::DbAdapterBase::GetSnapshotIsolationSupported(
  System::Boolean% bSnapshotSupported,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    bSnapshotSupported = false;
    sErrorMessage = L"";

    return pProvider->ProvideSnapshotIsolationSupported(bSnapshotSupported,
                                                        sErrorMessage);
}

System::Boolean Common::Data::DbAdapterBase::GetUpdateWithParameterizedSubQuerySupported()
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideUpdateWithParameterizedSubQuerySupported();
}

System::Boolean Common::Data::DbAdapterBase::AddCommandParameter(
  System::Data::Common::DbCommand^ Command,
  System::String^ sParameterName,
  System::Object^ Value)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideAddCommandParameter(Command, sParameterName, Value);
}

System::Boolean Common::Data::DbAdapterBase::ReadSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^% SettingsDict,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideReadSettings(RegKey, SettingsDict, sErrorMessage);
}

System::Boolean Common::Data::DbAdapterBase::WriteSettings(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->ProvideWriteSettings(RegKey, SettingsDict, sErrorMessage);
}

System::String^ Common::Data::DbAdapterBase::ReadSetting(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sDatabaseRegKey,
  System::String^ sSettingName,
  System::String^ sDefaultValue)
{
    Microsoft::Win32::RegistryKey^ DatabaseRegKey = RegKey->OpenSubKey(sDatabaseRegKey, false);
    System::Object^ value;

    if (DatabaseRegKey == nullptr)
    {
        return sDefaultValue;
    }

    value = DatabaseRegKey->GetValue(sSettingName, sDefaultValue);

    DatabaseRegKey->Close();

    if (value->GetType() == System::String::typeid)
    {
        return (System::String^)value;
    }

    return sDefaultValue;
}

System::UInt16 Common::Data::DbAdapterBase::ReadSetting(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sDatabaseRegKey,
  System::String^ sSettingName,
  System::UInt16 nDefaultValue)
{
    Microsoft::Win32::RegistryKey^ DatabaseRegKey = RegKey->OpenSubKey(sDatabaseRegKey, false);
    System::Object^ value;

    if (DatabaseRegKey == nullptr)
    {
        return nDefaultValue;
    }

    value = DatabaseRegKey->GetValue(sSettingName, nDefaultValue);

    DatabaseRegKey->Close();

    if (value->GetType() == System::Int32::typeid)
    {
        return System::Convert::ToUInt16((System::Int32)value);
    }

    return nDefaultValue;
}

System::Boolean Common::Data::DbAdapterBase::WriteSetting(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sDatabaseRegKey,
  System::String^ sSettingName,
  System::String^ sSettingValue)
{
    System::Boolean bResult = true;
    Microsoft::Win32::RegistryKey^ DatabaseRegKey = RegKey->CreateSubKey(sDatabaseRegKey);

    if (DatabaseRegKey == nullptr)
    {
        return false;
    }

    try
    {
        DatabaseRegKey->SetValue(sSettingName, sSettingValue, Microsoft::Win32::RegistryValueKind::String);
    }
    catch (System::Exception^ Exception)
    {
        m_Logging->DatabaseMessage(System::String::Format(L"WriteSetting exception: {0}", Exception->Message));

        bResult = false;
    }

    DatabaseRegKey->Close();

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::WriteSetting(
  Microsoft::Win32::RegistryKey^ RegKey,
  System::String^ sDatabaseRegKey,
  System::String^ sSettingName,
  System::UInt16 nSettingValue)
{
    System::Boolean bResult = true;
    Microsoft::Win32::RegistryKey^ DatabaseRegKey = RegKey->CreateSubKey(sDatabaseRegKey);

    if (DatabaseRegKey == nullptr)
    {
        return false;
    }

    try
    {
        DatabaseRegKey->SetValue(sSettingName, nSettingValue, Microsoft::Win32::RegistryValueKind::DWord);
    }
    catch (System::Exception^ Exception)
    {
        m_Logging->DatabaseMessage(System::String::Format(L"WriteSetting exception: {0}", Exception->Message));

        bResult = false;
    }

    DatabaseRegKey->Close();

    return bResult;
}

System::String^ Common::Data::DbAdapterBase::EncryptString(
  System::String^ sData)
{
    array<System::Byte>^ Data = System::Text::UTF8Encoding::UTF8->GetBytes(sData);
    System::Security::Cryptography::Aes^ Aes = nullptr;
    System::IO::MemoryStream^ MemoryStream = nullptr;
    System::Security::Cryptography::CryptoStream^ CryptoStream = nullptr;
    System::String^ sEncryptedData = nullptr;

    try
    {
        Aes = System::Security::Cryptography::Aes::Create();

        Aes->Key = lGetEncryptionKey();
        Aes->IV = lGetEncryptionIV();

        MemoryStream = gcnew System::IO::MemoryStream();

        CryptoStream = gcnew System::Security::Cryptography::CryptoStream(MemoryStream,
                                                                          Aes->CreateEncryptor(),
                                                                          System::Security::Cryptography::CryptoStreamMode::Write);

        CryptoStream->Write(Data, 0, Data->Length);
        CryptoStream->FlushFinalBlock();

        sEncryptedData = System::Convert::ToBase64String(MemoryStream->ToArray());
    }
    catch (System::Exception^ Exception)
    {
        m_Logging->DatabaseMessage(System::String::Format(L"EncryptString exception: {0}", Exception->Message));
    }

    if (Aes != nullptr)
    {
        delete Aes;
    }

    if (MemoryStream != nullptr)
    {
        MemoryStream->Close();

        delete MemoryStream;
    }

    if (CryptoStream != nullptr)
    {
        CryptoStream->Close();

        delete CryptoStream;
    }

    return sEncryptedData;
}

System::String^ Common::Data::DbAdapterBase::DecryptString(
  System::String^ sData)
{
    array<System::Byte>^ Data = System::Convert::FromBase64String(sData);
    System::Security::Cryptography::Aes^ Aes = nullptr;
    System::IO::MemoryStream^ MemoryStream = nullptr;
    System::Security::Cryptography::CryptoStream^ CryptoStream = nullptr;
    System::String^ sDecryptedData = nullptr;

    try
    {
        Aes = System::Security::Cryptography::Aes::Create();

        Aes->Key = lGetEncryptionKey();
        Aes->IV = lGetEncryptionIV();

        MemoryStream = gcnew System::IO::MemoryStream();

        CryptoStream = gcnew System::Security::Cryptography::CryptoStream(MemoryStream,
                                                                          Aes->CreateDecryptor(),
                                                                          System::Security::Cryptography::CryptoStreamMode::Write);
        CryptoStream->Write(Data, 0, Data->Length);
        CryptoStream->FlushFinalBlock();

        sDecryptedData = System::Text::Encoding::UTF8->GetString(MemoryStream->ToArray());
    }
    catch (System::Exception^ Exception)
    {
        m_Logging->DatabaseMessage(System::String::Format(L"DecryptString exception: {0}", Exception->Message));
    }

    if (Aes != nullptr)
    {
        delete Aes;
    }

    if (MemoryStream != nullptr)
    {
        MemoryStream->Close();

        delete MemoryStream;
    }

    if (CryptoStream != nullptr)
    {
        CryptoStream->Close();

        delete CryptoStream;
    }

    return sDecryptedData;
}

System::Boolean Common::Data::DbAdapterBase::Close(
  System::String^% sErrorMessage)
{
    System::Boolean bResult = false;

    sErrorMessage = L"";

    if (m_ConnectionPool != nullptr)
    {
        delete m_TableSchemaDictionary;
        
        m_TableSchemaDictionary = nullptr;

        if (m_ConnectionPool->UsedList->Count != 0)
        {
            sErrorMessage = L"Un-released database connections found.";
        }

        EmptyConnectionPool(m_ConnectionPool->UsedList);
        EmptyConnectionPool(m_ConnectionPool->FreeList);

        delete m_ConnectionPool;

        m_ConnectionPool = nullptr;

        m_TableSchemaMutex->Close();

        delete m_TableSchemaMutex;

        m_TableSchemaMutex = nullptr;

        m_ConnectionPoolMutex->Close();

        delete m_ConnectionPoolMutex;

        m_ConnectionPoolMutex = nullptr;

        if (sErrorMessage->Length == 0)
        {
            bResult = true;
        }
    }
    else
    {
        sErrorMessage = L"Database was never initialized.";
    }

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::CreateConnection(
  System::Data::Common::DbConnection^% Connection,
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult = false;
    System::String^ sConnectionString = nullptr;

    Connection = nullptr;
    sErrorMessage = gcnew System::String(L"");

    sConnectionString = pProvider->BuildConnectionString(sErrorMessage);

    if (sConnectionString == nullptr)
    {
        return false;
    }

    Connection = pProvider->ProvideConnection();

    Connection->ConnectionString = sConnectionString;

    try
    {
        Connection->Open();

        bResult = true;
    }

    catch (System::Data::Common::DbException^ Exception)
    {
        Connection = nullptr;
        sErrorMessage = Exception->Message;
    }

    catch (System::Exception^ Exception)
    {
        Connection = nullptr;
        sErrorMessage = Exception->Message;
    }

    return bResult;
}

void Common::Data::DbAdapterBase::EmptyConnectionPool(
  System::Collections::Generic::List<System::Data::Common::DbConnection^>^ ConnectionList)
{
    for each (System::Data::Common::DbConnection^ Connection in ConnectionList)
    {
		try
		{
			Connection->Close();
        }
        catch (System::Exception^ Exception)
        {
            m_Logging->DatabaseMessage(System::String::Format(L"EmptyConnectionPool exception: {0}", Exception->Message));
        }

        delete Connection;
    }

    ConnectionList->Clear();
}

void Common::Data::DbAdapterBase::CopySchema(
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^ SrcTableColumnList,
  System::Collections::Generic::List<Common::Data::DbTableColumn^>^% DestTableColumnList)
{
    DestTableColumnList = gcnew System::Collections::Generic::List<Common::Data::DbTableColumn^>();

    for each (Common::Data::DbTableColumn^ TableColumn in SrcTableColumnList)
    {
        DestTableColumnList->Add(
            gcnew Common::Data::DbTableColumn(TableColumn->ColumnName,
                                              TableColumn->ColumnLength,
                                              TableColumn->ColumnType));
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
