Imports Spiricon.Automation
Imports System.Threading

Public Class MainForm
    'Class needed to instantiate and start an IV5AppServer
    Dim _beamgageAutomation As AutomatedBeamGage

    ' Calibration status change events proxy for automation clients that don't fully support remoted events
    Dim _automationCalibrationEvents As AutomationCalibrationEvents

    ' On new frame events proxy for automation clients that don't fully support remoted events
    Dim _automationFrameEvents As AutomationFrameEvents

    ' Flag to remember the staus of the Start/Stop button
    Dim _running As Boolean
    ' Flag so that we don't try to access BGP after we call Shutdown()
    Dim _shutDown As Boolean

    Public Delegate Sub UpdateFormDelegate()

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ' Flag so that we don't try to access BGP after we call Shutdown()
        _shutDown = True

        ' Remove the new frame event because we are shutting down
        RemoveHandler _automationFrameEvents.OnNewFrame, AddressOf OnNewFrame

        ' Shutdown BGP when the form is closed
        _beamgageAutomation.Instance.Shutdown()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Class needed to instantiate and start an IV5AppServer
        _beamgageAutomation = New AutomatedBeamGage("BGPVisualBasicExample", True)

        ' initialize whether BGP is running or not
        _running = (_beamgageAutomation.DataSource.Status = ADataSourceStatus.RUNNING)
        If (_running) Then
            Button1.Text = "Stop"
        Else
            Button1.Text = "Start"
        End If

        ' The event that is called when a new frame is available
        _automationFrameEvents = New AutomationFrameEvents(_beamgageAutomation.ResultsPriorityFrame)
        AddHandler _automationFrameEvents.OnNewFrame, AddressOf OnNewFrame

        ' The event that is called when the calibration status changes
        _automationCalibrationEvents = New AutomationCalibrationEvents(_beamgageAutomation.Calibration)
        AddHandler _automationCalibrationEvents.OnCalibrationStatusChange, AddressOf OnCalibrationStatusChange

    End Sub

    ''' <summary>
    ''' This method is declared above as the delegate method for the OnNewFrame event
    ''' Any actions here occur within the BeamGage data aqcuisition cycle
    ''' To avoid blocking BeamGage, minimize actions here, Or get data And push work to other threads.
    ''' </summary>
    Private Sub OnNewFrame()
        If (Not _shutDown) Then
            Dim updateFormDelegate As UpdateFormDelegate = AddressOf UpdateForm
            Me.BeginInvoke(updateFormDelegate)
        End If
    End Sub

    Private Sub UpdateForm()
        ' Display the Total PowerEnergy result
        Total.Text = _beamgageAutomation.PowerEnergyResults.Total.ToString("#,#")

        ' The date and time as passed as an "OLE Automation Date" and must be converted to VB Date structure
        Dim dateTime As Date = Date.FromOADate(_beamgageAutomation.FrameInfoResults.Timestamp)
        TimeStamp.Text = dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString()

        ' DoubleData is a 1-dimensional array of the entire frame of data
        Dim frameData As Integer() = _beamgageAutomation.ResultsPriorityFrame.FrameData()

        ' Calculate the correct shift to get the data back to pixel counts
        Dim bpp As Integer = _beamgageAutomation.ResultsPriorityFrame.OriginalBpp + 1
        bpp = 32 - bpp
        frameData(0) >>= bpp
        frameData(1) >>= bpp

        ' Data values from the first row of data
        Row1Col1.Text = frameData(0).ToString
        Row1Col2.Text = frameData(1).ToString

        ' because DoubleData is a 1-dimensional array, we calculate the offset of each row as a multiple of Width
        Dim width As Integer = _beamgageAutomation.FrameInfoResults.Width

        ' Data values from the second row of data
        frameData(width) >>= bpp
        frameData(width + 1) >>= bpp
        Row2Col1.Text = frameData(width).ToString
        Row2Col2.Text = frameData(width + 1).ToString
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Interface for data source control
        Dim dataSource As IADataSource
        dataSource = _beamgageAutomation.DataSource

        ' if running stop, if not start
        If (_running) Then
            _running = False
            dataSource.Stop()
            Button1.Text = "Start"
        Else
            _running = True
            dataSource.Start()
            Button1.Text = "Stop"
        End If
    End Sub

    Private Sub UpdateLabel4()

        Select Case _beamgageAutomation.Calibration.Status
            ' BGP cannot calibrate if there is a beam on the detector
            Case CalibrationStatus.BEAM_DETECTED
                Label4.Text = "Please block the beam"
            Case CalibrationStatus.FAILED
                Label4.Text = "Calibration failed"
            Case CalibrationStatus.READY
                Label4.Text = ""
        End Select

    End Sub

    Private Sub OnCalibrationStatusChange()

        Dim updateFormDelegate As UpdateFormDelegate = AddressOf UpdateLabel4
        Me.Invoke(updateFormDelegate)

        If (_beamgageAutomation.Calibration.Status = CalibrationStatus.BEAM_DETECTED) Then
            ' Tell BGP to ignore the beam
            ' a more useful response might be to display a MessageBox
            _beamgageAutomation.Calibration.IgnoreBeam()
        End If


    End Sub

    Private Sub DoCalibrate()
        ' Calibrate - will not return until the calibration is complete
        _beamgageAutomation.Calibration.Ultracal()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        ' Just something to show that we're working
        Label4.Text = "Calibrating..."

        ' We are on the UI thread. 
        ' Need to call Ultracal from a separate thread otherwise the UI will be locked out
        Dim calThread As Thread = New Thread(New ThreadStart(AddressOf DoCalibrate))
        calThread.Start()

    End Sub
End Class
