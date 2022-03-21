/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2007-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Common
{
    namespace Data
    {
        /// <summary>
        /// Summary for MSAccess
        /// </summary>

        private ref class MSAccess sealed
        {
        public:
            static System::String^ BuildConnectionString(System::String^ sDatabaseFile);
            static System::Boolean GetLoggedInUsers(System::String^ sDatabaseFile,
                                                    System::Collections::Generic::List<LoggedInUser^>^% LoggedInUserList);
            static System::Boolean CompactDatabase(System::String^ sDatabaseFile);
            static System::Boolean GetDatabaseTableSchema(System::String^ sDatabaseFile,
                                                          System::String^ sTableName,
                                                          System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2007-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
