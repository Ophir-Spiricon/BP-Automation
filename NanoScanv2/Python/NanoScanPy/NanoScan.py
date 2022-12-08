# -*- coding: utf-8 -*-

""" A Python 3.10 example client for NanoScan v2

This module is intended as a client program for the purposes of utilizing the NanoScan v2
COM sever.  The NanoScan v2 Automation Interface is an API which interfaces with the NanoScan scanning slit laser
profiling application.

The NanoScanPy example code is provided as-is with no express guarantee or warranty of functionality and may be
provided and updated independently of NanoScan v2 releases.

This module may be adapted as needed without attribution.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
https://www.ophiropt.com/laser--measurement/software-download

Professional tier licenses for Ophir brand laser profiling systems may be purchased by contacting:
    Ophir USA Product Support\n
    1-800-383-0814\n
    service.ophir.usa@mksinst.com

MKS | Ophir  The True Measure of Laser Performance™

Methods which utilize two or more in/out byref arguments will not work prior to pywin32 release 300
such as: NsAsGetDeviceID or NsAsReadProfile
see: https://github.com/mhammond/pywin32/issues/1303

Todo:
    * Flush out inline documentation
    * Consider additional scenarios using more features and best practices
    * Provide a complete wrapper class for ease of use
    * investigate use of makepy generated wrapper class
"""

__author__ = "Russ Leikis"
__copyright__ = "Copyright 2022, Ophir-Spiricon, LLC"
__credits__ = ["Russ Leikis"]
__license__ = "GPL"
__version__ = "2.0"
__maintainer__ = "Russ Leikis"
__email__ = "russ.leikis@mksinst.com"
__status__ = "Beta"

import sys
import time
import collections

import array as arr

import pythoncom
import win32com.client
from win32com.client import Dispatch, VARIANT
from win32com.client import makepy
from icecream import ic


sys.argv = ["makepy", r"C:\Program Files (x86)\Photon\NanoScan v2\Automation\NanoScanII.tlb"]
makepy.main()

# instantiate NanoScan VII COM object, ob
ob = Dispatch('photon-nanoscan')

# ensure that the automation server is opened
time.sleep(2)

# initialize analyzer
ob.NsAsShowWindow = 1
ob.NsAsSelectParameters(0x1 | 0x2 | 0x10 | 0x20)

# get device count
numDevices = VARIANT(pythoncom.VT_I2 | pythoncom.VT_BYREF, None)
ob.NsAsGetNumDevices(numDevices)
ic(numDevices.value)

# get aperture bounds
sAperture = 0  # only aperture X for this example
sROIIndex = 0
pfLeftBound = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
pfRightBound = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
pbROIEnabled = VARIANT(pythoncom.VT_BOOL | pythoncom.VT_BYREF, False)

ob.NsAsGetApertureLimits(0, pfLeftBound, pfRightBound)
ic(pfLeftBound.value)
ic(pfRightBound.value)

ob.NsAsGetROI(sAperture, sROIIndex, pfLeftBound, pfRightBound, pbROIEnabled)
ic(pfLeftBound.value)
ic(pfRightBound.value)
ic(pbROIEnabled.value)

# Try getting device ID

deviceID = VARIANT(pythoncom.VT_I2 | pythoncom.VT_BYREF, None)
ob.NsAsGetDeviceID(deviceID)
ic(deviceID.value)

# collect data
numOfPoints = 100

# Calculate the decimation factor to only retrieve numOfPoints
pfSamplingResolution = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
ob.NsAsGetSamplingResolution(sAperture, pfSamplingResolution)
# define decimation value of the scans of the ROI (e.g. 10 provides every 10th scan of all scans in the current ROI)
sDecimation = ((pfRightBound.value - pfLeftBound.value) / pfSamplingResolution.value / numOfPoints)

# define byref safe arrays of VTS_R4 values for profile data
# Empty array object which will receive a Variant Array of VT_R4, which will be packaged in a Variant ByRef.
ampData = VARIANT(pythoncom.VT_ARRAY | pythoncom.VT_R4, [])
posData = VARIANT(pythoncom.VT_ARRAY | pythoncom.VT_R4, [])
psaAmplitude = VARIANT(pythoncom.VT_VARIANT | pythoncom.VT_BYREF, ampData)
psaPosition = VARIANT(pythoncom.VT_VARIANT | pythoncom.VT_BYREF, posData)

# Option 1: controlled acquisition of a single scan
# ob.NsAsAcquireSync1Rev
# ob.NsAsRunComputation

# start continuous data acquisition for 5 seconds
ob.NsAsDataAcquisition = True
time.sleep(5)

# accessing a property via method
fUserClip1 = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
ob.NsAsGetUserClipLevel1(fUserClip1)
# setting a property via method
ob.NsAsSetUserClipLevel1(50.0)

# accessing a calculation property
# define byref parameters for getting the beam width measurements
fWidthX = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
fWidthY = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, None)
ob.NsAsGetBeamWidth(0, 0, 13.5, fWidthX)
ob.NsAsGetBeamWidth(1, 0, 13.5, fWidthY)
ic(fWidthX.value)
ic(fWidthY.value)

# get profile data and print out the center value
ob.NsAsReadProfile(sAperture, pfLeftBound, pfRightBound, sDecimation, psaAmplitude, psaPosition)
ic(psaAmplitude.value[49])
ic(psaPosition.value[49])

# stop data acquisition
ob.NsAsDataAcquisition = False

# clear reference to the automation server so it can close
ob = None
