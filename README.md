# Beam Profiling Automation Interfaces
Automation Interface examples and documentation for Ophir-Spiricon laser beam profiling products

The classes and all associated subclasses, enumerations, and example code are provided as-is with no express guarantee or warranty of functionality and may be provided and updated independently of product releases.

Each program/interface/module may be customized as needed but all client functions should be implemented in a secondary program module.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
[https://www.ophiropt.com/laser--measurement/software-download](https://www.ophiropt.com/laser--measurement/software-download)

Professional tier licenses for Ophir brand beam profiling cameras may be purchased by contacting:

**Ophir USA Product Support**
+1 800-383-0814
[service.ophir.usa@mksinst.com](mailto://service.ophir.usa@mksinst.com)

## Targeting .NET Frameworks for Automation client applications
 
 MKS Ophir® products were designed to be a multi-client, multi-process applications utilizing the .NET Framework and its feature .NET Remoting to achieve high performance data transfer.  .NET Remoting is the predominant IPC technology in all modern Windows platforms.  The multi-process design decouples instabilities and performance of camera data transfer from the main analyzer process.
 
 .NET Remoting is only available in the .NET Framework and not .NET (previously .NET Core).  Microsoft details where each framework is best utilized [here](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework).
 
 In addition to what Microsoft lists, there are some additional advantages to the .NET Framework over .NET for developing applications, particularly in business and industrial applications, such as:
 * Being later in its lifecycle, provides a very robust feature set, and an accumulation of bug and security fixes
 * Less risk of breaking changes to the framework
 * High IPC performance
 * Strong compatibility with machine vision and scientific instruments.
 
 MKS Ophir® products target .NET Framework v4.7.2, and any of the versions of .NET Framework listed below may be targeted in your automation client project based on which operating systems you plan to support.  In most cases, .NET Framework v4.8.1 is recommended.
 
### .NET Framework Lifecycle
 .NET Framework is the basis for the majority of Windows versions and its features.  At the time of writing, there are no outstanding security advisories for any of the .NET Framework versions listed below.
 
### Support policies for the .NET Framework
 There was a time when Microsoft planned a more aggressive lifecycle for .NET Framework, .NET, and even Windows.  As of the time of writing, this approach has been tempered with Windows 11 remaining as 'the' standing version of Windows for years to come.
 
 Microsoft lists the support policies for each version .NET [here](https://learn.microsoft.com/en-us/lifecycle/faq/dotnet-framework).  These versions and policies are applicable to BeamGage.
 
### .NET Framework 4.7.2
 Support for .NET 4.7.2 follows the Lifecycle Policy of the parent OS. It is supported as a Windows component on the latest required operating system update for Windows:
* 7 SP1
* Server 2008 R2 SP1
* 8.1 Update
* 10 version 1607
* 10 version 1703
* 10 version 1709
* Server 2012
* Server 2012 R2
* Server 2016 
* Server version 1709
     
 Additionally  .NET 4.7.2 is supported on Windows:
* 10 version 1803
* 10 version 1809
* Server version 1803
* Server 2019
 
### .NET Framework 4.8
 Support for .NET 4.8 follows the Lifecycle Policy of the parent OS. It is supported as a Windows component on the latest required update for the following operating systems:
 
   * All Windows versions starting from Windows 7 SP1 to Windows 11 with no limiting version specified.
   
 We recommend customers upgrade to .NET Framework 4.8 to receive the highest level of performance, reliability, and security.
 
### .NET Framework 4.8.1
 Support for .NET 4.8.1 follows the Lifecycle Policy of the parent OS. It is supported as a Windows component on the latest required update for the following operating systems:
 
* Windows 10 versions:
  * 20H2 
  * 21H1 
  * 21H2
* Windows 11 versions:
  * 21H2 and later
  * Windows Server 2022

 #### Useful links:
 * [What is .Net Framework?](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework)
 * [Lifecycle FAQ - .NET Framework](https://learn.microsoft.com/en-us/lifecycle/faq/dotnet-framework)
 * [MicroSoft .NET Framework](https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-framework)
 * [.Net Framework versions and dependencies](https://learn.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies)
 
## Configuring .NET Language versioning
 Normally the C# compiler determines a default language version based on your project's target framework.  Visual Studio doesn't provide a UI to change the value, but you can change it by editing the csproj file.  When using a given framework version, you may customize this property to use language features that would not be available in your project's target framework. Microsoft cautions that choosing a language version newer than the default can cause hard to diagnose compile-time and runtime errors.  C# 8.0 provides a large set of features and works very well with .NET Framework v4.7.2 and later.
 
 If specific C# language features are a primary reason for wanting to target .NET (Core) in your project, the package [Polysharp](https://www.nuget.org/packages/PolySharp) may be used to get these features in earlier framework versions.
> Note: This is not an endorsement or guarantee of the Polysharp package.
 
#### Additional Useful links:
 * [C# language versioning](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-versioning)
 * [Configure C# language version](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)
 * [The history of C#](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history)


---
#### MKS | Ophir  - The True Measure of Laser Performance™


[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2FOphir-Spiricon%2FBP-Automation&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=hits&edge_flat=false)](https://hits.seeyoufarm.com)
