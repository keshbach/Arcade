/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppNet.h"

#using <System.Windows.Forms.dll>

#using "UtilNet.dll"
#using "UiUtilNet.dll"
#using "ArcadeForms.dll"

#pragma region "Constants"

#define CRegistryKey L"Software\\Kevin Eshbach\\ArcadeApp\\3.00"

#define CFormLocationsName L"FormLocations"

#pragma endregion

Arcade::Application::Startup::Startup()
{
}

System::UInt32 Arcade::Application::Startup::Execute()
{
	System::String^ sFormLocationsRegistryKey = System::String::Format(L"{0}\\{1}", CRegistryKey, CFormLocationsName);
    Arcade::Forms::MainForm^ AppForm;

	if (!Common::IO::TempFileManager::Initialize())
	{
	}

    try
    {
        System::Windows::Forms::Application::EnableVisualStyles();
        System::Windows::Forms::Application::SetCompatibleTextRenderingDefault(false);

        AppForm = gcnew Arcade::Forms::MainForm(CRegistryKey, sFormLocationsRegistryKey);

		gcnew Common::Forms::FormLocation(AppForm, sFormLocationsRegistryKey);

        Common::Forms::Application::Run(AppForm);
	}
    catch (System::Exception^ exception)
    {
        System::String^ sMsg;

        sMsg = System::String::Format(L"Unhandled exception.  ({0})\n\nThe application will now automatically close to prevent data loss.", 
                                      exception->Message);

        System::Windows::Forms::MessageBox::Show(sMsg,
            System::Windows::Forms::Application::ProductName,
            System::Windows::Forms::MessageBoxButtons::OK,
            System::Windows::Forms::MessageBoxIcon::Error);
    }

	if (!Common::IO::TempFileManager::Uninitialize())
	{
	}

    return 0;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2021 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
