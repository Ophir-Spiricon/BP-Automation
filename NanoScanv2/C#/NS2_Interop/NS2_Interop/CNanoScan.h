// Machine generated IDispatch wrapper class(es) created with Add Class from Typelib Wizard

#import "C:\\Program Files (x86)\\Photon\\NanoScan v2\\Automation\\NanoScanII.tlb" no_namespace
// CNanoScan wrapper class

class CNanoScan : public COleDispatchDriver
{
public:
	CNanoScan(){} // Calls COleDispatchDriver default constructor
	CNanoScan(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CNanoScan(const CNanoScan& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

	// Attributes
public:

	// Operations
public:


	// INanoScan methods
public:
	SCODE NsAsGetHeadCapabilities(long lCapabilityID, VARIANT * pvarHeadCapability)
	{
		SCODE result;
		static BYTE parms[] = VTS_I4 VTS_PVARIANT ;
		InvokeHelper(0xd, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, lCapabilityID, pvarHeadCapability);
		return result;
	}
	SCODE NsAsSetGain(short sAperture, short sGain)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 ;
		InvokeHelper(0xe, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sGain);
		return result;
	}
	SCODE NsAsGetGain(short sAperture, short * psGain)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PI2 ;
		InvokeHelper(0xf, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, psGain);
		return result;
	}
	SCODE NsAsSetFilter(short sAperture, float fFilter)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_R4 ;
		InvokeHelper(0x10, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, fFilter);
		return result;
	}
	SCODE NsAsGetFilter(short sAperture, float * pfFilter)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 ;
		InvokeHelper(0x11, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, pfFilter);
		return result;
	}
	SCODE NsAsSetSamplingResolution(float fSamplingResolution)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x12, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fSamplingResolution);
		return result;
	}
	SCODE NsAsGetSamplingResolution(short sAperture, float * pfSamplingResolution)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 ;
		InvokeHelper(0x13, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, pfSamplingResolution);
		return result;
	}
	SCODE NsAsSetRotationFrequency(float fRotationFrequency)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x14, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fRotationFrequency);
		return result;
	}
	SCODE NsAsGetRotationFrequency(float * pfRotationFrequency)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x15, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfRotationFrequency);
		return result;
	}
	SCODE NsAsGetMeasuredRotationFreq(float * pfMeasuredRotationFreq)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x16, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfMeasuredRotationFreq);
		return result;
	}
	SCODE NsAsAutoFind()
	{
		SCODE result;
		InvokeHelper(0x17, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}
	SCODE NsAsAddROI(short sAperture, float fLeftBound, float fRightBound, BOOL bROIEnabled)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_R4 VTS_R4 VTS_BOOL ;
		InvokeHelper(0x18, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, fLeftBound, fRightBound, bROIEnabled);
		return result;
	}
	SCODE NsAsDeleteROI(short sAperture, short sROIIndex)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 ;
		InvokeHelper(0x19, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex);
		return result;
	}
	SCODE NsAsUpdateROI(short sAperture, short sROIIndex, float fLeftBound, float fRightBound, BOOL bROIEnabled)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_R4 VTS_R4 VTS_BOOL ;
		InvokeHelper(0x1a, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, fLeftBound, fRightBound, bROIEnabled);
		return result;
	}
	SCODE NsAsGetNumberOfROIs(short sAperture, short * psNumberOfROIs)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PI2 ;
		InvokeHelper(0x1b, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, psNumberOfROIs);
		return result;
	}
	SCODE NsAsGetROI(short sAperture, short sROIIndex, float * pfLeftBound, float * pfRightBound, BOOL * pbROIEnabled)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 VTS_PR4 VTS_PBOOL ;
		InvokeHelper(0x1c, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfLeftBound, pfRightBound, pbROIEnabled);
		return result;
	}
	SCODE NsAsGetApertureLimits(short sAperture, float * pfLeftBound, float * pfRightBound)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 VTS_PR4 ;
		InvokeHelper(0x1d, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, pfLeftBound, pfRightBound);
		return result;
	}
	SCODE NsAsReadProfile(short sAperture, float fStartPosition, float fEndPosition, short sDecimationFactor, VARIANT * pvarProfileAmplitude, VARIANT * pvarProfilePosition)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_R4 VTS_R4 VTS_I2 VTS_PVARIANT VTS_PVARIANT ;
		InvokeHelper(0x1e, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, fStartPosition, fEndPosition, sDecimationFactor, pvarProfileAmplitude, pvarProfilePosition);
		return result;
	}
	SCODE NsAsSelectParameters(long lParameters)
	{
		SCODE result;
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x1f, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, lParameters);
		return result;
	}
	SCODE NsAsGetSelectedParameters(long * plParameters)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI4 ;
		InvokeHelper(0x20, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, plParameters);
		return result;
	}
	SCODE NsAsSetUserClipLevel1(float fUserClipLevel1)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x21, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fUserClipLevel1);
		return result;
	}
	SCODE NsAsSetUserClipLevel2(float fUserClipLevel2)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x22, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fUserClipLevel2);
		return result;
	}
	SCODE NsAsGetUserClipLevel1(float * pfUserClipLevel1)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x23, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfUserClipLevel1);
		return result;
	}
	SCODE NsAsGetUserClipLevel2(float * pfUserClipLevel2)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x24, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfUserClipLevel2);
		return result;
	}
	SCODE NsAsGetBeamWidth(short sAperture, short sROIIndex, float fClipLevel, float * pfBeamWidth)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_R4 VTS_PR4 ;
		InvokeHelper(0x25, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, fClipLevel, pfBeamWidth);
		return result;
	}
	SCODE NsAsGetBeamWidth4Sigma(short sAperture, short sROIIndex, float * pfBeamWidth4Sigma)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x26, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfBeamWidth4Sigma);
		return result;
	}
	SCODE NsAsGetCentroidPosition(short sAperture, short sROIIndex, float * pfCentroidPosition)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x27, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfCentroidPosition);
		return result;
	}
	SCODE NsAsGetPeakPosition(short sAperture, short sROIIndex, float * pfPeakPosition)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x28, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfPeakPosition);
		return result;
	}
	SCODE NsAsGetCentroidSeparation(short sAperture, short sROIIndex, float * pfCentroidSeparation)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x29, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfCentroidSeparation);
		return result;
	}
	SCODE NsAsGetPeakSeparation(short sAperture, short sROIIndex, float * pfPeakSeparation)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x2a, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfPeakSeparation);
		return result;
	}
	SCODE NsAsGetBeamIrradiance(short sAperture, short sROIIndex, float * pfBeamIrradiance)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x2b, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfBeamIrradiance);
		return result;
	}
	SCODE NsAsGetGaussianFit(short sAperture, short sROIIndex, float * pfGoodnessFit, float * pfRoughnessFit)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 VTS_PR4 ;
		InvokeHelper(0x2c, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfGoodnessFit, pfRoughnessFit);
		return result;
	}
	SCODE NsAsGetBeamEllipticity(short sROIIndex, float * pfBeamEllipticity)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 ;
		InvokeHelper(0x2d, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sROIIndex, pfBeamEllipticity);
		return result;
	}
	SCODE NsAsSetPulseFrequency(float fPulseFrequency)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x2e, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fPulseFrequency);
		return result;
	}
	SCODE NsAsGetPulseFrequency(float * pfPulseFrequency)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x2f, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfPulseFrequency);
		return result;
	}
	SCODE NsAsAcquireSync1Rev()
	{
		SCODE result;
		InvokeHelper(0x30, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}
	SCODE NsAsGetNumPwrCalibrations(short * nNumPwrCalibrations)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI2 ;
		InvokeHelper(0x31, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, nNumPwrCalibrations);
		return result;
	}
	SCODE NsAsGetPowerCalibration(short sIndexCalibration, VARIANT * pvarPwrCalibration)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PVARIANT ;
		InvokeHelper(0x32, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sIndexCalibration, pvarPwrCalibration);
		return result;
	}
	SCODE NsAsGetTotalPower(float * pfTotalPower)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x33, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfTotalPower);
		return result;
	}
	SCODE NsAsGetPower(short nROIIndex, float * pfROIPower)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 ;
		InvokeHelper(0x34, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, nROIIndex, pfROIPower);
		return result;
	}
	SCODE NsAsGetDeviceID(short * pwDeviceID)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI2 ;
		InvokeHelper(0x35, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pwDeviceID);
		return result;
	}
	SCODE NsAsSetDeviceID(short wDeviceID)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 ;
		InvokeHelper(0x36, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, wDeviceID);
		return result;
	}
	SCODE NsAsGetNumDevices(short * pwNumDevices)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI2 ;
		InvokeHelper(0x37, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pwNumDevices);
		return result;
	}
	SCODE NsAsOpenMotionPort(LPCTSTR strMotionPort)
	{
		SCODE result;
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0x38, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, strMotionPort);
		return result;
	}
	SCODE NsAsCloseMotionPort()
	{
		SCODE result;
		InvokeHelper(0x39, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}
	SCODE NsAsGo2Position(float fPosition)
	{
		SCODE result;
		static BYTE parms[] = VTS_R4 ;
		InvokeHelper(0x3a, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, fPosition);
		return result;
	}
	SCODE NsAsIsSignalSaturated(short sAperture, BOOL * bSignalSaturated)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PBOOL ;
		InvokeHelper(0x3b, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, bSignalSaturated);
		return result;
	}
	SCODE NsAsRecompute()
	{
		SCODE result;
		InvokeHelper(0x3c, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}
	SCODE NsAsGetBeamWidthRatio(short sROIIndex, float fClipLevel, float * pfBeamWidthRatio)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_R4 VTS_PR4 ;
		InvokeHelper(0x3d, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sROIIndex, fClipLevel, pfBeamWidthRatio);
		return result;
	}
	SCODE NsAsGetBeamWidth4SigmaRatio(short sROIIndex, float * pfBeamWidth4SigmaRatio)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PR4 ;
		InvokeHelper(0x3e, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sROIIndex, pfBeamWidth4SigmaRatio);
		return result;
	}
	SCODE NsAsGetMaxSamplingResolution(float * pfMaxSamplingRes)
	{
		SCODE result;
		static BYTE parms[] = VTS_PR4 ;
		InvokeHelper(0x3f, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pfMaxSamplingRes);
		return result;
	}
	SCODE NsAsSetDivergenceMethod(short sDivMethod, float fClipLevel, float fDistance)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_R4 VTS_R4 ;
		InvokeHelper(0x40, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sDivMethod, fClipLevel, fDistance);
		return result;
	}
	SCODE NsAsGetDivergenceMethod(short * psDivMethod, float * pfClipLevel, float * pfDistance)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI2 VTS_PR4 VTS_PR4 ;
		InvokeHelper(0x41, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, psDivMethod, pfClipLevel, pfDistance);
		return result;
	}
	SCODE NsAsGetDivergenceParameter(short sAperture, short sROIIndex, float * pfDivergence)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 VTS_PR4 ;
		InvokeHelper(0x42, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sAperture, sROIIndex, pfDivergence);
		return result;
	}
	SCODE NsAsGetDeviceList(VARIANT * pvarDeviceList)
	{
		SCODE result;
		static BYTE parms[] = VTS_PVARIANT ;
		InvokeHelper(0x43, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pvarDeviceList);
		return result;
	}
	SCODE NsAsGetAveraging(short * psFinite, short * psRolling)
	{
		SCODE result;
		static BYTE parms[] = VTS_PI2 VTS_PI2 ;
		InvokeHelper(0x44, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, psFinite, psRolling);
		return result;
	}
	SCODE NsAsSetAveraging(short psFinite, short psRolling)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_I2 ;
		InvokeHelper(0x45, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, psFinite, psRolling);
		return result;
	}
	SCODE NsAsRunComputation()
	{
		SCODE result;
		InvokeHelper(0x46, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}
	SCODE NsAsGetHeadGainTable(VARIANT * pvarGainTable)
	{
		SCODE result;
		static BYTE parms[] = VTS_PVARIANT ;
		InvokeHelper(0x4a, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pvarGainTable);
		return result;
	}
	SCODE NsAsGetHeadScanRates(VARIANT * pvarScanRates)
	{
		SCODE result;
		static BYTE parms[] = VTS_PVARIANT ;
		InvokeHelper(0x4b, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, pvarScanRates);
		return result;
	}
	SCODE NsAsGetPowerCalibrationBreakOut(short sIndexCalibration, BSTR * name, float * refPower, float * wavelength)
	{
		SCODE result;
		static BYTE parms[] = VTS_I2 VTS_PBSTR VTS_PR4 VTS_PR4 ;
		InvokeHelper(0x48, DISPATCH_METHOD, VT_ERROR, (void*)&result, parms, sIndexCalibration, name, refPower, wavelength);
		return result;
	}
	SCODE NsAsAutoFindForM2K()
	{
		SCODE result;
		InvokeHelper(0x3e8, DISPATCH_METHOD, VT_ERROR, (void*)&result, NULL);
		return result;
	}

	// INanoScan properties
public:
	BOOL GetNsAsShowWindow()
	{
		BOOL result;
		GetProperty(0x1, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsShowWindow(BOOL propVal)
	{
		SetProperty(0x1, VT_BOOL, propVal);
	}
	BOOL GetNsAsDataAcquisition()
	{
		BOOL result;
		GetProperty(0x2, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsDataAcquisition(BOOL propVal)
	{
		SetProperty(0x2, VT_BOOL, propVal);
	}
	BOOL GetNsAsAutoROI()
	{
		BOOL result;
		GetProperty(0x3, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsAutoROI(BOOL propVal)
	{
		SetProperty(0x3, VT_BOOL, propVal);
	}
	BOOL GetNsAsTrackGain()
	{
		BOOL result;
		GetProperty(0x4, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsTrackGain(BOOL propVal)
	{
		SetProperty(0x4, VT_BOOL, propVal);
	}
	BOOL GetNsAsTrackFilter()
	{
		BOOL result;
		GetProperty(0x5, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsTrackFilter(BOOL propVal)
	{
		SetProperty(0x5, VT_BOOL, propVal);
	}
	int GetNsAsPulsedMode()
	{
		int result;
		GetProperty(0x6, VT_I4, (void*)&result);
		return result;
	}
	void SetNsAsPulsedMode(int propVal)
	{
		SetProperty(0x6, VT_I4, propVal);
	}
	short GetNsAsDefaultCalibration()
	{
		short result;
		GetProperty(0x7, VT_I2, (void*)&result);
		return result;
	}
	void SetNsAsDefaultCalibration(short propVal)
	{
		SetProperty(0x7, VT_I2, propVal);
	}
	short GetNsAsPowerUnits()
	{
		short result;
		GetProperty(0x8, VT_I2, (void*)&result);
		return result;
	}
	void SetNsAsPowerUnits(short propVal)
	{
		SetProperty(0x8, VT_I2, propVal);
	}
	BOOL GetNsAsMultiROIMode()
	{
		BOOL result;
		GetProperty(0x9, VT_BOOL, (void*)&result);
		return result;
	}
	void SetNsAsMultiROIMode(BOOL propVal)
	{
		SetProperty(0x9, VT_BOOL, propVal);
	}
	float GetNsAsRailLength()
	{
		float result;
		GetProperty(0xa, VT_R4, (void*)&result);
		return result;
	}
	void SetNsAsRailLength(float propVal)
	{
		SetProperty(0xa, VT_R4, propVal);
	}
	short GetNsAsGaussFitMethod()
	{
		short result;
		GetProperty(0xb, VT_I2, (void*)&result);
		return result;
	}
	void SetNsAsGaussFitMethod(short propVal)
	{
		SetProperty(0xb, VT_I2, propVal);
	}
	float GetNsAsMagnificationFactor()
	{
		float result;
		GetProperty(0xc, VT_R4, (void*)&result);
		return result;
	}
	void SetNsAsMagnificationFactor(float propVal)
	{
		SetProperty(0xc, VT_R4, propVal);
	}
	short GetNsAsBeamWidthBasis()
	{
		short result;
		GetProperty(0x49, VT_I2, (void*)&result);
		return result;
	}
	void SetNsAsBeamWidthBasis(short propVal)
	{
		SetProperty(0x49, VT_I2, propVal);
	}

};