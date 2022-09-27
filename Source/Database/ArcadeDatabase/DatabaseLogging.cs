/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    internal sealed class DatabaseLogging :
        Common.Data.IDbLogging
    {
        #region "Member Variables"
        IArcadeDatabaseLogging m_ArcadeDatabaseLogging = null;
        #endregion

        #region "Constructors"
        private DatabaseLogging()
        {
        }

        public DatabaseLogging(IArcadeDatabaseLogging ArcadeDatabaseLogging)
        {
            m_ArcadeDatabaseLogging = ArcadeDatabaseLogging;
        }
        #endregion

        #region "Common.Data.IDbLogging"
        public void DatabaseMessage(System.String sMessage)
        {
            m_ArcadeDatabaseLogging.ArcadeDatabaseMessage(sMessage);
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
