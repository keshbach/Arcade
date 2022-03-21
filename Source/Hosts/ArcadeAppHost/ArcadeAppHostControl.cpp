/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostControl.h"

#include <new>

#include "ArcadeAppHostMemoryManager.h"
#if defined(ENABLE_HOSTTASKMANAGER)
#include "ArcadeAppHostTaskManager.h"
#endif
#include "ArcadeAppHostThreadpoolManager.h"
#include "ArcadeAppHostIOCompletionManager.h"
#include "ArcadeAppHostSyncManager.h"
#include "ArcadeAppHostGCManager.h"
#include "ArcadeAppHostAssemblyManager.h"
#include "ArcadeAppHostPolicyManager.h"
#include "ArcadeAppHostSecurityManager.h"

ArcadeAppHostControl::ArcadeAppHostControl()
{
    m_ulRefCount = 0;
}

ArcadeAppHostControl::~ArcadeAppHostControl()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostControl::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostControl::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostControl::QueryInterface(
  const IID& iid,
  void** ppv)
{
    iid;

    if (ppv == NULL)
    {
        return E_POINTER;
    }

    return E_NOINTERFACE;
}

#pragma endregion

#pragma region "IHostControl"

HRESULT STDMETHODCALLTYPE ArcadeAppHostControl::GetHostManager(
  REFIID riid,
  void** ppObject)
{
    if (ppObject == NULL)
    {
        return E_POINTER;
    }

    if (riid == IID_IHostMemoryManager)
    {
        ArcadeAppHostMemoryManager* pHostMemoryManager = new (std::nothrow) ArcadeAppHostMemoryManager();

        if (pHostMemoryManager)
        {
            pHostMemoryManager->AddRef();

            *ppObject = pHostMemoryManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostTaskManager)
    {
#if defined(ENABLE_HOSTTASKMANAGER)
        ArcadeAppHostTaskManager* pHostTaskManager = new (std::nothrow) ArcadeAppHostTaskManager();

        if (pHostTaskManager)
        {
            pHostTaskManager->AddRef();

            *ppObject = pHostTaskManager;

            return S_OK;
        }

		return E_FAIL;
#endif
    }
    else if (riid == IID_IHostThreadpoolManager)
    {
        ArcadeAppHostThreadpoolManager* pHostThreadpoolManager = new (std::nothrow) ArcadeAppHostThreadpoolManager();

        if (pHostThreadpoolManager)
        {
            pHostThreadpoolManager->AddRef();

            *ppObject = pHostThreadpoolManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostIoCompletionManager)
    {
        ArcadeAppHostIOCompletionManager* pHostIOCompletionManager = new (std::nothrow) ArcadeAppHostIOCompletionManager();

        if (pHostIOCompletionManager)
        {
            pHostIOCompletionManager->AddRef();

            *ppObject = pHostIOCompletionManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostSyncManager)
    {
        ArcadeAppHostSyncManager* pHostSyncManager = new (std::nothrow) ArcadeAppHostSyncManager();

        if (pHostSyncManager)
        {
            pHostSyncManager->AddRef();

            *ppObject = pHostSyncManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostAssemblyManager)
    {
        ArcadeAppHostAssemblyManager* pHostAssemblyManager = new (std::nothrow) ArcadeAppHostAssemblyManager();

        if (pHostAssemblyManager)
        {
            pHostAssemblyManager->AddRef();

            *ppObject = pHostAssemblyManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostGCManager)
    {
        ArcadeAppHostGCManager* pHostGCManager = new (std::nothrow) ArcadeAppHostGCManager();

        if (pHostGCManager)
        {
            pHostGCManager->AddRef();

            *ppObject = pHostGCManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostPolicyManager)
    {
        ArcadeAppHostPolicyManager* pHostPolicyManager = new (std::nothrow) ArcadeAppHostPolicyManager();

        if (pHostPolicyManager)
        {
            pHostPolicyManager->AddRef();

            *ppObject = pHostPolicyManager;

            return S_OK;
        }

        return E_FAIL;
    }
    else if (riid == IID_IHostSecurityManager)
    {
        ArcadeAppHostSecurityManager* pHostSecurityManager = new (std::nothrow) ArcadeAppHostSecurityManager();

        if (pHostSecurityManager)
        {
            pHostSecurityManager->AddRef();

            *ppObject = pHostSecurityManager;

            return S_OK;
        }

        return E_FAIL;
    }

    return E_NOINTERFACE;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostControl::SetAppDomainManager(
  DWORD dwAppDomainID,
  IUnknown* pUnkAppDomainManager)
{
    dwAppDomainID;
    pUnkAppDomainManager;

    return E_NOTIMPL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
