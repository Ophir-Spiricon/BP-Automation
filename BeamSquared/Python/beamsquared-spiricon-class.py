# -*- coding: utf-8 -*-
""" A Python 3.10 wrapper class for BeamSquared v2.5.2

    This class was contributed by a customer it illustrates an implementation of a wrapper for the AutomatedBeamSquared
    class.  This is not a replete example and additional implementation for instantiation and shutdown of the automation
    server.

    This class has not been reviewed for at this time.
"""

class Spiricon_Beamsquared:
    def __init__(self):
        #Handling server start up issues. Try to connect the first time, if it fails, try again
        try:
            self.beamsquare = AutomatedBeamSquared()
            time.sleep(3)
        except:
            self.beamsquare = AutomatedBeamSquared()
            print(self.beamsquare.RunManager.RunStatus)
            if str(self.beamsquare.RunManager.RunStatus) == "READY":
                #print("Connected to M2 Application")
            else:
                print("Failed to connect to M2 Application")
    
    def close(self):
        #Add close to shutdown server connect connectly
        self.beamsquare.Instance.Shutdown()
        return 0
    def __del__(self):
        #Call self.close() once the instant finished.
        self.close()
        
    def live(self,status):
        self.__live = status
        self.beamsquare.Capture.LiveMode = self.__live 
        
    def peak_intensity(self):
        return self.beamsquare.QuantitativeResults.Peak
    
    def min_intensity(self):
        return self.beamsquare.QuantitativeResults.Min
    
    def total_intensity(self):
        return self.beamsquare.QuantitativeResults.Total
    
    def peak_x(self):
        return self.beamsquare.QuantitativeResults.PeakLocX
    
    def peak_y(self):
        return self.beamsquare.QuantitativeResults.PeakLocY
        
    def start_m2_run(self):
        """Calls the StartM2Run command of the M2X server."""
        self.beamsquare.RunManager.StartRun()
        time.sleep(120)

    def finish_m2_run(self):
        """Calls the StartM2Run command of the M2X server."""
        self.beamsquare.RunManager.FinishRun()

    def abort_m2_run(self):
        """Calls the StartM2Run command of the M2X server."""
        self.beamsquare.RunManager.AbortRun()    
    
    def load_laser_location(self, location):
        """
        load laser location parameters in mm
        distance between laser output to the spiricon box input
        """
        self.__location = float(location)
        self.beamsquare.Setup.LaserLocation = self.__location

    def load_laser_wavelength(self, wavelength):
        self.__wavelength = float(wavelength)
        self.beamsquare.Setup.WaveLength = self.__wavelength
        
    def load_focal_length(self, focallength):
        self.__focallength = float(focallength)
        self.beamsquare.Setup.FocalLength = self.__focallength
    
    def save_data(self, filename):
        self.__filename = filename
        self.beamsquare.SaveLoadData.SaveData(self.__filename)
    
    def z_start(self, z_start):
        self.__z_start = float(z_start)
        self.beamsquare.SuggestedZGenerator.ZStart = self.__z_start    
    
    def z_end(self, z_end):
        self.__z_end = float(z_end)
        self.beamsquare.SuggestedZGenerator.ZEnd = self.__z_end
        
    def z_step(self, z_step):
        self.__z_step = float(z_step)
        self.beamsquare.SuggestedZGenerator.Step = self.__z_step 

    def get_laser_results(self):
        self.__m2X = round(self.beamsquare.LaserResults.M2X,3)
        self.__m2Y = round(self.beamsquare.LaserResults.M2Y,3)
        self.__bppX = round(self.beamsquare.LaserResults.BPPX,3)
        self.__bppY = round(self.beamsquare.LaserResults.BPPY,3)
        self.__divergenceX = round(self.beamsquare.LaserResults.DivergenceX,3)
        self.__divergenceY = round(self.beamsquare.LaserResults.DivergenceY,3)
        self.__waistwidthX = round(self.beamsquare.LaserResults.WaistWidthX,3)
        self.__waistwidthY = round(self.beamsquare.LaserResults.WaistWidthY,3)
        self.__waistlocationX = round(self.beamsquare.LaserResults.WaistLocationX,3)
        self.__waistlocationY = round(self.beamsquare.LaserResults.WaistLocationY,3)
        self.__rayleighX = round(self.beamsquare.LaserResults.RayleighX,3)
        self.__rayleighY = round(self.beamsquare.LaserResults.RayleighY,3)
        self.__astigmatism = round(self.beamsquare.LaserResults.Astigmatism,3)
        self.__asymmetry = round(self.beamsquare.LaserResults.Asymmetry,3)
        
        return [self.__m2X,self.__m2Y,self.__bppX,self.__bppY,self.__divergenceX,self.__divergenceY,self.__waistwidthX,
                    self.__waistwidthY,self.__waistlocationX,self.__waistlocationY,self.__rayleighX,self.__rayleighY,
                    self.__astigmatism,self.__asymmetry]
