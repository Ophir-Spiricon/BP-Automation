function readhdf5()
%% This is the filename from which the data will be read
filename = 'C:\\Users\\joe.public\\Desktop\\200.bgData';


%% Read various paramaters from the file
numcols = hdf5read(filename,'/BG_DATA/1/RAWFRAME/WIDTH');
numrows = hdf5read(filename,'/BG_DATA/1/RAWFRAME/HEIGHT');
pixelscalexum = hdf5read(filename,'/BG_DATA/1/RAWFRAME/PIXELSCALEXUM');
pixelscaleyum = hdf5read(filename,'/BG_DATA/1/RAWFRAME/PIXELSCALEYUM');
data = hdf5read(filename,'/BG_DATA/1/DATA');
power_calibration_multiplier = hdf5read(filename,'/BG_DATA/1/RAWFRAME/ENERGY/POWER_CALIBRATION_MULTIPLIER');
encoding = hdf5read(filename,'/BG_DATA/1/RAWFRAME/BITENCODING');
encoding = encoding.Data;

%% Convert the data to the right format or power calibration
BitsPerPixel = 32;

if (power_calibration_multiplier == 0)
    power_calibration_multiplier = 1;  % do not apply calibration, mulitplier at 1
    switch lower(encoding)
        case {'l8','l16_8'} % 8 bit
           data = data ./ 2^(BitsPerPixel - 8 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'l16_10' % 10 bit
           data = data ./ 2^(BitsPerPixel - 10 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'l16_12' % 12 bit
           data = data ./ 2^(BitsPerPixel - 12 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'l16_14' % 14 bit
           data = data ./ 2^(BitsPerPixel - 14 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case {'l16_16','l16'} % 16 bit
           data = data ./ 2^(BitsPerPixel - 16 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case {'r8','r16_8'} % 8 bit
           data = data ./ 2^(BitsPerPixel - 8 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'r16_10' % 10 bit
           data = data ./ 2^(BitsPerPixel - 10 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'r16_12' % 12 bit
           data = data ./ 2^(BitsPerPixel - 12 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 'r16_14' % 14 bit
           data = data ./ 2^(BitsPerPixel - 14 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
       case {'r16_16','r16'} % 16 bit
           data = data ./ 2^(BitsPerPixel - 16 - 1);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case 's16_14'  % signed 14 bit
           data = data ./ 2^(BitsPerPixel - 14);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case {'s16_16'} % signed 16 bit
           data = data ./ 2^(BitsPerPixel - 16);
           I = hdf5data_to_matrix(data,numcols,numrows);
        case {'s32'} % signed 32 bit
           I = hdf5data_to_matrix(data,numcols,numrows);
        otherwise
            error('Unknown bitencoding.')
    end
else % Use the Power Calibration Multiplier instead
    I = hdf5data_to_matrix(data,numcols,numrows);
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
