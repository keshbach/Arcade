/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostPolicyManager : public IHostPolicyManager
{
private:
    ULONG m_ulRefCount;

public:
    ArcadeAppHostPolicyManager();
    virtual ~ArcadeAppHostPolicyManager();

private:
    ArcadeAppHostPolicyManager(const ArcadeAppHostPolicyManager&);
    ArcadeAppHostPolicyManager& operator = (const ArcadeAppHostPolicyManager&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IHostPolicyManager
public:
    virtual HRESULT STDMETHODCALLTYPE OnDefaultAction(EClrOperation operation, EPolicyAction action);
    virtual HRESULT STDMETHODCALLTYPE OnTimeout(EClrOperation operation, EPolicyAction action);
    virtual HRESULT STDMETHODCALLTYPE OnFailure(EClrFailure failure, EPolicyAction action);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
