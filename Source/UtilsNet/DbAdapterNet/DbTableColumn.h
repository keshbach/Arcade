/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        /// <summary>
        /// Summary for DbTableColumn
        /// </summary>

        public ref class DbTableColumn sealed
        {
        public:
            enum class EColumnType
            {
                Unknown,
                Character,
                Integer,
                DateTime,
                Boolean
            };

        public:
            property System::String^ ColumnName
            {
                System::String^ get()
                {
                    return m_sColumnName;
                }
            }

            property System::Int32 ColumnLength
            {
                System::Int32 get()
                {
                    return m_nColumnLength;
                }
            }

            property EColumnType ColumnType
            {
                EColumnType get()
                {
                    return m_ColumnType;
                }
            }

        public:
            DbTableColumn(System::String^ sColumnName,
                          System::Int32 nColumnLength,
                          EColumnType ColumnType);

        protected:
            ~DbTableColumn();

        protected:
            System::String^ m_sColumnName;
            System::Int32 m_nColumnLength;
            EColumnType m_ColumnType;

        private:
            DbTableColumn();
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
