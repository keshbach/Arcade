/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Form.h"

#include "UiArcadeCtrlsUtil.h"

#include "resource.h"

#include <Includes/UiMacros.h>
#include <Includes/UtMacros.h>

#include <wincodec.h>
#include <d2d1.h>
#include <d2d1_1.h>

#define IDT_SPINNER_TIMER 1

#define CMillisecondsPerSecond 1000

#pragma region "Location Functions"

#pragma unmanaged

static HBITMAP lCreateScreenshot(
  _In_ HWND hWnd)
{
	HDC hScreenDC, hWindowDC, hMemDC, hClientMemDC;
	HBITMAP hBitmap, hClientBitmap;
	RECT Rect, ClientRect;
	POINT Point;

	Point.x = 0;
	Point.y = 0;

	::ClientToScreen(hWnd, &Point);

	::GetWindowRect(hWnd, &Rect);
	::GetClientRect(hWnd, &ClientRect);

	hScreenDC = ::GetDC(NULL);
	hWindowDC = ::GetDC(hWnd);

	hMemDC = ::CreateCompatibleDC(hWindowDC);

	if (hMemDC == NULL)
	{
		::ReleaseDC(NULL, hScreenDC);
		::ReleaseDC(hWnd, hWindowDC);

		return NULL;
	}

	hClientMemDC = ::CreateCompatibleDC(hWindowDC);

	if (hClientMemDC == NULL)
	{
		::DeleteDC(hMemDC);

		::ReleaseDC(NULL, hScreenDC);
		::ReleaseDC(hWnd, hWindowDC);

		return NULL;
	}

	hBitmap = ::CreateCompatibleBitmap(hScreenDC, MRectWidth(Rect), MRectHeight(Rect));
	hClientBitmap = ::CreateCompatibleBitmap(hScreenDC, MRectWidth(ClientRect), MRectHeight(ClientRect));

	::SaveDC(hMemDC);
	::SaveDC(hClientMemDC);

	::SelectObject(hMemDC, hBitmap);
	::SelectObject(hClientMemDC, hClientBitmap);

	// Must include the PRF_NONCLIENT flag or the drawn image will be wrong

	::SendMessage(hWnd, WM_PRINT, (WPARAM)hMemDC,
		          PRF_CHILDREN | PRF_CLIENT | PRF_NONCLIENT | PRF_ERASEBKGND);

	::BitBlt(hClientMemDC, 0, 0, MRectWidth(ClientRect), MRectHeight(ClientRect),
		     hMemDC, Point.x - Rect.left, Point.y - Rect.top, SRCCOPY);

	::RestoreDC(hMemDC, -1);
	::RestoreDC(hClientMemDC, -1);

	::DeleteDC(hMemDC);
	::DeleteDC(hClientMemDC);

	::ReleaseDC(NULL, hScreenDC);
	::ReleaseDC(hWnd, hWindowDC);

	::DeleteBitmap(hBitmap);

	return hClientBitmap;
}

static HBITMAP lCopyBitmapFromDeviceContext(
  _In_ HDC hDC)
{
	HDC hMemDC;
	HBITMAP hBitmap;
	BITMAP Bitmap;

	hBitmap = (HBITMAP)::GetCurrentObject(hDC, OBJ_BITMAP);

	if (hBitmap == NULL)
	{
		return NULL;
	}

	if (::GetObject((HANDLE)hBitmap, sizeof(Bitmap), &Bitmap) == 0)
	{
		return NULL;
	}

	hMemDC = ::CreateCompatibleDC(hDC);

	if (hMemDC == NULL)
	{
		return NULL;
	}

	hBitmap = ::CreateCompatibleBitmap(hDC, Bitmap.bmWidth, Bitmap.bmHeight);

	if (hBitmap == NULL)
	{
		::DeleteDC(hMemDC);

		return NULL;
	}

	::SaveDC(hMemDC);

	::SelectObject(hMemDC, hBitmap);

	::BitBlt(hMemDC, 0, 0, Bitmap.bmWidth, Bitmap.bmHeight, hDC, 0, 0, SRCCOPY);

	::RestoreDC(hMemDC, -1);

	::DeleteDC(hMemDC);

	return hBitmap;
}

