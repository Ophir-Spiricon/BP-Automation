function BSQ_AutomatedMode_Example
    clear
    clc
    
    try
        InitializeBSQAutomation();
        VerifyAlignment();
        ConfigureRun();
        StartRun();
        WaitForRunEnd();
    catch ME
        PrintException(ME, true);
    end
    
    cleanup = onCleanup(@Shutdown);
end

 %% Initialization of BeamSquared Automation instance
 function InitializeBSQAutomation
    global bsq
    
    LoadAssemblies();
    
    % Instantiate BeamSquared Automation instance
    bsq = M2.Automation.AutomatedBeamSquared(true);
 end

 %% Configure and command BeamSquared runs
 function VerifyAlignment
    global bsq
    if (bsq.RunManager.RunStatus == M2.Automation.Interfaces.ARunManagerStatus.READY) && ~bsq.Capture.LiveMode
        fprintf("Starting Live Mode\n");
        bsq.Capture.LiveMode = true;
    end
    
    railControl = bsq.RailControl;
    moveHome = 'Rail Home';
    moveEnd = 'Rail End';
    finished = 'Finished';
    while (true)
        choice = menu('Confirm Beam Alignment',moveHome,moveEnd,finished);

        switch(choice)
            case 1
                railControl.Position = railControl.MinPosition;
            case 2
                railControl.Position = railControl.MaxPosition;            
            case 3
                fprintf("Stopping Live Mode\n");
                bsq.Capture.LiveMode = false;
                return;
        end
    end
 end
 
 % Must configure these parameters to match optical and laser environemnt
 function ConfigureRun
    global bsq
    m2Setup = bsq.Setup;
    m2Setup.Model = "Example laser Model";
    m2Setup.SerialNumber = "Example Serial number";
    m2Setup.Comment = "This is a example project for BeamSquared";
    m2Setup.WaveLength = 632;
    m2Setup.LaserLocation = 120;
    fprintf("Calculated Focal Length: %.3f\n", m2Setup.FocalLength);
    
    zGenerator = bsq.SuggestedZGenerator;
    zGenerator.ZStart = 370;
    zGenerator.ZEnd = 670;
    zGenerator.Step = 20;
 end
 
 function StartRun
    global bsq isRunning
    global hCurveFitAvailable hRunManagerStatus hUltracalVerifierStatus
    global hFrameAnalyzerStatus hFrameAvailableStatus hPositionSet
    
    runManager = bsq.RunManager;
    railControl = bsq.RailControl;
    frameAnalyzer = bsq.Analyzer;
    frameCapture = bsq.Capture;
    ultracalVerifier = bsq.UltracalVerifier;
    isRunning = false;
    
    % Register RunManager event delegate handlers
    % OnCurveFitAvailable - provides notification that the initial curve fit has been calculated after the first five step locations
    % OnStatusChange - provides notification that the M2 run status has changed
    runManagerEvents = M2.Automation.Interfaces.AutomationRunManagerEvents(runManager);
    hCurveFitAvailable = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.CurveFitAvailableEventArgs'}, @OnCurveFitAvailable);
        runManagerEvents.GetType().GetEvent('OnCurveFitAvailable').AddEventHandler(runManagerEvents,hCurveFitAvailable);
    hRunManagerStatus = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.RunStatusChangeEventArgs'}, @OnRunStatusChange);
        runManagerEvents.GetType().GetEvent('OnStatusChange').AddEventHandler(runManagerEvents,hRunManagerStatus);

    % Register UltracalVerifier event delegate handler
    % OnStatusChange - Provides notification of Ultracal background calibration status on the current frame 
    ultracalVerifierEvents = M2.Automation.Interfaces.AutomationUltracalVerifierEvents(ultracalVerifier);
    hUltracalVerifierStatus = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.UltracalVerifierEventArgs'}, @OnUltracalVerifierStatusChange);
        ultracalVerifierEvents.GetType().GetEvent('OnStatusChange').AddEventHandler(ultracalVerifierEvents,hUltracalVerifierStatus);
    
    % Register FrameAnalyzer event delegate handler
    % OnStatusChange - Provides notification of quality issues on the current frame 
    frameAnalyzerEvents = M2.Automation.Interfaces.AutomationAnalyzerEvents(frameAnalyzer);
    hFrameAnalyzerStatus = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.FrameAnalyzerEventArgs'}, @OnFrameAnalyzerStatusChange);
        frameAnalyzerEvents.GetType().GetEvent('OnStatusChange').AddEventHandler(frameAnalyzerEvents,hFrameAnalyzerStatus);
        
    % Register Capture event delegate handler
    % OnFrameAvailable - provides notification that a good frame for measurement is available
    automationCaptureEvents = M2.Automation.Interfaces.AutomationCaptureEvents(frameCapture);
    hFrameAvailableStatus = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.FrameAvailableEventArgs'}, @OnGoodFrameAvailable);
        automationCaptureEvents.GetType().GetEvent('OnFrameAvailable').AddEventHandler(automationCaptureEvents,hFrameAvailableStatus);    
        
    % Register RailControl event delegate handler for BeamSquared hardware system
    % OnPositionSet - provides notification that the rail position of the BeamSquared hardware is set
    railControlEvents = M2.Automation.Interfaces.AutomationRailControlEvents(railControl);
    hPositionSet = NET.createGeneric('System.EventHandler', {'M2.Automation.Interfaces.RailControlPositionEventArgs'}, @OnPositionSet);
        railControlEvents.GetType().GetEvent('OnPositionSet').AddEventHandler(railControlEvents,hPositionSet);
    
    runManager.RunMethod = M2.Automation.Interfaces.ARunManagerMethods.SUGGESTED;
    
    runManager.StartRun();
    
    % wait for the run to start to prevent falling through to exit
    while (~isRunning)
        pause(1);
    end
 end
 
 %% Obtain Frame and Run results
 function GetQuantitativeResults
    global bsq
    fprintf('---Quantitative Results---\n');
    frameResults = bsq.QuantitativeResults;
    fprintf('BeamWidth X: %.3f\n', frameResults.BeamWidthX);
    fprintf('BeamWidth Y: %.3f\n', frameResults.BeamWidthY);
    fprintf('Centroid X: %.3f\n', frameResults.CentroidX);
    fprintf('Centroid Y: %.3f\n', frameResults.CentroidY);
    fprintf('Peak: %.3f\n', frameResults.Peak);
    fprintf('\n');
 end
 
 function GetLaserResults
    global bsq
    fprintf('---Laser Results---\n');
    laserResults = bsq.LaserResults;
    fprintf('M2 X: %.3f\n', laserResults.M2X);
    fprintf('M2 Y: %.3f\n', laserResults.M2Y);
    fprintf('Divergence X: %.3f\n', laserResults.DivergenceX);
    fprintf('Divergence Y: %.3f\n', laserResults.DivergenceY);
    fprintf('Rayleigh X: %.3f\n', laserResults.RayleighX);
    fprintf('Rayleigh Y: %.3f\n', laserResults.RayleighY);
    fprintf('WaistLocation X: %.3f\n', laserResults.WaistLocationX);
    fprintf('WaistLocation Y: %.3f\n', laserResults.WaistLocationY);
    fprintf('WaistWidth X: %.3f\n', laserResults.WaistWidthX);
    fprintf('WaistWidth Y: %.3f\n', laserResults.WaistWidthY);
    fprintf('Astigmatism: %.3f\n', laserResults.Astigmatism);
    fprintf('Asymmetry: %.3f\n', laserResults.Asymmetry);
    fprintf('\n');
 end
 
 function GetLensResults()
    global bsq
    fprintf('---After Lens Results---\n');
    lensResults = bsq.LensResults;
    fprintf('WaistWidth X: %.3f\n', lensResults.WaistWidthX);
    fprintf('WaistWidth Y: %.3f\n', lensResults.WaistWidthY);
    fprintf('\n');
 end
 
 %% Instance Shutdown
 function Shutdown
    global bsq
    global hCurveFitAvailable hRunManagerStatus hUltracalVerifierStatus
    global hFrameAnalyzerStatus hFrameAvailableStatus hPositionSet

    try
        % unregister delegates
        delete(hCurveFitAvailable);
        delete(hRunManagerStatus);
        delete(hUltracalVerifierStatus);
        delete(hFrameAnalyzerStatus);
        delete(hFrameAvailableStatus);
        delete(hPositionSet);
    
        % Shutdown BeamGage
        bsq.Instance.Shutdown;
     catch  ME
         %do nothing, just close
     end
 end
 
 function WaitForRunEnd
    global isRunning
    while (isRunning)
        pause(1);
    end
 end
 
 %% Event handler delegates
 % To stay synchronized with BeamSquared six events are provided.
 % FrameData and Results should be obtained during these delegates
 % Implementation in delegates is blocking, so should be kept miminal
 % pushing obtained data to other threads for further processing or
 % calculation.
 function OnRunStatusChange(objSender, eventData)
    global isRunning
    switch eventData.Status
        case M2.Automation.Interfaces.ARunManagerStatus.RUNNING
            fprintf('\n');
            fprintf('***Run Started***\n');
            isRunning = true;
        case M2.Automation.Interfaces.ARunManagerStatus.RUN_FINISHED
            fprintf('\n');
            fprintf('***Run Complete***\n');
            GetLaserResults();
            GetQuantitativeResults();
            isRunning = false;
        case M2.Automation.Interfaces.ARunManagerStatus.ABORTED
            fprintf('\n');
            fprintf('!!!Run Aborted!!!\n');
            isRunning = false;
    end
 end
 
 function OnGoodFrameAvailable(objSender, eventData)
    global isRunning
    
    if ~isRunning
        return;
    end
    
    if eventData.Available
        fprintf('A good frame is available\n');
        GetQuantitativeResults();
    else
        % Console.WriteLine("A good frame is no longer available\n");
    end
 end
 
 function OnFrameAnalyzerStatusChange(objSender, eventData)
    global bsq isRunning
    
    if ~isRunning
        return;
    end
    
    maxAD = 2^bsq.QuantitativeResults.BitsPerPixel;
    peakPercent = (bsq.QuantitativeResults.Peak / maxAD) * 100;
    widthX = bsq.QuantitativeResults.BeamWidthX;
    widthY = bsq.QuantitativeResults.BeamWidthY;
    
    switch eventData.Status
        case M2.Automation.Interfaces.AAnalyzerStatus.BIG
            fprintf('Beam is too big | Width X: %.3f | Width Y: %.3f\n',widthX,widthY);
        case M2.Automation.Interfaces.AAnalyzerStatus.SMALL
            fprintf('Beam is too small | Width X: %.3f | Width Y: %.3f\n}',widthX,widthY);
        case M2.Automation.Interfaces.AAnalyzerStatus.BRIGHT
            fprintf('Beam is too bright. | Peak: %.2f%% |  Peak of <90%% Max Recommended\n',peakPercent);
        case M2.Automation.Interfaces.AAnalyzerStatus.DIM
            fprintf('Beam is too dim. | Peak: %.2f%% | Peak of >50%% Recommended\n',peakPercent);
