/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Arcade
{
    namespace Application
    {
        public ref class Startup sealed
        {
        public:
            enum class EDatabaseMode
            {
                Access,
                SQLServer
            };

        public:
            Startup();

        public:
            System::UInt32 Execute(EDatabaseMode DatabaseMode);
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
