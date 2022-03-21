/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostTaskManager.h"

#include <new>

#include "ArcadeAppHostTask.h"

#include "UtArcadeAppHostTasks.h"
#include "UtArcadeAppHostUtility.h"

ArcadeAppHostTaskManager::ArcadeAppHostTaskManager()
{
    m_ulRefCount = 0;
    m_pCLRTaskManager = NULL;
}

ArcadeAppHostTaskManager::~ArcadeAppHostTaskManager()
{
    if (m_pCLRTaskManager)
    {
        m_pCLRTaskManager->Release();

        m_pCLRTaskManager = NULL;
    }
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostTaskManager::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostTaskManager::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
        delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::QueryInterface(
  const IID& iid,
  void** ppv)
{
    iid;

    if (ppv == NULL)
    {
        return E_POINTER;
    }

    return E_NOINTERFACE;
}

#pragma endregion

#pragma region "IHostTaskManager"

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::GetCurrentTask(
  IHostTask** ppTask)
{
	ArcadeAppHostTask* pArcadeAppHostTask;

    if (ppTask == NULL)
    {
        return E_POINTER;
    }

	if (FALSE == UtArcadeAppHostTasksFind(::GetCurrentThreadId(), &pArcadeAppHostTask))
	{
		return E_FAIL;
	}

	if (pArcadeAppHostTask == NULL)
	{
		if (FALSE == UtArcadeAppHostTasksCreate(::GetCurrentThreadId(), &pArcadeAppHostTask))
		{
			return E_FAIL;
		}
	}

	pArcadeAppHostTask->AddRef();

	*ppTask = pArcadeAppHostTask;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::CreateTask(
  DWORD dwStackSize,
  LPTHREAD_START_ROUTINE pStartAddress,
  PVOID pvParameter,
  IHostTask** ppTask)
{
    if (ppTask == NULL)
    {
        return E_POINTER;
    }

	if (FALSE == UtArcadeAppHostTasksCreate(dwStackSize, pStartAddress, pvParameter, ppTask))
	{
		return E_FAIL;
	}

	(*ppTask)->AddRef();

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::Sleep(
  DWORD dwMilliseconds,
  DWORD dwOption)
{
	return UtArcadeAppHostUtilitySleep(dwMilliseconds, dwOption);
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::SwitchToTask(
  DWORD dwOption)
{
    dwOption;

	::SwitchToThread();

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::SetUILocale(
  LCID lcid)
{
    lcid;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::SetLocale(
  LCID lcid)
{
    lcid;

	if (!::SetThreadLocale(lcid)) {
		return E_FAIL;
	}

	return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::CallNeedsHostHook(
  SIZE_T target,
  BOOL* pbCallNeedsHostHook)
{
    target;

    if (pbCallNeedsHostHook == NULL)
    {
        return E_POINTER;
    }

    *pbCallNeedsHostHook = FALSE;
    
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::LeaveRuntime(
  SIZE_T target)
{
    target;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::EnterRuntime(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::ReverseLeaveRuntime(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::ReverseEnterRuntime(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::BeginDelayAbort(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::EndDelayAbort(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::BeginThreadAffinity(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::EndThreadAffinity(void)
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::SetStackGuarantee(
  ULONG guarantee)
{
    guarantee;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::GetStackGuarantee(
  ULONG* pGuarantee)
{
	if (pGuarantee == NULL)
	{
		return E_POINTER;
	}

    *pGuarantee = 0;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTaskManager::SetCLRTaskManager(
  ICLRTaskManager* pManager)
{
    if (m_pCLRTaskManager)
    {
        m_pCLRTaskManager->Release();
    }

    m_pCLRTaskManager = pManager;

    m_pCLRTaskManager->AddRef();

    return S_OK;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
