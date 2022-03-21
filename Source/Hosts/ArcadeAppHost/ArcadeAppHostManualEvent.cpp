/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostManualEvent.h"

#include "UtArcadeAppHostUtility.h"

ArcadeAppHostManualEvent::ArcadeAppHostManualEvent(
  BOOL bInitialState,
  SIZE_T Cookie)
{
    m_ulRefCount = 0;

    m_Cookie = Cookie;

    m_hEvent = ::CreateEvent(NULL, TRUE, bInitialState, NULL);
}

ArcadeAppHostManualEvent::~ArcadeAppHostManualEvent()
{
    if (m_hEvent)
    {
        ::CloseHandle(m_hEvent);

        m_hEvent = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostManualEvent::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostManualEvent::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostManualEvent::QueryInterface(
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

#pragma region "IHostManualEvent"

HRESULT STDMETHODCALLTYPE ArcadeAppHostManualEvent::Wait(
  DWORD dwMilliseconds,
  DWORD dwOption)
{
    if (m_hEvent)
    {
		return UtArcadeAppHostUtilityWait(m_hEvent, dwMilliseconds, dwOption);
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostManualEvent::Reset(void)
{
    if (m_hEvent)
    {
        if (::ResetEvent(m_hEvent))
        {
            return S_OK;
        }
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostManualEvent::Set(void)
{
    if (m_hEvent)
    {
        if (::SetEvent(m_hEvent))
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
