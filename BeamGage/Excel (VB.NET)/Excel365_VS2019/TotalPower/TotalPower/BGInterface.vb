Imports System.ComponentModel
Imports System.Reflection
Imports Spiricon.Automation
Imports Spiricon.BeamGage.Automation
Imports System.Runtime.CompilerServices

Public Class BGInterface
    Dim _beamgageAutomation As AutomatedBeamGage
    Dim _NewFrameDelegate As newFrame
    Dim _CalibrationStatusChangeDelegate As NonRemotingCalibrationStatusDelegate

    Dim _shutDown As Boolean = False


    Public Delegate Sub UpdateFormDelegate()

    Public Sub Initialize()
        _beamgageAutomation = New AutomatedBeamGage("BGPExcelExample", False)
        IsShuttingDown = False

        RegisterForOnNewFrameEvents()

        Globals.Sheet1.UpdateSheet()
        Globals.Sheet1.UpdateUltracalStatus()
    End Sub

    Public Sub Shutdown()
        Try
            IsShuttingDown = True

            ' This doesn't seem to remove the event handlers possibly VB creates anonymous signatures
            ' for the handlers that don't match the originals.
            ' This causes the sheet to hang for a few seconds upon exit.
            ' The EventFields module extension method included with this class are several
            ' attempts at circumventing this, but seem to also fail at obtaining the runtime instance
            ' https://stackoverflow.com/questions/17426571/vb-net-cant-remove-handler
            If (FrameEvents IsNot Nothing) Then
                RemoveHandler FrameEvents.OnNewFrame, _NewFrameDelegate
                FrameEvents = Nothing
            End If

            If (CalibrationEvents IsNot Nothing) Then
                RemoveHandler CalibrationEvents.OnCalibrationStatusChange, _CalibrationStatusChangeDelegate
                CalibrationEvents = Nothing
            End If

            If (_beamgageAutomation IsNot Nothing) Then
                If (_beamgageAutomation.Instance IsNot Nothing) Then

                    _beamgageAutomation.Instance.Shutdown()
                    _beamgageAutomation.Dispose()
                End If
            End If
        Catch ex As Exception
            'Do Nothing
        End Try
    End Sub

    Public Sub RegisterForOnNewFrameEvents()
        _NewFrameDelegate = AddressOf OnNewFrame
        _CalibrationStatusChangeDelegate = AddressOf OnCalibrationStatusChange

        FrameEvents = New AutomationFrameEvents(_beamgageAutomation.ResultsPriorityFrame)
        AddHandler FrameEvents.OnNewFrame, _NewFrameDelegate

        CalibrationEvents = New AutomationCalibrationEvents(_beamgageAutomation.Calibration)
        AddHandler CalibrationEvents.OnCalibrationStatusChange, _CalibrationStatusChangeDelegate

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

Module EventFields
    <Extension()>
    Public Sub ClearEventInvocations(obj As Object, eventName As String)
        Dim fi = obj.GetType().GetEventField(eventName)

        If (fi Is Nothing) Then Return

        fi.SetValue(obj, Nothing)
    End Sub

    <Extension()>
    Public Sub RemoveEventHandler(obj As Object, eventName As String, handler As [Delegate])
        If (obj Is Nothing Or eventName IsNot GetType(String)) Then Return

        Dim events As EventInfo() = obj.GetType().GetEvents()
        Dim ei As EventInfo = obj.GetType().GetEvent(eventName)


        ei.RemoveEventHandler(obj, handler)
    End Sub

    <Extension()>
    Private Function GetEventField(type As Type, eventName As String)
        Dim field As FieldInfo = Nothing

        While (type IsNot Nothing)
            'Find events defined as field
            field = type.GetField(eventName, BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase)

            If ((field IsNot Nothing) And (field.FieldType Is GetType(MulticastDelegate) Or field.FieldType.IsSubclassOf(GetType(MulticastDelegate)))) Then Exit While

            'Find events defined as property { add; remove; }
            field = type.GetField("EVENT_" + eventName.ToUpper(), BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.NonPublic)

            If (field IsNot Nothing) Then Exit While
            type = type.BaseType
        End While

        Return field
    End Function

    <Extension()>
    Public Function GetEventHandler(obj As Object, eventName As String)
        Dim retDelegate As [Delegate]() = Nothing
        Dim fi As FieldInfo = Nothing

        fi = obj.GetType().GetField(eventName, BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase)

        If (fi IsNot Nothing) Then
            Dim value As Object = fi.GetValue(obj)
            If (TypeOf value Is [Delegate]()) Then
                retDelegate = value
            ElseIf (value IsNot Nothing) Then 'value Then may be just Object
                Dim pi As PropertyInfo = obj.GetType().GetProperty("Events", BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase)
                If (pi IsNot Nothing) Then
                    Dim eventHandlers As EventHandlerList = pi.GetValue(obj)
                    If (eventHandlers IsNot Nothing) Then
                        retDelegate = eventHandlers(value).GetInvocationList()
                    End If
                End If
            End If
        End If
        Return retDelegate
    End Function
End Module

