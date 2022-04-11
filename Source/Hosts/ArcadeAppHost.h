/***************************************************************************/
/*  Copyright (C) 2019-2022 Kevin Eshbach                                  */
/***************************************************************************/

#if !defined(ArcadeAppHost_H)
#define ArcadeAppHost_H

#include <Includes/UtExternC.h>

#define ARCADEAPPHOSTAPI __stdcall

#define CArcadeAppHostAccessDatabaseMode 0x01
#define CArcadeAppHostSQLServerDatabaseMode 0x02

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostInitialize(VOID);

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostUninitialize(VOID);

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostExecute(_In_ INT nDatabaseMode, _Out_ LPDWORD pdwExitCode);

#endif /* end of ArcadeAppHost_H */

/***************************************************************************/
/*  Copyright (C) 2019-2022 Kevin Eshbach                                  */
/***************************************************************************/
