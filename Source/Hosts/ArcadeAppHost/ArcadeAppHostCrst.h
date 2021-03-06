/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostCrst : public IHostCrst
{
private:
    ULONG m_ulRefCount;
    CRITICAL_SECTION m_CriticalSection;
    BOOL m_bValid;

public:
    ArcadeAppHostCrst();
    ArcadeAppHostCrst(DWORD dwSpinCount);
    virtual ~ArcadeAppHostCrst();

private:
    ArcadeAppHostCrst(const ArcadeAppHostCrst&);
    ArcadeAppHostCrst& operator = (const ArcadeAppHostCrst&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostCrst
public:
    virtual HRESULT STDMETHODCALLTYPE Enter(DWORD option);
    virtual HRESULT STDMETHODCALLTYPE Leave(void);
    virtual HRESULT STDMETHODCALLTYPE TryEnter(DWORD option, BOOL* pbSucceeded);
    virtual HRESULT STDMETHODCALLTYPE SetSpinCount(DWORD dwSpinCount);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
