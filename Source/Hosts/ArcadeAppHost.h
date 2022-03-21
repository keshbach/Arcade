/***************************************************************************/
/*  Copyright (C) 2019-2021 Kevin Eshbach                                  */
/***************************************************************************/

#if !defined(ArcadeAppHost_H)
#define ArcadeAppHost_H

#include <Includes/UtExternC.h>

#define ARCADEAPPHOSTAPI __stdcall

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostInitialize(VOID);

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostUninitialize(VOID);

MExternC BOOL ARCADEAPPHOSTAPI ArcadeAppHostExecute(_Out_ LPDWORD pdwExitCode);

#endif /* end of ArcadeAppHost_H */

/***************************************************************************/
/*  Copyright (C) 2019-2021 Kevin Eshbach                                  */
/***************************************************************************/
