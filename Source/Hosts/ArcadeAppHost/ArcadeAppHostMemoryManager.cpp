/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostMemoryManager.h"

#include <new>

#include "ArcadeAppHostMalloc.h"

ArcadeAppHostMemoryManager::ArcadeAppHostMemoryManager()
{
    m_ulRefCount = 0;
    m_pMemoryNotificationCallback = NULL;
}

ArcadeAppHostMemoryManager::~ArcadeAppHostMemoryManager()
{
    if (m_pMemoryNotificationCallback)
    {
        m_pMemoryNotificationCallback->Release();
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostMemoryManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostMemoryManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::QueryInterface(
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

#pragma region "IHostMemoryManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::CreateMalloc(
  DWORD dwMallocType,
  IHostMalloc** ppMalloc)
{
    ArcadeAppHostMalloc* pArcadeAppHostMalloc;

    if (ppMalloc == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostMalloc = new (std::nothrow) ArcadeAppHostMalloc((dwMallocType & MALLOC_THREADSAFE) ? TRUE : FALSE,
                                                                  (dwMallocType & MALLOC_EXECUTABLE) ? TRUE : FALSE);

    if (pArcadeAppHostMalloc == NULL)
    {
        return E_FAIL;
    }

    pArcadeAppHostMalloc->AddRef();

    *ppMalloc = pArcadeAppHostMalloc;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::VirtualAlloc(
  void* pvAddress,
  SIZE_T dwSize,
  DWORD flAllocationType,
  DWORD flProtect,
  EMemoryCriticalLevel eCriticalLevel,
  void** ppvMem)
{
    eCriticalLevel;

    if (ppvMem == NULL)
    {
        return E_FAIL;
    }

    *ppvMem = ::VirtualAlloc(pvAddress, dwSize, flAllocationType, flProtect);

    if (*ppvMem)
    {
        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::VirtualFree(
  void* pvAddress,
  SIZE_T dwSize,
  DWORD dwFreeType)
{
    if (::VirtualFree(pvAddress, dwSize, dwFreeType))
    {
        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::VirtualQuery(
  void* pvAddress,
  void* pvBuffer,
  SIZE_T dwLength,
  SIZE_T* pResult)
{
    if (pResult == NULL)
    {
        return E_POINTER;
    }

    *pResult = ::VirtualQuery(pvAddress, (PMEMORY_BASIC_INFORMATION)pvBuffer, dwLength);

    if (*pResult)
    {
        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::VirtualProtect(
  void* pvAddress,
  SIZE_T dwSize,
  DWORD flNewProtect,
  DWORD* pflOldProtect)
{
    if (::VirtualProtect(pvAddress, dwSize, flNewProtect, pflOldProtect))
    {
        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::GetMemoryLoad(
  DWORD* pdwMemoryLoad,
  SIZE_T* pAvailableBytes)
{
    MEMORYSTATUSEX MemoryStatusEx;

    MemoryStatusEx.dwLength = sizeof(MemoryStatusEx);

    if (::GlobalMemoryStatusEx(&MemoryStatusEx))
    {
        *pdwMemoryLoad = MemoryStatusEx.dwMemoryLoad;
        *pAvailableBytes = (SIZE_T)MemoryStatusEx.ullAvailVirtual;

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::RegisterMemoryNotificationCallback(
  ICLRMemoryNotificationCallback* pCallback)
{
    if (pCallback == NULL)
    {
        return E_POINTER;
    }

    if (m_pMemoryNotificationCallback)
    {
        m_pMemoryNotificationCallback->Release();
    }

    m_pMemoryNotificationCallback = pCallback;

    m_pMemoryNotificationCallback->Release();

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::NeedsVirtualAddressSpace(
  void* pvStartAddress,
  SIZE_T size)
{
    pvStartAddress;
    size;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::AcquiredVirtualAddressSpace(
  void* pvStartAddress,
  SIZE_T size)
{
    pvStartAddress;
    size;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMemoryManager::ReleasedVirtualAddressSpace(
  void* pvStartAddress)
{
    pvStartAddress;

    return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
