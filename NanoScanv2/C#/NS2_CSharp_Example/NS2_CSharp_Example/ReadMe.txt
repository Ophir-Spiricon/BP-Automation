========================================================================
						NanoScan v2 COM Interop
========================================================================

Instructions for use in C#:

	1. Must have previously installed and run NanoScan v2.
	2. Must have a professional license for each scan head used with
	   the automation client.
	3. Add NS2_Interop.dll to your C# automation project.
	4. Add a project reference to the dll.
	5. Add DllImports.cs to your c# automation project and update the
	   namespace to match your automation project namespace.

Under normal use, it is not necessary to modify the imports source
code as they are provided. The .NET sample project, and related source
code are provided with limited support and limited internal testing
by Spiricon.  Automation support requests should be directed to the
Ophir-Spiricon Customer Service department.

/////////////////////////////////////////////////////////////////////////////
This file contains a description of the provided files.

Spiricon.ImageGenerator.dll
	A .NET assembly which takes as an input BeamGage FrameData and produces a
	BitmapSource object in memory which can be consumed into a .NET application
	GUI

Spiricon.ImageGenerator.xml
	A .NET XML documentation file.  This file when copied to the project folder
	of a .NET application will provide IntelliSense documentation within Visual
	Studio. When using non-IntelliSense languages the XML file can still be
	human read with any text editor.

Example.cs
	An example C# application which utilizes the BeamGage Automation Interface
	and implements the Spiricon.ImageGenerator class.