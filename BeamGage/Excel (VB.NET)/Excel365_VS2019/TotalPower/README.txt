========================================================================
		BeamGage VB.NET/Excel Automation Client Example
========================================================================

Instructions for use:

  1. Must have previously installed and run BeamGage Professional or better
  2. Must have Visual Studio 2019 Community or better to open the development project
  3. Must have Excel 2007 or later to run the output Excel file
  4. Once the solution is opened, you may need to update the assembly references to BeamGage
      to the version you have installed.  Instructions on how do to do this are included in
      the VB.NET tutorial in the BeamGage Automation Interface documentation.

Traditionally, Excel has developer support for Visual Basic for Applications or VBA.  Beginning with
Excel 2007 it is possible to develop advanced Excel projects in VB.NET.  VB.NET is a .NET
compatible language yet has the same syntax of earlier versions of Visual Basic.  The BeamGage
Automation Interface is a .NET interface, and due to this requires a .NET language be used to
interface with it.  Use of VBA in BeamGage v5.x is not possible.
	
The version of Visual Studio will impact the version of Excel you will be developing for.  Visual
Studio 2019 can create Excel Workbooks for 2007 or later.  Visual Studio can
be purchased at the following link, or a variety of online retailers.  (http://www.microsoft.com/visualstudio/eng/buy)
The free Visual Studio Community may also be a viable options for many developers new to using VSTO and VB.NET.

To aid in any learning more about this type of Excel development, it is important to know that the
proper name attributed to these projects is Office Development with Visual Studio nicknamed VSTO.
At the time this example was created, the following is a valid link to the main VSTO webpage, which
is filled with information, tutorials and examples.
https://docs.microsoft.com/en-us/visualstudio/vsto/office-and-sharepoint-development-in-visual-studio?view=vs-2019

A word of advice to the traditional VBA developer, while there is some learning curve to VSTO, the
experience translates easily, and the syntax is familiar in VB.NET.  The power and flexibility of the
.NET frameworks is likely worth the switch.  It is even possible to add-in your own custom Ribbon UI
controls into Office applications which may provide a more native feel to your Workbooks.
(see https://docs.microsoft.com/en-us/visualstudio/vsto/walkthrough-creating-a-custom-tab-by-using-ribbon-xml?view=vs-2019)

Likewise, a word of caution.  The output of VSTO development is more similar to normal Windows application
development, and your workbook may require a few or many dll's that will need to be deployed alongside
your workbook.  Managing the deployment this output can be handled via the built in ClickOnce
publisher, or through other means as well.  Digital signing of the project output is also highly recommended.
Acquisition of a digital code signing certificate is the responsibility of the automation developer.

When deploying an automation client for BeamGage, please note it is necessary to rebuild the client for each
new release of BeamGage and versioning of the client worksheet is recommended.

/////////////////////////////////////////////////////////////////////////////

This example as provided performs the following actions using the BeamGage Automation Interface:
  - Initialization/Shutdown a BeamGage session
  - Start, Stop and Ultracal the data source
  - Retirieve the Total Power, and frame Timestamp values
  - Retrieve the status of the current Data Source and Ultracal
  - Retrieves the current frame data and displays the first 5 columns and 10 rows
  
The examples illustrates the following development scenarios in a Visual Studio Excel Workbook:
  - General VSTO development
  - Accessing data ranges in an Excel sheet
  - Defining attributes of data ranges in an Excel sheet
  - Updating the value content of data ranges in an Excel sheet
  - Providing and interacting with UI controls anchored to an Excel sheet
  - Use of the workbook designer to predefine content and controls in a Excel sheet
  - Use of .NET assemblies
	
/////////////////////////////////////////////////////////////////////////////
This file contains a description of the provided files.

..\TotalPower.sln
  This is the solution file for the example.  This is the file that should be opened in Visual Studio.  It is
  a container for projects of a similar nature, or that support each other.

..\TotalPower\TotalPower.vbproj
  This is the project file.  The example has one project, but the solution can contain as many as needed.

..\TotalPower\Sheet1.vb
  While working with the example in Visual Studio, this file is where the main VB.NET code exists.
  Depending on what your Workbook needs to do, the VB code can be placed in any of the workbook or
  sheet .vb files just as was done in VBA.
 
 ..\TotalPower\BGInterface.vb
  This file contains the BGInterface class.  In this example, all the work that needs to be done to
  communicate with BeamGage has been abstracted to it's own class.  This simplifies the readability
  and maintainability of your application, and keeps a single point of maintanance for working with
  BeamGage.  This is just one example of how to do this.