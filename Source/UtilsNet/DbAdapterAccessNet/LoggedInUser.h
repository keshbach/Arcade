/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        /// <summary>
        /// Summary for LoggedInUser
        /// </summary>

        private ref class LoggedInUser sealed
        {
        public:
            property System::String^ ComputerName
            {
                System::String^ get()
                {
                    return m_sComputerName;
                }
            }

            property System::String^ UserName
            {
                System::String^ get()
                {
                    return m_sUserName;
                }
            }

            property System::Boolean Connected
            {
                System::Boolean get()
                {
                    return m_bConnected;
                }
            }

            property System::Boolean SuspectState
            {
                System::Boolean get()
                {
                    return m_bSuspectState;
                }
            }

        public:
            LoggedInUser(System::String^ sComputerName,
                         System::String^ sUserName,
                         System::Boolean bConnected,
                         System::Boolean bSuspectState);

        protected:
            ~LoggedInUser();

        protected:
            System::String^ m_sComputerName;
            System::String^ m_sUserName;
            System::Boolean m_bConnected;
            System::Boolean m_bSuspectState;

        private:
            LoggedInUser() {}
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
