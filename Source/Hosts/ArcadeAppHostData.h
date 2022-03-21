/***************************************************************************/
/*  Copyright (C) 2019-2021 Kevin Eshbach                                  */
/***************************************************************************/

#if !defined(ArcadeAppHostData_H)
#define ArcadeAppHostData_H

#if defined(_MSC_VER)
#if defined(_X86_)
#pragma pack(push, 4)
#elif defined(_WIN64)
#pragma pack(push, 8)
#else
#error Need to specify cpu architecture to configure structure padding
#endif
#else
#error Need to specify how to enable byte aligned structure padding
#endif

typedef struct tagTArcadeAppHostData
{
    DWORD dwExitCode;
} TArcadeAppHostData;

#if defined(_MSC_VER)
#pragma pack(pop)
#else
#error Need to specify how to restore original structure padding
#endif

#endif /* end of ArcadeAppHostData_H */

/***************************************************************************/
/*  Copyright (C) 2019-2021 Kevin Eshbach                                  */
/***************************************************************************/
