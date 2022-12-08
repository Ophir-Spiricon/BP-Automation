// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the NS2_Interop_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// NS2_Interop_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef NS2_Interop_EXPORTS
#define NS2_Interop_API __declspec(dllexport)
#else
#define NS2_Interop_API __declspec(dllimport)
#endif

// This class is exported from the NS2_Interop.dll
class NS2_Interop_API CNS2_Interop {
public:
	CNS2_Interop(void);
	// TODO: add your methods here.
};

extern NS2_Interop_API int nNS2_Interop;

NS2_Interop_API int fnNS2_Interop(void);
