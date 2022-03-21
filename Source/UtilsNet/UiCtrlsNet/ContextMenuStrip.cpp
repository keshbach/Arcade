/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ContextMenuStrip.h"

Common::Forms::ContextMenuStrip::ContextMenuStrip()
{
	InitializeComponent();
}

Common::Forms::ContextMenuStrip::~ContextMenuStrip()
{
	if (components)
	{
		delete components;
	}
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2020 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
