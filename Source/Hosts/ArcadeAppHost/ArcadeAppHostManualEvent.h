/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostManualEvent : public IHostManualEvent
{
private:
    ULONG m_ulRefCount;
    SIZE_T m_Cookie;
    HANDLE m_hEvent;

public:
    ArcadeAppHostManualEvent(BOOL bInitialState, SIZE_T Cookie);
    virtual ~ArcadeAppHostManualEvent();

private:
    ArcadeAppHostManualEvent();
    ArcadeAppHostManualEvent(const ArcadeAppHostManualEvent&);
    ArcadeAppHostManualEvent& operator = (const ArcadeAppHostManualEvent&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostManualEvent
public:
    virtual HRESULT STDMETHODCALLTYPE Wait(DWORD dwMilliseconds, DWORD option);
    virtual HRESULT STDMETHODCALLTYPE Reset(void);
    virtual HRESULT STDMETHODCALLTYPE Set(void);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