static HBITMAP lCreateDimmedBitmap(
  _In_ HBITMAP hBitmap)
{
	const D2D1_PIXEL_FORMAT PixelFormat = D2D1::PixelFormat(
		DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE_UNKNOWN);
	const D2D1_RENDER_TARGET_PROPERTIES RenderTargetProperties = D2D1::RenderTargetProperties(
		D2D1_RENDER_TARGET_TYPE_DEFAULT, PixelFormat, 0.0f, 0.0f,
		D2D1_RENDER_TARGET_USAGE_GDI_COMPATIBLE);
	IWICImagingFactory* pImagingFactory = NULL;
	IWICBitmap* pBitmap = NULL;
	ID2D1RenderTarget* pDirect2dRenderTarget = NULL;
	ID2D1Factory* pDirect2dFactory = NULL;
	ID2D1SolidColorBrush* pDirect2dSolidColorBrush = NULL;
	ID2D1GdiInteropRenderTarget* pDirect2dGdiInteropRenderTarget = NULL;
	HDC hDirect2dDC = NULL;
	HBITMAP hDimmedBitmap = NULL;
	D2D1_COLOR_F Direct2dColor;
	D2D1_RECT_F Direct2dRect;
	RECT Rect;
	BITMAP Bitmap;

	if (::GetObject((HANDLE)hBitmap, sizeof(Bitmap), &Bitmap) == 0)
	{
		return NULL;
	}

	Direct2dColor.r = 0.0f;
	Direct2dColor.g = 0.0f;
	Direct2dColor.b = 0.0f;
	Direct2dColor.a = 0.25f;

	Direct2dRect.left = 0.0f;
	Direct2dRect.top = 0.0f;
	Direct2dRect.right = (FLOAT)Bitmap.bmWidth;
	Direct2dRect.bottom = (FLOAT)Bitmap.bmHeight;

	if (FAILED(::CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER,
                                  IID_IWICImagingFactory, (LPVOID*)&pImagingFactory)))
	{
		return NULL;
	}

	if (SUCCEEDED(pImagingFactory->CreateBitmapFromHBITMAP(hBitmap, NULL, WICBitmapIgnoreAlpha, &pBitmap)) &&
		SUCCEEDED(::D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &pDirect2dFactory)) &&
		SUCCEEDED(pDirect2dFactory->CreateWicBitmapRenderTarget(pBitmap, RenderTargetProperties, &pDirect2dRenderTarget)) &&
		SUCCEEDED(pDirect2dRenderTarget->CreateSolidColorBrush(Direct2dColor, &pDirect2dSolidColorBrush)))
	{
		pDirect2dRenderTarget->BeginDraw();

		pDirect2dRenderTarget->FillRectangle(&Direct2dRect, pDirect2dSolidColorBrush);

		if (SUCCEEDED(pDirect2dRenderTarget->QueryInterface(IID_ID2D1GdiInteropRenderTarget, (LPVOID*)&pDirect2dGdiInteropRenderTarget)) &&
			SUCCEEDED(pDirect2dGdiInteropRenderTarget->GetDC(D2D1_DC_INITIALIZE_MODE_COPY, &hDirect2dDC)))
		{
			hDimmedBitmap = lCopyBitmapFromDeviceContext(hDirect2dDC);
		}

		if (hDirect2dDC)
		{
			pDirect2dGdiInteropRenderTarget->ReleaseDC(&Rect);
		}

		pDirect2dRenderTarget->EndDraw();
	}

	if (pDirect2dGdiInteropRenderTarget)
	{
		pDirect2dGdiInteropRenderTarget->Release();
	}

	if (pDirect2dSolidColorBrush)
	{
		pDirect2dSolidColorBrush->Release();
	}

	if (pDirect2dRenderTarget)
	{
		pDirect2dRenderTarget->Release();
	}

	if (pDirect2dFactory)
	{
		pDirect2dFactory->Release();
	}

	if (pBitmap)
	{
		pBitmap->Release();
	}

	pImagingFactory->Release();

	return hDimmedBitmap;
}

