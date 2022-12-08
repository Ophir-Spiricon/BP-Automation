#	Initialize the NanoScan COM Server
$ns = New-Object -ComObject Photon-NanoScan

#	This should enumerate the properties of the object
#	This is the output I get:
#	PS C:\WINDOWS\system32> $ns
#	System.__ComObject
$ns

#	This should enumerate all properties, methods, collections and events of the object, but doesn't return any members in the IDL
#	This is the output I get:
#	PS C:\WINDOWS\system32> $ns | Get-Member
#	   TypeName: System.__ComObject
#	
#	Name                      MemberType Definition
#	----                      ---------- ----------
#	CreateObjRef              Method     System.Runtime.Remoting.ObjRef CreateObjRef(type requestedType)
#	Equals                    Method     bool Equals(System.Object obj)
#	GetHashCode               Method     int GetHashCode()
#	GetLifetimeService        Method     System.Object GetLifetimeService()
#	GetType                   Method     type GetType()
#	InitializeLifetimeService Method     System.Object InitializeLifetimeService()
#	ToString                  Method     string ToString()
$ns | Get-Member

#	Despite this, the COM object is addressable

#	Show the window
$ns.NsAsShowWindow = $true

#	Select beam width and centroid results
[byte]$percentPeak =  0x1;
[byte]$fourSigma =  0x10;
[byte]$centroid =  0x20;
[byte]$resultIDs = $percentPeak -bor $fourSigma -bor $centroid
$ns.NsAsSelectParameters($resultIDs)

$deviceCount = new-object System.Int32
$ns.NsAsGetNumDevices(([ref]$deviceCount))
Write-Host "Device Count: " $deviceCount

#	acquire data for five seconds
$ns.NsAsDataAcquisition = $true
Start-Sleep -s 5
$ns.NsAsDataAcquisition = $false

#	Don't know the correct byref syntax.  ([ref]object) works for PS functions, but doesn't seem to work with COM methods
#$xWidth = new-object System.Double
#$ns.NsAsGetBeamWidth(0,1,13.5,([ref]$xWidth))
#Write-Host $xWidth

#	Release the COM object and shutdown the server
[System.Runtime.InteropServices.Marshal]::ReleaseComObject([System.__ComObject]$ns) | out-null