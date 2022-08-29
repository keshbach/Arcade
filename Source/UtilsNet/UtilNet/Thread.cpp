/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Thread.h"

void Common::Threading::Thread::RunWorkerThread(
  System::Threading::ThreadStart^ ThreadStart,
  System::String^ sThreadName)
{
	System::Threading::Thread^ Thread = gcnew System::Threading::Thread(ThreadStart);

	Thread->Name = sThreadName;

	Thread->Start();
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////