#pragma managed

#pragma endregion

#pragma region "Constructor"

Arcade::Forms::Form::Form()
{
	InitializeComponent();

	m_bBusyVisible = false;
	m_SpinnerSize = ESpinnerSize::Normal;

	m_ControlVisibleDict = gcnew System::Collections::Generic::Dictionary<System::Windows::Forms::Control^, System::Boolean>();

	m_hFocusedControl = NULL;
}

#pragma endregion

#pragma region "Destructor"

Arcade::Forms::Form::~Form()
{
	if (components)
	{
		delete components;
	}
}

#pragma endregion

void Arcade::Forms::Form::OnPaintBackground(
  System::Windows::Forms::PaintEventArgs^ e)
{
	System::IntPtr hDC;
	POINT Point;

	if (!m_bBusyVisible)
	{
		System::Windows::Forms::Control::OnPaintBackground(e);
	}
	else
	{
		hDC = e->Graphics->GetHdc();

		::DrawState((HDC)hDC.ToPointer(), NULL, NULL, LPARAM(m_hBitmap), 0, 0, 0,
			        ClientSize.Width, ClientSize.Height, DST_BITMAP | DSS_NORMAL);

		Point.x = (ClientSize.Width - m_SpinnerImageSize.Width) / 2;
		Point.y = (ClientSize.Height - m_SpinnerImageSize.Height) / 2;

		UiArcadeCtrlsDrawImage((HDC)hDC.ToPointer(), m_phSpinnerImages[m_nActiveSpinnerImage], &Point);

		e->Graphics->ReleaseHdc(hDC);
	}
}

void Arcade::Forms::Form::WndProc(System::Windows::Forms::Message% Message)
{
	if (m_bBusyVisible == false)
	{
		Common::Forms::Form::WndProc(Message);
	}
	else
	{
		BusyVisibleProcessMessage(Message);
	}
}