%         case bitor(M2.Automation.Interfaces.AAnalyzerStatus.SMALL,M2.Automation.Interfaces.AAnalyzerStatus.BRIGHT)
%         	fprintf('Beam is too small and bright | Peak: %.3f%% of Max | Width X: %.3f | Width Y: %.3f\n',...
%                 peakPercent,widthX,widthY);
%         case bitor(M2.Automation.Interfaces.AAnalyzerStatus.BIG,M2.Automation.Interfaces.AAnalyzerStatus.DIM)
%             fprintf('Beam is too big and dim | Peak: %.3f%% of Max | Width X: %.3f | Width Y: %.3f\n',...
%                 peakPercent,widthX,widthY);
        case M2.Automation.Interfaces.AAnalyzerStatus.ALIGNMENT
            fprintf('Beam is not aligned parallel\n');
        case M2.Automation.Interfaces.AAnalyzerStatus.NOT_ULTRACALED
            fprintf('Baseline is not calibrated\n');
        case M2.Automation.Interfaces.AAnalyzerStatus.GOOD
            fprintf('Beam is OK | Peak: %.2f%% of Max | Width X: %.3f | Width Y: %.3f\n',...
                peakPercent,widthX,widthY);
        case M2.Automation.Interfaces.AAnalyzerStatus.UNKNOWN
            fprintf('Frame status is unknown\n');
    end
 end
 
 function OnUltracalVerifierStatusChange(objSender, eventData)
    global isRunning
     
    if ~isRunning
        return;
    end
    
    switch eventData.Status
        case M2.Automation.Interfaces.AUltracalVerifierStatus.VALIDATING
                fprintf('Validating Baseline Bias\n');
        case M2.Automation.Interfaces.AUltracalVerifierStatus.ULTRACALING
                fprintf('Ultracal Started\n');
        case M2.Automation.Interfaces.AUltracalVerifierStatus.FINISHED
                fprintf('Waiting For Beam Block/Unblock\n');
    end
 end
 
 function OnCurveFitAvailable(objSender, eventData)
    global isRunning
     
    if ~isRunning
        return;
    end
    
    if eventData.Available
        fprintf('Initial curve fit is available\n');
        GetLaserResults();
        GetLensResults();
     end
 end

 function OnPositionSet(objSender, eventData)
     fprintf('Position was set to %.3f\n', eventData.Position);
 end
 
 %% .NET Assembly Loading
 % In order to allow Matlab to work with BeamSquared, the following
 % files must be deployed from the BeamSquared installation directory 
 % to the MATLAB bin\win64 directory
 % 
 % This deployment must be managed carefully.
 % Mismatched versions and duplicates of these files on the file system
 % can cause frustrating problems down the road. 
 % 
 % BitMiracle.LibTiff.NET.dll
 % M2.Automation.dll
 % M2.Automation.dll.config
 % M2.Interfaces.dll
 % M2.Shared.dll
 % M2.Shared.dll.config
 % netstandard.dll
 % Polly.dll
 % SimpleInjector.dll
 % SimpleInjector.Packaging.dll
 % Spiricon.Abstractions.dll
 % Spiricon.Abstractions.dll.config
 % Spiricon.Interfaces.ConsoleService.dll
 % Spiricon.Interfaces.dll
 % Spiricon.Remoting.dll
 % Spiricon.Shared.dll
 % Spiricon.Shared.dll.config
 % Spiricon.TreePattern.dll
 % Spiricon.TreePattern.Interfaces.dll
 % System.Collections.Immutable.dll
 % System.Interactive.dll
 % System.Reactive.dll
 % System.Runtime.CompilerServices.Unsafe.dll
 % System.Threading.Tasks.Extensions.dll
 % WriteableBitmapEx.Wpf.dll
 function LoadAssemblies()
    % Load the Assemblies from the MATLAB bin\win64 directory
    try
        m2aInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','M2.Automation.dll'));
        csiInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.Interfaces.ConsoleService.dll'));
        stpInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.TreePattern.dll'));
    catch ME
        fprintf(ME.identifier);
        fprintf('Ensure that all BeamSquared files are deployed and match the current version\n');
    end
    %GetClassInfo('M2.Automation.Interfaces.AutomationRunManagerEvents')
 end

 function GetClassInfo(fullClassString)
    methods(fullClassString,'-full')
    properties(fullClassString)
 end

%% Error Logging
% Sometimes .NET Exceptions seems to get a bit buried, this approach provides more detail
 function PrintException(ME, verboseLogging)
    fprintf('Exception: %s\n', ME.identifier); 
    fprintf(' %s\n', ME.message);
    
    if verboseLogging
        if numel(ME.stack) > 0
            fprintf('Stack:\n');
            for i = 1:numel(ME.stack)
                fprintf('    Function: %s\n', ME.stack(i).name);
                fprintf('    Line: %.f\n', ME.stack(i).line);
            end
        end

        if numel(ME.cause) > 0
            fprintf('Caused by: \n');
            for i = 1:numel(ME.cause)
                fprintf('    %s', ME.cause(i));
            end
        end

        if numel(ME.Correction) > 0
            fprintf('Correction: \n');
            for i = 1:numel(ME.Correction)
                fprintf('    %s', ME.Correction(i));
            end
        end
    end
 end