/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

namespace Arcade
{
    namespace Forms
    {
        public ref class Form : Common::Forms::Form
        {
        protected:
            enum class ESpinnerSize
            {
                /// <summary>
                /// Image 50 x 50 pixels 
                /// </summary>
                Small,
                /// <summary>
                /// Image 100 x 100 pixels 
                /// </summary>
                Normal,
                /// <summary>
                /// Image 150 x 150 pixels 
                /// </summary>
                Large,
                /// <summary>
                /// Image 200 x 200 pixels 
                /// </summary>
                ExtraLarge
            };

        public:
            Form();

        protected:
            property System::Boolean BusyControlVisible
            {
                System::Boolean get()
                {
                    return m_bBusyVisible;
                }

                void set(System::Boolean value)
                {
                    SetBusyVisible(value);
                }
            }

            property ESpinnerSize SpinnerSize
            {
                ESpinnerSize get()
                {
                    return m_SpinnerSize;
                }

                void set(ESpinnerSize value)
                {
                    SetSpinnerSize(value);
                }
            }

        protected:
            System::Boolean OpenFile(System::String^ sFile, System::String^% sErrorMessage);

            void UpdateControlVisibility(System::Windows::Forms::Control^ Control, System::Boolean bVisible);
            void UpdateFocusedControl(System::Windows::Forms::Control^ Control);

        protected:
            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            ~Form();

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
            void OnPaintBackground(System::Windows::Forms::PaintEventArgs^ e) override;
            void WndProc(System::Windows::Forms::Message% Message) override;

        private:
            void SetBusyVisible(System::Boolean bVisible);
            void SetSpinnerSize(ESpinnerSize SpinnerSize);

            void BusyVisibleProcessMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessTimerMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientHitTestMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientMouseMoveMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientLeftButtonDownMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientLeftButtonUpMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientLeftButtonDoubleClickMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientRightButtonDownMessage(System::Windows::Forms::Message% Message);
            void BusyVisibleProcessNonClientRightButtonUpMessage(System::Windows::Forms::Message% Message);

            void LoadSpinner50PxBitmaps();
            void LoadSpinner100PxBitmaps();
            void LoadSpinner150PxBitmaps();
            void LoadSpinner200PxBitmaps();
            void ReleaseSpinnerBitmaps();

            void LoadSpinnerBitmaps(LPINT pnSpinnerIds, INT nTotalSpinnerIds, INT nWidth, INT nHeight);

        private:
            System::Boolean m_bBusyVisible;

            ESpinnerSize m_SpinnerSize;

            System::Collections::Generic::Dictionary<System::Windows::Forms::Control^, System::Boolean>^ m_ControlVisibleDict;

            HWND m_hFocusedControl;

            HBITMAP m_hBitmap;

            PHANDLE m_phSpinnerImages;
            INT m_nTotalSpinnerImages;
            INT m_nActiveSpinnerImage;

            System::Drawing::Size m_SpinnerImageSize;
        };
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
