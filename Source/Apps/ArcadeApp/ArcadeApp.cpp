/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Resource.h"
#include "ArcadeApp.h"

#include "ArcadeAppSplashWindow.h"

#include "UtArcadeApp.h"

#include <Includes/UtThread.inl>

#include <Includes/UtVersion.h>
#include <Includes/UtMacros.h>

#include <Utils/UtHeapProcess.h>

#include <Hosts/ArcadeAppHost.h>

#pragma region Constants

#define CArcadeAppRunningMutexName L"ArcadeAppRunningMutex"

// Command Line Arguments definitions

#define CDisableDPIArgument L"/disabledpi"
#define CAccessDatabaseArgument L"/accessdatabase"

#pragma endregion

#pragma region Typedefs

typedef BOOL (WINAPI* TSetProcessDPIAware)(VOID);

typedef HRESULT (STDAPICALLTYPE* TSetProcessDpiAwareness)(PROCESS_DPI_AWARENESS value);

typedef BOOL (WINAPI* TSetProcessDpiAwarenessContext)(DPI_AWARENESS_CONTEXT value);

typedef BOOL (ARCADEAPPHOSTAPI* TArcadeAppHostInitializeFunc)(VOID);
typedef BOOL (ARCADEAPPHOSTAPI* TArcadeAppHostUninitializeFunc)(VOID);
typedef BOOL (ARCADEAPPHOSTAPI* TArcadeAppHostExecuteFunc)(_In_ INT nDatabaseMode, _Out_ LPDWORD pdwExitCode);

typedef VOID (STDAPICALLTYPE *TPathRemoveExtensionWFunc)(_Inout_ LPWSTR pszPath);
typedef BOOL (STDAPICALLTYPE *TPathRemoveFileSpecWFunc)(_Inout_ LPWSTR pszPath);
typedef BOOL (STDAPICALLTYPE *TPathAppendWFunc)(_Inout_ LPWSTR pszPath, _In_ LPCWSTR pszMore);

#pragma endregion

#pragma region Structures

typedef struct tagTArcadeAppHostModuleData
{
    HMODULE hModule;
    TArcadeAppHostInitializeFunc pInitialize;
    TArcadeAppHostUninitializeFunc pUninitialize;
    TArcadeAppHostExecuteFunc pExecute;
} TArcadeAppHostModuleData;

typedef struct tagTArcadeAppData
{
    HINSTANCE hInstance;
    TArcadeAppHostModuleData AppHostModuleData;
} TArcadeAppData;

#pragma endregion

#pragma region Global Variables

static HANDLE l_hAppRunningMutex = NULL;

#pragma endregion

#pragma region "Local Functions"

static void lDisplayUnsupportedOS()
{
	LPCWSTR pszAppTitle, pszMessage;

	pszAppTitle = UtArcadeAppAllocString(IDS_APPTITLE);
	pszMessage = UtArcadeAppAllocString(IDS_UNSUPPORTEDWINDOWSVERSION);

	::MessageBox(NULL, pszMessage, pszAppTitle, MB_OK | MB_ICONINFORMATION);

	UtArcadeAppFreeString(pszAppTitle);
	UtArcadeAppFreeString(pszMessage);
}

static VOID lDisplayCommandLineHelp()
{
	LPCWSTR pszAppTitle, pszMessage;

    pszAppTitle = UtArcadeAppAllocString(IDS_APPTITLE);
	pszMessage = UtArcadeAppAllocString(IDS_COMMANDLINEHELP);

	::MessageBox(NULL, pszMessage, pszAppTitle, MB_OK | MB_ICONINFORMATION);

	UtArcadeAppFreeString(pszAppTitle);
	UtArcadeAppFreeString(pszMessage);
}

static void lDisplayCannotCreateAppWindow()
{
	LPCWSTR pszAppTitle, pszMessage;

	pszAppTitle = UtArcadeAppAllocString(IDS_APPTITLE);
	pszMessage = UtArcadeAppAllocString(IDS_CANNOTCREATEAPPWINDOW);

	::MessageBox(NULL, pszMessage, pszAppTitle, MB_OK | MB_ICONINFORMATION);

	UtArcadeAppFreeString(pszAppTitle);
	UtArcadeAppFreeString(pszMessage);
}

static void lDisplayCannotCreateWorkerThread()
{
	LPCWSTR pszAppTitle, pszMessage;

	pszAppTitle = UtArcadeAppAllocString(IDS_APPTITLE);
	pszMessage = UtArcadeAppAllocString(IDS_CANNOTCREATEWORKERTHREAD);

	::MessageBox(NULL, pszMessage, pszAppTitle, MB_OK | MB_ICONINFORMATION);

	UtArcadeAppFreeString(pszAppTitle);
	UtArcadeAppFreeString(pszMessage);
}

