/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppActionOnCLREvent : public IActionOnCLREvent
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppActionOnCLREvent();
    virtual ~ArcadeAppActionOnCLREvent();

private:
    ArcadeAppActionOnCLREvent(const ArcadeAppActionOnCLREvent&);
    ArcadeAppActionOnCLREvent& operator = (const ArcadeAppActionOnCLREvent&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IActionOnCLREvent
public:
    virtual HRESULT STDMETHODCALLTYPE OnEvent(EClrEvent event, PVOID pvData);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
