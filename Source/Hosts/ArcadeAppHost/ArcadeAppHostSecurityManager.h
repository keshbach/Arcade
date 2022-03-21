/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostSecurityManager : public IHostSecurityManager
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostSecurityManager();
    virtual ~ArcadeAppHostSecurityManager();

private:
    ArcadeAppHostSecurityManager(const ArcadeAppHostSecurityManager&);
    ArcadeAppHostSecurityManager& operator = (const ArcadeAppHostSecurityManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostSecurityManager
public:
    virtual HRESULT STDMETHODCALLTYPE ImpersonateLoggedOnUser(HANDLE hToken);
    virtual HRESULT STDMETHODCALLTYPE RevertToSelf(void);
    virtual HRESULT STDMETHODCALLTYPE OpenThreadToken(DWORD dwDesiredAccess, BOOL bOpenAsSelf, HANDLE* phThreadToken);
    virtual HRESULT STDMETHODCALLTYPE SetThreadToken(HANDLE hToken);
    virtual HRESULT STDMETHODCALLTYPE GetSecurityContext(EContextType eContextType, IHostSecurityContext** ppSecurityContext);
    virtual HRESULT STDMETHODCALLTYPE SetSecurityContext(EContextType eContextType, IHostSecurityContext* pSecurityContext);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
