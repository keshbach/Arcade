/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include <Hosts/ArcadeAppHostData.h>

#include "ArcadeAppNet.h"

static VOID lExecuteArcadeApp(
  _In_ TArcadeAppHostData* pArcadeAppHostData)
{
    Arcade::Application::Startup^ Startup = gcnew Arcade::Application::Startup();

    pArcadeAppHostData->dwExitCode = Startup->Execute();
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
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
