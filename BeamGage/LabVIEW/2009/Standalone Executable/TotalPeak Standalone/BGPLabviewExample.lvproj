<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="9008000">
	<Item Name="My Computer" Type="My Computer">
		<Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.tcp.enabled" Type="Bool">false</Property>
		<Property Name="server.tcp.port" Type="Int">0</Property>
		<Property Name="server.tcp.serviceName" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.tcp.serviceName.default" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
		<Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="specify.custom.address" Type="Bool">false</Property>
		<Item Name="BeamGage Callback with notification.vi" Type="VI" URL="/C/Program Files/Spiricon/BeamGage Professional/Automation/Examples/LabVIEW 2009/TotalPeakChartExample/BeamGage Callback with notification.vi"/>
		<Item Name="BeamGage Chart.vi" Type="VI" URL="/C/Program Files/Spiricon/BeamGage Professional/Automation/Examples/LabVIEW 2009/TotalPeakChartExample/BeamGage Chart.vi"/>
		<Item Name="Dependencies" Type="Dependencies">
			<Item Name="vi.lib" Type="Folder">
				<Item Name="Simple Error Handler.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Simple Error Handler.vi"/>
				<Item Name="DialogType.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/DialogType.ctl"/>
				<Item Name="General Error Handler.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/General Error Handler.vi"/>
				<Item Name="DialogTypeEnum.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/DialogTypeEnum.ctl"/>
				<Item Name="General Error Handler CORE.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/General Error Handler CORE.vi"/>
				<Item Name="Check Special Tags.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Check Special Tags.vi"/>
				<Item Name="TagReturnType.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/TagReturnType.ctl"/>
				<Item Name="Set String Value.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Set String Value.vi"/>
				<Item Name="GetRTHostConnectedProp.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/GetRTHostConnectedProp.vi"/>
				<Item Name="Error Code Database.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Error Code Database.vi"/>
				<Item Name="whitespace.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/whitespace.ctl"/>
				<Item Name="Trim Whitespace.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Trim Whitespace.vi"/>
				<Item Name="Format Message String.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Format Message String.vi"/>
				<Item Name="Find Tag.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Find Tag.vi"/>
				<Item Name="Search and Replace Pattern.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Search and Replace Pattern.vi"/>
				<Item Name="Set Bold Text.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Set Bold Text.vi"/>
				<Item Name="Details Display Dialog.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Details Display Dialog.vi"/>
				<Item Name="ErrWarn.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/ErrWarn.ctl"/>
				<Item Name="Clear Errors.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Clear Errors.vi"/>
				<Item Name="eventvkey.ctl" Type="VI" URL="/&lt;vilib&gt;/event_ctls.llb/eventvkey.ctl"/>
				<Item Name="Not Found Dialog.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Not Found Dialog.vi"/>
				<Item Name="Three Button Dialog.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Three Button Dialog.vi"/>
				<Item Name="Three Button Dialog CORE.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Three Button Dialog CORE.vi"/>
				<Item Name="Longest Line Length in Pixels.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Longest Line Length in Pixels.vi"/>
				<Item Name="Convert property node font to graphics font.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Convert property node font to graphics font.vi"/>
				<Item Name="Get Text Rect.vi" Type="VI" URL="/&lt;vilib&gt;/picture/picture.llb/Get Text Rect.vi"/>
				<Item Name="Get String Text Bounds.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Get String Text Bounds.vi"/>
				<Item Name="LVBoundsTypeDef.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/miscctls.llb/LVBoundsTypeDef.ctl"/>
				<Item Name="BuildHelpPath.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/BuildHelpPath.vi"/>
				<Item Name="GetHelpDir.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/GetHelpDir.vi"/>
			</Item>
			<Item Name="Spiricon.BeamGage.Automation.Interfaces.dll" Type="Document" URL="/C/Program Files/Spiricon/BeamGage Professional/Spiricon.BeamGage.Automation.Interfaces.dll"/>
			<Item Name="Spiricon.BeamGage.Automation.dll" Type="Document" URL="/C/Program Files/Spiricon/BeamGage Professional/Spiricon.BeamGage.Automation.dll"/>
		</Item>
		<Item Name="Build Specifications" Type="Build">
			<Item Name="BGP LabView Example" Type="EXE">
				<Property Name="App_applicationGUID" Type="Str">{75D0AA8A-197B-4BA0-9A25-D90530A0C076}</Property>
				<Property Name="App_applicationName" Type="Str">BGPTotalPeakExample_x64.exe</Property>
				<Property Name="App_companyName" Type="Str">Ophir-Spiricon, LLC</Property>
				<Property Name="App_fileDescription" Type="Str">BGP LabView Example</Property>
				<Property Name="App_fileVersion.major" Type="Int">1</Property>
				<Property Name="App_INI_aliasGUID" Type="Str">{2A1139AA-1021-4E4D-90DC-BFCCAFCFF724}</Property>
				<Property Name="App_INI_GUID" Type="Str">{3D20841A-1188-410E-B69C-506725A9EBF1}</Property>
				<Property Name="App_internalName" Type="Str">BGP LabView Example</Property>
				<Property Name="App_legalCopyright" Type="Str">Copyright © 2010 Ophir-Spiricon, LLC</Property>
				<Property Name="App_productName" Type="Str">BGP LabView Example</Property>
				<Property Name="Bld_buildSpecName" Type="Str">BGP LabView Example</Property>
				<Property Name="Bld_excludeLibraryItems" Type="Bool">true</Property>
				<Property Name="Bld_excludePolymorphicVIs" Type="Bool">true</Property>
				<Property Name="Bld_modifyLibraryFile" Type="Bool">true</Property>
				<Property Name="Destination[0].destName" Type="Str">BGPTotalPeakExample_x64.exe</Property>
				<Property Name="Destination[0].path" Type="Path">/F/BaseI/CodeBase/Installations/Examples/BGLabView2009Examples/TotalPeakChartExample/Builds/BGPTotalPeakExample_x64.exe</Property>
				<Property Name="Destination[0].path.type" Type="Str">&lt;none&gt;</Property>
				<Property Name="Destination[0].preserveHierarchy" Type="Bool">true</Property>
				<Property Name="Destination[0].type" Type="Str">App</Property>
				<Property Name="Destination[1].destName" Type="Str">Support Directory</Property>
				<Property Name="Destination[1].path" Type="Path">/F/BaseI/CodeBase/Installations/Examples/BGLabView2009Examples/TotalPeakChartExample/Builds/data</Property>
				<Property Name="Destination[1].path.type" Type="Str">&lt;none&gt;</Property>
				<Property Name="DestinationCount" Type="Int">2</Property>
				<Property Name="Source[0].itemID" Type="Str">{C423C471-031D-4B9D-BF40-0F6D34572361}</Property>
				<Property Name="Source[0].type" Type="Str">Container</Property>
				<Property Name="Source[1].destinationIndex" Type="Int">0</Property>
				<Property Name="Source[1].itemID" Type="Ref">/My Computer/BeamGage Chart.vi</Property>
				<Property Name="Source[1].sourceInclusion" Type="Str">TopLevel</Property>
				<Property Name="Source[1].type" Type="Str">VI</Property>
				<Property Name="Source[2].destinationIndex" Type="Int">0</Property>
				<Property Name="Source[2].itemID" Type="Ref">/My Computer/BeamGage Callback with notification.vi</Property>
				<Property Name="Source[2].sourceInclusion" Type="Str">Include</Property>
				<Property Name="Source[2].type" Type="Str">VI</Property>
				<Property Name="SourceCount" Type="Int">3</Property>
			</Item>
			<Item Name="LabVIEW runtime installer for BeamGage examples" Type="Installer">
				<Property Name="arpCompany" Type="Str">Ophir-Spiricon LLC</Property>
				<Property Name="arpContact" Type="Str">service@us.ophiropt.com</Property>
				<Property Name="arpPhone" Type="Str">435-753-3729</Property>
				<Property Name="arpURL" Type="Str">http://www.Ophir-Spiricon,LLC.com/</Property>
				<Property Name="AutoIncrement" Type="Bool">true</Property>
				<Property Name="BldInfo.Count" Type="Int">3</Property>
				<Property Name="BldInfo[0].Dir" Type="Str">{886C1E39-A1B3-46E0-8EF3-609575DA9428}</Property>
				<Property Name="BldInfo[0].Tag" Type="Ref">/My Computer/BeamGage Chart.vi</Property>
				<Property Name="BldInfo[1].Dir" Type="Str">{886C1E39-A1B3-46E0-8EF3-609575DA9428}</Property>
				<Property Name="BldInfo[1].Tag" Type="Ref"></Property>
				<Property Name="BldInfo[2].Dir" Type="Str">{886C1E39-A1B3-46E0-8EF3-609575DA9428}</Property>
				<Property Name="BldInfo[2].Tag" Type="Ref">/My Computer/Build Specifications</Property>
				<Property Name="BldInfo[3].Dir" Type="Str">{886C1E39-A1B3-46E0-8EF3-609575DA9428}</Property>
				<Property Name="BldInfo[3].Tag" Type="Ref">/My Computer/Build Specifications</Property>
				<Property Name="BuildLabel" Type="Str">LabVIEW runtime installer for BeamGage examples</Property>
				<Property Name="BuildLocation" Type="Path">../../../BGLabView2009Examples/Standalone installers</Property>
				<Property Name="DirInfo.Count" Type="Int">1</Property>
				<Property Name="DirInfo.DefaultDir" Type="Str">{EC20BF18-EBDF-472E-9BBC-8DED4ACDF531}</Property>
				<Property Name="DirInfo[0].DirName" Type="Str">Spiricon</Property>
				<Property Name="DirInfo[0].DirTag" Type="Str">{EC20BF18-EBDF-472E-9BBC-8DED4ACDF531}</Property>
				<Property Name="DirInfo[0].ParentTag" Type="Str">{3912416A-D2E5-411B-AFEE-B63654D690C0}</Property>
				<Property Name="DistID" Type="Str">{B6D196A7-FF54-42A0-9DCC-3460903564B7}</Property>
				<Property Name="DistParts.Count" Type="Int">1</Property>
				<Property Name="DistPartsInfo[0].FlavorID" Type="Str">DefaultFull</Property>
				<Property Name="DistPartsInfo[0].ProductID" Type="Str">{0AAB121C-8EA7-49F5-B37C-DF117FB46771}</Property>
				<Property Name="DistPartsInfo[0].ProductName" Type="Str">NI LabVIEW Run-Time Engine 2009 SP1</Property>
				<Property Name="DistPartsInfo[0].UpgradeCode" Type="Str">{1DA01FF3-1E36-4A14-802B-D195819E159B}</Property>
				<Property Name="InstSpecVersion" Type="Str">9018011</Property>
				<Property Name="LicenseFile" Type="Ref"></Property>
				<Property Name="OSCheck" Type="Int">0</Property>
				<Property Name="OSCheck_Vista" Type="Bool">false</Property>
				<Property Name="ProductName" Type="Str">BeamGage Automation Examples</Property>
				<Property Name="ProductVersion" Type="Str">1.0.8</Property>
				<Property Name="ReadmeFile" Type="Ref"></Property>
				<Property Name="UpgradeCode" Type="Str">{B2D52878-EA96-4C8B-B33C-26EC8857801F}</Property>
				<Property Name="WindowMessage" Type="Str">This application will install the LabVIEW runtime required to run stand-alone (.exe) automation examples, written for BeamGage,  in LabVIEW G</Property>
				<Property Name="WindowTitle" Type="Str">LabVIEW runtime installer for BeamGage examples</Property>
			</Item>
		</Item>
	</Item>
</Project>
