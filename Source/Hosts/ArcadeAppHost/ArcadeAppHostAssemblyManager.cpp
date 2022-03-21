/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostAssemblyManager.h"

#include <new>

#include "ArcadeAppHostCLRAssemblyReferenceList.h"
#include "ArcadeAppHostAssemblyStore.h"

ArcadeAppHostAssemblyManager::ArcadeAppHostAssemblyManager()
{
    m_ulRefCount = 0;
}

ArcadeAppHostAssemblyManager::~ArcadeAppHostAssemblyManager()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostAssemblyManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostAssemblyManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyManager::QueryInterface(
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

#pragma region "IHostAssemblyManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyManager::GetNonHostStoreAssemblies(
  ICLRAssemblyReferenceList** ppReferenceList)
{
	ArcadeAppCLRAssemblyReferenceList* pArcadeAppCLRAssemblyReferenceList;
	
	if (ppReferenceList == NULL)
    {
        return E_POINTER;
    }

	pArcadeAppCLRAssemblyReferenceList = new (std::nothrow) ArcadeAppCLRAssemblyReferenceList();

	if (pArcadeAppCLRAssemblyReferenceList)
	{
		pArcadeAppCLRAssemblyReferenceList->AddRef();

		*ppReferenceList = pArcadeAppCLRAssemblyReferenceList;

		return S_OK;
	}

	return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyManager::GetAssemblyStore(
  IHostAssemblyStore** ppAssemblyStore)
{
	ArcadeAppHostAssemblyStore* pArcadeAppHostAssemblyStore;

    if (ppAssemblyStore == NULL)
    {
        return E_POINTER;
    }

	pArcadeAppHostAssemblyStore = new (std::nothrow) ArcadeAppHostAssemblyStore();

	if (pArcadeAppHostAssemblyStore)
	{
		pArcadeAppHostAssemblyStore->AddRef();

		*ppAssemblyStore = pArcadeAppHostAssemblyStore;

		return S_OK;
	}

	return E_FAIL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
