/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "LoggedInUser.h"

Common::Data::LoggedInUser::LoggedInUser(
  System::String^ sComputerName,
  System::String^ sUserName,
  System::Boolean bConnected,
  System::Boolean bSuspectState)
{
    m_sComputerName = sComputerName;
    m_sUserName = sUserName;
    m_bConnected = bConnected;
    m_bSuspectState = bSuspectState;
}

Common::Data::LoggedInUser::~LoggedInUser()
{
    m_sComputerName = nullptr;
    m_sUserName = nullptr;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
