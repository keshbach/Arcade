/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostGCManager.h"

ArcadeAppHostGCManager::ArcadeAppHostGCManager()
{
    m_ulRefCount = 0;
}

ArcadeAppHostGCManager::~ArcadeAppHostGCManager()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostGCManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostGCManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostGCManager::QueryInterface(
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

#pragma region "IHostGCManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostGCManager::ThreadIsBlockingForSuspension(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostGCManager::SuspensionStarting(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostGCManager::SuspensionEnding(DWORD dwGeneration)
{
    dwGeneration;

    return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
