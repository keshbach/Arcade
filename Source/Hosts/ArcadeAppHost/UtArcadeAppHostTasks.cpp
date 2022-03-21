/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include <new>

#include "ArcadeAppHostTask.h"

#include "Includes/UtMacros.h"

#pragma region "Constants"

#define CMaxArcadeAppHostTaskData 20

#pragma endregion

#pragma region "Structures"

typedef struct tagTArcadeAppTaskData
{
	DWORD dwThreadId;
	ArcadeAppHostTask* pArcadeAppHostTask;
} TArcadeAppHostTaskData;

#pragma endregion

#pragma region "Local Variables"

static TArcadeAppHostTaskData l_ArcadeAppHostTaskData[CMaxArcadeAppHostTaskData] = {0};
static INT l_nArcadeAppHostTaskDataLen = 0;

static CRITICAL_SECTION l_CriticalSection = {NULL};

#pragma endregion

BOOL UtArcadeAppHostTasksInitialize()
{
	::InitializeCriticalSection(&l_CriticalSection);

	return TRUE;
}

BOOL UtArcadeAppHostTasksUninitialize()
{
	::DeleteCriticalSection(&l_CriticalSection);

	return TRUE;
}

BOOL UtArcadeAppHostTasksCreate(
  DWORD dwThreadId,
  ArcadeAppHostTask** ppHostTask)
{
	BOOL bResult = FALSE;
	TArcadeAppHostTaskData* pArcadeAppHostTaskData;

	::EnterCriticalSection(&l_CriticalSection);

	if (l_nArcadeAppHostTaskDataLen + 1 > MArrayLen(l_ArcadeAppHostTaskData))
	{
		::LeaveCriticalSection(&l_CriticalSection);

		return FALSE;
	}

	pArcadeAppHostTaskData = &l_ArcadeAppHostTaskData[l_nArcadeAppHostTaskDataLen];

	pArcadeAppHostTaskData->dwThreadId = dwThreadId;
	pArcadeAppHostTaskData->pArcadeAppHostTask = new (std::nothrow) ArcadeAppHostTask(dwThreadId);

	if (pArcadeAppHostTaskData->pArcadeAppHostTask != NULL)
	{
		++l_nArcadeAppHostTaskDataLen;

		*ppHostTask = pArcadeAppHostTaskData->pArcadeAppHostTask;

		bResult = TRUE;
	}

	::LeaveCriticalSection(&l_CriticalSection);

	return bResult;
}

BOOL UtArcadeAppHostTasksCreate(
  DWORD dwStackSize,
  LPTHREAD_START_ROUTINE pStartAddress,
  PVOID pvParameter,
  IHostTask** ppHostTask)
{
	TArcadeAppHostTaskData* pArcadeAppHostTaskData;

	::EnterCriticalSection(&l_CriticalSection);

	if (l_nArcadeAppHostTaskDataLen + 1 > MArrayLen(l_ArcadeAppHostTaskData))
	{
		::LeaveCriticalSection(&l_CriticalSection);

		return FALSE;
	}

	pArcadeAppHostTaskData = &l_ArcadeAppHostTaskData[l_nArcadeAppHostTaskDataLen];

	pArcadeAppHostTaskData->pArcadeAppHostTask = new (std::nothrow) ArcadeAppHostTask(dwStackSize, pStartAddress, pvParameter);

	if (pArcadeAppHostTaskData->pArcadeAppHostTask == NULL)
	{
		::LeaveCriticalSection(&l_CriticalSection);

		return FALSE;
	}

	if (pArcadeAppHostTaskData->pArcadeAppHostTask->GetThreadId() == 0)
	{
		delete pArcadeAppHostTaskData->pArcadeAppHostTask;

		::LeaveCriticalSection(&l_CriticalSection);

		return FALSE;
	}

	pArcadeAppHostTaskData->dwThreadId = pArcadeAppHostTaskData->pArcadeAppHostTask->GetThreadId();

	++l_nArcadeAppHostTaskDataLen;

	*ppHostTask = pArcadeAppHostTaskData->pArcadeAppHostTask;

	::LeaveCriticalSection(&l_CriticalSection);

	return TRUE;
}

BOOL UtArcadeAppHostTasksDestroy(
  DWORD dwThreadId)
{
	::EnterCriticalSection(&l_CriticalSection);

	for (INT nIndex = 0; nIndex < l_nArcadeAppHostTaskDataLen; ++nIndex)
	{
		if (l_ArcadeAppHostTaskData[nIndex].dwThreadId == dwThreadId)
		{
			::MoveMemory(&l_ArcadeAppHostTaskData[nIndex],
                                     &l_ArcadeAppHostTaskData[nIndex + 1],
                                     (l_nArcadeAppHostTaskDataLen - (nIndex + 1)) * sizeof(TArcadeAppHostTaskData));

#if !defined(NDEBUG)
			::ZeroMemory(&l_ArcadeAppHostTaskData[l_nArcadeAppHostTaskDataLen - 1],
                         sizeof(TArcadeAppHostTaskData));
#endif

			--l_nArcadeAppHostTaskDataLen;

			::LeaveCriticalSection(&l_CriticalSection);

			return TRUE;
		}
	}

	::LeaveCriticalSection(&l_CriticalSection);

	return FALSE;
}

BOOL UtArcadeAppHostTasksFind(
  DWORD dwThreadId,
  ArcadeAppHostTask** ppHostTask)
{
	*ppHostTask = NULL;

	::EnterCriticalSection(&l_CriticalSection);

	for (INT nIndex = 0; nIndex < l_nArcadeAppHostTaskDataLen; ++nIndex)
	{
		if (l_ArcadeAppHostTaskData[nIndex].dwThreadId == dwThreadId)
		{
			*ppHostTask = l_ArcadeAppHostTaskData[nIndex].pArcadeAppHostTask;

			(*ppHostTask)->AddRef();

			::LeaveCriticalSection(&l_CriticalSection);

			return TRUE;
		}
	}

	::LeaveCriticalSection(&l_CriticalSection);

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
