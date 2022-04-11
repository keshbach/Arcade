/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include <Hosts/ArcadeAppHost.h>
#include <Hosts/ArcadeAppHostData.h>

#include <Utils/UtHeapProcess.h>

#include "ArcadeAppHostControl.h"

#include <new>

#include "ArcadeAppActionOnCLREvent.h"

#include "UtArcadeAppHostTasks.h"

#pragma region "Constants"

#define CNetFrameworkRuntimeVersion L"v4.0.30319"

#define CArcadeAppHostNetLibraryName L"ArcadeAppHostNet.dll"

#define CArcadeAppHostNetExecuteInAppDomainFuncName "ArcadeAppHostNetExecuteInAppDomain"

#pragma endregion

#pragma region "Type Defs"

typedef HRESULT (__stdcall* TArcadeAppHostNetExecuteInAppDomainFunc)(void* cookie);

#pragma endregion

#pragma region "Structures"

typedef struct tagTArcadeAppHostRuntimeData
{
    BOOL bCOMInitialized;
	BOOL bArcadeAppHostTasksInitialized;
    BOOL bRuntimeStarted;
    ICLRMetaHost* pCLRMetaHost;
    ICLRRuntimeInfo* pCLRRuntimeInfo;
    ICLRRuntimeHost* pCLRRuntimeHost;
    ICLRControl* pCLRControl;
    ICLROnEventManager* pCLROnEventManager;
    ICLRPolicyManager* pCLRPolicyManager;
    ICLRHostProtectionManager* pCLRHostProtectionManager;
    ArcadeAppHostControl* pArcadeAppHostControl;
    ArcadeAppActionOnCLREvent* pArcadeAppActionOnCLREvent;

    HMODULE hHostNetLibrary;
    TArcadeAppHostNetExecuteInAppDomainFunc pArcadeAppHostNetExecuteInAppDomain;
} TArcadeAppHostRuntimeData;

#pragma endregion

#pragma region "Local Variables"

static TArcadeAppHostRuntimeData l_ArcadeAppHostRuntimeData;

static TArcadeAppHostData l_ArcadeAppHostData;

#pragma endregion

#pragma region "Local Functions"

static BOOL lUninitialize(_In_ TArcadeAppHostRuntimeData* pArcadeAppHostRuntimeData);

static BOOL lInitialize(
  _In_ TArcadeAppHostRuntimeData* pArcadeAppHostRuntimeData)
{
    if (S_OK != ::CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))
    {
        return FALSE;
    }

    pArcadeAppHostRuntimeData->bCOMInitialized = TRUE;

	if (FALSE == UtArcadeAppHostTasksInitialize())
	{
		lUninitialize(pArcadeAppHostRuntimeData);

		return FALSE;
	}

	pArcadeAppHostRuntimeData->bArcadeAppHostTasksInitialized = TRUE;

    if (S_OK != ::CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost,
                                    (LPVOID*)&pArcadeAppHostRuntimeData->pCLRMetaHost))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    if (S_OK != pArcadeAppHostRuntimeData->pCLRMetaHost->GetRuntime(CNetFrameworkRuntimeVersion,
                                                                    IID_ICLRRuntimeInfo,
                                                                    (LPVOID*)&pArcadeAppHostRuntimeData->pCLRRuntimeInfo))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    if (S_OK != pArcadeAppHostRuntimeData->pCLRRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost,
                                                                         IID_ICLRRuntimeHost,
                                                                         (LPVOID*)&pArcadeAppHostRuntimeData->pCLRRuntimeHost))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pArcadeAppHostControl = new (std::nothrow) ArcadeAppHostControl();

    if (pArcadeAppHostRuntimeData->pArcadeAppHostControl == NULL)
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pArcadeAppHostControl->AddRef();






	if (S_OK != pArcadeAppHostRuntimeData->pCLRRuntimeHost->GetCLRControl(&pArcadeAppHostRuntimeData->pCLRControl))
	{
		lUninitialize(pArcadeAppHostRuntimeData);

		return FALSE;
	}

