/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostThreadpoolManager.h"

ArcadeAppHostThreadpoolManager::ArcadeAppHostThreadpoolManager()
{
    m_ulRefCount = 0;
}

ArcadeAppHostThreadpoolManager::~ArcadeAppHostThreadpoolManager()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::QueryInterface(
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

#pragma region "IHostThreadpoolManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::QueueUserWorkItem(
  LPTHREAD_START_ROUTINE pFunction, 
  PVOID pvContext,
  ULONG Flags)
{
    pFunction;
    pvContext;
    Flags;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::SetMaxThreads(
  DWORD dwMaxWorkerThreads)
{
    dwMaxWorkerThreads;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::GetMaxThreads(
  DWORD* pdwMaxWorkerThreads)
{
    if (pdwMaxWorkerThreads == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::GetAvailableThreads(
  DWORD* pdwAvailableWorkerThreads)
{
    if (pdwAvailableWorkerThreads == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::SetMinThreads(
  DWORD dwMinIOCompletionThreads)
{
    dwMinIOCompletionThreads;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostThreadpoolManager::GetMinThreads(
  DWORD* pdwMinIOCompletionThreads)
{
    if (pdwMinIOCompletionThreads == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
