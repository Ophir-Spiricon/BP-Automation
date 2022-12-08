 function BG_Basic_Example
    clear
    clc
    
    try
        InitializeBGAutomation();
        
        ConfigureAnalyzer();
        
        StartDataSource();
        WaitForRunEnd();
        StopDataSource();
    catch ME
        PrintException(ME, true);
    end
    
    cleanup = onCleanup(@Shutdown);
 end


  %% Initialization of BeamGage Automation instance
 function InitializeBGAutomation
    global bg
    
    LoadAssemblies();
    
    % Instantiate AutomationBeamGage instance
    try
        bg = Spiricon.Automation.AutomatedBeamGage('ClientOne',true);
        StopDataSource();
    catch ME
        PrintException(ME, true);
    end
 end
 
 function Shutdown
    global bg
    global frameAvailable

    try
        % unregister delegate
        delete(frameAvailable);
    
        % Shutdown BeamGage
        bg.Instance.Shutdown;
     catch ME
         %do nothing, just close
     end
 end
 
 function StartDataSource
    global bg
    
    RegisterOnNewFrame();
    
    try    
        bg.DataSource.Start();
    catch ME
        PrintException(ME, true);
    end
 end
 
 function StopDataSource
    global bg
    
    try
        bg.DataSource.Stop();
    catch ME
        PrintException(ME, true);
    end
 end
 
 function Ultracal
    global bg
    
    fprintf('Press a key in the Command Window to begin Ultracal cycle.\n');
    pause

    try
        % invoke an ultracal cycle
        fprintf('Ultracal cycle starting...\n');
        bg.Calibration.Ultracal();
        
        % delay momentarily for cycle to start
        pause(0.5)
        
        % poll the CalibrationStatus
        while bg.Calibration.Status ~= Spiricon.Automation.CalibrationStatus.READY
            switch bg.Calibration.Status
                case Spiricon.Automation.CalibrationStatus.BEAM_DETECTED
                    fprintf('Beam Detected.  Please Block Beam.\n');
                case Spiricon.Automation.CalibrationStatus.CALIBRATING
                    fprintf('..\n');
                case Spiricon.Automation.CalibrationStatus.FAILED
                    fprintf('Ultracal Failed.  Please check camera and beam conditions.\n');
                case Spiricon.Automation.CalibrationStatus.NOTSUPPORTED
                    fprintf('Ultracal not supported on current data source.\n');
            end
            pause(1)
        end
        
        fprintf('Ultracal Successful.\n');
    catch ME
        PrintException(ME, true);
    end
 end
  
 function LoadSetupFile(setupFile)
    global bg
    
    try
        bg.SaveLoadSetup.LoadSetup(setupFile);
    catch ME
        PrintException(ME, true);
    end
 end
 
  % Configure the analyzer to match optical and laser environment
 function ConfigureAnalyzer
    global bg

    LoadSetupFile('.\setup\beammaker.bgSetup');
    
%     Configure other per-session analyzer settings here
    
    Ultracal();
 end
 
  % Obtain frame data array 
 function [frameData] = GetFrameData
    global bg
    global framePriorityMode
    
    try
        % Obtain the frame data and print some values
        frameData = double(framePriorityMode.DoubleData);
        
        fprintf('---Frame Statistics---\n');
        fprintf('ArrayCount: %d\n', numel(frameData));
        fprintf('Max Value: %d\n', max(frameData(:)));
        fprintf('Min Value: %d\n', min(frameData(:)));
        
        % Optionally reshape the array back to sensor format using Width
        % and Height results
        frameData = reshape(frameData,[bg.FrameInfoResults.Width, bg.FrameInfoResults.Height]);
    catch(ME)
        PrintException(ME, true);
    end
 end
 
 %% Obtain frame results
 function GetResults
    global bg
    try
        % Print the ID and Total counts of the frame,         
        fprintf('---Frame Results---\n');   
        fprintf('Frame # %d\n', bg.FrameInfoResults.ID);
        fprintf('Total: %d\n', bg.PowerEnergyResults.Total);
        fprintf('Peak: %.3f\n', bg.PowerEnergyResults.Peak);
        fprintf('D4Sigma X: %.3f\n', bg.SpatialResults.D4SigmaMajor);
        fprintf('D4Sigma Y: %.3f\n', bg.SpatialResults.D4SigmaMinor);
        fprintf('Centroid X: %.3f\n', bg.SpatialResults.CentroidX);
        fprintf('Centroid Y: %.3f\n', bg.SpatialResults.CentroidY);
    catch ME
        PrintException(ME, true);
    end
 end
  
