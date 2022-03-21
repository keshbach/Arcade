/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostPolicyManager.h"

ArcadeAppHostPolicyManager::ArcadeAppHostPolicyManager()
{
    m_ulRefCount = 0;
}

ArcadeAppHostPolicyManager::~ArcadeAppHostPolicyManager()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostPolicyManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostPolicyManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostPolicyManager::QueryInterface(
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

#pragma region "IHostPolicyManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostPolicyManager::OnDefaultAction(
  EClrOperation operation,
  EPolicyAction action)
{
    operation;
    action;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostPolicyManager::OnTimeout(
  EClrOperation operation,
  EPolicyAction action)
{
    operation;
    action;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostPolicyManager::OnFailure(
  EClrFailure failure,
  EPolicyAction action)
{
    failure;
    action;

    return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
