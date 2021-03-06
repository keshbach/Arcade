/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostThreadpoolManager : public IHostThreadpoolManager
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostThreadpoolManager();
    virtual ~ArcadeAppHostThreadpoolManager();

private:
    ArcadeAppHostThreadpoolManager(const ArcadeAppHostThreadpoolManager&);
    ArcadeAppHostThreadpoolManager& operator = (const ArcadeAppHostThreadpoolManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostThreadpoolManager
public:
    virtual HRESULT STDMETHODCALLTYPE QueueUserWorkItem(LPTHREAD_START_ROUTINE pFunction, PVOID pvContext, ULONG Flags);
    virtual HRESULT STDMETHODCALLTYPE SetMaxThreads(DWORD dwMaxWorkerThreads);
    virtual HRESULT STDMETHODCALLTYPE GetMaxThreads(DWORD* pdwMaxWorkerThreads);
    virtual HRESULT STDMETHODCALLTYPE GetAvailableThreads(DWORD* pdwAvailableWorkerThreads);
    virtual HRESULT STDMETHODCALLTYPE SetMinThreads(DWORD dwMinIOCompletionThreads);
    virtual HRESULT STDMETHODCALLTYPE GetMinThreads(DWORD* pdwMinIOCompletionThreads);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