function WaitForRunEnd
    pause
    
    % Implement any
%     global isRunning
%     while (isRunning)
%         pause(1);
%     end
end
 
%% Event delegate registration
% The registration of the delegate to the event
% Note framePriorityMode:
% bg.ResultsPriorityFrame - Must use if calculated results or logging modes
% are required. The analyzer will only raise the event when calculations are
% complete.
% bg.FramePriorityFrame - Only use if only frameData is needed at maximum
% rate. The analyzer will raise the event when the frameData is available.
% Calculations may be skipped if another frame is ready.
 function RegisterOnNewFrame
    global bg
    global frameAvailable
    global framePriorityMode
    
    % Create and register for the new frame event
    try
        framePriorityMode = bg.ResultsPriorityFrame;
        frameAvailable = Spiricon.Automation.AutomationFrameEvents(framePriorityMode);
        frameAvailable.GetType().GetEvent('OnNewFrame').AddEventHandler(frameAvailable,...
            Spiricon.Automation.newFrame(@NewFrameFunction));
    catch ME
        PrintException(ME, true);
    end
 end
 
 %% Event handler delegates
 % To stay synchronized with BeamGage data acquistion an OnNewFrame event
 % is provided. FrameData and Results should be obtained during these delegates
 % Implementation in delegates is blocking, so should be kept miminal
 % pushing obtained data to other threads for further processing or
 % calculation.
 % Changing the data source settings and acquistion state should not be
 % performed in the delegate.
 % FrameData should be accessed via the framePriorityMode that was registered for.
function NewFrameFunction()
    GetResults();
    GetFrameData();
    
    fprintf('Press a key in the command window to exit.\n');
    fprintf('\n');
end
 
%% .NET Assembly Loading
 % In order to allow Matlab to work with BeamGage, the following
 % files must be deployed from the BeamSquared installation directory 
 % to the MATLAB bin\win64 directory
 % 
 % This deployment must be managed carefully.
 % Mismatched versions and duplicates of these files on the file system
 % can cause frustrating problems down the road.
 % 
 %   BG.Interfaces.dll
 %   Spiricon.Abstractions.dll
 %   Spiricon.Abstractions.dll.config
 %   Spiricon.Automation.dll
 %   Spiricon.Automation.dll.config
 %   Spiricon.BeamGage.Automation.dll
 %   Spiricon.BeamGage.Automation.dll.config
 %   Spiricon.Interfaces.ConsoleService.dll
 %   Spiricon.Interfaces.dll
 %   Spiricon.Remoting.dll
 %   Spiricon.Shared.dll
 %   Spiricon.Shared.dll.config
 %   Spiricon.TreePattern.dll
 %   Spiricon.TreePattern.Interfaces.dll
function LoadAssemblies()
    % Load the Assemblies from the MATLAB bin\win64 directory
    
    try
        spaInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.Automation.dll'));
        sbaInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.BeamGage.Automation.dll'));
        csiInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.Interfaces.ConsoleService.dll'));
        stpInfo = NET.addAssembly(fullfile(matlabroot,'bin','win64','Spiricon.TreePattern.dll'));
    catch ME
        fprintf(ME.identifier);
        fprintf('Ensure that all BeamGage files are deployed and match the current version.\n');
    end

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