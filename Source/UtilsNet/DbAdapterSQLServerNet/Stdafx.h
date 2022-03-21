/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2018 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

#using "DbAdapterNet.dll"

#include <vcclr.h>

#define STRICT
#define _WIN32_DCOM

#define ODBCVER 0x0300

#include <windows.h>
#include <objbase.h>

#pragma warning(push)
#pragma warning(disable : 4091) 
#include <adoint.h>
#pragma warning(pop)

#include <odbcinst.h>
#include <sql.h>
#include <sqlext.h>
#include <sqltypes.h>

#include <strsafe.h>

#using <System.Core.dll>

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2018 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
