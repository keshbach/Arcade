/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{               
    namespace Data
    {
        /// <summary>
        /// Interface to be implemented by a database adapter.
        /// </summary>
        public interface class IDbProvider
        {
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
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
