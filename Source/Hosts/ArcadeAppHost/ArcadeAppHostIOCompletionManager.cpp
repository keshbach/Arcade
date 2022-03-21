/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostIOCompletionManager.h"

ArcadeAppHostIOCompletionManager::ArcadeAppHostIOCompletionManager()
{
    m_ulRefCount = 0;
    m_pCLRIoCompletionManager = NULL;
}

ArcadeAppHostIOCompletionManager::~ArcadeAppHostIOCompletionManager()
{
    if (m_pCLRIoCompletionManager)
    {
        m_pCLRIoCompletionManager->Release();

        m_pCLRIoCompletionManager = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::QueryInterface(
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

#pragma region "IHostIoCompletionManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::CreateIoCompletionPort(
  HANDLE* phPort)
{
    if (phPort == NULL)
    {
        return E_POINTER;
    }

    *phPort = ::CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);

    if (*phPort)
    {
        return S_OK;
    }

    return E_OUTOFMEMORY;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::CloseIoCompletionPort(
  HANDLE hPort)
{
    if (::CloseHandle(hPort))
    {
        return S_OK;
    }

    return E_INVALIDARG;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::SetMaxThreads(
  DWORD dwMaxIOCompletionThreads)
{
    dwMaxIOCompletionThreads;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::GetMaxThreads(
  DWORD* pdwMaxIOCompletionThreads)
{
    if (pdwMaxIOCompletionThreads == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::GetAvailableThreads(
  DWORD* pdwAvailableIOCompletionThreads)
{
    if (pdwAvailableIOCompletionThreads == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::GetHostOverlappedSize(
  DWORD* pcbSize)
{
    if (pcbSize == NULL)
    {
        return E_POINTER;
    }

    *pcbSize = 0;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::SetCLRIoCompletionManager(
  ICLRIoCompletionManager* pManager)
{
    if (m_pCLRIoCompletionManager)
    {
        m_pCLRIoCompletionManager->Release();
    }

    m_pCLRIoCompletionManager = pManager;

    m_pCLRIoCompletionManager->AddRef();
    
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::InitializeHostOverlapped(
  void* pvOverlapped)
{
    pvOverlapped;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::Bind(
  HANDLE hPort,
  HANDLE hHandle)
{
    hPort;
    hHandle;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::SetMinThreads(
  DWORD dwMinIOCompletionThreads)
{
    dwMinIOCompletionThreads;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostIOCompletionManager::GetMinThreads(
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
