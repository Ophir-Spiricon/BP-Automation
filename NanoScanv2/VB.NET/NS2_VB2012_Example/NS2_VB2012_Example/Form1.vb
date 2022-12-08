Imports System.Threading
Imports System.Runtime.InteropServices


Public Class Form1
    Private _ns As NanoScanII.INanoScanII
    Private _isServerRunning As Boolean
    Private _isDAQRunning As Boolean
    Private _fileStr As System.IO.StreamWriter
    Private _profileDataX As List(Of Single) = New List(Of Single)
    Private _profileDataY As List(Of Single) = New List(Of Single)

    Private Sub Form1_Load() Handles MyBase.Load
        status_ToolStripStatusLabel.Text = String.Empty
        _isServerRunning = False
        _isDAQRunning = False
        initServer_Button.Text = "Initialize Server"


        ''Initialize an array of doubles.
        'Dim array(100) As Double

        ''Bind the double array to the Y axis points of the data series.
        'profileChart.Series("Profile X").Points.DataBindY(array)
    End Sub

    Private Sub InitializeServer()
        ' first we need to open NanoScan, there are two methods early and late binding
        ' to add a reference (for early binding):
        ' right click on the project name in solution explorer, choose add reference, switch to the COM tab find NanoScanII.
        ' NOTE: nanoscan must be run at least once before adding the reference. (also necessary for late binding as well)

        ' early binding (requires reference to be added to project) 
        ' intellisense will give you a list of functions.
        status_ToolStripStatusLabel.Text = "Initializing Server..."
        _ns = New NanoScanII.NsAs
        _isServerRunning = True

        ' Late binding, no need to add the reference, will still work.
        ' But no intellisense list of functions.
        ' Dim ns As Object
        ' ns = GetObject("", "photon-nanoscan")

        ' perform any initialization here.
        ' i.e. set ROIs or AutoROI.
        ' etc.

        ' if you have multiple nanoscan controllers installed on the system, you will have to select your head here
        ' ns.NsAsGetDeviceList(...) 'you will have to replace the elipses (...) with actual code
        ' ...
        ' ns.NsAsSetDeviceID(...)

        ' since the loop is an interval loop, if you want to start your measurements at a specific time you will need to
        ' test against the current time, check the documentation for System.DateTime.
        ' System.DateTime.Now
        status_ToolStripStatusLabel.Text = "Launching NanoScan v2 UI"

        _ns.NsAsShowWindow = True 'the window is hidden by default property toggles visiblity.

        status_ToolStripStatusLabel.Text = "Ready"

        ' make sure the folder exists to create the file
        If My.Computer.FileSystem.DirectoryExists("C:\temp") = False Then
            My.Computer.FileSystem.CreateDirectory("C:\Temp")
        End If

        ' create the output file, one should change "c:/temp/output.txt" to the path to the file you want to create
        ' note this code always creates a new file, if the file exists, it will be overwriten, change IO.FileMode.Create  to change this behavior see FileStream documentation.
        _fileStr = New System.IO.StreamWriter(New System.IO.FileStream("c:/temp/output.txt", IO.FileMode.Create))
    End Sub

    Private Sub StartDAQ()
        startDAQ_Button.Text = "Stop DAQ"
        _isDAQRunning = True


        Dim msInterval As Integer = 5 * 1000 ' 5seconds * 1000 ms/s == 5 sec in ms
        Dim totalIntervals As Integer = 2 'total # of intervals 
        Dim intervalSamples As Integer = 10 '# of samples/interval

        _ns.NsAsSelectParameters(1) ' need to turn the result on to get data.

        ' instead of this simple interval loop, one may want to use some other type of timing.
        For i As Integer = 1 To totalIntervals
            'TODO: put some sort of support for ending the script early, as killing the script would fail to close nanoscan, and possibly the filestream.
            If Not _isDAQRunning Then
                Return
            End If

            'Dim sampleThread = New Thread(Sub() Me.Sample(_ns, intervalSamples, _fileStr))
            'sampleThread.Start() ' take samples
            Sample(intervalSamples, _fileStr)

            If i < totalIntervals Then
                Threading.Thread.Sleep(msInterval) ' wait for the next interval.
            End If
        Next
        StopDAQ()
    End Sub

    Private Sub StopDAQ()
        startDAQ_Button.Text = "Start DAQ"
        _isDAQRunning = False
    End Sub

    'Sub Sample(ByRef ns As Object, ByVal n As Integer) ' late binding
    Public Sub Sample(ByVal n As Integer, ByRef outFile As IO.StreamWriter) ' early binding

        Dim x As Single
        Dim y As Single
        ' todo: add other values that need to be logged

        _ns.NsAsDataAcquisition = True

        ' note: there are many more ways of getting this data out at this point

        ' note: an autofind or something similar in not necessary before sampling in free runnig mode, the tracking algorithms should have stablized
        ' since we wait 1 second before getting a sample. If the sleep time is changed based on the rotation frequency one may need to add an autofind
        ' or wait a second for the tracking algoristhms to stabalize.

        For i As Integer = 1 To n
            If Not _isDAQRunning Then
                i = n
                _ns.NsAsDataAcquisition = False
                Return
            End If

            ' we should probably base this time on the rotation frequency
            Threading.Thread.Sleep(1000) 'make sure we get distinct samples

            _ns.NsAsGetBeamWidth(0, 0, 13.5F, x)
            _ns.NsAsGetBeamWidth(1, 0, 13.5F, y)
            ' todo: get other values that need to be logged

            ReadProfile()

            ' output the data to the screen
            System.Console.WriteLine("beam width x:{0} y:{1}", x, y) ' not strictly necessary

            beamWidthX_TextBox.Text = x.ToString()
            beamWidthY_TextBox.Text = y.ToString()

            'output data to the file
            outFile.WriteLine("beam width x:{0} y:{1}", x, y)
            'todo: log other values, or edit above to output other values, other things like time/date could also be logged.

        Next i

        _ns.NsAsDataAcquisition = False

    End Sub

    Private Sub ReadProfile()
        'currently we are only reading data from Aperture 1 (or X axis) to simplify the code.
        Dim aperture As Short = 0
        Dim leftBound As Single = 0
        Dim rightBound As Single = 0
        Dim samplingRes As Single = 0
        Dim decimation As Short = 1
        Dim numOfPoints As Integer = 100

        ' Initialize new arrays that will be marshaled through the COM interop.  The interop
        ' will handle conversion from VARIANT to .NET Array.  In this case, they will come
        ' back as an array of floats.
        Dim amplitude(numOfPoints) As Single
        Dim position(numOfPoints) As Single
        Dim ampVar As Object = New System.Runtime.InteropServices.VariantWrapper(amplitude)
        Dim posVar As Object = New System.Runtime.InteropServices.VariantWrapper(position)

        'Calculate the decimation factor to only retrieve numOfPoints
        _ns.NsAsGetApertureLimits(aperture, leftBound, rightBound)
        _ns.NsAsGetSamplingResolution(aperture, samplingRes)
        decimation = ((rightBound - leftBound) / samplingRes / numOfPoints)

        _ns.NsAsReadProfile(aperture, leftBound, rightBound, decimation, ampVar, posVar)

        'Stuff the data into an IEnumerable
        _profileDataX.Clear()
        _profileDataY.Clear()

        For value As Integer = 0 To (ampVar.Length - 1)
            _profileDataX.Add(Math.Round(posVar(value), 0))
            _profileDataY.Add(Math.Round(ampVar(value), 0))

            'Console.WriteLine("Values: (Amplitude, Position)")
            'Console.WriteLine("({0}, {1})", ampVar(value), posVar(value))
        Next

        RefreshChart()
    End Sub

    Private Sub RefreshChart()
        ' Auto axis scale
        profileChart.ChartAreas("ChartArea1").AxisY.Minimum = [Double].NaN
        profileChart.ChartAreas("ChartArea1").AxisY.Maximum = [Double].NaN

        profileChart.Series("Profile X").Points.DataBindXY(_profileDataX, _profileDataY)
        profileChart.Update()
    End Sub

    Private Sub initServer_Button_Click(sender As Object, e As EventArgs) Handles initServer_Button.Click
        If _isServerRunning Then
            _ns = Nothing
            _isServerRunning = False
            initServer_Button.Text = "Initialize Server"
            status_ToolStripStatusLabel.Text = "Server Closed"
            GC.Collect()
        Else
            initServer_Button.Text = "Close Server"
            Dim initThread As System.Threading.Thread
            initThread = New System.Threading.Thread(AddressOf InitializeServer)
            initThread.Start()
        End If
    End Sub

    Private Sub startDAQ_Button_Click(sender As Object, e As EventArgs) Handles startDAQ_Button.Click
        If _isDAQRunning Then
            StopDAQ()
        Else
            StartDAQ()
        End If
    End Sub
End Class
