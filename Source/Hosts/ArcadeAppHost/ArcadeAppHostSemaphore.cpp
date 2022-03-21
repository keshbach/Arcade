/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostSemaphore.h"

#include "UtArcadeAppHostUtility.h"

ArcadeAppHostSemaphore::ArcadeAppHostSemaphore(
  DWORD dwInitial,
  DWORD dwMax)
{
    m_ulRefCount = 0;

    m_hSemaphore = ::CreateSemaphore(NULL, dwInitial, dwMax, NULL);
}

ArcadeAppHostSemaphore::~ArcadeAppHostSemaphore()
{
    if (m_hSemaphore)
    {
        ::CloseHandle(m_hSemaphore);

        m_hSemaphore = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostSemaphore::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostSemaphore::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSemaphore::QueryInterface(
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

#pragma region "IHostSemaphore"

HRESULT STDMETHODCALLTYPE ArcadeAppHostSemaphore::Wait(
  DWORD dwMilliseconds,
  DWORD dwOption)
{
    if (m_hSemaphore)
    {
		return UtArcadeAppHostUtilityWait(m_hSemaphore, dwMilliseconds, dwOption);
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSemaphore::ReleaseSemaphore(
    LONG lReleaseCount,
    LONG* plPreviousCount)
{
    if (m_hSemaphore)
    {
        if (::ReleaseSemaphore(m_hSemaphore, lReleaseCount, plPreviousCount))
        {
            return S_OK;
        }
    }

    return E_FAIL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
