/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostGCManager : public IHostGCManager
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostGCManager();
    virtual ~ArcadeAppHostGCManager();

private:
    ArcadeAppHostGCManager(const ArcadeAppHostGCManager&);
    ArcadeAppHostGCManager& operator = (const ArcadeAppHostGCManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostGCManager
public:
    virtual HRESULT STDMETHODCALLTYPE ThreadIsBlockingForSuspension(void);
    virtual HRESULT STDMETHODCALLTYPE SuspensionStarting(void);
    virtual HRESULT STDMETHODCALLTYPE SuspensionEnding(DWORD dwGeneration);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
