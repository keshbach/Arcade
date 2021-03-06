/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2018 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

#using "DbAdapterNet.dll"

#include <vcclr.h>

#define STRICT
#define _WIN32_DCOM 

#include <windows.h>
#include <shlwapi.h>
#include <objbase.h>

#pragma warning(push)
#pragma warning(disable : 4091) 
#include <adoint.h>
#pragma warning(pop)
#include <adojet.h>

#include <strsafe.h>

#using <System.Core.dll>

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2018 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
