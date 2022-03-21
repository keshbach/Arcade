/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostSecurityManager.h"

#include <new>

#include "ArcadeAppHostSecurityContext.h"

ArcadeAppHostSecurityManager::ArcadeAppHostSecurityManager()
{
    m_ulRefCount = 0;
}

ArcadeAppHostSecurityManager::~ArcadeAppHostSecurityManager()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostSecurityManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostSecurityManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::QueryInterface(
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

#pragma region "IHostSecurityManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::ImpersonateLoggedOnUser(
  HANDLE hToken)
{
    return ::ImpersonateLoggedOnUser(hToken) ? S_OK : E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::RevertToSelf(void)
{
    return ::RevertToSelf() ? S_OK : E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::OpenThreadToken(
  DWORD dwDesiredAccess,
  BOOL bOpenAsSelf,
  HANDLE* phThreadToken)
{
	BOOL bResult;

    if (phThreadToken == NULL)
    {
        return E_POINTER;
    }

	bResult = ::OpenThreadToken(::GetCurrentThread(), dwDesiredAccess, bOpenAsSelf, phThreadToken);
	
	if (bResult == FALSE && ::GetLastError() == ERROR_NO_TOKEN && bOpenAsSelf)
	{
		bResult = ::OpenProcessToken(::GetCurrentProcess(), dwDesiredAccess, phThreadToken);
	}

	return bResult ? S_OK : E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::SetThreadToken(
  HANDLE hToken)
{
    return ::SetThreadToken(NULL, hToken) ? S_OK : E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::GetSecurityContext(
  EContextType eContextType,
  IHostSecurityContext** ppSecurityContext)
{
    ArcadeAppHostSecurityContext* pArcadeAppHostSecurityContext;

    if (ppSecurityContext == NULL)
    {
        return E_POINTER;
    }

    pArcadeAppHostSecurityContext = new (std::nothrow) ArcadeAppHostSecurityContext(eContextType);

    if (pArcadeAppHostSecurityContext)
    {
        pArcadeAppHostSecurityContext->AddRef();

        *ppSecurityContext = pArcadeAppHostSecurityContext;

        return S_OK;
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostSecurityManager::SetSecurityContext(
  EContextType eContextType,
  IHostSecurityContext* pSecurityContext)
{
	eContextType;
	pSecurityContext;
	
	return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
