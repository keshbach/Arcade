/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppActionOnCLREvent.h"

ArcadeAppActionOnCLREvent::ArcadeAppActionOnCLREvent()
{
    m_ulRefCount = 0;
}

ArcadeAppActionOnCLREvent::~ArcadeAppActionOnCLREvent()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppActionOnCLREvent::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppActionOnCLREvent::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppActionOnCLREvent::QueryInterface(
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

#pragma region "IActionOnCLREvent"

HRESULT STDMETHODCALLTYPE ArcadeAppActionOnCLREvent::OnEvent(
  EClrEvent Event,
  PVOID pvData)
{
    Event;
    pvData;

    return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
