/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostAssemblyStore : public IHostAssemblyStore
{
private:
	ULONG m_ulRefCount;

public:
	ArcadeAppHostAssemblyStore();
	virtual ~ArcadeAppHostAssemblyStore();

private:
	ArcadeAppHostAssemblyStore(const ArcadeAppHostAssemblyStore&);
	ArcadeAppHostAssemblyStore& operator = (const ArcadeAppHostAssemblyStore&);

	// IUnknown
public:
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

	// IHostAssemblyStore
public:
	virtual HRESULT STDMETHODCALLTYPE ProvideAssembly(AssemblyBindInfo* pBindInfo, UINT64* pAssemblyId, UINT64* pContext, IStream** ppStmAssemblyImage, IStream** ppStmPDB);
	virtual HRESULT STDMETHODCALLTYPE ProvideModule(ModuleBindInfo* pBindInfo, DWORD* pdwModuleId, IStream** ppStmModuleImage, IStream** ppStmPDB);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
