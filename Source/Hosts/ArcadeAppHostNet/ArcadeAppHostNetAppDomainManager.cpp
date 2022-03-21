/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ArcadeAppHostNetAppDomainManager.h"

Arcade::Application::ArcadeAppHostNetAppDomainManager::ArcadeAppHostNetAppDomainManager()
{
}

void Arcade::Application::ArcadeAppHostNetAppDomainManager::InitializeNewDomain(
  System::AppDomainSetup^ AppDomainInfo)
{
	AppDomainInfo;
}

System::Object^ Arcade::Application::ArcadeAppHostNetAppDomainManager::InitializeLifetimeService()
{
	return nullptr;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2019 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
