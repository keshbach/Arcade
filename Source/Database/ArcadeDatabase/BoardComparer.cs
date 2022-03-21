/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    internal sealed class BoardComparer :
        System.Collections.Generic.IComparer<DatabaseDefs.TBoard>
    {
        public int Compare(
            DatabaseDefs.TBoard Board1,
            DatabaseDefs.TBoard Board2)
        {
            System.Int32 nBoard1Compare, nBoard2Compare;

            nBoard1Compare = System.String.Compare(Board1.sBoardTypeName,
                                                   DatabaseDefs.CCartridgeName, true);
            nBoard2Compare = System.String.Compare(Board2.sBoardTypeName,
                                                   DatabaseDefs.CCartridgeName, true);

            if (nBoard1Compare == 0 && nBoard2Compare == 0)
            {
                return System.String.Compare(Board1.sBoardName,
                                             Board2.sBoardName, true);
            }

            if (nBoard1Compare == 0)
            {
                return -1;
            }

            if (nBoard2Compare == 0)
            {
                return 1;
            }

            return System.String.Compare(Board1.sBoardTypeName,
                                         Board2.sBoardTypeName, true);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
