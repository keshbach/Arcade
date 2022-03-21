/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Common
{
    namespace IO
    {
        public ref class File sealed
        {
        public:
            /// <summary>
            /// Generates a unique temporary fully qualified file name with the given file extension.
            /// </summary>

            static System::String^ GenerateTempFileName(System::String^ sFileExtension);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
