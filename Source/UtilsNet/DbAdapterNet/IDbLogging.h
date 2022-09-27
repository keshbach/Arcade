/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace Data
    {
        public interface class IDbLogging
        {
        public:
            /// <summary>
            /// Log a message from the the database.
            /// <param name="sMessage">
            /// Message to be logged.
            /// </param>
            /// </summary>

            virtual void DatabaseMessage(System::String^ sMessage);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
