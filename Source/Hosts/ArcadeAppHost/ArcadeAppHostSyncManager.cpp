/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostSyncManager.h"

#include <new>

#include "ArcadeAppHostManualEvent.h"
#include "ArcadeAppHostAutoEvent.h"
#include "ArcadeAppHostSemaphore.h"
#include "ArcadeAppHostCrst.h"

ArcadeAppHostSyncManager::ArcadeAppHostSyncManager()
{
    m_ulRefCount = 0;
    m_pCLRSyncManager = NULL;
}

ArcadeAppHostSyncManager::~ArcadeAppHostSyncManager()
{
    if (m_pCLRSyncManager)
    {
        m_pCLRSyncManager->Release();

        m_pCLRSyncManager = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostSyncManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostSyncManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::QueryInterface(
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

#pragma region "IHostSyncManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::SetCLRSyncManager(
   ICLRSyncManager* pManager)
{
    if (m_pCLRSyncManager)
    {
        m_pCLRSyncManager->Release();
    }

    m_pCLRSyncManager = pManager;

    m_pCLRSyncManager->AddRef();
    
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateCrst(
  IHostCrst** ppCrst)
{
    ArcadeAppHostCrst* pArcadeAppHostCrst;

    if (ppCrst == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostCrst = new (std::nothrow) ArcadeAppHostCrst();

    if (pArcadeAppHostCrst)
    {
        *ppCrst = pArcadeAppHostCrst;

        pArcadeAppHostCrst->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateCrstWithSpinCount(
  DWORD dwSpinCount,
  IHostCrst** ppCrst)
{
    ArcadeAppHostCrst* pArcadeAppHostCrst;

    if (ppCrst == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostCrst = new (std::nothrow) ArcadeAppHostCrst(dwSpinCount);

    if (pArcadeAppHostCrst)
    {
        *ppCrst = pArcadeAppHostCrst;

        pArcadeAppHostCrst->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateAutoEvent(
  IHostAutoEvent** ppEvent)
{
    ArcadeAppHostAutoEvent* pArcadeAppHostAutoEvent;

    if (ppEvent == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostAutoEvent = new (std::nothrow) ArcadeAppHostAutoEvent(0);

    if (pArcadeAppHostAutoEvent)
    {
        *ppEvent = pArcadeAppHostAutoEvent;

        pArcadeAppHostAutoEvent->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateManualEvent(
  BOOL bInitialState,
  IHostManualEvent** ppEvent)
{
    ArcadeAppHostManualEvent* pArcadeAppHostManualEvent;

    if (ppEvent == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostManualEvent = new (std::nothrow) ArcadeAppHostManualEvent(bInitialState, 0);

    if (pArcadeAppHostManualEvent)
    {
        *ppEvent = pArcadeAppHostManualEvent;

        pArcadeAppHostManualEvent->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateMonitorEvent(
  SIZE_T Cookie,
  IHostAutoEvent** ppEvent)
{
    ArcadeAppHostAutoEvent* pArcadeAppHostAutoEvent;

    if (ppEvent == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostAutoEvent = new (std::nothrow) ArcadeAppHostAutoEvent(Cookie);

    if (pArcadeAppHostAutoEvent)
    {
        *ppEvent = pArcadeAppHostAutoEvent;

        pArcadeAppHostAutoEvent->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateRWLockWriterEvent(
  SIZE_T Cookie,
  IHostAutoEvent** ppEvent)
{
    ArcadeAppHostAutoEvent* pArcadeAppHostAutoEvent;

    if (ppEvent == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostAutoEvent = new (std::nothrow) ArcadeAppHostAutoEvent(Cookie);

    if (pArcadeAppHostAutoEvent)
    {
        *ppEvent = pArcadeAppHostAutoEvent;

        pArcadeAppHostAutoEvent->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateRWLockReaderEvent(
  BOOL bInitialState,
  SIZE_T Cookie,
  IHostManualEvent** ppEvent)
{
    ArcadeAppHostManualEvent* pArcadeAppHostManualEvent;

    if (ppEvent == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostManualEvent = new (std::nothrow) ArcadeAppHostManualEvent(bInitialState, Cookie);

    if (pArcadeAppHostManualEvent)
    {
        *ppEvent = pArcadeAppHostManualEvent;

        pArcadeAppHostManualEvent->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSyncManager::CreateSemaphore(
  DWORD dwInitial,
  DWORD dwMax,
  IHostSemaphore** ppSemaphore)
{
    ArcadeAppHostSemaphore* pArcadeAppHostSemaphore;

    if (ppSemaphore == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostSemaphore = new (std::nothrow) ArcadeAppHostSemaphore(dwInitial, dwMax);

    if (pArcadeAppHostSemaphore)
    {
        *ppSemaphore = pArcadeAppHostSemaphore;

        pArcadeAppHostSemaphore->AddRef();

        return S_OK;
    }

    return E_FAIL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
