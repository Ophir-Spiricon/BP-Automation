Imports Spiricon.BeamGage.Automation
Imports Spiricon.BeamGage.Automation.Interfaces
Imports System.Threading
Imports Microsoft.Win32

Public Class Sheet1

    Dim _bgInterface As BGInterface = New BGInterface()
    Dim _isServerRunning As Boolean = False

    Dim _runStateCell As Excel.Range = Me.Application.Range("C3")
    Dim _ultracalStatusCell As Excel.Range = Me.Application.Range("C4")
    Dim _totalPwrCell As Excel.Range = Me.Application.Range("C5")
    Dim _totalPwrUnitsCell As Excel.Range = Me.Application.Range("D5")
    Dim _timeStampCell As Excel.Range = Me.Application.Range("C6")

    Dim _frameDataRange As Excel.Range = Me.Application.Range("B9:F18")


    Private Sub Sheet1_Startup(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Startup
        _totalPwrCell.NumberFormat = "0.000E+00"
        _timeStampCell.NumberFormat = "m/d/yyyy hh:mm:ss.000 AM/PM"

        CreateWoW6432RegKey() 'We need to create a new set of the BeamGage keys in 64-bit
    End Sub

    Private Sub Sheet1_Shutdown() Handles Me.Shutdown
        Try
            _bgInterface.Shutdown()
        Catch ex As Exception
            'Do Nothing
        End Try
    End Sub

    Private Sub automationButton_Click(sender As Object, e As EventArgs) Handles automationButton.Click
        If (Not _isServerRunning) Then
            automationButton.Text = "Initializing Server..."
            _bgInterface.Initialize()
            _isServerRunning = True
            automationButton.Text = "Close Server"
        Else
            _bgInterface.Shutdown()
            _isServerRunning = False
            automationButton.Text = "Initialize Server"
        End If
    End Sub

    Private Sub ultracalButton_Click(sender As System.Object, e As System.EventArgs) Handles ultracalButton.Click
        If (Not _isServerRunning) Then
            Return
        End If

        ' Just something to show that we're working
        _ultracalStatusCell.Value = "Calibrating..."

        ' We are on the UI thread. 
        ' Need to call Ultracal from a separate thread otherwise the UI will be locked out
        Dim calThread As Thread = New Thread(New ThreadStart(AddressOf _bgInterface.Calibration.Ultracal))
        calThread.Start()
    End Sub

    Private Sub startStopButton_Click(sender As System.Object, e As System.EventArgs) Handles startStopButton.Click
        If (Not _isServerRunning) Then
            Return
        End If

        ' if running stop, if not start
        If (_bgInterface.DataSource.Status = ADataSourceStatus.RUNNING) Then
            _bgInterface.DataSource.Stop()
            startStopButton.Text = "Start"
        Else
            _bgInterface.DataSource.Start()
            startStopButton.Text = "Stop"
        End If
        UpdateSheet()
    End Sub


    Public Sub UpdateSheet()
        If (_bgInterface.IsShuttingDown) Then
            Return
        End If
        Try
            'TODO fix occasional crash here
            If (_bgInterface.DataSource.Status = ADataSourceStatus.RUNNING) Then
                _runStateCell.Value = "Running"
                startStopButton.Text = "Stop"
            Else
                _runStateCell.Value = "Stopped"
                startStopButton.Text = "Start"
            End If

            _totalPwrCell.Value = _bgInterface.PowerEnergyResults.Total.ToString("#,#")
            _timeStampCell.Value = _bgInterface.FrameInfoResults.Timestamp.ToString()

            Dim frameData As Integer() = _bgInterface.Frame.FrameData

            ' Calculate the correct shift to get the data back to pixel counts
            Dim bpp As Integer = _bgInterface.Frame.OriginalBpp + 1
            bpp = 32 - bpp

            Dim width As Integer = _bgInterface.FrameInfoResults.Width
            Dim data As Double(,)
            ReDim data(_frameDataRange.Rows.Count, width)
            Dim valueIndex As Integer = 0

            For row As Integer = 0 To _frameDataRange.Rows.Count - 1
                For value As Integer = 0 To width - 1
                    frameData(valueIndex) >>= bpp
                    data(row, value) = frameData(valueIndex)

                    valueIndex += 1
                Next
            Next
            _frameDataRange.Value = data
        Catch ex As Runtime.InteropServices.COMException
            Return 'Sometimes we get a random COMException when clicking on a cell
        End Try
    End Sub

    Public Sub UpdateUltracalStatus()
        _ultracalStatusCell.Value = _bgInterface.Calibration.Status.ToString()

        Select Case _bgInterface.Calibration.Status
            Case CalibrationStatus.BEAM_DETECTED
                _ultracalStatusCell.Value = "Please block the beam"
            Case CalibrationStatus.FAILED
                _ultracalStatusCell.Value = "Calibration Failed"
            Case CalibrationStatus.READY
                _ultracalStatusCell.Value = "Ready"
            Case CalibrationStatus.CALIBRATING
                _ultracalStatusCell.Value = "Calibrating..."
            Case CalibrationStatus.NOT_SUPPORTED
                _ultracalStatusCell.Value = "Not Supported"
        End Select
    End Sub

    Private Sub CreateWoW6432RegKey()
        Dim regVersion5 As RegistryKey
        Dim key As String

        key = "Software\\Spiricon\\Version5"

        regVersion5 = Registry.LocalMachine.OpenSubKey(key, False)

        If (regVersion5 Is Nothing) Then
            regVersion5 = Registry.LocalMachine.CreateSubKey(key)
            If (Not regVersion5 Is Nothing) Then
                ' Create the(New Keys)
                regVersion5.SetValue("BGE_", "C:\Program Files\Spiricon\BeamGage Enterprise")
                regVersion5.SetValue("Console Service_", "C:\Program Files\Spiricon\Console Service")
            End If
        End If
        regVersion5.Close()
    End Sub

    'Private Sub RecurseCopyKey(sourceKey As RegistryKey, destinationKey As RegistryKey)
    '    'copy all the values
    '    For Each valueName As String In sourceKey.GetValueNames()
    '        Dim objValue As Object = sourceKey.GetValue(valueName)
    '        Dim valKind As RegistryValueKind = sourceKey.GetValueKind(valueName)
    '        destinationKey.SetValue(valueName, objValue, valKind)
    '    Next

    '    'For Each subKey 
    '    'Create a new subKey in destinationKey 
    '    'Call myself 
    '    For Each sourceSubKeyName As String In sourceKey.GetSubKeyNames()
    '        Dim sourceSubKey As RegistryKey = sourceKey.OpenSubKey(sourceSubKeyName)
    '        Dim destSubKey As RegistryKey = destinationKey.CreateSubKey(sourceSubKeyName)
    '        RecurseCopyKey(sourceSubKey, destSubKey)
    '    Next
    'End Sub
End Class
