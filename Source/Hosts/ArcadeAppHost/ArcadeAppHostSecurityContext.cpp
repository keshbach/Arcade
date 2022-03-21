/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostSecurityContext.h"

#include <new>

ArcadeAppHostSecurityContext::ArcadeAppHostSecurityContext(
  EContextType ContextType)
{
    m_ulRefCount = 0;
    m_ContextType = ContextType;
}

ArcadeAppHostSecurityContext::~ArcadeAppHostSecurityContext()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostSecurityContext::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostSecurityContext::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityContext::QueryInterface(
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

#pragma region "IHostSecurityContext"

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityContext::Capture(
  IHostSecurityContext** ppClonedContext)
{
    ArcadeAppHostSecurityContext* pArcadeAppHostSecurityContext;

    if (ppClonedContext == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostSecurityContext = new (std::nothrow) ArcadeAppHostSecurityContext(m_ContextType);

    if (pArcadeAppHostSecurityContext)
    {
        pArcadeAppHostSecurityContext->AddRef();

        *ppClonedContext = pArcadeAppHostSecurityContext;

        return S_OK;
    }

    return E_FAIL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
