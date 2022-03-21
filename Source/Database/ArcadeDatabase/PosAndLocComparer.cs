/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    internal sealed class PosAndLocComparer :
        System.Collections.Generic.IComparer<DatabaseDefs.TBoardPartLocation>
    {
        public int Compare(
            DatabaseDefs.TBoardPartLocation Location1,
            DatabaseDefs.TBoardPartLocation Location2)
        {
            System.Int32 nLocationCompare;

            nLocationCompare = System.String.Compare(Location1.sBoardPartPosition,
                                                     Location2.sBoardPartPosition,
                                                     true);

            if (nLocationCompare != 0)
            {
                return nLocationCompare;
            }

            return System.String.Compare(Location1.sBoardPartLocation,
                                         Location2.sBoardPartLocation, true);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
