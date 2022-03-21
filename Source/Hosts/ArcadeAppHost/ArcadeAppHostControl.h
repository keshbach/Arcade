/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostControl : public IHostControl
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostControl();
    virtual ~ArcadeAppHostControl();

private:
    ArcadeAppHostControl(const ArcadeAppHostControl&);
    ArcadeAppHostControl& operator = (const ArcadeAppHostControl&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostControl
public:
    virtual HRESULT STDMETHODCALLTYPE GetHostManager(REFIID riid, void** ppObject);
    virtual HRESULT STDMETHODCALLTYPE SetAppDomainManager(DWORD dwAppDomainID, IUnknown* pUnkAppDomainManager);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
