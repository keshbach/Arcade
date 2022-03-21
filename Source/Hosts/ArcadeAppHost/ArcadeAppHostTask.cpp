/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostTask.h"

#include "UtArcadeAppHostTasks.h"
#include "UtArcadeAppHostUtility.h"

#pragma region "Local Functions"

static VOID NTAPI lArcadeAppHostTaskAsynchronousProcedureCall(
  _In_ ULONG_PTR pParameter)
{
	pParameter;
}

#pragma endregion

ArcadeAppHostTask::ArcadeAppHostTask(
  DWORD dwThreadId)
{
	m_ulRefCount = 0;
	m_pCLRTask = NULL;
	m_hThread = NULL;
	m_dwThreadId = dwThreadId;
	m_pStartAddress = NULL;
	m_pvParameter = NULL;

	if (FALSE == ::DuplicateHandle(::GetCurrentProcess(), ::GetCurrentThread(),
                                   ::GetCurrentProcess(), &m_hThread,
                                   0, FALSE, DUPLICATE_SAME_ACCESS))
	{
		// How handle this?
	}
}

ArcadeAppHostTask::ArcadeAppHostTask(
  DWORD dwStackSize,
  LPTHREAD_START_ROUTINE pStartAddress,
  PVOID pvParameter)
{
	m_ulRefCount = 0;
    m_pCLRTask = NULL;
	m_hThread = NULL;
	m_dwThreadId = 0;
	m_pStartAddress = pStartAddress;
	m_pvParameter = pvParameter;

	m_hThread = ::CreateThread(NULL, dwStackSize, ThreadTask, this,
                               CREATE_SUSPENDED | STACK_SIZE_PARAM_IS_A_RESERVATION,
                               &m_dwThreadId);
}

ArcadeAppHostTask::~ArcadeAppHostTask()
{
    if (m_hThread)
    {
        ::CloseHandle(m_hThread);
    }

    if (m_pCLRTask)
    {
        m_pCLRTask->Release();

        m_pCLRTask = NULL;
    }

	if (m_pStartAddress == NULL)
	{
		UtArcadeAppHostTasksDestroy(m_dwThreadId);
	}
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ArcadeAppHostTask::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ArcadeAppHostTask::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
		UtArcadeAppHostTasksDestroy(m_dwThreadId);
		
		delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::QueryInterface(
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

#pragma region "IHostTask"

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::Start(void)
{
    if (m_hThread)
    {
        if (::ResumeThread(m_hThread))
        {
            return S_OK;
        }
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::Alert(void)
{
    if (m_hThread)
    {
		if (::QueueUserAPC(lArcadeAppHostTaskAsynchronousProcedureCall, m_hThread, NULL))
		{
			return S_OK;
		}
    }

	return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::Join(
  DWORD dwMilliseconds,
  DWORD dwOption)
{
    if (m_hThread)
    {
		return UtArcadeAppHostUtilityWait(m_hThread, dwMilliseconds, dwOption);
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::SetPriority(
  int newPriority)
{
    if (m_hThread)
    {
        if (::SetThreadPriority(m_hThread, newPriority))
        {
            return S_OK;
        }
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::GetPriority(
  int* pPriority)
{
	if (pPriority == NULL)
	{
		return E_POINTER;
	}

    if (m_hThread)
    {
        *pPriority = ::GetThreadPriority(m_hThread);

        if (*pPriority != THREAD_PRIORITY_ERROR_RETURN)
        {
            return S_OK;
        }
    }

    return E_FAIL;
}

HRESULT STDMETHODCALLTYPE ArcadeAppHostTask::SetCLRTask(
  ICLRTask* pCLRTask)
{
    if (m_pCLRTask)
    {
        m_pCLRTask->Release();
    }

    m_pCLRTask = pCLRTask;

    m_pCLRTask->AddRef();

    return S_OK;
}

#pragma endregion

DWORD WINAPI ArcadeAppHostTask::ThreadTask(
  _In_ LPVOID pvParameter)
{
	ArcadeAppHostTask* pThis = (ArcadeAppHostTask*)pvParameter;
	DWORD dwResult;

	dwResult = pThis->m_pStartAddress(pThis->m_pvParameter);

	return dwResult;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
