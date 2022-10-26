/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        /// <summary>
        /// Summary for DbAdapterSQLServer
        /// </summary>

        public ref class DbAdapterSQLServer sealed :
            Common::Data::DbAdapterBase,
            Common::Data::IDbProvider
        {
        public:
            DbAdapterSQLServer();

            // Common::Data::IDbProvider overrides
        public:
            virtual System::Boolean InitDatabase(Microsoft::Win32::RegistryKey^ RegKey,
                                                 System::String^% sErrorMessage);
            virtual System::Boolean UninitDatabase(System::String^% sErrorMessage);
            virtual System::String^ GetIdentityQueryStatement(void);
            virtual System::String^ BuildConnectionString(System::String^% sErrorMessage);
            virtual System::Boolean ProvideTableSchema(System::String^ sTableName,
                                                       System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
                                                       System::String^% sErrorMessage);
            virtual System::String^ ProvideSQLBooleanValue(System::Boolean bValue);
            virtual System::String^ ProvideSQLSumInt32Function(System::String^ sColumnName);
            virtual System::Boolean ProvideSnapshotIsolationSupported(System::Boolean% bSnapshotSupported,
                                                                      System::String^% sErrorMessage);
            virtual System::Boolean ProvideUpdateWithParameterizedSubQuerySupported();
            virtual System::Data::Common::DbConnection^ ProvideConnection(void);
            virtual System::Boolean ProvideConnectionActive(System::Data::Common::DbConnection^ Connection);
            virtual System::Boolean ProvideAddCommandParameter(System::Data::Common::DbCommand^ Command,
                                                               System::String^ sParameterName,
                                                               System::Object^ Value);
            virtual System::Boolean ProvideReadSettings(Microsoft::Win32::RegistryKey^ RegKey,
                                                        System::Collections::Generic::Dictionary<System::String^, System::Object^>^% SettingsDict,
                                                        System::String^% sErrorMessage);
            virtual System::Boolean ProvideWriteSettings(Microsoft::Win32::RegistryKey^ RegKey,
                                                         System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict,
                                                         System::String^% sErrorMessage);

        private:
            System::Boolean ReadSettings(Microsoft::Win32::RegistryKey^ RegKey,
                                         System::String^% sServer,
                                         System::Int32% nPort,
                                         System::String^% sCatalog,
                                         System::String^% sUserName,
                                         System::String^% sPassword,
                                         System::String^% sConnectionMode);

        private:
            System::String^ m_sServer;
            System::Int32 m_nPort;
            System::String^ m_sCatalog;
            System::String^ m_sUserName;
            System::String^ m_sPassword;
            System::String^ m_sConnectionMode;
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
