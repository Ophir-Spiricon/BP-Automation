
% Best documentation to start with for using COM in Matlab
% https://www.mathworks.com/help/matlab/using-com-objects-in-matlab.html

% Good documentation commands for working with Matlab and COM
% doc invoke
% doc get

% Normally Matlab will recognize the registered COM server's .tlb file, but it isn't for Nanoscan
% 
% https://www.mathworks.com/matlabcentral/answers/315129-how-to-use-output-variables-with-com-objects

% This feature is going to be needed to the the profile array data from the NanoScan server.
%
% https://www.mathworks.com/matlabcentral/answers/94888-how-can-i-pass-arguments-by-reference-to-an-activex-server-from-matlab-7-0-r14
% Note on passing Numeric arrays by reference: MATLAB supports passing a VARIANT
% type by reference to COM object. Since cell arrays are converted as SAFEARRAY
% of VARIANT's, COM_PassSafeArrayByRef works only with cell arrays. Numeric arrays
% are passed by reference only if they are marked as [in,out] or [out] in the 
% COM object's IDL file (.tlb).

% launch NanoScan COM server (also starts NanoScanII.exe)
ns = actxserver('Photon-NanoScan');

% This is supposed to list out all the interface, it does not work
invoke(ns)

% show the NanoScan GUI and start data acquisition
set(ns,'NsAsShowWindow',true);
set(ns,'NsAsDataAcquisition',true);

% allow scan head to run for 5 seconds
pause(5)

% stop data acquisition
set(ns,'NsAsDataAcquisition',false);


% enable 13.5% peak beam width
    % This method notation should work and is preferred, but doesn't with the NanoScan ActiveX/COM server 
    % It looks like the NanoScanII.tlb file registration with the OS isn't being recognized
    % by Matlab.  (I don't know why)
    % https://www.mathworks.com/matlabcentral/answers/95331-why-are-none-of-the-properties-or-methods-available-from-my-activex-server-com-object
%ns.NsAsSelectParameters(1);

    % This method notation is discouraged, but does work with the NanoScan ActiveX/COM server
invoke(ns,'NsAsSelectParameters',1); 


% in most languages you need to create the variables that will store the
% output values in advance. Do you need to in Matlab?
xWidth = -1.0;
yWidth = -1.0;

% The GetBeamWidth method requires 'xWidth' and 'yWidth' to be passed "by reference", 
% this means the memory is allocated in Matlab (the client) and the values
% are written into that memory by NanoScan (the server).  This is very common
% in COM servers.
% The byref arguments are supported by NanoScan and should work by the handle.method
% signature (e.g. ns.NsAsGetBeamWidth) if Matlab was reconizing the tlb
% registration, which it is not.  So this fails.
%ns.NsAsGetBeamWidth(0,0,13.5,xWidth);
%ns.NsAsGetBeamWidth(0,0,13.5,yWidth);

% The invoke method does not support byref arguments, so this also fails.
%invoke(ns,'NsAsGetBeamWidth', 0, 0, 13.5, xWidth);
%invoke(ns,'NsAsGetBeamWidth', 1, 0, 13.5, yWidth);

% close the server and release its resources
quit(ns)
delete(ns)