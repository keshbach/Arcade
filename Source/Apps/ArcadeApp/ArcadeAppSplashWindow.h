/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

#define ARCADEAPPSPLASHWINDOWEXECUTEAPI __stdcall

typedef VOID (ARCADEAPPSPLASHWINDOWEXECUTEAPI* TArcadeAppSplashWindowExecuteFunc)(_In_ LPVOID pvData);

BOOL ArcadeAppSplashWindowCreate(_In_ HINSTANCE hInstance);
BOOL ArcadeAppSplashWindowDestroy(_In_ HINSTANCE hInstance);

VOID ArcadeAppSplashWindowMessagePump();

VOID ArcadeAppSplashWindowQuitMessagePump();

VOID ArcadeAppSplashWindowDisplayAppAlreadyRunning();
VOID ArcadeAppSplashWindowDisplayUnknownError();

VOID ArcadeAppSplashWindowExecute(_In_ TArcadeAppSplashWindowExecuteFunc pExecute, _In_ LPVOID pvData);

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
