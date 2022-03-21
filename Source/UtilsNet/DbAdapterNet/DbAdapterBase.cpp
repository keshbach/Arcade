/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbTableColumn.h"
#include "IDbAdapter.h"
#include "IDbProvider.h"

#include "DbAdapterBase.h"

#define CMaxConnectionsAllowed 4

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
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;

    return pProvider->InitDatabase(RegKey, sErrorMessage);
}

System::Boolean Common::Data::DbAdapterBase::Uninitialize(
  System::String^% sErrorMessage)
{
    Common::Data::IDbProvider^ pProvider = (Common::Data::IDbProvider^)this;
    System::Boolean bResult;

    bResult = Close(sErrorMessage);

    if (false == pProvider->UninitDatabase(sErrorMessage))
    {
	    bResult = false;
    }

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
                if (DataReader->GetDataTypeName(0) == L"numeric")
                {
                    System::Decimal dValue = DataReader->GetDecimal(0);

                    nIdentity = (System::Int32)DataReader->GetDecimal(0);
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
        catch (System::Data::Common::DbException^)
        {
        }

        sErrorMessage = Exception->Message;
    }

    return bResult;
}

System::Boolean Common::Data::DbAdapterBase::AllocConnection(
  System::Data::Common::DbConnection^% Connection,
  System::String^% sErrorMessage)
{
    System::Boolean bResult = false;
    System::Data::Common::DbConnection^ TmpConnection = nullptr;

    Connection = nullptr;
    sErrorMessage = L"";

    m_ConnectionPoolMutex->WaitOne();

    if (m_ConnectionPool->FreeList->Count == 0)
    {
        if (CreateConnection(TmpConnection, sErrorMessage))
        {
            m_ConnectionPool->FreeList->Add(TmpConnection);

            bResult = true;

            System::Diagnostics::Debug::Assert(m_ConnectionPool->FreeList->Count + m_ConnectionPool->UsedList->Count <= CMaxConnectionsAllowed);
        }
    }
    else
    {
        bResult = true;
    }

    if (bResult)
    {
        TmpConnection = (System::Data::Common::DbConnection^)m_ConnectionPool->FreeList[0];

        if (TmpConnection->State == System::Data::ConnectionState::Closed)
        {
            // If the connection is closed try to re-open it because
            // the connection could have been temporarily severed and
            // forcing the user to re-open the application is very
            // painful.

            try
            {
                TmpConnection->Open();
            }
            catch (System::Data::Common::DbException^ Exception)
            {
                TmpConnection = nullptr;

                sErrorMessage = Exception->Message;

                bResult = false;
            }
        }

        if (bResult == true)
        {
            Connection = TmpConnection;

            m_ConnectionPool->FreeList->RemoveAt(0);
            m_ConnectionPool->UsedList->Add(Connection);
        }
    }

    m_ConnectionPoolMutex->ReleaseMutex();

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
        catch (System::Exception^)
        {
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
//  Copyright (C) 2014-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
