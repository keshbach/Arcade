/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostMalloc.h"

ArcadeAppHostMalloc::ArcadeAppHostMalloc(
  BOOL bThreadSafe,
  BOOL bExecutable)
{
    m_ulRefCount = 0;

    m_hHeap = ::HeapCreate(bThreadSafe ? 0 : HEAP_NO_SERIALIZE |
                               bExecutable ? HEAP_CREATE_ENABLE_EXECUTE : 0,
                           0, 0);

    if (m_hHeap)
    {
        ::HeapSetInformation(m_hHeap, HeapEnableTerminationOnCorruption, NULL, 0);
    }
}

ArcadeAppHostMalloc::~ArcadeAppHostMalloc()
{
    if (m_hHeap)
    {
        ::HeapDestroy(m_hHeap);

        m_hHeap = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostMalloc::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostMalloc::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMalloc::QueryInterface(
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

#pragma region "IHostMalloc"

HRESULT STDMETHODCALLTYPE ArcadeAppHostMalloc::Alloc(
  SIZE_T cbSize,
  EMemoryCriticalLevel eCriticalLevel,
  void** ppvMem)
{
    eCriticalLevel;

    if (ppvMem == NULL)
    {
        return E_POINTER;
    }

    if (m_hHeap == NULL)
    {
        return E_FAIL;
    }

    *ppvMem = ::HeapAlloc(m_hHeap, 0, cbSize);

    if (*ppvMem)
    {
        return S_OK;
    }

    return E_OUTOFMEMORY;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMalloc::DebugAlloc(
  SIZE_T cbSize,
  EMemoryCriticalLevel eCriticalLevel,
  char* pszFileName,
  int iLineNo,
  _Outptr_result_maybenull_ void** ppvMem)
{
    cbSize;
    eCriticalLevel;
    pszFileName;
    iLineNo;

    if (ppvMem == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostMalloc::Free(
  void* pvMem)
{
    if (m_hHeap == NULL)
    {
        return E_FAIL;
    }

    if (::HeapFree(m_hHeap, 0, pvMem))
    {
        return S_OK;
    }

    return E_FAIL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
