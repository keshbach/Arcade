/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostSecurityContext : public IHostSecurityContext
{
private:
    ULONG m_ulRefCount;
    EContextType m_ContextType;

public:
    ArcadeAppHostSecurityContext(EContextType ContextType);
    virtual ~ArcadeAppHostSecurityContext();

private:
    ArcadeAppHostSecurityContext();
    ArcadeAppHostSecurityContext(const ArcadeAppHostSecurityContext&);
    ArcadeAppHostSecurityContext& operator = (const ArcadeAppHostSecurityContext&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostSecurityContext
public:
    virtual HRESULT STDMETHODCALLTYPE Capture(IHostSecurityContext** ppClonedContext);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
