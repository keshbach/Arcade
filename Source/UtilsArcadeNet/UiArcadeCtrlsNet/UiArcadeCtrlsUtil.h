/***************************************************************************/
/*  Copyright (C) 2010-2022 Kevin Eshbach                                  */
/***************************************************************************/

#if !defined(UiArcadeCtrlsUtil_H)
#define UiArcadeCtrlsUtil_H

VOID UiArcadeCtrlsSetInstance(_In_ HINSTANCE hInstance);
HINSTANCE UiArcadeCtrlsGetInstance(VOID);

HANDLE UiArcadeCtrlsLoadImage(_In_ INT nResourceId);
BOOL UiArcadeCtrlsDeleteImage(_In_ HANDLE hImage);

PHANDLE UiArcadeCtrlsAllocImages(_In_ INT nTotalImages);
VOID UiArcadeCtrlsFreeImages(_In_ PHANDLE phImages);

BOOL UiArcadeCtrlsDrawImage(_In_ HDC hDC, _In_ HANDLE hImage, _In_ LPPOINT pPoint);

VOID UiArcadeCtrlsStartup(VOID);

VOID UiArcadeCtrlsShutdown(VOID);

#endif

/***************************************************************************/
/*  Copyright (C) 2010-2022 Kevin Eshbach                                  */
/***************************************************************************/
