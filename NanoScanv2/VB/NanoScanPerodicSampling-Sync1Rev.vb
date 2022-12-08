' this code assumes it is a console app, and has a console.
Module Module1

    ' periodic sampling sample script.

    ' this script is designed to take a finite number of samples at regular intervals and log them to a file and/or the screen.
    '
    ' as a sample, clarity of code is the most important, and most error checking has been omitted for clarity.
    '
    ' this script contains two types of comments, explanatory comments, and commented out code.
    '
    ' the commented out code come in two varieties: optional and code meant to be toggled between the two choices.

    Sub Main()

        ' first we need to open NanoScan, there are two methods early and late binding
        ' to add a reference (for early binding):
        ' right click on the project name in solution explorer, choose add reference, switch to the COM tab find NanoScanII.
        ' NOTE: nanoscan must be run at least once before adding the reference. (also necessary for late binding as well)

        ' early binding (requires reference to be added to project) 
        ' intellisense will give you a list of functions.
        Dim ns As NanoScanII.INanoScanII
        ns = New NanoScanII.NsAs

        'Late binding, no need to add the reference, will still work.
        'But no intellisense list of functions.
        'Dim ns As Object
        'ns = GetObject("", "photon-nanoscan")

        ' if you have multiple nanoscan controllers installed on the system, you will have to select your head here
        ' ns.NsAsGetDeviceList(...) 'you will have to replace the elipses (...) with actual code
        ' ...
        ' ns.NsAsSetDeviceID(...)

        ' perform any initialization here.
        ' i.e. set ROIs or AutoROI.
        ' etc.

        ' 10 second delay for testing, so i can move the window, and/or make some quick changes like starting the stats window.
        'Threading.Thread.Sleep(10000)

        ' Sync1Rev doesn't run tracking at all, so we need to set the parameters accordingly, this is espesially necessary if using recompute
        ' instead of the new RunComputation, RunComputation will do tracking so the first few samples could be incorrect if this call is omitted.
        ns.NsAsAutoFind()

        ' since the loop is an interval loop, if you want to start your measurements at a specific time you will need to
        ' test against the current time, check the documentation for System.DateTime.
        ' System.DateTime.Now

        ns.NsAsShowWindow = True 'the window is hidden by default property toggles visiblity.

        ' make sure the folder exists to create the file
        If My.Computer.FileSystem.DirectoryExists("C:\temp") = False Then
            My.Computer.FileSystem.CreateDirectory("C:\Temp")
        End If

        ' create the output file, one should change "c:/temp/output.txt" to the path to the file you want to create
        ' note this code always creates a new file, if the file exists, it will be overwriten, change IO.FileMode.Create to change this behavior see FileStream documentation.
        Dim fileStr As System.IO.StreamWriter = New System.IO.StreamWriter(New System.IO.FileStream("c:/temp/output.txt", IO.FileMode.Create))

        ' Interval setup
        Dim msInterval As Integer = 30 * 60 * 1000 ' 30min * 60s/min * 1000 ms/s == 30 min in ms
        Dim totalIntervals As Integer = 200 'total # of intervals 
        Dim intervalSamples As Integer = 10 '# of samples/interval

        ' Interval setup for testing the script.
        'Dim msInterval As Integer = 10 * 1000 ' 10 second interval for testing
        'Dim totalIntervals As Integer = 2 'total # of intervals 

        'Sample(ns, intervalSamples, fileStr) 'optional: take a sample at start.

        ns.NsAsSelectParameters(1) ' need to turn the result on to get data.

        ' instead of this simple interval loop, one may want to use some other type of timing.
        For i As Integer = 1 To totalIntervals

            'TODO: put some sort of support for ending the script early, as killing the script would fail to close nanoscan, and possibly
            'fail to close the file stream as well. this is likely because the process is asleep when it gets killed.

            Threading.Thread.Sleep(msInterval) ' wait for the next interval.
            'Threading.Thread.Sleep(msInterval - deltaSampleTime) ' instead you may want to account for the time it takes to call sample.

            Sample(ns, intervalSamples, fileStr) ' take samples

        Next

    End Sub

    'Sub Sample(ByRef ns As Object, ByVal n As Integer) ' late binding
    Sub Sample(ByRef ns As NanoScanII.INanoScanII, ByVal n As Integer, ByRef outFile As IO.StreamWriter) ' early binding

        Dim x As Single
        Dim y As Single
        ' todo: add other values that need to be logged

        ' note: there are many more ways of getting this data out at this  point

        For i As Integer = 1 To n

            ns.NsAsAcquireSync1Rev() 'sync1rev won't do profile averaging at all, even if it is setup

            'ns.NsAsRecompute() ' no tracking performed even if setup 'sync1rev
            ns.NsAsRunComputation() ' run computations just as if the acquisition loop were running, i.e. run tracking etc. 'sync1rev

            ' get values
            ns.NsAsGetBeamWidth(0, 0, 13.5F, x)
            ns.NsAsGetBeamWidth(1, 0, 13.5F, y)
            ' todo: get other values that need to be logged

            ' output the data to the screen
            System.Console.WriteLine("beam width x:{0} y:{1}", x, y) ' not strictly necessary, but shows progress in the console when it happens.

            'output data to the file
            outFile.WriteLine("beam width x:{0} y:{1}", x, y)
            'todo: log other values, or edit above to output other values, other things like time/date could also be logged.

        Next i

    End Sub

End Module
