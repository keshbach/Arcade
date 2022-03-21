/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostIOCompletionManager : public IHostIoCompletionManager
{
private:
    ULONG m_ulRefCount;
    ICLRIoCompletionManager* m_pCLRIoCompletionManager;

public:
    ArcadeAppHostIOCompletionManager();
    virtual ~ArcadeAppHostIOCompletionManager();

private:
    ArcadeAppHostIOCompletionManager(const ArcadeAppHostIOCompletionManager&);
    ArcadeAppHostIOCompletionManager& operator = (const ArcadeAppHostIOCompletionManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostIoCompletionManager
public:
    virtual HRESULT STDMETHODCALLTYPE CreateIoCompletionPort(HANDLE* phPort);
    virtual HRESULT STDMETHODCALLTYPE CloseIoCompletionPort(HANDLE hPort);
    virtual HRESULT STDMETHODCALLTYPE SetMaxThreads(DWORD dwMaxIOCompletionThreads);
    virtual HRESULT STDMETHODCALLTYPE GetMaxThreads(DWORD* pdwMaxIOCompletionThreads);
    virtual HRESULT STDMETHODCALLTYPE GetAvailableThreads(DWORD* pdwAvailableIOCompletionThreads);
    virtual HRESULT STDMETHODCALLTYPE GetHostOverlappedSize(DWORD* pcbSize);
    virtual HRESULT STDMETHODCALLTYPE SetCLRIoCompletionManager(ICLRIoCompletionManager* pManager);
    virtual HRESULT STDMETHODCALLTYPE InitializeHostOverlapped(void* pvOverlapped);
    virtual HRESULT STDMETHODCALLTYPE Bind(HANDLE hPort, HANDLE hHandle);
    virtual HRESULT STDMETHODCALLTYPE SetMinThreads(DWORD dwMinIOCompletionThreads);
    virtual HRESULT STDMETHODCALLTYPE GetMinThreads(DWORD* pdwMinIOCompletionThreads);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