void Arcade::Forms::Form::SetBusyVisible(
  System::Boolean bVisible)
{
	HBITMAP hBitmap;
	HDWP hDeferWindowPos, hTempDeferWindowPos;

	if (bVisible)
	{
		if (m_bBusyVisible)
		{
			return;
		}

		hBitmap = lCreateScreenshot((HWND)Handle.ToPointer());

		m_hBitmap = lCreateDimmedBitmap(hBitmap);
		m_bBusyVisible = true;
		m_nActiveSpinnerImage = 0;

		::DeleteObject(hBitmap);

		this->DoubleBuffered = true;
		this->Cursor = System::Windows::Forms::Cursors::WaitCursor;

		m_hFocusedControl = ::GetFocus();

		hDeferWindowPos = ::BeginDeferWindowPos(this->Controls->Count);

		for each (System::Windows::Forms::Control^ Control in this->Controls)
		{
			m_ControlVisibleDict->Add(Control, Control->Visible);

			if (hDeferWindowPos && Control->Visible)
			{
				hTempDeferWindowPos = ::DeferWindowPos(hDeferWindowPos,
                                                       (HWND)Control->Handle.ToPointer(),
					                                   NULL, 0, 0, 0, 0,
					                                   SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER |
					                                       SWP_NOSIZE | SWP_NOZORDER | SWP_HIDEWINDOW);

				if (hTempDeferWindowPos)
				{
					hDeferWindowPos = hTempDeferWindowPos;
				}
				else
				{
					hDeferWindowPos = NULL;

					Control->Visible = false;
				}
			}
			else
			{
				Control->Visible = false;
			}
		}

		if (hDeferWindowPos)
		{
			::EndDeferWindowPos(hDeferWindowPos);
		}

		UiArcadeCtrlsStartup();

		switch (m_SpinnerSize)
		{
			case ESpinnerSize::Small:
				LoadSpinner50PxBitmaps();
				break;
			case ESpinnerSize::Normal:
				LoadSpinner100PxBitmaps();
				break;
			case ESpinnerSize::Large:
				LoadSpinner150PxBitmaps();
				break;
			case ESpinnerSize::ExtraLarge:
				LoadSpinner200PxBitmaps();
				break;
			default:
				System::Diagnostics::Debug::Assert(false);
				break;
		}

		this->Invalidate();

		::SetTimer((HWND)Handle.ToPointer(), IDT_SPINNER_TIMER,
			       CMillisecondsPerSecond / m_nTotalSpinnerImages,
			       (TIMERPROC)NULL);
	}
	else
	{
		if (!m_bBusyVisible)
		{
			return;
		}

		::KillTimer((HWND)Handle.ToPointer(), IDT_SPINNER_TIMER);

		ReleaseSpinnerBitmaps();

		UiArcadeCtrlsShutdown();

		::DeleteObject(m_hBitmap);

		m_hBitmap = NULL;
		m_bBusyVisible = false;

		hDeferWindowPos = ::BeginDeferWindowPos(m_ControlVisibleDict->Count);

		for each (System::Collections::Generic::KeyValuePair<System::Windows::Forms::Control^, System::Boolean> ControlVisiblePair in m_ControlVisibleDict)
		{
			if (hDeferWindowPos && ControlVisiblePair.Value)
			{
				hTempDeferWindowPos = ::DeferWindowPos(hDeferWindowPos,
                                                       (HWND)ControlVisiblePair.Key->Handle.ToPointer(),
					                                   NULL, 0, 0, 0, 0,
                                                       SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOOWNERZORDER |
                                                           SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);

				if (hTempDeferWindowPos)
				{
					hDeferWindowPos = hTempDeferWindowPos;
				}
				else
				{
					hDeferWindowPos = NULL;

					ControlVisiblePair.Key->Visible = ControlVisiblePair.Value;
				}
			}
			else
			{
				ControlVisiblePair.Key->Visible = ControlVisiblePair.Value;
			}
		}

		if (hDeferWindowPos)
		{
			::EndDeferWindowPos(hDeferWindowPos);
		}

		m_ControlVisibleDict->Clear();

		if (m_hFocusedControl)
		{
			::SetFocus(m_hFocusedControl);

			m_hFocusedControl = NULL;
		}

		this->DoubleBuffered = false;
		this->Cursor = System::Windows::Forms::Cursors::Arrow;

		this->Invalidate();
	}
}

void Arcade::Forms::Form::SetSpinnerSize(
  ESpinnerSize SpinnerSize)
{
	if (m_bBusyVisible)
	{
		SetBusyVisible(false);

		m_SpinnerSize = SpinnerSize;

		SetBusyVisible(true);
	}
	else
	{
		m_SpinnerSize = SpinnerSize;
	}
}

void Arcade::Forms::Form::BusyVisibleProcessMessage(
  System::Windows::Forms::Message% Message)
{
	switch (Message.Msg)
	{
		case WM_TIMER:
			BusyVisibleProcessTimerMessage(Message);
			break;
		case WM_NCHITTEST:
			BusyVisibleProcessNonClientHitTestMessage(Message);
			break;
		case WM_NCMOUSEMOVE:
			BusyVisibleProcessNonClientMouseMoveMessage(Message);
			break;
		case WM_NCLBUTTONDOWN:
			BusyVisibleProcessNonClientLeftButtonDownMessage(Message);
			break;
		case WM_NCLBUTTONUP:
			BusyVisibleProcessNonClientLeftButtonUpMessage(Message);
			break;
		case WM_NCLBUTTONDBLCLK:
			BusyVisibleProcessNonClientLeftButtonDoubleClickMessage(Message);
			break;
		case WM_NCRBUTTONDOWN:
			BusyVisibleProcessNonClientRightButtonDownMessage(Message);
			break;
		case WM_NCRBUTTONUP:
			BusyVisibleProcessNonClientRightButtonUpMessage(Message);
			break;
		default:
			Common::Forms::Form::WndProc(Message);
			break;
	}
}

