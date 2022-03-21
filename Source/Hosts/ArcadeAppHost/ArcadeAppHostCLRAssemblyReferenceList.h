/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppCLRAssemblyReferenceList : public ICLRAssemblyReferenceList
{
private:
	ULONG m_ulRefCount;

public:
	ArcadeAppCLRAssemblyReferenceList();
	virtual ~ArcadeAppCLRAssemblyReferenceList();

private:
	ArcadeAppCLRAssemblyReferenceList(const ArcadeAppCLRAssemblyReferenceList&);
	ArcadeAppCLRAssemblyReferenceList& operator = (const ArcadeAppCLRAssemblyReferenceList&);

	// IUnknown
public:
	virtual ULONG STDMETHODCALLTYPE AddRef();
	virtual ULONG STDMETHODCALLTYPE Release();
	virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

	// ICLRAssemblyReferenceList
public:
	virtual HRESULT STDMETHODCALLTYPE IsStringAssemblyReferenceInList(LPCWSTR pwzAssemblyName);
	virtual HRESULT STDMETHODCALLTYPE IsAssemblyReferenceInList(IUnknown* pUnknown);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