static VOID lEnableDPIAwareness(VOID)
{
	HMODULE hModule;
	TSetProcessDPIAware pSetProcessDPIAware;
	TSetProcessDpiAwarenessContext pSetProcessDpiAwarenessContext;
	TSetProcessDpiAwareness pSetProcessDpiAwareness;

	// Check for Windows 10

    hModule = ::LoadLibraryW(L"user32.dll");

	if (hModule)
	{
		pSetProcessDpiAwarenessContext = (TSetProcessDpiAwarenessContext)::GetProcAddress(hModule, "SetProcessDpiAwarenessContext");

		if (pSetProcessDpiAwarenessContext)
		{
			if (!pSetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2))
			{
				pSetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);
			}

			::FreeLibrary(hModule);

			return;
		}

		::FreeLibrary(hModule);
	}

	// Check for Windows 8.1

	hModule = ::LoadLibraryW(L"shcore.dll");

	if (hModule)
	{
		pSetProcessDpiAwareness = (TSetProcessDpiAwareness)::GetProcAddress(hModule, "SetProcessDpiAwareness");

		if (pSetProcessDpiAwareness)
		{
			pSetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE);

			::FreeLibrary(hModule);

			return;
		}

		::FreeLibrary(hModule);
	}

	// Check for Vista

	hModule = ::LoadLibraryW(L"user32.dll");

	if (hModule)
	{
		pSetProcessDPIAware = (TSetProcessDPIAware)::GetProcAddress(hModule, "SetProcessDPIAware");

		if (pSetProcessDPIAware)
		{
			pSetProcessDPIAware();
		}

		::FreeLibrary(hModule);
	}
}

static BOOL lInitialize(
  _Out_writes_bytes_(sizeof(BOOL)) LPBOOL pbAlreadyRunning)
{
    BOOL bResult = FALSE;

    *pbAlreadyRunning = FALSE;

    // Check if the application is already running

    l_hAppRunningMutex = ::CreateMutex(NULL, FALSE, CArcadeAppRunningMutexName);

    if (l_hAppRunningMutex)
    {
        if (::GetLastError() == ERROR_ALREADY_EXISTS)
        {
            *pbAlreadyRunning = TRUE;
        }

        bResult = TRUE;
    }
    else
    {
        if (::GetLastError() == ERROR_INVALID_HANDLE)
        {
            // name matches a different object
        }
        else if (::GetLastError() == ERROR_ACCESS_DENIED)
        {
            *pbAlreadyRunning = TRUE;

            bResult = TRUE;
        }
    }

    return bResult;
}

static VOID lUninitialize()
{
    ::CloseHandle(l_hAppRunningMutex);

    l_hAppRunningMutex = NULL;
}

static BOOL lInitializeArcadeAppHostModuleData(
  _In_ TArcadeAppHostModuleData* pArcadeAppHostModuleData)
{
	pArcadeAppHostModuleData->hModule = ::LoadLibrary(L"ArcadeAppHost.dll");

	if (pArcadeAppHostModuleData->hModule == NULL)
	{
		return FALSE;
	}

	pArcadeAppHostModuleData->pInitialize = (TArcadeAppHostInitializeFunc)::GetProcAddress(pArcadeAppHostModuleData->hModule, "ArcadeAppHostInitialize");
	pArcadeAppHostModuleData->pUninitialize = (TArcadeAppHostUninitializeFunc)::GetProcAddress(pArcadeAppHostModuleData->hModule, "ArcadeAppHostUninitialize");
	pArcadeAppHostModuleData->pExecute = (TArcadeAppHostExecuteFunc)::GetProcAddress(pArcadeAppHostModuleData->hModule, "ArcadeAppHostExecute");

	if (pArcadeAppHostModuleData->pInitialize == NULL ||
		pArcadeAppHostModuleData->pUninitialize == NULL ||
		pArcadeAppHostModuleData->pExecute == NULL)
	{
		::FreeLibrary(pArcadeAppHostModuleData->hModule);

		return FALSE;
	}

	return TRUE;
}

static BOOL lUninitializeArcadeAppHostModuleData(
  _In_ TArcadeAppHostModuleData* pArcadeAppHostModuleData)
{
	pArcadeAppHostModuleData;

	// Cannot free the library because there is no way to unload the .NET Framework.

	//::FreeLibrary(pArcadeAppHostData->hModule);

	return TRUE;
}

