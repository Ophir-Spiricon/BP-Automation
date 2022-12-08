//NS2_Interop.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "NS2_Interop.h"
#include "CNanoScan.h"
#include "Version.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CWinApp theApp;
CNanoScan *pNanoScan;

using namespace std;

int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;

	HMODULE hModule = ::GetModuleHandle(NULL);

	if (hModule != NULL)
	{
		// initialize MFC and print and error on failure
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			// TODO: change error code to suit your needs
			_tprintf(_T("Fatal Error: MFC initialization failed\n"));
			nRetCode = 1;
		}
		else
		{
		}
	}
	else
	{
		_tprintf(_T("Fatal Error: GetModuleHandle failed\n"));
		nRetCode = 1;
	}

	return nRetCode;
}

extern "C"
{
	_declspec(dllexport) int InitNsInterop()
	{
		COleDispatchException* pException;
		pException = new COleDispatchException(_T("DispatchException"), 200, 425);
		pException->m_scError = 0;
		pNanoScan = new CNanoScan();

		try
		{
			static const CLSID clsid = { 0xfaad0d22, 0xc718, 0x459a, { 0x81, 0xca, 0x26, 0x8c, 0xcf, 0x18, 0x88, 0x7 } };
			if ( !pNanoScan->CreateDispatch(clsid, (COleException *)pException) )
			{
				throw pException;
			}
			else
			{
				return 1;
			}
		}
		catch (COleDispatchException* pe) 
		{
			CString cStr;

			if (!pe->m_strSource.IsEmpty())
			cStr = pe->m_strSource + _T(" - ");
			if (!pe->m_strDescription.IsEmpty())
				cStr += pe->m_strDescription;
			else
				cStr += _T("unknown error");

			AfxMessageBox(cStr, MB_OK, 
			(pe->m_strHelpFile.IsEmpty()) ?  0 : pe->m_dwHelpContext);
		}

		pException->Delete();
		return 0;
	}

	_declspec(dllexport) void ShutdownNsInterop()
	{
	   pNanoScan->m_bAutoRelease = TRUE;
	   pNanoScan->ReleaseDispatch();
	   delete pNanoScan;
	   pNanoScan = NULL;
	}

	_declspec(dllexport) void NsInteropSetDataAcquisition(bool acquisitionState)
	{
		pNanoScan->SetNsAsDataAcquisition(acquisitionState);
	}

	_declspec(dllexport) bool NsInteropGetDataAcquisition()
	{
		return pNanoScan->GetNsAsDataAcquisition() != 0;
	}

	_declspec(dllexport) void NsInteropSetShowWindow(bool showGUI)
	{
		pNanoScan->SetNsAsShowWindow(showGUI);
	}
	
	_declspec(dllexport) SCODE NsInteropSelectParameters(int parameters)
	{
		return pNanoScan->NsAsSelectParameters(parameters);
	}

	_declspec(dllexport) SCODE NsInteropGetNumDevices(short *numDevices)
	{
		return pNanoScan->NsAsGetNumDevices(numDevices);
	}

	_declspec(dllexport) SCODE NsInteropReadProfile(short sAperture, float fStartPosition, float fEndPosition, short sDecimationFactor, VARIANT *pvarProfileAmplitude, VARIANT *pvarProfilePosition)
	{
		return pNanoScan->NsAsReadProfile(sAperture, fStartPosition, fEndPosition, sDecimationFactor, pvarProfileAmplitude, pvarProfilePosition);
	}

	_declspec(dllexport) SCODE NsInteropGetApertureLimits(short Aperture, float *StartPosition, float *EndPosition)
	{
		return pNanoScan->NsAsGetApertureLimits(Aperture, StartPosition, EndPosition);
	}

	_declspec(dllexport) SCODE NsInteropGetSamplingResolution(short Aperture, float *SampleResolution)
	{
		return pNanoScan->NsAsGetSamplingResolution(Aperture, SampleResolution);
	}
	
	_declspec(dllexport) SCODE NsInteropGetCentroidPosition(short sAperture, short sROIIndex, float *pfCentroidPosition)
	{
		return pNanoScan->NsAsGetCentroidPosition(sAperture, sROIIndex, pfCentroidPosition);
	}

	_declspec(dllexport) SCODE NsInteropGetHeadCapabilities(long lCapabilityID, VARIANT * pvarHeadCapability)
	{
		return pNanoScan->NsAsGetHeadCapabilities(lCapabilityID, pvarHeadCapability);
	}

	_declspec(dllexport) SCODE NsInteropSetGain(short sAperture, short sGain)
	{
		return pNanoScan->NsAsSetGain(sAperture, sGain);
	}

	_declspec(dllexport) SCODE NsInteropGetGain(short sAperture, short * psGain)
	{
		return pNanoScan->NsAsGetGain(sAperture, psGain);
	}

	_declspec(dllexport) SCODE NsInteropSetFilter(short sAperture, float fFilter)
	{
		return pNanoScan->NsAsSetFilter(sAperture, fFilter);
	}

	_declspec(dllexport) SCODE NsInteropGetFilter(short sAperture, float * pfFilter)
	{
		return pNanoScan->NsAsGetFilter(sAperture, pfFilter);
	}

	_declspec(dllexport) SCODE NsInteropSetSamplingResolution(float fSamplingResolution)
	{
		return pNanoScan->NsAsSetSamplingResolution(fSamplingResolution);
	}

	_declspec(dllexport) SCODE NsInteropSetRotationFrequency(float fRotationFrequency)
	{
		return pNanoScan->NsAsSetRotationFrequency(fRotationFrequency);
	}

	_declspec(dllexport) SCODE NsInteropGetRotationFrequency(float * pfRotationFrequency)
	{
		return pNanoScan->NsAsGetRotationFrequency(pfRotationFrequency);
	}

	_declspec(dllexport) SCODE NsInteropGetMeasuredRotationFreq(float * pfMeasuredRotationFreq)
	{
		return pNanoScan->NsAsGetMeasuredRotationFreq(pfMeasuredRotationFreq);
	}
	
	_declspec(dllexport) SCODE NsInteropAutoFind()
	{
		return pNanoScan->NsAsAutoFind();
	}
	
	_declspec(dllexport) SCODE NsInteropAddROI(short sAperture, float fLeftBound, float fRightBound, bool bROIEnabled)
	{
		return pNanoScan->NsAsAddROI(sAperture, fLeftBound, fRightBound, bROIEnabled);
	}

	_declspec(dllexport) SCODE NsInteropDeleteROI(short sAperture, short sROIIndex)
	{
		return pNanoScan->NsAsDeleteROI(sAperture, sROIIndex);
	}

	_declspec(dllexport) SCODE NsInteropUpdateROI(short sAperture, short sROIIndex, float fLeftBound, float fRightBound, bool bROIEnabled )
	{
		return pNanoScan->NsAsUpdateROI(sAperture, sROIIndex, fLeftBound, fRightBound, bROIEnabled ? 1 : 0);
	}

	_declspec(dllexport) SCODE NsInteropGetNumberOfROIs(short sAperture, short * psNumberOfROIs)
	{
		return pNanoScan->NsAsGetNumberOfROIs(sAperture, psNumberOfROIs);
	}

	_declspec(dllexport) SCODE NsInteropGetROI(short sAperture, short sROIIndex, float * pfLeftBound, float * pfRightBound, bool * pbROIEnabled)
	{
		BOOL roiEnabled;
		SCODE returnValue = pNanoScan->NsAsGetROI(sAperture, sROIIndex, pfLeftBound, pfRightBound, &roiEnabled);
		*pbROIEnabled = roiEnabled ? 1 : 0;
		return returnValue;
	}

	_declspec(dllexport) SCODE NsInteropGetSelectedParameters(long * plParameters)
	{
		return pNanoScan->NsAsGetSelectedParameters(plParameters);
	}

	_declspec(dllexport) SCODE NsInteropSetUserClipLevel1(float fUserClipLevel1)
	{
		return pNanoScan->NsAsSetUserClipLevel1(fUserClipLevel1);
	}

	_declspec(dllexport) SCODE NsInteropSetUserClipLevel2(float fUserClipLevel2)
	{
		return pNanoScan->NsAsSetUserClipLevel2(fUserClipLevel2);
	}

	_declspec(dllexport) SCODE NsInteropGetUserClipLevel1(float * pfUserClipLevel1)
	{
		return pNanoScan->NsAsGetUserClipLevel1(pfUserClipLevel1);
	}

	_declspec(dllexport) SCODE NsInteropGetUserClipLevel2(float * pfUserClipLevel2)
	{
		return pNanoScan->NsAsGetUserClipLevel2(pfUserClipLevel2);
	}

	_declspec(dllexport) SCODE NsInteropGetBeamWidth(short sAperture, short sROIIndex, float fClipLevel, float * pfBeamWidth)
	{
		return pNanoScan->NsAsGetBeamWidth(sAperture, sROIIndex, fClipLevel, pfBeamWidth);
	}

	_declspec(dllexport) SCODE NsInteropGetBeamWidth4Sigma(short sAperture, short sROIIndex, float * pfBeamWidth4Sigma)
	{
		return pNanoScan->NsAsGetBeamWidth4Sigma(sAperture, sROIIndex, pfBeamWidth4Sigma);
	}

	_declspec(dllexport) SCODE NsInteropGetPeakPosition(short sAperture, short sROIIndex, float * pfPeakPosition)
	{
		return pNanoScan->NsAsGetPeakPosition(sAperture, sROIIndex, pfPeakPosition);
	}

	_declspec(dllexport) SCODE NsInteropGetCentroidSeparation(short sAperture, short sROIIndex, float * pfCentroidSeparation)
	{
		return pNanoScan->NsAsGetCentroidSeparation(sAperture, sROIIndex, pfCentroidSeparation);
	}

	_declspec(dllexport) SCODE NsInteropGetPeakSeparation(short sAperture, short sROIIndex, float * pfPeakSeparation)
	{
		return pNanoScan->NsAsGetPeakSeparation(sAperture, sROIIndex, pfPeakSeparation);
	}

	_declspec(dllexport) SCODE NsInteropGetBeamIrradiance(short sAperture, short sROIIndex, float * pfBeamIrradiance)
	{
		return pNanoScan->NsAsGetBeamIrradiance(sAperture, sROIIndex, pfBeamIrradiance);
	}
	
	_declspec(dllexport) SCODE NsInteropGetGaussianFit(short sAperture, short sROIIndex, float * pfGoodnessFit, float * pfRoughnessFit)
	{
		return pNanoScan->NsAsGetGaussianFit(sAperture, sROIIndex, pfGoodnessFit, pfRoughnessFit);
	}

	_declspec(dllexport) SCODE NsInteropGetBeamEllipticity(short sROIIndex, float * pfBeamEllipticity)
	{
		return pNanoScan->NsAsGetBeamEllipticity(sROIIndex, pfBeamEllipticity);
	}

	_declspec(dllexport) SCODE NsInteropSetPulseFrequency(float fPulseFrequency)
	{
		return pNanoScan->NsAsSetPulseFrequency(fPulseFrequency);
	}

	_declspec(dllexport) SCODE NsInteropGetPulseFrequency(float * pfPulseFrequency)
	{
		return pNanoScan->NsAsGetPulseFrequency(pfPulseFrequency);
	}

	_declspec(dllexport) SCODE NsInteropAcquireSync1Rev()
	{
		return pNanoScan->NsAsAcquireSync1Rev();
	}

	_declspec(dllexport) SCODE NsInteropGetNumPwrCalibrations(short * nNumPwrCalibrations)
	{
		return pNanoScan->NsAsGetNumPwrCalibrations(nNumPwrCalibrations);
	}

	#pragma pack(1)
	struct MUDTPwrCalibration
    {
        BSTR pstrDescriptor;
		float fRefPower;
        float fWaveLength;
    };

	_declspec(dllexport) SCODE NsInteropGetPowerCalibration(short sIndexCalibration, VARIANT *pvarPwrCalibration)
	{
		if (V_VT(pvarPwrCalibration) != VT_RECORD)
		{
			return 0x8004021A;
		}

		SCODE status = pNanoScan->NsAsGetPowerCalibration(sIndexCalibration, pvarPwrCalibration);
		
		IRecordInfo* pIRecordInfo = V_RECORDINFO(pvarPwrCalibration);
		MUDTPwrCalibration* pManagedUDT = (MUDTPwrCalibration*)(V_RECORD(pvarPwrCalibration));

		if (pManagedUDT)
		{
		  printf ("pManagedUDT -> pstrDescriptor : [%S].\r\n", pManagedUDT -> pstrDescriptor);
		  printf ("pManagedUDT -> fRefPower : [%4.2f].\r\n", pManagedUDT -> fRefPower);
		  printf ("pManagedUDT -> fWaveLength : [%4.2f].\r\n", pManagedUDT -> fWaveLength);
		}

		return status;
	}
	#pragma pack()

	_declspec(dllexport) SCODE NsInteropGetTotalPower(float * pfTotalPower)
	{
		return pNanoScan->NsAsGetTotalPower(pfTotalPower);
	}

	_declspec(dllexport) SCODE NsInteropGetPower(short nROIIndex, float * pfROIPower)
	{
		return pNanoScan->NsAsGetPower(nROIIndex, pfROIPower);
	}

	_declspec(dllexport) SCODE NsInteropGetDeviceID(short * pwDeviceID)
	{
		return pNanoScan->NsAsGetDeviceID(pwDeviceID);
	}

	_declspec(dllexport) SCODE NsInteropSetDeviceID(short wDeviceID)
	{
		return pNanoScan->NsAsSetDeviceID(wDeviceID);
	}

	_declspec(dllexport) SCODE NsInteropOpenMotionPort(LPCTSTR strMotionPort)
	{
		return pNanoScan->NsAsOpenMotionPort(strMotionPort);
	}

	_declspec(dllexport) SCODE NsInteropCloseMotionPort()
	{
		return pNanoScan->NsAsCloseMotionPort();
	}

	_declspec(dllexport) SCODE NsInteropGo2Position(float fPosition)
	{
		return pNanoScan->NsAsGo2Position(fPosition);
	}

	_declspec(dllexport) SCODE NsInteropIsSignalSaturated(short sAperture, bool * bSignalSaturated)
	{
		BOOL signalSaturated;
		SCODE returnValue = pNanoScan->NsAsIsSignalSaturated(sAperture, &signalSaturated);
		*bSignalSaturated = signalSaturated ? 1 : 0;
		return returnValue;
	}

	_declspec(dllexport) SCODE NsInteropRecompute()
	{
		return pNanoScan->NsAsRecompute();
	}

	_declspec(dllexport) SCODE NsInteropGetBeamWidthRatio(short sROIIndex, float fClipLevel, float * pfBeamWidthRatio)
	{
		return pNanoScan->NsAsGetBeamWidthRatio(sROIIndex, fClipLevel, pfBeamWidthRatio);
	}

	_declspec(dllexport) SCODE NsInteropGetBeamWidth4SigmaRatio(short sROIIndex, float * pfBeamWidth4SigmaRatio)
	{
		return pNanoScan->NsAsGetBeamWidth4SigmaRatio(sROIIndex, pfBeamWidth4SigmaRatio);
	}

	_declspec(dllexport) SCODE NsInteropGetMaxSamplingResolution(float * pfMaxSamplingRes)
	{
		return pNanoScan->NsAsGetMaxSamplingResolution(pfMaxSamplingRes);
	}

	_declspec(dllexport) SCODE NsInteropSetDivergenceMethod(short sDivMethod, float fClipLevel, float fDistance)
	{
		return pNanoScan->NsAsSetDivergenceMethod(sDivMethod, fClipLevel, fDistance);
	}

	_declspec(dllexport) SCODE NsInteropGetDivergenceMethod(short * psDivMethod, float * pfClipLevel, float * pfDistance)
	{
		return pNanoScan->NsAsGetDivergenceMethod(psDivMethod, pfClipLevel, pfDistance);
	}

	_declspec(dllexport) SCODE NsInteropGetDivergenceParameter(short sAperture, short sROIIndex, float * pfDivergence)
	{
		return pNanoScan->NsAsGetDivergenceParameter(sAperture, sROIIndex, pfDivergence);
	}

	_declspec(dllexport) SCODE NsInteropGetDeviceList(VARIANT * pvarDeviceList)
	{
		return pNanoScan->NsAsGetDeviceList(pvarDeviceList);
	}

	_declspec(dllexport) SCODE NsInteropGetAveraging(short * psFinite, short * psRolling)
	{
		return pNanoScan->NsAsGetAveraging(psFinite, psRolling);
	}

	_declspec(dllexport) SCODE NsInteropSetAveraging(short psFinite, short psRolling)
	{
		return pNanoScan->NsAsSetAveraging(psFinite, psRolling);
	}

	_declspec(dllexport) SCODE NsInteropRunComputation()
	{
		return pNanoScan->NsAsRunComputation();
	}

	_declspec(dllexport) SCODE NsInteropAutoFindForM2K()
	{
		return pNanoScan->NsAsAutoFindForM2K();
	}	
	
	_declspec(dllexport) SCODE NsInteropGetHeadGainTable(VARIANT *pvarGainTable)
	{
		return pNanoScan->NsAsGetHeadGainTable(pvarGainTable);
	}

	_declspec(dllexport) SCODE NsInteropGetHeadScanRates(VARIANT *pvarScanRates)
	{
		return pNanoScan->NsAsGetHeadScanRates(pvarScanRates);
	}

	_declspec(dllexport) SCODE NsInteropGetPowerCalibrationBreakOut(short sIndexCalibration,  BSTR * sDescriptor, float * fRefPower, float * fWaveLength)
	{
		return pNanoScan->NsAsGetPowerCalibrationBreakOut(sIndexCalibration, sDescriptor, fRefPower, fWaveLength);;
	}	

	_declspec(dllexport) bool NsInteropGetShowWindow()
	{
		return pNanoScan->GetNsAsShowWindow() != 0;
	}

	_declspec(dllexport) bool NsInteropGetAutoROI()
	{
		return pNanoScan->GetNsAsAutoROI() != 0;
	}

	_declspec(dllexport) void NsInteropSetAutoROI(bool propVal)
	{
		pNanoScan->SetNsAsAutoROI(propVal);
	}

	_declspec(dllexport) bool NsInteropGetTrackGain()
	{
		return pNanoScan->GetNsAsTrackGain() != 0;
	}

	_declspec(dllexport) void NsInteropSetTrackGain(bool propVal)
	{
		pNanoScan->SetNsAsTrackGain(propVal);
	}

	_declspec(dllexport) bool NsInteropGetTrackFilter()
	{
		return pNanoScan->GetNsAsTrackFilter() !=  0;
	}

	_declspec(dllexport) void NsInteropSetTrackFilter(bool propVal)
	{
		pNanoScan->SetNsAsTrackFilter(propVal);
	}

	_declspec(dllexport) int NsInteropGetPulsedMode()
	{
		return pNanoScan->GetNsAsPulsedMode();
	}

	_declspec(dllexport) void NsInteropSetPulsedMode(int propVal)
	{
		pNanoScan->SetNsAsPulsedMode(propVal);
	}

	_declspec(dllexport) short NsInteropGetDefaultCalibration()
	{
		return pNanoScan->GetNsAsDefaultCalibration();
	}

	_declspec(dllexport) void NsInteropSetDefaultCalibration(short propVal)
	{
		pNanoScan->SetNsAsDefaultCalibration(propVal);
	}

	_declspec(dllexport) short NsInteropGetPowerUnits()
	{
		return pNanoScan->GetNsAsPowerUnits();
	}

	_declspec(dllexport) void NsInteropSetPowerUnits(short propVal)
	{
		pNanoScan->SetNsAsPowerUnits(propVal);
	}

	_declspec(dllexport) bool NsInteropGetMultiROIMode()
	{
		return pNanoScan->GetNsAsMultiROIMode() != 0;
	}

	_declspec(dllexport) void NsInteropSetMultiROIMode(bool propVal)
	{
		pNanoScan->SetNsAsMultiROIMode(propVal);
	}

	_declspec(dllexport) float NsInteropGetRailLength()
	{
		return pNanoScan->GetNsAsRailLength();
	}

	_declspec(dllexport) void NsInteropSetRailLength(float propVal)
	{
		pNanoScan->SetNsAsRailLength(propVal);
	}

	_declspec(dllexport) short NsInteropGetGaussFitMethod()
	{
		return pNanoScan->GetNsAsGaussFitMethod();
	}

	_declspec(dllexport) void NsInteropSetGaussFitMethod(short propVal)
	{
		pNanoScan->SetNsAsGaussFitMethod(propVal);
	}

	_declspec(dllexport) float NsInteropGetMagnificationFactor()
	{
		return pNanoScan->GetNsAsMagnificationFactor();
	}

	_declspec(dllexport) void NsInteropSetMagnificationFactor(float propVal)
	{
		pNanoScan->SetNsAsMagnificationFactor(propVal);
	}

	_declspec(dllexport) short NsInteropGetBeamWidthBasis()
	{
		return pNanoScan->GetNsAsBeamWidthBasis();
	}

	_declspec(dllexport) void NsInteropSetBeamWidthBasis(short propVal)
	{
		pNanoScan->SetNsAsBeamWidthBasis(propVal);
	}
}
