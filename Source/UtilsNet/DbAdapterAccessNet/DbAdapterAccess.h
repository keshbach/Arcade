/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        /// <summary>
        /// Summary for DbAdapterAccess
        /// </summary>

        public ref class DbAdapterAccess sealed :
            Common::Data::DbAdapterBase,
            Common::Data::IDbProvider
        {
        public:
            DbAdapterAccess();

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
            virtual System::Boolean ProvideSnapshotIsolationSupported(System::Boolean% bSnapshotSupported,
                                                                      System::String^% sErrorMessage);
            virtual System::Boolean ProvideUpdateWithParameterizedSubQuerySupported();
            virtual System::Data::Common::DbConnection^ ProvideConnection(void);
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
                                         System::String^% sDatabaseFile);

        private:
            System::String^ m_sDatabaseFile;
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
