/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include <Hosts/ArcadeAppHostData.h>

#include "ArcadeAppNet.h"

static VOID lExecuteArcadeApp(
  _In_ TArcadeAppHostData* pArcadeAppHostData)
{
    Arcade::Application::Startup^ Startup = gcnew Arcade::Application::Startup();
    Arcade::Application::Startup::EDatabaseMode DatabaseMode;

    switch (pArcadeAppHostData->nDatabaseMode)
    {
        case CArcadeAppHostDataAccessDatabaseMode:
            DatabaseMode = Arcade::Application::Startup::EDatabaseMode::Access;
            break;
        case CArcadeAppHostDataSQLServerDatabaseMode:
        default:
            DatabaseMode = Arcade::Application::Startup::EDatabaseMode::SQLServer;
            break;
    }

    pArcadeAppHostData->dwExitCode = Startup->Execute(DatabaseMode);
}

extern "C"
{

#pragma unmanaged

HRESULT __stdcall ArcadeAppHostNetExecuteInAppDomain(
  _In_ void* cookie)
{
    TArcadeAppHostData* pArcadeAppHostData = (TArcadeAppHostData*)cookie;

    lExecuteArcadeApp(pArcadeAppHostData);

    return S_OK;
}

#pragma managed

}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
