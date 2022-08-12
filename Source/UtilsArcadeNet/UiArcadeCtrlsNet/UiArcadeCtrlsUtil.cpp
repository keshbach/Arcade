/***************************************************************************/
/*  Copyright (C) 2010-2022 Kevin Eshbach                                  */
/***************************************************************************/

#include <Windows.h>

#pragma warning(push)
#pragma warning(disable : 4458) 
#include <gdiplus.h>
#pragma warning(pop)

#include <new>

#include "UiArcadeCtrlsUtil.h"

#include "ImageStream.h"

#include "resource.h"

#pragma region "Local Variables"

static HINSTANCE l_hInstance = NULL;

static ULONG_PTR l_pulToken = NULL;

#pragma endregion

#pragma region "Local Functions"

static BOOL lFindBitmapResource(
  _In_ INT nResourceId,
  LPCVOID* ppvData,
  LPDWORD pdwDataLen)
{
	HRSRC hResource;
	HGLOBAL hResData;

	*ppvData = NULL;
	*pdwDataLen = 0;

	hResource = ::FindResource(UiArcadeCtrlsGetInstance(),
                               MAKEINTRESOURCE(nResourceId),
                               MAKEINTRESOURCE(RT_TRANSPARENTBITMAP));

	if (hResource == NULL)
	{
		return FALSE;
	}

	*pdwDataLen = ::SizeofResource(UiArcadeCtrlsGetInstance(), hResource);

	hResData = ::LoadResource(UiArcadeCtrlsGetInstance(), hResource);

	if (hResData == NULL)
	{
		return FALSE;
	}

	*ppvData = ::LockResource(hResData);

	return TRUE;
}

#pragma endregion

VOID UiArcadeCtrlsSetInstance(
  _In_ HINSTANCE hInstance)
{
	l_hInstance = hInstance;
}

HINSTANCE UiArcadeCtrlsGetInstance(VOID)
{
	return l_hInstance;
}

HANDLE UiArcadeCtrlsLoadImage(
  _In_ INT nResourceId)
{
	LPCVOID pvData;
	DWORD dwDataLen;
	ImageStream* pImageStream;
	Gdiplus::GpBitmap* pBitmap;
	Gdiplus::GpStatus Status;

	if (FALSE == lFindBitmapResource(nResourceId, &pvData, &dwDataLen))
	{
		return NULL;
	}
	
	pImageStream = new (std::nothrow) ImageStream(pvData, dwDataLen);

	if (pImageStream == nullptr)
	{
		return NULL;
	}

	pImageStream->AddRef();

	Status = Gdiplus::DllExports::GdipCreateBitmapFromStream(pImageStream, &pBitmap);

	pImageStream->Release();

	switch (Status)
	{
		case Gdiplus::Ok:
	    	return pBitmap;
	}

	return NULL;
}

BOOL UiArcadeCtrlsDeleteImage(
  _In_ HANDLE hImage)
{
	Gdiplus::GpBitmap* pBitmap = (Gdiplus::GpBitmap*)hImage;
	Gdiplus::GpStatus Status;

	Status = Gdiplus::DllExports::GdipDisposeImage(pBitmap);

	switch (Status)
	{
		case Gdiplus::Ok:
			return TRUE;
	}

	return FALSE;
}

PHANDLE UiArcadeCtrlsAllocImages(
  _In_ INT nTotalImages)
{
	return new (std::nothrow) HANDLE[nTotalImages];
}

VOID UiArcadeCtrlsFreeImages(
  _In_ PHANDLE phImages)
{
	delete phImages;
}

BOOL UiArcadeCtrlsDrawImage(
  _In_ HDC hDC,
  _In_ HANDLE hImage,
  _In_ LPPOINT pPoint)
{
	Gdiplus::GpBitmap* pBitmap = (Gdiplus::GpBitmap*)hImage;
	Gdiplus::GpGraphics *pGraphics;
	Gdiplus::GpStatus Status;

	Status = Gdiplus::DllExports::GdipCreateFromHDC(hDC, &pGraphics);

	switch (Status)
	{
		case Gdiplus::Ok:
			break;
		default:
			return FALSE;
	}

	Status = Gdiplus::DllExports::GdipDrawImageI(pGraphics, pBitmap, pPoint->x, pPoint->y);

	switch (Status)
	{
		case Gdiplus::Ok:
			break;
		default:
			return FALSE;
	}

	Status = Gdiplus::DllExports::GdipDeleteGraphics(pGraphics);

	switch (Status)
	{
		case Gdiplus::Ok:
			break;
		default:
			return FALSE;
	}

	return TRUE;
}

VOID UiArcadeCtrlsStartup(VOID)
{
	Gdiplus::GpStatus Status;
	Gdiplus::GdiplusStartupInput StartupInput;
	Gdiplus::GdiplusStartupOutput StartupOutput;

	StartupInput.GdiplusVersion = 1;
	StartupInput.DebugEventCallback  = NULL;
	StartupInput.SuppressBackgroundThread = FALSE;
	StartupInput.SuppressExternalCodecs = FALSE;

	Status = Gdiplus::GdiplusStartup(&l_pulToken, &StartupInput, &StartupOutput);

	switch (Status)
	{
		case Gdiplus::Ok:
			break;
	}
}

VOID UiArcadeCtrlsShutdown(VOID)
{
	Gdiplus::GdiplusShutdown(l_pulToken);

	l_pulToken = NULL;
}

/***************************************************************************/
/*  Copyright (C) 2010-2022 Kevin Eshbach                                  */
/***************************************************************************/
