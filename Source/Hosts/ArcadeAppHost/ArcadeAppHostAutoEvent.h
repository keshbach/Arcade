/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostAutoEvent : public IHostAutoEvent
{
private:
    ULONG m_ulRefCount;
    SIZE_T m_Cookie;
    HANDLE m_hEvent;

public:
    ArcadeAppHostAutoEvent(SIZE_T Cookie);
    virtual ~ArcadeAppHostAutoEvent();

private:
    ArcadeAppHostAutoEvent();
    ArcadeAppHostAutoEvent(const ArcadeAppHostAutoEvent&);
    ArcadeAppHostAutoEvent& operator = (const ArcadeAppHostAutoEvent&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostAutoEvent
public:
    virtual HRESULT STDMETHODCALLTYPE Wait(DWORD dwMilliseconds, DWORD option);
    virtual HRESULT STDMETHODCALLTYPE Set(void);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