void Arcade::Forms::Form::BusyVisibleProcessTimerMessage(
  System::Windows::Forms::Message% Message)
{
	++m_nActiveSpinnerImage;

	if (m_nActiveSpinnerImage >= m_nTotalSpinnerImages)
	{
		m_nActiveSpinnerImage = 0;
	}

	::RedrawWindow((HWND)Handle.ToPointer(), NULL, NULL, RDW_ERASE | RDW_INVALIDATE);

	Message.Result = System::IntPtr(0);
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientHitTestMessage(
  System::Windows::Forms::Message% Message)
{
	Common::Forms::Form::WndProc(Message);

	switch ((int)Message.Result)
	{
		case HTBOTTOM:
		case HTBOTTOMLEFT:
		case HTBOTTOMRIGHT:
		case HTLEFT:
		case HTRIGHT:
		case HTTOP:
		case HTTOPLEFT:
		case HTTOPRIGHT:
			Message.Result = System::IntPtr(HTCAPTION);
			break;
	}
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientMouseMoveMessage(
  System::Windows::Forms::Message% Message)
{
	LRESULT Result = ::SendMessage((HWND)Handle.ToPointer(), WM_NCHITTEST, 0, (LPARAM)Message.LParam.ToPointer());

	if (Result == HTCLOSE)
	{
		::SetCursor(::LoadCursor(NULL, IDC_NO));

		Message.Result = System::IntPtr(0);
	}
	else
	{
		Common::Forms::Form::WndProc(Message);
	}
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientLeftButtonDownMessage(
  System::Windows::Forms::Message% Message)
{
	LRESULT Result = ::SendMessage((HWND)Handle.ToPointer(), WM_NCHITTEST, 0, (LPARAM)Message.LParam.ToPointer());

	if (Result == HTCLOSE)
	{
		::SetCursor(::LoadCursor(NULL, IDC_NO));

		Message.Result = System::IntPtr(0);
	}
	else
	{
		Common::Forms::Form::WndProc(Message);
	}
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientLeftButtonUpMessage(
  System::Windows::Forms::Message% Message)
{
	LRESULT Result = ::SendMessage((HWND)Handle.ToPointer(), WM_NCHITTEST, 0, (LPARAM)Message.LParam.ToPointer());

	if (Result == HTCLOSE)
	{
		::SetCursor(::LoadCursor(NULL, IDC_NO));

		Message.Result = System::IntPtr(0);
	}
	else
	{
		Common::Forms::Form::WndProc(Message);
	}
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientLeftButtonDoubleClickMessage(
  System::Windows::Forms::Message% Message)
{
	LRESULT Result = ::SendMessage((HWND)Handle.ToPointer(), WM_NCHITTEST, 0, (LPARAM)Message.LParam.ToPointer());

	if (Result == HTCLOSE)
	{
		Message.Result = System::IntPtr(0);
	}
	else
	{
		Common::Forms::Form::WndProc(Message);
	}
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientRightButtonDownMessage(
  System::Windows::Forms::Message% Message)
{
	Message.Result = System::IntPtr(0);
}

void Arcade::Forms::Form::BusyVisibleProcessNonClientRightButtonUpMessage(
  System::Windows::Forms::Message% Message)
{
	Message.Result = System::IntPtr(0);
}

void Arcade::Forms::Form::LoadSpinner50PxBitmaps()
{
	static INT nSpinnerIds[] = {
        IDB_SPINNER_50_PX_1,
        IDB_SPINNER_50_PX_2,
        IDB_SPINNER_50_PX_3,
        IDB_SPINNER_50_PX_4,
        IDB_SPINNER_50_PX_5,
        IDB_SPINNER_50_PX_6,
        IDB_SPINNER_50_PX_7,
        IDB_SPINNER_50_PX_8,
        IDB_SPINNER_50_PX_9,
        IDB_SPINNER_50_PX_10,
        IDB_SPINNER_50_PX_11,
        IDB_SPINNER_50_PX_12,
        IDB_SPINNER_50_PX_13,
        IDB_SPINNER_50_PX_14,
        IDB_SPINNER_50_PX_15,
        IDB_SPINNER_50_PX_16,
        IDB_SPINNER_50_PX_17,
        IDB_SPINNER_50_PX_18,
        IDB_SPINNER_50_PX_19,
        IDB_SPINNER_50_PX_20,
        IDB_SPINNER_50_PX_21,
        IDB_SPINNER_50_PX_22,
        IDB_SPINNER_50_PX_23,
        IDB_SPINNER_50_PX_24,
        IDB_SPINNER_50_PX_25,
        IDB_SPINNER_50_PX_26,
        IDB_SPINNER_50_PX_27,
        IDB_SPINNER_50_PX_28,
        IDB_SPINNER_50_PX_29,
        IDB_SPINNER_50_PX_30};

	LoadSpinnerBitmaps(nSpinnerIds, MArrayLen(nSpinnerIds), 50, 50);
}

void Arcade::Forms::Form::LoadSpinner100PxBitmaps()
{
	static INT nSpinnerIds[] = {
		IDB_SPINNER_100_PX_1,
		IDB_SPINNER_100_PX_2,
		IDB_SPINNER_100_PX_3,
		IDB_SPINNER_100_PX_4,
		IDB_SPINNER_100_PX_5,
		IDB_SPINNER_100_PX_6,
		IDB_SPINNER_100_PX_7,
		IDB_SPINNER_100_PX_8,
		IDB_SPINNER_100_PX_9,
		IDB_SPINNER_100_PX_10,
		IDB_SPINNER_100_PX_11,
		IDB_SPINNER_100_PX_12,
		IDB_SPINNER_100_PX_13,
		IDB_SPINNER_100_PX_14,
		IDB_SPINNER_100_PX_15,
		IDB_SPINNER_100_PX_16,
		IDB_SPINNER_100_PX_17,
		IDB_SPINNER_100_PX_18,
		IDB_SPINNER_100_PX_19,
		IDB_SPINNER_100_PX_20,
		IDB_SPINNER_100_PX_21,
		IDB_SPINNER_100_PX_22,
		IDB_SPINNER_100_PX_23,
		IDB_SPINNER_100_PX_24,
		IDB_SPINNER_100_PX_25,
		IDB_SPINNER_100_PX_26,
		IDB_SPINNER_100_PX_27,
		IDB_SPINNER_100_PX_28,
		IDB_SPINNER_100_PX_29,
		IDB_SPINNER_100_PX_30};

	LoadSpinnerBitmaps(nSpinnerIds, MArrayLen(nSpinnerIds), 100, 100);
}

void Arcade::Forms::Form::LoadSpinner150PxBitmaps()
{
	static INT nSpinnerIds[] = {
		IDB_SPINNER_150_PX_1,
		IDB_SPINNER_150_PX_2,
		IDB_SPINNER_150_PX_3,
		IDB_SPINNER_150_PX_4,
		IDB_SPINNER_150_PX_5,
		IDB_SPINNER_150_PX_6,
		IDB_SPINNER_150_PX_7,
		IDB_SPINNER_150_PX_8,
		IDB_SPINNER_150_PX_9,
		IDB_SPINNER_150_PX_10,
		IDB_SPINNER_150_PX_11,
		IDB_SPINNER_150_PX_12,
		IDB_SPINNER_150_PX_13,
		IDB_SPINNER_150_PX_14,
		IDB_SPINNER_150_PX_15,
		IDB_SPINNER_150_PX_16,
		IDB_SPINNER_150_PX_17,
		IDB_SPINNER_150_PX_18,
		IDB_SPINNER_150_PX_19,
		IDB_SPINNER_150_PX_20,
		IDB_SPINNER_150_PX_21,
		IDB_SPINNER_150_PX_22,
		IDB_SPINNER_150_PX_23,
		IDB_SPINNER_150_PX_24,
		IDB_SPINNER_150_PX_25,
		IDB_SPINNER_150_PX_26,
		IDB_SPINNER_150_PX_27,
		IDB_SPINNER_150_PX_28,
		IDB_SPINNER_150_PX_29,
		IDB_SPINNER_150_PX_30};

	LoadSpinnerBitmaps(nSpinnerIds, MArrayLen(nSpinnerIds), 150, 150);
}

void Arcade::Forms::Form::LoadSpinner200PxBitmaps()
{
	static INT nSpinnerIds[] = {
		IDB_SPINNER_200_PX_1,
		IDB_SPINNER_200_PX_2,
		IDB_SPINNER_200_PX_3,
		IDB_SPINNER_200_PX_4,
		IDB_SPINNER_200_PX_5,
		IDB_SPINNER_200_PX_6,
		IDB_SPINNER_200_PX_7,
		IDB_SPINNER_200_PX_8,
		IDB_SPINNER_200_PX_9,
		IDB_SPINNER_200_PX_10,
		IDB_SPINNER_200_PX_11,
		IDB_SPINNER_200_PX_12,
		IDB_SPINNER_200_PX_13,
		IDB_SPINNER_200_PX_14,
		IDB_SPINNER_200_PX_15,
		IDB_SPINNER_200_PX_16,
		IDB_SPINNER_200_PX_17,
		IDB_SPINNER_200_PX_18,
		IDB_SPINNER_200_PX_19,
		IDB_SPINNER_200_PX_20,
		IDB_SPINNER_200_PX_21,
		IDB_SPINNER_200_PX_22,
		IDB_SPINNER_200_PX_23,
		IDB_SPINNER_200_PX_24,
		IDB_SPINNER_200_PX_25,
		IDB_SPINNER_200_PX_26,
		IDB_SPINNER_200_PX_27,
		IDB_SPINNER_200_PX_28,
		IDB_SPINNER_200_PX_29,
		IDB_SPINNER_200_PX_30};

	LoadSpinnerBitmaps(nSpinnerIds, MArrayLen(nSpinnerIds), 200, 200);
}

void Arcade::Forms::Form::ReleaseSpinnerBitmaps()
{
	if (m_phSpinnerImages)
	{
		for (INT nIndex = 0; nIndex < m_nTotalSpinnerImages; ++nIndex)
		{
			UiArcadeCtrlsDeleteImage(m_phSpinnerImages[nIndex]);
		}

		UiArcadeCtrlsFreeImages(m_phSpinnerImages);
	}
}

void Arcade::Forms::Form::LoadSpinnerBitmaps(
  LPINT pnSpinnerIds,
  INT nTotalSpinnerIds,
  INT nWidth,
  INT nHeight)
{
	m_nTotalSpinnerImages = nTotalSpinnerIds;
	m_nActiveSpinnerImage = 0;
	m_phSpinnerImages = UiArcadeCtrlsAllocImages(nTotalSpinnerIds);

	m_SpinnerImageSize.Width = nWidth;
	m_SpinnerImageSize.Height = nHeight;

	if (m_phSpinnerImages == nullptr)
	{
		m_nTotalSpinnerImages = 0;

		return;
	}

	for (INT nIndex = 0; nIndex < nTotalSpinnerIds; ++nIndex)
	{
		m_phSpinnerImages[nIndex] = UiArcadeCtrlsLoadImage(pnSpinnerIds[nIndex]);
	}
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
