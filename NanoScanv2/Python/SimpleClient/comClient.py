# -*- coding: utf-8 -*-
"""
Created on Fri Jan 18 10:48:12 2019

@author: russ.leikis
"""

# -*- coding: utf-8 -*-
import win32com.client
from win32com.client import VARIANT
import pythoncom

# instantiate NanoScan VII COM object, ob
ob = win32com.client.DispatchEx('app.Application')
ob.Show()

# get feature bounds
sAperture = 0
sROIIndex = 0
pfLeftBound = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, 999.9)
pfRightBound = VARIANT(pythoncom.VT_R4 | pythoncom.VT_BYREF, 999.9)
pbEnabled = VARIANT(pythoncom.VT_BOOL | pythoncom.VT_BYREF, False)

print("-----Input & Input/Output Arguments-----")
ob.GetData_WithInput(0, 0, pfLeftBound, pfRightBound, pbEnabled)
print("left bound: " + str(pfLeftBound.value))
print("right bound: " + str(pfRightBound.value))
print("enabled: " + str(pbEnabled.value))

print("-----No Input Arguments-----")
ob.GetData_NoInput(pfLeftBound, pfRightBound, pbEnabled)
print("left bound: " + str(pfLeftBound.value))
print("right bound: " + str(pfRightBound.value))
print("enabled: " + str(pbEnabled.value))

print("-----Single Boolean-----")
ob.GetData_SingleBoolean(pbEnabled)
print("enabled: " + str(pbEnabled.value))

print("-----Single Float-----")
ob.GetData_SingleFloat(pfLeftBound)
print("left bound: " + str(pfLeftBound.value))

ob = None