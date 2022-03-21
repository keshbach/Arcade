/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "UtControlsHelper.h"

#include "Includes/UtMacros.h"

#pragma unmanaged

BOOL UtControlsHelperIsEditControl(
  _In_ HWND hWnd)
{
    TCHAR cClassName[10];

    ::GetClassName(hWnd, cClassName, MArrayLen(cClassName));

    return (::lstrcmpi(cClassName, TEXT("edit")) == 0) ? TRUE : FALSE;
}

#pragma managed

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
