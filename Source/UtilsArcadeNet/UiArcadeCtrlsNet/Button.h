/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Arcade
{
    namespace Forms
    {
        public ref class Button : System::Windows::Forms::Button
        {
        public:
            Button();

        protected:
            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            ~Button();

        private:
            /// <summary>
            /// Required designer variable.
            /// </summary>
            System::ComponentModel::Container^ components;

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>		
            void InitializeComponent(void)
            {
            }

        protected:
            void OnClick(System::EventArgs^ e) override;
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
