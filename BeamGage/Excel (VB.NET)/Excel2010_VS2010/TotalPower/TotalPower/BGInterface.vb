Imports Spiricon.BeamGage.Automation

Public Class BGInterface
    'objects
    Dim _beamgageAutomation As BeamGageAutomation
    Dim _automationCLT As IAutomationCLT
    Dim _automationInstance As IAutomationInstance

    Dim _dataSource As IADataSource
    Dim _iaFrame As IAFrame
    Dim _iaCalibration As IACalibration

    Dim _iaResultsPowerEnergy As IAResultsPowerEnergy
    Dim _iaResultsFrameInfo As IAResultsFrameInfo

    Dim _automationFrameEvents As AutomationFrameEvents
    Dim _automationCalibrationEvents As AutomationCalibrationEvents

    'states
    Dim _shutDown As Boolean = False


    Public Delegate Sub UpdateFormDelegate()

    Public Sub Initialize()
        _beamgageAutomation = New BeamGageAutomation
        _automationCLT = _beamgageAutomation.GetV5Instance("BGPExcelExample", False)
        _shutDown = False

        _automationInstance = _automationCLT.GetInterfaceX("AUTOMATION_INSTANCE")
        _dataSource = _automationCLT.GetInterfaceX("AUTOMATION_DATA_SOURCE")
        _iaFrame = _automationCLT.GetInterfaceX("AUTOMATION_RESULTS_PRIORITY")
        _iaCalibration = _automationCLT.GetInterfaceX("AUTOMATION_CALIBRATION")

        _iaResultsPowerEnergy = _automationCLT.GetInterfaceX("AUTOMATION_RESULTS_POWER_ENERGY")
        _iaResultsFrameInfo = _automationCLT.GetInterfaceX("AUTOMATION_RESULTS_FRAME_INFO")

        FrameEvents = New AutomationFrameEvents(Frame)
        AddHandler FrameEvents.OnNewFrame, AddressOf OnNewFrame

        _automationCalibrationEvents = New AutomationCalibrationEvents(_iaCalibration)
        AddHandler _automationCalibrationEvents.OnCalibrationStatusChange, AddressOf OnCalibrationStatusChange

        Globals.Sheet1.UpdateSheet()
        Globals.Sheet1.UpdateUltracalStatus()
    End Sub

    Public Sub Shutdown()
        IsShuttingDown = True
        RemoveHandler FrameEvents.OnNewFrame, AddressOf OnNewFrame
        RemoveHandler _automationCalibrationEvents.OnCalibrationStatusChange, AddressOf OnCalibrationStatusChange
        _automationInstance.Shutdown()
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
            Return _dataSource
        End Get
    End Property

    Public ReadOnly Property Frame As IAFrame
        Get
            Return _iaFrame
        End Get
    End Property

    Public ReadOnly Property Calibration As IACalibration
        Get
            Return _iaCalibration
        End Get
    End Property

    Public Property FrameEvents As AutomationFrameEvents
        Get
            Return _automationFrameEvents
        End Get
        Set(value As AutomationFrameEvents)
            _automationFrameEvents = value
        End Set
    End Property

    Public ReadOnly Property PowerEnergyResults As IAResultsPowerEnergy
        Get
            Return _iaResultsPowerEnergy
        End Get
    End Property

    Public ReadOnly Property FrameInfoResults As IAResultsFrameInfo
        Get
            Return _iaResultsFrameInfo
        End Get
    End Property
End Class