/*
	HRESULT hResult;

	hResult = pArcadeAppHostRuntimeData->pCLRControl->SetAppDomainManagerType(L"C:\\git\\Arcade\\Source\\bin\\Debug\\x86\\ArcadeAppHostNet.dll",
	                                                                          L"Arcade.Application.ArcadeAppHostNetAppDomainManager");

	if (hResult != S_OK)
	{
		::OutputDebugString(L"error");
	}
*/

    if (S_OK != pArcadeAppHostRuntimeData->pCLRRuntimeHost->SetHostControl(pArcadeAppHostRuntimeData->pArcadeAppHostControl))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    if (S_OK != pArcadeAppHostRuntimeData->pCLRControl->GetCLRManager(IID_ICLROnEventManager,
                                                                      (LPVOID*)&pArcadeAppHostRuntimeData->pCLROnEventManager))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent = new (std::nothrow) ArcadeAppActionOnCLREvent();

    if (pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent == NULL)
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent->AddRef();

    pArcadeAppHostRuntimeData->pCLROnEventManager->RegisterActionOnEvent(Event_DomainUnload, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
    pArcadeAppHostRuntimeData->pCLROnEventManager->RegisterActionOnEvent(Event_ClrDisabled, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
    pArcadeAppHostRuntimeData->pCLROnEventManager->RegisterActionOnEvent(Event_MDAFired, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
    pArcadeAppHostRuntimeData->pCLROnEventManager->RegisterActionOnEvent(Event_StackOverflow, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
    //pArcadeAppHostRuntimeData->pCLROnEventManager->RegisterActionOnEvent(MaxClrEvent, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);

    if (S_OK != pArcadeAppHostRuntimeData->pCLRControl->GetCLRManager(IID_ICLRPolicyManager,
                                                                      (LPVOID*)&pArcadeAppHostRuntimeData->pCLRPolicyManager))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pCLRPolicyManager->SetUnhandledExceptionPolicy(eHostDeterminedPolicy);

    if (S_OK != pArcadeAppHostRuntimeData->pCLRControl->GetCLRManager(IID_ICLRHostProtectionManager,
                                                                      (LPVOID*)&pArcadeAppHostRuntimeData->pCLRHostProtectionManager))
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->pCLRHostProtectionManager->SetProtectedCategories(eNoChecks);

    if (S_OK != pArcadeAppHostRuntimeData->pCLRRuntimeHost->Start())
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    pArcadeAppHostRuntimeData->bRuntimeStarted = TRUE;

    pArcadeAppHostRuntimeData->hHostNetLibrary = ::LoadLibrary(CArcadeAppHostNetLibraryName);

    if (pArcadeAppHostRuntimeData->hHostNetLibrary)
    {
        pArcadeAppHostRuntimeData->pArcadeAppHostNetExecuteInAppDomain = (TArcadeAppHostNetExecuteInAppDomainFunc)::GetProcAddress(pArcadeAppHostRuntimeData->hHostNetLibrary, CArcadeAppHostNetExecuteInAppDomainFuncName);
    }

    if (pArcadeAppHostRuntimeData->hHostNetLibrary == NULL ||
        pArcadeAppHostRuntimeData->pArcadeAppHostNetExecuteInAppDomain == NULL)
    {
        lUninitialize(pArcadeAppHostRuntimeData);

        return FALSE;
    }

    return TRUE;
}

static BOOL lUninitialize(
  _In_ TArcadeAppHostRuntimeData* pArcadeAppHostRuntimeData)
{
    if (pArcadeAppHostRuntimeData->bRuntimeStarted)
    {
        pArcadeAppHostRuntimeData->pCLRRuntimeHost->Stop();

        pArcadeAppHostRuntimeData->bRuntimeStarted = FALSE;
    }

    if (pArcadeAppHostRuntimeData->hHostNetLibrary)
    {
        ::FreeLibrary(pArcadeAppHostRuntimeData->hHostNetLibrary);

        pArcadeAppHostRuntimeData->hHostNetLibrary = NULL;
        pArcadeAppHostRuntimeData->pArcadeAppHostNetExecuteInAppDomain = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRHostProtectionManager)
    {
        pArcadeAppHostRuntimeData->pCLRHostProtectionManager->Release();

        pArcadeAppHostRuntimeData->pCLRHostProtectionManager = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRPolicyManager)
    {
        pArcadeAppHostRuntimeData->pCLRPolicyManager->Release();

        pArcadeAppHostRuntimeData->pCLRPolicyManager = NULL;
    }

    if (pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent)
    {
        pArcadeAppHostRuntimeData->pCLROnEventManager->UnregisterActionOnEvent(Event_DomainUnload, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
        pArcadeAppHostRuntimeData->pCLROnEventManager->UnregisterActionOnEvent(Event_ClrDisabled, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
        pArcadeAppHostRuntimeData->pCLROnEventManager->UnregisterActionOnEvent(Event_MDAFired, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
        pArcadeAppHostRuntimeData->pCLROnEventManager->UnregisterActionOnEvent(Event_StackOverflow, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);
        //pArcadeAppHostRuntimeData->pCLROnEventManager->UnregisterActionOnEvent(MaxClrEvent, pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent);

        pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent->Release();

        pArcadeAppHostRuntimeData->pArcadeAppActionOnCLREvent = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLROnEventManager)
    {
        pArcadeAppHostRuntimeData->pCLROnEventManager->Release();

        pArcadeAppHostRuntimeData->pCLROnEventManager = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRControl)
    {
        pArcadeAppHostRuntimeData->pCLRControl->Release();

        pArcadeAppHostRuntimeData->pCLRControl = NULL;
    }

    if (pArcadeAppHostRuntimeData->pArcadeAppHostControl)
    {
        pArcadeAppHostRuntimeData->pArcadeAppHostControl->Release();

        pArcadeAppHostRuntimeData->pArcadeAppHostControl = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRRuntimeHost)
    {
        pArcadeAppHostRuntimeData->pCLRRuntimeHost->Release();

        pArcadeAppHostRuntimeData->pCLRRuntimeHost = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRRuntimeInfo)
    {
        pArcadeAppHostRuntimeData->pCLRRuntimeInfo->Release();

        pArcadeAppHostRuntimeData->pCLRRuntimeInfo = NULL;
    }

    if (pArcadeAppHostRuntimeData->pCLRMetaHost)
    {
        pArcadeAppHostRuntimeData->pCLRMetaHost->Release();

        pArcadeAppHostRuntimeData->pCLRMetaHost = NULL;
    }

	if (pArcadeAppHostRuntimeData->bArcadeAppHostTasksInitialized == TRUE)
	{
		UtArcadeAppHostTasksUninitialize();

		pArcadeAppHostRuntimeData->bArcadeAppHostTasksInitialized = FALSE;
	}

    if (pArcadeAppHostRuntimeData->bCOMInitialized)
    {
        ::CoUninitialize();

        pArcadeAppHostRuntimeData->bCOMInitialized = FALSE;
    }

    return TRUE;
}

#pragma endregion

#pragma region "Public Functions"

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostInitialize(VOID)
{
    ::ZeroMemory(&l_ArcadeAppHostRuntimeData, sizeof(l_ArcadeAppHostRuntimeData));
    ::ZeroMemory(&l_ArcadeAppHostData, sizeof(l_ArcadeAppHostData));

    return TRUE;
}

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostUninitialize(VOID)
{
    return TRUE;
}

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostExecute(
  _In_ INT nDatabaseMode,
  _Out_ LPDWORD pdwExitCode)
{
    DWORD dwAppDomainId = 0;

    *pdwExitCode = 0;

	if (FALSE == lInitialize(&l_ArcadeAppHostRuntimeData))
	{
		return FALSE;
	}

    switch (nDatabaseMode)
    {
        case CArcadeAppHostAccessDatabaseMode:
            l_ArcadeAppHostData.nDatabaseMode = CArcadeAppHostDataAccessDatabaseMode;
            break;
        case CArcadeAppHostSQLServerDatabaseMode:
        default:
            l_ArcadeAppHostData.nDatabaseMode = CArcadeAppHostDataSQLServerDatabaseMode;
            break;
    }

    l_ArcadeAppHostRuntimeData.pCLRRuntimeHost->GetCurrentAppDomainId(&dwAppDomainId);

    l_ArcadeAppHostRuntimeData.pCLRRuntimeHost->ExecuteInAppDomain(dwAppDomainId,
                                                                   l_ArcadeAppHostRuntimeData.pArcadeAppHostNetExecuteInAppDomain,
                                                                   &l_ArcadeAppHostData);

    lUninitialize(&l_ArcadeAppHostRuntimeData);

    *pdwExitCode = l_ArcadeAppHostData.dwExitCode;

    return TRUE;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
