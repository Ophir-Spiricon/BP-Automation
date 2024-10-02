========================================================================
		beamgage_basic_example-matlab-v1.0 Beta Software
========================================================================

Instructions for use:

	1. Unzip the entire package to a known directory accessible to Matlab
	2. Follow the inline documentation in the example .m file to deploy 
	BeamGage .NET assemblies to be visible by Matlab.
	

This software package is provided with limited support and limited internal 
testing by Spiricon R&D and is provided without express guarantee or 
warranty.  Spiricon beta version software intended for diagnostic debugging
or early evaluation of the included functionality and design. Questions 
and feedback should be directed to the Ophir-Spiricon Product Support
department.

/////////////////////////////////////////////////////////////////////////////
Changes in this version:
-	Initial beta release

    PLEASE NOTE: Using the BeamGage Professional automation interface via
    a Matlab client is not a supported feature.  Matlab support listed in our
    catalog is about reading the HDF5 file format used by BeamGage.

    Matlab DOES natively support:
        - reading HDF5 files via native hd5 functions
            - https://www.mathworks.com/help/matlab/hdf5-files.html
        - working with standalone .NET assemblies
            - https://www.mathworks.com/help/matlab/matlab_external/using-net-from-matlab-an-overview.html

    Matlab DOES NOT work well with:
        - working with a .NET server that is not a single standalone assembly
        - The primary limitation is in the way that Matlab references .NET assemblies
            - It expects a standalone assembly registered in the global assembly cache (GAC).
            - It expects a standalone assembly on the file system that can be deployed into the Matlab
             installation directory or deployed alongside a compiled Matlab executable.
        - Matlab CAN BE used this way, but it will always have these limitations unless they support referencing
        .NET assemblies that is deployed in-place to the file system.

/////////////////////////////////////////////////////////////////////////////
This file contains a description of the provided files in this package.

.\BG_Basic_Example.m
	An example file for utilizing the BeamGage Professional Automation
	Interface via Matlab.  Inline comments are provided as basic documentation.
	Full class documentation of the interface provided with BeamGage 
	Professional software installation and may be accessed via the Windows 
	Start Menu->Beam Profiler Documentation->BeamGage Professional Automation
	Interface.lnk

setup\beammaker.bgSetup
	An example bgSetup file that loads during the example code which uses the
	BeamMaker beam generator data source.

README.txt
	This document.