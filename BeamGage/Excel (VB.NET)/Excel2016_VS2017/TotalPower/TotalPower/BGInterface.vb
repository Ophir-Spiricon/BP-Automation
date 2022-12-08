Imports Spiricon.Automation
Imports Spiricon.BeamGage.Automation

Public Class BGInterface
    'objects
    Dim _beamgageAutomation As AutomatedBeamGage

    'states
    Dim _shutDown As Boolean = False


    Public Delegate Sub UpdateFormDelegate()

    Public Sub Initialize()
        _beamgageAutomation = New AutomatedBeamGage("BGPExcelExample", False)
        _shutDown = False

        FrameEvents = New AutomationFrameEvents(_beamgageAutomation.ResultsPriorityFrame)
        AddHandler FrameEvents.OnNewFrame, AddressOf OnNewFrame

        CalibrationEvents = New AutomationCalibrationEvents(_beamgageAutomation.Calibration)
        AddHandler CalibrationEvents.OnCalibrationStatusChange, AddressOf OnCalibrationStatusChange

        Globals.Sheet1.UpdateSheet()
        Globals.Sheet1.UpdateUltracalStatus()
    End Sub

    Public Sub Shutdown()
        IsShuttingDown = True
        RemoveHandler FrameEvents.OnNewFrame, AddressOf OnNewFrame
        RemoveHandler CalibrationEvents.OnCalibrationStatusChange, AddressOf OnCalibrationStatusChange
        _beamgageAutomation.Instance.Shutdown()
    End Sub

    Public Sub RegisterForOnNewFrameEvents()
        ' The interface for frame availability and frame data
        ' The event that is called when a new frame is available

    End Sub

    Public Sub OnNewFrame()
        If (Not IsShuttingDown) Then
            Dim updateFormDelegate As UpdateFormDelegate = AddressOf Globals.Sheet1.UpdateSheet
            Globals.Sheet1.startStopButton.BeginInvoke(updateFormDelegate)
        End If
    End Sub



    Public Sub OnCalibrationStatusChange()
        Dim updateFormDelegate As BGInterface.UpdateFormDelegate = AddressOf Globals.Sheet1.UpdateUltracalStatus
        Globals.Sheet1.ultracalButton.Invoke(updateFormDelegate)

        If (Calibration.Status = CalibrationStatus.BEAM_DETECTED) Then
            ' Tell BGP to ignore the beam
            ' a more useful response might be to display a MessageBox
            Calibration.IgnoreBeam()
        End If
    End Sub

    Private Sub DoCalibrate()
        ' Calibrate - will not return until the calibration is complete
        Calibration.Ultracal()
    End Sub

    Public Property IsShuttingDown As Boolean
        Get
            Return _shutDown
        End Get
        Set(value As Boolean)
            _shutDown = value
        End Set
    End Property

    Public ReadOnly Property DataSource As IADataSource
        Get
            Return _beamgageAutomation.DataSource
        End Get
    End Property

    Public ReadOnly Property Frame As IAFrame
        Get
            Return _beamgageAutomation.ResultsPriorityFrame
        End Get
    End Property

    Public ReadOnly Property Calibration As IACalibration
        Get
            Return _beamgageAutomation.Calibration
        End Get
    End Property

    Public Property FrameEvents As AutomationFrameEvents

    Public Property CalibrationEvents As AutomationCalibrationEvents

    Public ReadOnly Property PowerEnergyResults As IAResultsPowerEnergy
        Get
            Return _beamgageAutomation.PowerEnergyResults
        End Get
    End Property

    Public ReadOnly Property FrameInfoResults As IAResultsFrameInfo
        Get
            Return _beamgageAutomation.FrameInfoResults
        End Get
    End Property
End Class
