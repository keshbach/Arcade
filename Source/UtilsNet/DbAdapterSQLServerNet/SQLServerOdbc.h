/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Common
{
    namespace Data
    {
        private ref class SQLServerOdbc sealed
        {
        public:
            static System::String^ BuildConnectionString(System::String^ sServer,
                                                         System::Int32 nPort,
                                                         System::String^ sCatalog,
                                                         System::String^ sUserName,
                                                         System::String^ sPassword);
            static System::Boolean GetDatabaseTableSchema(System::String^ sServer,
                                                          System::Int32 nPort,
                                                          System::String^ sCatalog,
                                                          System::String^ sUserName,
                                                          System::String^ sPassword,
                                                          System::String^ sTableName,
                                                          System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList);
            static System::Boolean GetProvideSnapshotIsolationSupported(System::String^ sServer,
                                                                        System::Int32 nPort,
                                                                        System::String^ sCatalog,
                                                                        System::String^ sUserName,
                                                                        System::String^ sPassword,
                                                                        System::Boolean% bSnapshotSupported);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
