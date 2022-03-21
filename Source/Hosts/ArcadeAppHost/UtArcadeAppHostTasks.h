/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ArcadeAppHostTask;

BOOL UtArcadeAppHostTasksInitialize();
BOOL UtArcadeAppHostTasksUninitialize();

BOOL UtArcadeAppHostTasksCreate(DWORD dwThreadId, ArcadeAppHostTask** ppHostTask);

BOOL UtArcadeAppHostTasksCreate(DWORD dwStackSize, LPTHREAD_START_ROUTINE pStartAddress, PVOID pvParameter, IHostTask** ppHostTask);

BOOL UtArcadeAppHostTasksDestroy(DWORD dwThreadId);

BOOL UtArcadeAppHostTasksFind(DWORD dwThreadId, ArcadeAppHostTask** ppHostTask);

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
