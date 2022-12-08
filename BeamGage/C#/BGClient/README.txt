========================================================================
			BeamGage C# Automation Example
========================================================================

Instructions for use in C#:

	1. Must have previously installed and run BeamGage Professional.
	2. Must allow any Window's firewall expcetions to be created for the
	    example the first time it is run.  This is to allow .NET remoting, which is
	    process-to-process TCP communication.  This will not grant unrestricted network
	    access to BeamGage or the automation client.
	3. The project must target .NET 4 or greater.
	4. Add references to the following BeamGage dll's to your C# automation project.
		a. Spiricon.BeamGage.Automation.dll
		b. Spiricon.BeamGage.Automation.Interfaces.dll
		c. Spiricon.Interfaces.ConsoleService.dll
		d. Spiricon.TreePattern.dll
	5. In some development environments it may be necessary to run Visual Studio with
	    elevated privileges to grant the client the ability to launch an instance of
	    BeamGage. (e.g Run As Administrator, or programmatic elevation)

The .NET sample project, and related source code are provided with limited support
and limited internal testing by Spiricon.  Automation support requests should be directed
to the Ophir-Spiricon Customer Service department and may require a paid support contract.

/////////////////////////////////////////////////////////////////////////////
This file contains a description of the provided files.

Program.cs
	A sample program that runs the two provided test methods, ConnectTest
	and ReconnectTest.

BGInterface.cs
	A sample interface class to BeamGage.  This class handles all the
	explicit functionality of interfacing with BeamGage.  This model is
	recommended to keep this functionality abstracted from the rest of the
 	automation client's code.

Automation.Connect.Tests.cs
	An example class that utilizes the functionality implemented in
	BGInterface.cs.

	ConnectTest
		1. Connects and intializes a new BeamGage session by name
		    with the GUI enabled.
		2. Loads an installed setup file
		3. Gets several spatial result values
		4. Disconnects.  Does not close the active session.

	ReconnectTest
		1. Connects to the previously initialized session of BeamGage
		    by name.
		2. Gets several spatial result values
		3. Disconnects and exits the active session.
Test.Utils.cs
	Provides simple utlity behavior to the tests.
	This class is not relevant to the BeamGage interface.