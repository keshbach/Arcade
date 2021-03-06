/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostTaskManager : public IHostTaskManager
{
private:
    ULONG m_ulRefCount;
    ICLRTaskManager* m_pCLRTaskManager;

public:
    ArcadeAppHostTaskManager();
    virtual ~ArcadeAppHostTaskManager();

private:
    ArcadeAppHostTaskManager(const ArcadeAppHostTaskManager&);
    ArcadeAppHostTaskManager& operator = (const ArcadeAppHostTaskManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostTaskManager
public:
    virtual HRESULT STDMETHODCALLTYPE GetCurrentTask(IHostTask** ppTask);
    virtual HRESULT STDMETHODCALLTYPE CreateTask(DWORD dwStackSize, LPTHREAD_START_ROUTINE pStartAddress, PVOID pvParameter, IHostTask** ppTask);
    virtual HRESULT STDMETHODCALLTYPE Sleep(DWORD dwMilliseconds, DWORD dwOption);
    virtual HRESULT STDMETHODCALLTYPE SwitchToTask(DWORD dwOption);
    virtual HRESULT STDMETHODCALLTYPE SetUILocale(LCID lcid);
    virtual HRESULT STDMETHODCALLTYPE SetLocale(LCID lcid);
    virtual HRESULT STDMETHODCALLTYPE CallNeedsHostHook(SIZE_T target, BOOL* pbCallNeedsHostHook);
    virtual HRESULT STDMETHODCALLTYPE LeaveRuntime(SIZE_T target);
    virtual HRESULT STDMETHODCALLTYPE EnterRuntime(void);
    virtual HRESULT STDMETHODCALLTYPE ReverseLeaveRuntime(void);
    virtual HRESULT STDMETHODCALLTYPE ReverseEnterRuntime(void);
    virtual HRESULT STDMETHODCALLTYPE BeginDelayAbort(void);
    virtual HRESULT STDMETHODCALLTYPE EndDelayAbort(void);
    virtual HRESULT STDMETHODCALLTYPE BeginThreadAffinity(void);
    virtual HRESULT STDMETHODCALLTYPE EndThreadAffinity(void);
    virtual HRESULT STDMETHODCALLTYPE SetStackGuarantee(ULONG guarantee);
    virtual HRESULT STDMETHODCALLTYPE GetStackGuarantee(ULONG* pGuarantee);
    virtual HRESULT STDMETHODCALLTYPE SetCLRTaskManager(ICLRTaskManager* pManager);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
