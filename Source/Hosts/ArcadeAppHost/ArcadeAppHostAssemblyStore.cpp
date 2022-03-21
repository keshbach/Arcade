/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostAssemblyStore.h"

ArcadeAppHostAssemblyStore::ArcadeAppHostAssemblyStore()
{
	m_ulRefCount = 0;
}

ArcadeAppHostAssemblyStore::~ArcadeAppHostAssemblyStore()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostAssemblyStore::AddRef()
{
	return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostAssemblyStore::Release()
{
	if (::InterlockedDecrement(&m_ulRefCount) == 0)
	{
		delete this;

		return 0;
	}

	return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyStore::QueryInterface(
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

#pragma region "IHostAssemblyStore"

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyStore::ProvideAssembly(
  AssemblyBindInfo* pBindInfo,
  UINT64* pAssemblyId,
  UINT64* pContext,
  IStream** ppStmAssemblyImage,
  IStream** ppStmPDB)
{
	pBindInfo;
	pAssemblyId;
	pContext;
	ppStmAssemblyImage;
	ppStmPDB;

	return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostAssemblyStore::ProvideModule(
  ModuleBindInfo* pBindInfo,
  DWORD* pdwModuleId,
  IStream** ppStmModuleImage,
  IStream** ppStmPDB)
{
	pBindInfo;
	pdwModuleId;
	ppStmModuleImage;
	ppStmPDB;

	return E_NOTIMPL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
