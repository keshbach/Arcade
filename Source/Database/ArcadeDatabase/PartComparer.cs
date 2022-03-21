/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    internal sealed class PartComparer :
        System.Collections.Generic.IComparer<DatabaseDefs.TPart>
    {
        public int Compare(
            DatabaseDefs.TPart Part1,
            DatabaseDefs.TPart Part2)
        {
            System.String[] Values1 = {Part1.sPartName,
                                       Part1.sPartCategoryName,
                                       Part1.sPartTypeName,
                                       Part1.sPartPackageName};
            System.String[] Values2 = {Part2.sPartName,
                                       Part2.sPartCategoryName,
                                       Part2.sPartTypeName,
                                       Part2.sPartPackageName};
            System.Int32 nCompare = 0;

            for (System.Int32 nIndex = 0; nIndex < Values1.Length; ++nIndex)
            {
                nCompare = System.String.Compare(Values1[nIndex],
                                                 Values2[nIndex], true);

                if (nCompare != 0)
                {
                    return nCompare;
                }
            }

            return nCompare;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2015 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
