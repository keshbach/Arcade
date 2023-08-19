/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Button.h"

#pragma region "Constructor"

Arcade::Forms::Button::Button()
{
    InitializeComponent();
}

#pragma endregion

#pragma region "Destructor"

Arcade::Forms::Button::~Button()
{
    if (components)
    {
        delete components;
    }
}

#pragma endregion

void Arcade::Forms::Button::OnClick(System::EventArgs^ e)
{
    HWND hWnd = (HWND)Handle.ToPointer();

    if (::IsWindowVisible(hWnd))
    {
        System::Windows::Forms::Button::OnClick(e);
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
