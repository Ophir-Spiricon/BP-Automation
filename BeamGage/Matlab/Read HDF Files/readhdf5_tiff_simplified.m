function I=readhdf5_tiff(frame,filename)

%% Read paramaters from the file
width=['/BG_DATA/' num2str(frame) '/RAWFRAME/WIDTH'];
numcols = h5read(filename,width);
height=['/BG_DATA/' num2str(frame) '/RAWFRAME/HEIGHT'];
numrows = h5read(filename,height);
pxscaleX=['/BG_DATA/' num2str(frame) '/RAWFRAME/PIXELSCALEXUM'];
pixelscalexum = h5read(filename,pxscaleX);
pxscaleY=['/BG_DATA/' num2str(frame) '/RAWFRAME/PIXELSCALEYUM'];
pixelscaleyum = h5read(filename,pxscaleY);
bitdepth=['/BG_DATA/' num2str(frame) '/RAWFRAME/BITENCODING'];
encoding = h5read(filename,bitdepth);
encoding = cell2mat(encoding);

%% Read the frame data                              %%% (side comments are assuming 1224x1624 frame, the array dimensions will differ by camera model and frame format.)
data=['/BG_DATA/' num2str(frame) '/DATA'];
data = h5read(filename,data);
data = cell2mat(data);                              %%% data    [output]    2468704x1 uint8     (gray32bppFloat TIFF with headers)

%% Write frame data to TIFF format the data to the right format or power calibration
fileID =fopen('tiffframe.tiff','w');
fwrite(fileID,data);                                %%% data    [input]     2468704x1 uint8     (gray32bppFloat TIFF with headers)
fclose(fileID);
J = imread('tiffframe.tiff');                       %%% J       [output]    1224x1624 uint32    (gray32bppFloat integers only)

%% Reshape the array to use typecast and reshape back, convert to the default Beamgage format
J_fin = reshape(J,[1,(numrows*numcols)]);           %%% J_fin   [output]    1x1987776 uint32    (gray32bppFloat integers)
J_fin = typecast(J_fin, 'single');                  %%% J_fin   [output]    1x1987776 single    (floating point values)
J_final = reshape(J_fin, [numrows,numcols]);        %%% J_fin   [output]    1224x1624 uint32    (floating point values)

%% Convert the floating point pixels to native depth pixels
I = FloatPixelsToNativePixels(encoding, J_final);   %%% I       [output]    1224x1624 single    (12-bit pixels)

%% Write values to the screen
[numrows,numcols] = size(I);

screendump(numrows);
screendump(numcols);
screendump(I(1,1));
screendump(encoding);
screendump(pixelscalexum);
screendump(pixelscaleyum);
screendump(power_calibration_multiplier);

function [I] = FloatPixelsToNativePixels(encoding, data)
    switch lower(encoding)
        case {'l8','l16_8','r8','r16_8'} % 8 bit
            nativebitdepth = 8;
        case {'l16_10','r16_10'} % 10 bit
            nativebitdepth = 10;
        case {'l16_12','r16_12'} % 12 bit
            nativebitdepth = 12;
        case {'l16_14','r16_14'} % 14 bit
            nativebitdepth = 14;
        case {'l16_16','l16','r16_16','r16'} % 16 bit
            nativebitdepth = 16;
        case 's16_14'  % signed 14 bit
            nativebitdepth = 14;
        case {'s16_16'} % signed 16 bit
            nativebitdepth = 16;
        case {'s32'} % signed 32 bit
            nativebitdepth = data;
        otherwise
            error('Unknown bitencoding.')
    end
        I = data * 2^nativebitdepth;

function screendump(value)
% Dump the variable name of value and the value to the display
if (ischar(value))
    fprintf('%s = %s\n', inputname(1),value)
else
    fprintf('%s = %.4f\n', inputname(1),value)
end
