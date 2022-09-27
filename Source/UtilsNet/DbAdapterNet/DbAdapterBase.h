/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        public ref class DbAdapterBase abstract : Common::Data::IDbAdapter
        {
        public:
            DbAdapterBase();

        protected:
            ~DbAdapterBase();

        // Common::Data::IDbAdapter interface overrides
        public:
            virtual System::Boolean Initialize(Microsoft::Win32::RegistryKey^ RegKey,
                                               Common::Data::IDbLogging^ Logging,
                                               System::String^% sErrorMessage);
            virtual System::Boolean Uninitialize(System::String^% sErrorMessage);
            virtual System::Boolean GetIdentityValue(System::Data::Common::DbCommand^ Command,
                                                     System::Int32% nIdentity,
                                                     System::String^% sErrorMessage);
            virtual System::Boolean AllocConnection(System::Data::Common::DbConnection^% Connection,
                                                    System::String^% sErrorMessage);
            virtual System::Boolean FreeConnection(System::Data::Common::DbConnection^ Connection,
                                                   System::String^% sErrorMessage);
            virtual System::Boolean GetTableSchema(System::String^ sTableName,
                                                   System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
                                                   System::String^% sErrorMessage);
            virtual System::String^ GetSQLBooleanValue(System::Boolean bValue);
            virtual System::Boolean GetSnapshotIsolationSupported(System::Boolean% bSnapshotSupported,
                                                                  System::String^% sErrorMessage);
            virtual System::Boolean GetUpdateWithParameterizedSubQuerySupported();
            virtual System::Boolean AddCommandParameter(System::Data::Common::DbCommand^ Command,
                                                        System::String^ sParameterName,
                                                        System::Object^ Value);
            virtual System::Boolean ReadSettings(Microsoft::Win32::RegistryKey^ RegKey,
                                                 System::Collections::Generic::Dictionary<System::String^, System::Object^>^% SettingsDict,
                                                 System::String^% sErrorMessage);
            virtual System::Boolean WriteSettings(Microsoft::Win32::RegistryKey^ RegKey,
                                                  System::Collections::Generic::Dictionary<System::String^, System::Object^>^ SettingsDict,
                                                  System::String^% sErrorMessage);

        protected:
            System::String^ ReadSetting(Microsoft::Win32::RegistryKey^ RegKey,
                                        System::String^ sDatabaseRegKey,
                                        System::String^ sSettingName,
                                        System::String^ sDefaultValue);
            System::UInt16 ReadSetting(Microsoft::Win32::RegistryKey^ RegKey,
                                        System::String^ sDatabaseRegKey,
                                        System::String^ sSettingName,
                                        System::UInt16 nDefaultValue);

            System::Boolean WriteSetting(Microsoft::Win32::RegistryKey^ RegKey, 
                                         System::String^ sDatabaseRegKey,
                                         System::String^ sSettingName,
                                         System::String^ sSettingValue);
            System::Boolean WriteSetting(Microsoft::Win32::RegistryKey^ RegKey,
                                         System::String^ sDatabaseRegKey,
                                         System::String^ sSettingName,
                                         System::UInt16 nSettingValue);

            System::String^ EncryptString(System::String^ sData);
            System::String^ DecryptString(System::String^ sData);

        private:
            System::Boolean Close(System::String^% sErrorMessage);

            System::Boolean CreateConnection(System::Data::Common::DbConnection^% Connection,
                                             System::String^% sErrorMessage);

             void EmptyConnectionPool(System::Collections::Generic::List<System::Data::Common::DbConnection^>^ ConnectionList);

            static void CopySchema(System::Collections::Generic::List<Common::Data::DbTableColumn^>^ SrcTableColumnArrayList,
                                   System::Collections::Generic::List<Common::Data::DbTableColumn^>^% DestTableColumnArrayList);

        private:
            Common::Data::IDbLogging^ m_Logging;

            System::Threading::Mutex^ m_ConnectionPoolMutex;
            System::Threading::Mutex^ m_TableSchemaMutex;

            ref struct TConnectionPool
            {
                System::Collections::Generic::List<System::Data::Common::DbConnection^>^ FreeList;
                System::Collections::Generic::List<System::Data::Common::DbConnection^>^ UsedList;
            };

            TConnectionPool^ m_ConnectionPool;

            System::Collections::Generic::Dictionary<System::String^, System::Collections::Generic::List<Common::Data::DbTableColumn^>^>^ m_TableSchemaDictionary;
        };
    }
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
