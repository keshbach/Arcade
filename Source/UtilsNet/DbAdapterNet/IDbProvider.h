/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{               
    namespace Data
    {
        /// <summary>
        /// Interface not to be implemented.
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
            virtual System::Boolean ProvideSnapshotIsolationSupported(System::Boolean% bSnapshotSupported,
                                                                      System::String^% sErrorMessage);
            virtual System::Boolean ProvideUpdateWithParameterizedSubQuerySupported();
            virtual System::Data::Common::DbConnection^ ProvideConnection(void);
            virtual System::Boolean ProvideAddCommandParameter(System::Data::Common::DbCommand^ Command,
                                                               System::String^ sParameterName,
                                                               System::Object^ Value);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
