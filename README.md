# Arcade

Application for tracking arcade game boards and their associated electronic components, controls and display information.

Copyright (C) 2006-2023 Kevin Eshbach  

## Requirements

Windows 7, Windows 8, Windows 8.1, Windows 10 or Windows 11

The application requires either a Microsoft Access or SQL Server database.

## Building

- Install Visual Studio 2019
- Install Windows 10 SDK, version 10.0.19041.1
- Launch a Command Prompt and go to the 'Source' directory
- Run the command "cscript build.wsf /verbose:+ /binaryType:Release" (without the quotes) to build the release configuration (To build the debug configuration replace "Release" with "Debug".)

## Programming Languages

The application is a combination of C, Managed C++ and C#.