static DWORD WINAPI lRunSetupThreadProc(
  _In_ LPVOID pvParameter)
{
	TArcadeAppData* pArcadeAppData = (TArcadeAppData*)pvParameter;
	BOOL bAlreadyRunning;

    if (!lInitialize(&bAlreadyRunning))
    {
        ArcadeAppSplashWindowDisplayUnknownError();

        return FALSE;
    }

    if (bAlreadyRunning)
    {
        ArcadeAppSplashWindowDisplayAppAlreadyRunning();

        lUninitialize();

        return FALSE;
    }

	if (FALSE == lInitializeArcadeAppHostModuleData(&pArcadeAppData->AppHostModuleData))
	{
		ArcadeAppSplashWindowDisplayUnknownError();

        lUninitialize();

        return FALSE;
    }

	if (FALSE == pArcadeAppData->AppHostModuleData.pInitialize())
	{
		lUninitializeArcadeAppHostModuleData(&pArcadeAppData->AppHostModuleData);

		ArcadeAppSplashWindowDisplayUnknownError();

		lUninitialize();

		return FALSE;
	}

    ArcadeAppSplashWindowQuitMessagePump();

    return TRUE;
}

#pragma endregion

#pragma region Public Functions

INT ArcadeAppExecute(
  _In_ HINSTANCE hInstance,
  _In_ INT nTotalArgs,
  _In_z_ LPWSTR* ppszArgs)
{
	TArcadeAppData* pArcadeAppData = (TArcadeAppData*)UtAllocMem(sizeof(TArcadeAppData));
	INT nDatabaseMode = CArcadeAppHostSQLServerDatabaseMode;
	HANDLE hThread;
	DWORD dwThreadId, dwExitCode;
	INT nArgIndex;
	BOOL bDisableDPI, bDisplayHelp;

	if (!IsWindows7OrGreater())
	{
		lDisplayUnsupportedOS();

		return 1;
	}

	pArcadeAppData = (TArcadeAppData*)UtAllocMem(sizeof(TArcadeAppData));

	::ZeroMemory(pArcadeAppData, sizeof(TArcadeAppData));

	nArgIndex = 1;
	bDisableDPI = FALSE;
	bDisplayHelp = FALSE;

	while (nArgIndex < nTotalArgs && bDisplayHelp == FALSE)
	{
		if (::lstrcmpi(ppszArgs[nArgIndex], CDisableDPIArgument) == 0)
		{
			bDisableDPI = TRUE;

			++nArgIndex;
		}
		else if (::lstrcmpi(ppszArgs[nArgIndex], CAccessDatabaseArgument) == 0)
		{
			nDatabaseMode = CArcadeAppHostAccessDatabaseMode;

			++nArgIndex;
		}
		else 
		{
			bDisplayHelp = TRUE;
		}
	}

	if (bDisplayHelp == TRUE)
	{
		UtFreeMem(pArcadeAppData);

		lDisplayCommandLineHelp();

		return 1;
	}

	if (bDisableDPI == FALSE)
	{
		lEnableDPIAwareness();
	}

	if (!ArcadeAppSplashWindowCreate(hInstance))
    {
		lDisplayCannotCreateAppWindow();

		UtFreeMem(pArcadeAppData);

        return 1;
    }

    pArcadeAppData->hInstance = hInstance;

    hThread = ::CreateThread(NULL, 0, lRunSetupThreadProc, pArcadeAppData, 0, &dwThreadId);

    if (hThread == NULL)
    {
		lDisplayCannotCreateWorkerThread();

        ArcadeAppSplashWindowDestroy(hInstance);

		UtFreeMem(pArcadeAppData);

        return 1;
    }

    UtSetThreadName(dwThreadId, "App Thread");

    ArcadeAppSplashWindowMessagePump();

    ::WaitForSingleObject(hThread, INFINITE);

    ::GetExitCodeThread(hThread, &dwExitCode);

    ::CloseHandle(hThread);

    ArcadeAppSplashWindowDestroy(hInstance);

    if (dwExitCode == FALSE)
    {
		UtFreeMem(pArcadeAppData);
		
		return 1;
    }

    pArcadeAppData->AppHostModuleData.pExecute(nDatabaseMode, &dwExitCode);

	lUninitializeArcadeAppHostModuleData(&pArcadeAppData->AppHostModuleData);

	UtFreeMem(pArcadeAppData);

    lUninitialize();

    return dwExitCode;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
