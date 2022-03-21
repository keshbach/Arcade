/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostAssemblyManager : public IHostAssemblyManager
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostAssemblyManager();
    virtual ~ArcadeAppHostAssemblyManager();

private:
    ArcadeAppHostAssemblyManager(const ArcadeAppHostAssemblyManager&);
    ArcadeAppHostAssemblyManager& operator = (const ArcadeAppHostAssemblyManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostAssemblyManager
public:
    virtual HRESULT STDMETHODCALLTYPE GetNonHostStoreAssemblies(ICLRAssemblyReferenceList** ppReferenceList);
    virtual HRESULT STDMETHODCALLTYPE GetAssemblyStore(IHostAssemblyStore** ppAssemblyStore);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
