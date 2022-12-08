function I=readhdf5_tiff3(frame,fileN)
%% This is the filename from which the data will be read
filename = fileN;

%% Read various paramaters from the file

text1=['/BG_DATA/' num2str(frame) '/RAWFRAME/WIDTH'];
numcols = h5read(filename,text1);
text2=['/BG_DATA/' num2str(frame) '/RAWFRAME/HEIGHT'];
numrows = h5read(filename,text2);
text3=['/BG_DATA/' num2str(frame) '/RAWFRAME/PIXELSCALEXUM'];
pixelscalexum = h5read(filename,text3);
text4=['/BG_DATA/' num2str(frame) '/RAWFRAME/PIXELSCALEYUM'];
pixelscaleyum = h5read(filename,text4);
text5=['/BG_DATA/' num2str(frame) '/DATA'];
data = h5read(filename,text5);
data = cell2mat(data);
text6=['/BG_DATA/' num2str(frame) '/RAWFRAME/ENERGY/POWER_CALIBRATION_MULTIPLIER'];
power_calibration_multiplier = h5read(filename,text6);
text7=['/BG_DATA/' num2str(frame) '/RAWFRAME/BITENCODING'];
encoding = h5read(filename,text7);
encoding = cell2mat(encoding);

%% Convert the data to the right format or power calibration
BitsPerPixel = 32;

fileID =fopen('tiffframe.tiff','w');
fwrite(fileID,data);
fclose(fileID);
J = imread('tiffframe.tiff');

J_fin = reshape(J,[1,(250*250)]);
J_fin = typecast(J_fin, 'single');
J_fin = reshape(J_fin, [250, 250]);
J_final = 2147483647 * J_fin;  
data = J_final;
spiricon_colormap(data, 31);


if (power_calibration_multiplier == 0)
    power_calibration_multiplier = 1;  % do not apply calibration, mulitplier at 1
    switch lower(encoding)
        %  Notes for Left and Right shifted native formats:
        %    1) is unnormalized the same way, unless the underlying
        %       data format matters to the user, then use bitshifting
        %       instead of the normalization factor.
        %
        %    2) should subtract one extra to ignore the sign bit
        %       from the S32 value
        case {'l8','l16_8','r8','r16_8'} % 8 bit
            I = data ./ 2^(BitsPerPixel - 8 - 1);
        case {'l16_10','r16_10'} % 10 bit
            I = data ./ 2^(BitsPerPixel - 10 - 1);
        case {'l16_12','r16_12'} % 12 bit
            I = data ./ 2^(BitsPerPixel - 12 - 1);
        case {'l16_14','r16_14'} % 14 bit
            I = data ./ 2^(BitsPerPixel - 14 - 1);
        case {'l16_16','l16','r16_16','r16'} % 16 bit
            I = data ./ 2^(BitsPerPixel - 16 - 1);
        case 's16_14'  % signed 14 bit
            I = data ./ 2^(BitsPerPixel - 14);
        case {'s16_16'} % signed 16 bit
            I = data ./ 2^(BitsPerPixel - 16);
        case {'s32'} % signed 32 bit
            I = data;
        otherwise
            error('Unknown bitencoding.')
    end
else % Use the Power Calibration Multiplier instead
    I = I.*power_calibration_multiplier;
end
%% Write values to the screen
[numrows,numcols] = size(I);

screendump(numrows);
screendump(numcols);
screendump(I(1,1));
screendump(encoding);
screendump(pixelscalexum);
screendump(pixelscaleyum);
screendump(power_calibration_multiplier);
% % 
function [I] = hdf5data_to_matrix(data,width,height)
%  Convert the 1-D array of data into a 2-D matrix, or image.
I = zeros(height,width);
index = 1;
for i = 1:height
    for j = 1:width
        I(i,j) = data(index);
        index = index + 1;
    end
end


function screendump(value)
% Dump the variable name of value and the value to the display
if (ischar(value))
    fprintf('%s = %s\n', inputname(1),value)
else
    fprintf('%s = %.4f\n', inputname(1),value)
end
