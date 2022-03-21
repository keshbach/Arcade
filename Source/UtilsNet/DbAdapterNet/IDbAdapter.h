/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        public interface class IDbAdapter
        {
        public:
            /// <summary>
            /// Initializes the database.
            /// <param name="RegKey">
            /// Registry key to read the database settings from.
            /// </param>
            /// <param name="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean Initialize(Microsoft::Win32::RegistryKey^ RegKey,
                                               System::String^% sErrorMessage);

            /// <summary>
            /// Uninitializes the database.
            /// <param name="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean Uninitialize(System::String^% sErrorMessage);

            /// <summary>
            /// Allocates a new connection to the database.
            /// <param name="Connection">
            /// A connection to the database.
            /// </param>
            /// <param name="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean AllocConnection(System::Data::Common::DbConnection^% Connection,
                                                    System::String^% sErrorMessage);

            /// <summary>
            /// Frees an existing connection to the database.
            /// <param name="Connection">
            /// Connection to the database.
            /// </param>
            /// <param name="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean FreeConnection(System::Data::Common::DbConnection^ Connection,
                                                   System::String^% sErrorMessage);

            /// <summary>
            /// Retrieves the identifier of the last row added to the database.
            /// <param name="Command">
            /// OleDbCommand to retrieve the identifier from.
            /// </param>
            /// <param name="nIdentity">
            /// Identifier of the last row added.
            /// </param>
            /// <param name="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean GetIdentityValue(System::Data::Common::DbCommand^ Command,
                                                     System::Int32% nIdentity,
                                                     System::String^% sErrorMessage);

            /// <summary>
            /// Retrieves the schema to an existing table in the database.
            /// <param name="sTableName">
            /// Name of the table in the database.
            /// </param>
            /// <param name="TableColumnList">
            /// Array describing the columns in the table.
            /// </param>
            /// <param name ="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean GetTableSchema(System::String^ sTableName,
                                                   System::Collections::Generic::List<Common::Data::DbTableColumn^>^% TableColumnList,
                                                   System::String^% sErrorMessage);

            /// <summary>
            /// Retrieves the string representation of a boolean value for a SQL statement.
            /// <param name="bValue">
            /// Boolean value.
            /// </param>
            /// </summary>

            virtual System::String^ GetSQLBooleanValue(System::Boolean bValue);

            /// <summary>
            /// Retrieves whether the database supports snapshot isolation.
            /// <param name="bSnapshotSupported">
            /// A boolean value indicating whether snapshot isolation is available.
            /// </param>
            /// <param name ="sErrorMessage">
            /// On return will contain a message if an error occurred.
            /// </param>
            /// </summary>

            virtual System::Boolean GetSnapshotIsolationSupported(System::Boolean% bSnapshotSupported,
                                                                  System::String^% sErrorMessage);

            /// <summary>
            /// Retrieves if the database supports using an parameter in a subquery of an update statement.
            /// </summary>

            virtual System::Boolean GetUpdateWithParameterizedSubQuerySupported();

            /// <summary>
            /// Adds a parameter to a database command.
            /// <param name="Command">
            /// A database command object to add a parameter to.
            /// </param>
            /// <param name ="sParameterName">
            /// The name of the parameter.
            /// </param>
            /// <param name ="Value">
            /// The value of the parameter.
            /// </param>
            /// </summary>

            virtual System::Boolean AddCommandParameter(System::Data::Common::DbCommand^ Command,
                                                        System::String^ sParameterName,
                                                        System::Object^ Value);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
