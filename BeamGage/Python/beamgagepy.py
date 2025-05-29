# -*- coding: utf-8 -*-
""" A Python 3.10 wrapper class for BeamGage Professional v6.21 or later

This module is not intended to be run at the Python command line but is instead accessed by a secondary program
module for the purposes of utilizing the BeamGage Professional Automation Interface.  The BeamGage Automation
Interface is an API which interfaces with the BeamGage beam profiling application.  This module does not provide
direct camera API access and such access is not provided or supported with Ophir brand beam profiling cameras.
Note that the bulk of laser beam profiling is performed in the beam profiler application, not the camera, and direct
camera access is not useful.  For custom machine vision applications alternative products are available
commercially.

The BeamGagePy wrapper class and all associated subclasses, enumerations, and example code are provided as-is
with no express guarantee or warranty of functionality and may be provided and updated independently of BeamGage
Professional releases.

This module may be customized as needed but all client functions should be implemented in a secondary program
module.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
https://www.ophiropt.com/laser--measurement/software-download

Professional tier licenses for Ophir brand beam profiling cameras may be purchased by contacting:
    Ophir USA Product Support\n
    1-800-383-0814\n
    service.ophir.usa@mksinst.com

MKS | Ophir  The True Measure of Laser Performance™

Example:

    beamgage = beamgagepy.BeamGagePy("camera", True)

As a scripting language, integration of .NET libraries via Python is sometimes more complicated and can suffer
from some limitations.  The example wrapper class provides solutions and implementation for all the unusual cases.
From here this wrapper can be extended to meet the needs of almost any project.  Here are some suggested ideas.
Todo:
    * Implement access of results individually or limit to individual needs
    * Complete wrapper implementation of other individual BeamGage interfaces based on individual needs
        * Calibration
        * Processor
        * ResultsPriorityFrame
        * FramePriorityFrame
        * AutoAperture
        * ManualAperture
        * FrameBuffer
        * Commenting
        * PowerEnergy
        * OpticalScale
        * ProgrammableSettings
        * Logger
        * Multitap
        * Crosshair
        * Cursor
        * SmearCorrection
    * Ensure shutdown releases all added references
"""

import enum
import os
import time

import clr
import ctypes

import sys
if sys.version_info[0:2] != (3, 10):
    raise Exception('Requires python 3.10')

__author__ = "R. Leikis"
__copyright__ = "Copyright 2024, Ophir-Spiricon, LLC"
__credits__ = ["R. Leikis"]
__license__ = "MIT"
__version__ = "1.0"
__maintainer__ = "Ophir-Spiricon R&D"
__email__ = "service.ophir.usa@mksinst.com"
__status__ = "Release"


# main wrapper class
class BeamGagePy:
    """ A Python 3.10 wrapper class for BeamGage Professional v6.21

    Attributes:
        current_frame (IAFrame):
    """

    # load .NET assembly to control the camera
    path2BGP = r'C:\Program Files\Spiricon\BeamGage Professional'
    clr.AddReference(os.path.join(path2BGP, 'Spiricon.Automation.dll'))
    clr.AddReference(os.path.join(path2BGP, 'Spiricon.BeamGage.Automation.dll'))
    clr.AddReference(os.path.join(path2BGP, 'Spiricon.Interfaces.ConsoleService.dll'))
    clr.AddReference(os.path.join(path2BGP, 'Spiricon.TreePattern.dll'))

    # class attributes
    current_frame = None

    def __init__(self, instance_name, show_gui):
        """ Default constructor for BeamGagePy interface

        Args:
            instance_name (str): A string used to identify this automation instance.
            show_gui (bool): A bool indicating that a GUI should be created and shown with this automation instance.

        Returns:
            object: An instantiated AutomatedBeamGage object, all interfaces are served via its properties
        """
        # the class constructor automatically instantiates a BeamGage instance
        import Spiricon.Automation as SpA
        self.beamgage = SpA.AutomatedBeamGage(instance_name, show_gui)

        # initialize events
        self.frameevents = SpA.AutomationFrameEvents(self.beamgage.ResultsPriorityFrame)
        self.calibrationevents = SpA.AutomationCalibrationEvents(self.beamgage.Calibration)

        # initialize control classes
        self.data_source = DataSource(self.beamgage)
        self.save_load_setup = SaveLoadSetup(self.beamgage)
        self.partition = Partition(self.beamgage)

        # initialize results classes
        precision = 3
        self.power_energy_results = PowerEnergyResults(self.beamgage, precision)
        self.spatial_results = SpatialResults(self.beamgage, precision)
        self.divergence_results = DivergenceResults(self.beamgage, precision)
        self.frame_results = FrameResults(self.beamgage, precision)

    def shutdown(self):
        """ Calls the shutdown for BeamGage and releases all references to BeamGage in this module, and the pythonnet
         module.

        Returns:
            None:
        """
        del self.data_source
        del self.save_load_setup
        del self.partition
        del self.power_energy_results
        del self.spatial_results
        del self.frame_results
        del self.frameevents

        self.beamgage.Instance.Shutdown()
        self.beamgage.Dispose()
        del self.beamgage

    def get_frame_data(self):
        """ Get the current Frame

        This is acceptable use if the data source is stopped, but when running this should be synchronized with the
        OnNewFrame event.

        Returns:
            Any: ResultsPriorityFrame
        """
        self.current_frame = self.beamgage.ResultsPriorityFrame
        return self.current_frame


# interface wrappers
class DataSource:
    source_list = None

    def __init__(self, bg_instance):
        """ A constructor used by BeamGagePy to wrap the IADataSource, IADataSourceEGB, IACalibration, and
        IAExternalTrigger interfaces.

        Note:
            Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

        Args:
              bg_instance (Any): An AutomatedBeamGage instance
        Returns:
            None:
        """
        import Spiricon.Automation as SpA
        self.spa = SpA
        self.beamgage = bg_instance
        self.beamgage.data_source = bg_instance.DataSource

    @property
    def current_source(self):
        """ Get the source string for the currently selected data source.

        Recommended to get list of available sources before using setter to ensure valid source string.

        Returns:
            str: A string which describes the currently selected data source.
        """
        return self.beamgage.DataSource.DataSource

    @current_source.setter
    def current_source(self, source_string: str):
        self.beamgage.DataSource.DataSource = source_string

    @property
    def list_sources(self):
        """ Get a list of available data sources.

        Returns:
             list[str]: An list of strings which describe each available data source.
        """
        self.source_list = self.beamgage.DataSource.DataSourceList
        return self.source_list

    @property
    def status(self):
        """ Current data source status.

        Returns:
             object: A DataSourceStatus enumeration value.
        """
        status = self.beamgage.DataSource.Status
        return status

    def start(self):
        """ Starts the data acquisition of the data source.

        Returns:
             None:
        """
        self.beamgage.DataSource.Start()

    def stop(self):
        """ Stops the data acquisition of the data source.

        Returns:
             None:
        """
        self.beamgage.DataSource.Stop()

    def ultracal(self):
        """ Executes Ultracal background calibration.

        Ultracal background calibration is recommended in almost all configurations to neutralize the baseline noise
        profile which is a necessity for any accurate beam profiling measurements.  The current exposure and gain
        settings will be retained but black level will be modified during Ultracal.

        The ultracal_status attribute should be checked for errors such as a beam present in the frame.  BeamGage will
        automatically proceed if the positive signal is blocked.  It is recommended to block the beam near the laser
        course, not in front of the camera, to account for ambient signal in the environment.  If the signal remaining
        after blocking the source is still detectable, the ignore_beam method may be used to bypass the check and
        continue the calibration, though this situation normally represents measurement conditions that will result in
        erroneous results.

        Returns:
            None:
        """
        self.beamgage.Calibration.Ultracal()
        time.sleep(2)

    @property
    def ultracal_status(self):
        """ Get current status of background calibration.

        Returns:
            Enum: A CalibrationStatus enumeration value.
        """
        status = self.beamgage.Calibration.Status
        return status

    def ignore_beam(self):
        """ In conjunction with the CalibrationStatus.BeamDetected status, instructs the analyzer to ignore the signal
        present and continue with the background calibration.

        Note:
            Any positive signal present will be subtracted from future frames resulting in depressed or negative pixels
            in the measurement frame which will cause erroneous results.

        Returns:
            None:
        """
        self.beamgage.Calibration.IgnoreBeam()

    def setup_egb(self):
        """ Executes SetupEGB feature on the current data source.

        SetupEGB automatically finds the best Exposure and Gain settings for the current optical configuration, then
        executes an Ultracal.  External attenuation should be set prior to using SetupEGB. The ultracal_status
        attribute should be checked for errors such as a beam present in the frame.

        Returns:
            None:
        """
        self.beamgage.Calibration.SetupEGB()

    def autox_enable(self):
        """ AutoX is an automatic exposure mode which continually monitors the beam signal and adjusts the exposure,
        gain, and black level settings to ensure that the peak beam signal is near 90% of the maximum range.

        This mode is recommended only for early configuration or for situations where the peak laser signal
        varies more than 20% of the detector range.

        Note:
            This mode cannot be used with Ultracal or SetupEGB.

        Returns:
            None:
        """
        self.beamgage.Calibration.AutoX()

    def autox_disable(self):
        """ AutoX is an automatic exposure mode which continually monitors the beam signal and adjusts the exposure,
        gain, and black level settings to ensure that the peak beam signal is near 90% of the maximum range.

        This mode is recommended only for early configuration or for situations where the peak laser signal
        varies a significant amount for a static exposure and gain setting (e.g. 50% of the detector response range).

        Note:
            This mode cannot be used with Ultracal or SetupEGB.

        Returns:
            None:
        """
        self.beamgage.Calibration.AutoX()

    @property
    def autox_isenabled(self):
        """ Indicates the enabled state for the AutoX automatic exposure mode.

        Returns:
            bool: Returns the boolean state for the AutoX mode.
        """
        return self.beamgage.Calibration.AutoXIsEnabled

    @property
    def exposure_range(self):
        """ Retrieves the current exposure range [min, max].

        Returns:
             list[float]: A list of floats which describe the current exposure range.
        """
        return [self.beamgage.EGB.RangeMin(self.spa.EGBDesignator.EXPOSURE),
                self.beamgage.EGB.RangeMax(self.spa.EGBDesignator.EXPOSURE)]

    @property
    def exposure_increment(self) -> float:
        """ Gets the exposure increment of the current data source.

        Returns:
             float: The current increment value for the exposure setting.
        """
        return self.beamgage.EGB.Increment(self.spa.EGBDesignator.EXPOSURE)

    @property
    def exposure_units(self) -> str:
        """ Gets the exposure units of the current data source.

        Returns:
             str: A string value which describes the units applicable to the exposure setting.
        """
        return self.beamgage.EGB.Units(self.spa.EGBDesignator.EXPOSURE)

    @property
    def exposure(self):
        """ Gets the exposure value of the current data source. Exposure is the integration time of the sensor and
        impacts overall frame rate.

        In CW operation, exposure time should be minimized to achieve highest frame rate possible, and shortest temporal
        measurement of the laser.

        In Pulsed operation, exposure time should be matched to best integrate a single laser pulse, or consistent group
        of pulses.

        Returns:
             float: A float value which represents the current exposure setting of the data source.
        """
        return self.beamgage.EGB.Get(self.spa.EGBDesignator.EXPOSURE)

    @exposure.setter
    def exposure(self, value):
        if isinstance(value, float):
            self.beamgage.EGB.Set(self.spa.EGBDesignator.EXPOSURE, value)
        else:
            raise Exception("Argument for value must be a float")

    @property
    def gain_range(self):
        """ Retrieves the current gain range [min, max].

        Returns:
            list[float]: A list of floats which describe the current gain range.
        """
        return [self.beamgage.EGB.RangeMin(self.spa.EGBDesignator.GAIN),
                self.beamgage.EGB.RangeMax(self.spa.EGBDesignator.GAIN)]

    @property
    def gain_increment(self):
        """ Gets the gain increment of the current data source.

        Returns:
             float: The current increment value for the gain setting.
        """
        return self.beamgage.EGB.Increment(self.spa.EGBDesignator.GAIN)

    @property
    def gain_units(self):
        """ Gets the gain units of the current data source

        Returns:
             str: A string value which describes the units applicable to the gain setting.
        """
        return self.beamgage.EGB.Units(self.spa.EGBDesignator.GAIN)

    @property
    def gain(self):
        """ Gets the gain value of the current data source.

        Gain is an electronic amplification of the sensor readout at the cost of adding electronic noise
        to the measurement which increases measurement uncertainty.

        Useful for when adequate measurement signal cannot be achieved through adjustment of optical attenutation
        or exposure time, but should otherwise avoided.

        Returns:
            float: A float value which represents the current gain setting of the data source.
        """
        return self.beamgage.EGB.Get(self.spa.EGBDesignator.GAIN)

    @gain.setter
    def gain(self, value: float):
        self.beamgage.EGB.Set(self.spa.EGBDesignator.GAIN, value)

    @property
    def black_level_range(self):
        """ Retrieves the current black level range [min, max].

        Returns:
             list[float]: A list of floats which describe the current black level range.
        """
        return [self.beamgage.EGB.RangeMin(self.spa.EGBDesignator.BLACKLEVEL),
                self.beamgage.EGB.RangeMax(self.spa.EGBDesignator.BLACKLEVEL)]

    @property
    def black_level_increment(self):
        """ Gets the black level increment of the current data source.

        Returns:
            float: The current increment value for the black level setting.
        """
        return self.beamgage.EGB.Increment(self.spa.EGBDesignator.BLACKLEVEL)

    @property
    def black_level_units(self):
        """ Gets the black level units of the current data source.

        Returns:
             str: A string value which describes the units applicable to the black level setting.
        """
        return self.beamgage.EGB.Units(self.spa.EGBDesignator.BLACKLEVEL)

    @property
    def black_level(self):
        """ Gets the black level value of the current data source.

        Black level is the electronic zero point of the sensor readout.

        This is adjusted during Ultracal to raise the baseline noise so that all electronic noise is accessible for
        calibration and neutralization.

        Returns:
             float: A float value which represents the current gain setting of the data source.
        """
        return self.beamgage.EGB.Get(self.spa.EGBDesignator.BLACKLEVEL)

    @black_level.setter
    def black_level(self, value: float):
        self.beamgage.EGB.Set(self.spa.EGBDesignator.BLACKLEVEL, value)

    @property
    def trigger_delay_range(self):
        """ Gets the trigger delay range of the current data source.

        Returns:
             list[float]: A list of floats which describe the current trigger delay range.
        """
        return [self.beamgage.ExternalTrigger.DelayMin, self.beamgage.ExternalTrigger.DelayMax]

    @property
    def trigger_delay_units(self):
        """ Gets the trigger delay units of the current data source.

        Returns:
             str: A string value which describes the units applicable to the trigger delay setting.
        """
        return self.beamgage.ExternalTrigger.DelayUnits

    @property
    def trigger_delay(self):
        """ Gets the trigger delay value of the current data source.

        Returns:
            float: A float value which represents the current trigger delay setting of the data source.
        """
        return self.beamgage.ExternalTrigger.Delay

    @trigger_delay.setter
    def trigger_delay(self, value: float):
        self.beamgage.ExternalTrigger.Delay(value)

    @property
    def trigger_state(self) -> bool:
        """ Gets the external trigger state of the current data source.

        Returns:
            bool: A boolean value which represents the current external trigger state (enabled/disabled) of the data
            source.
        """
        return self.beamgage.ExternalTrigger.TriggerIn

    @trigger_state.setter
    def trigger_state(self, value: bool):
        self.beamgage.ExternalTrigger.TriggerIn = value


class SaveLoadSetup:
    """ A constructor used by BeamGagePy to wrap the IASaveLoadSetup interface
        Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

        Args:
              bg_instance (Any): An AutomatedBeamGage instance
        """

    def __init__(self, bg_instance):
        self.beamgage = bg_instance

    def save_setup(self, filename):
        """ Saves a .bgSetup file containing the current analyzer and camera settings.

        The data source should be stopped to use the IASaveLoadSetup interface, in best practice.

        Returns:
            None:
        """
        if self.beamgage.DataSource.Status != DataSourceStatus.Running:
            self.beamgage.SaveLoadSetup.SaveSetup(filename)
        else:
            raise RuntimeError("Save Setup called while data source is Running")

    def load_setup(self, filename):
        """ Loads a .bgSetup file restoring both analyzer and camera settings.

        The data source should be stopped to use the IASaveLoadSetup interface, in best practice.

        Returns:
            None:
        """
        if self.beamgage.DataSource.Status != DataSourceStatus.Running:
            self.beamgage.SaveLoadSetup.LoadSetup(filename)
        else:
            raise RuntimeError("Load Setup called while data source is Running")

class Export:
    """ A constructor used by BeamGagePy to wrap the IAExport interface
    Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

    Args:
          bg_instance (Any): An AutomatedBeamGage instance
    """

    def __init__(self, bg_instance):
        self.beamgage = bg_instance

    def save_image_2d(self, filename, ext_num, frame_buffer_index, export_format):
        """ Saves a screenshot image of the 2D Beam Display.  The resulting image is not suitable for post process
         analysis.

        The data source must be stopped to use the Export interface.  If the GUI is disabled, a default view will be
        generated. This cannot be customized.

        Returns:
            bool: A boolean value which represents the current external trigger state (enabled/disabled) of the data
            source.
        """
        if self.beamgage.DataSource.Status != DataSourceStatus.Running:
            self.beamgage.Export(filename, ext_num, frame_buffer_index, export_format)
        else:
            raise RuntimeError("Export called while data source is Running")


class Partition:
    """ A constructor used by BeamGagePy to wrap the IAPartition interface
    Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

    Args:
          bg_instance (Any): An AutomatedBeamGage instance
    """
    partition_list = None

    def __init__(self, bg_instance):
        self.beamgage = bg_instance

    def create_partition(self, center_x, center_y, width, height, string_name):
        self.beamgage.Partition.Create(center_x, center_y, width, height, string_name)

    def delete_partition(self, string_name):
        self.beamgage.Partition.Delete(string_name)

    def move_partition(self, center_x, center_y, width, height, string_name):
        self.beamgage.Partition.Move(center_x, center_y, width, height, string_name)

    def rename_partition(self, string_name, old_string_name):
        self.beamgage.Partition.Rename(string_name, old_string_name)

    def set_active_partition(self, string_name):
        self.beamgage.PartitionResults.SetPartition(string_name)

    def get_partition_names(self):
        self.partition_list = self.beamgage.PartitionResults.PartitionNames
        return self.partition_list


# results group wrappers
class PowerEnergyResults:
    def __init__(self, bg_instance, precision):
        """ A constructor used by BeamGagePy to wrap the IAResultsPowerEnergy interface.
        Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

        Args:
              bg_instance (Any): An AutomatedBeamGage instance.
              precision (int): An integer value indicating decimal precision of returned values.

        Attributes:
            self.count (int): number of results in this group
            self.precision (int): decimal precision of returned values
            todo document all attributes?

        Returns:
            None:
        """
        self.beamgage = bg_instance
        self.count = 8
        self.precision = precision
        self.total = 0.0
        self.peak = 0.0
        self.min = 0.0
        self.peak_pulse_power = 0.0
        self.average_pulse_power = 0.0
        self.average_power_density = 0.0
        self.efficiency = 0.0
        self.percent_in_aperture = 0.0

    def update(self):
        self.total = round(self.beamgage.PowerEnergyResults.Total, self.precision)
        self.peak = round(self.beamgage.PowerEnergyResults.Peak, self.precision)
        self.min = round(self.beamgage.PowerEnergyResults.Minimum, self.precision)
        self.peak_pulse_power = round(self.beamgage.PowerEnergyResults.PeakPulsePower, self.precision)
        self.average_pulse_power = round(self.beamgage.PowerEnergyResults.AveragePulsePower, self.precision)
        self.average_power_density = round(self.beamgage.PowerEnergyResults.AveragePowerDensity, self.precision)
        self.efficiency = round(self.beamgage.PowerEnergyResults.Effeciency, self.precision)
        self.percent_in_aperture = round(self.beamgage.PowerEnergyResults.PercentInAperture, self.precision)

    def disable(self, result_name):
        self.beamgage.PowerEnergyResults(result_name)


class SpatialResults:
    def __init__(self, bg_instance, precision):
        """ A constructor used by BeamGagePy to wrap the IAResultsPowerEnergy interface.
        Normally managed by the BeamGagePy class.  It is not necessary to call this constructor directly.

        Args:
              bg_instance (Any): An AutomatedBeamGage instance.
              precision (int): An integer value indicating decimal precision of returned values.

        Attributes:
            self.count (int): number of results in this group
            self.precision (int): decimal precision of returned values
            todo document all attributes?

        Returns:
            None:
        """
        self.beamgage = bg_instance
        self.count = 34
        self.precision = precision
        self.centroid_location_x = 0.0
        self.centroid_location_y = 0.0
        self.peak_location_x = 0.0
        self.peak_location_x = 0.0
        self.d_4sigma_x = 0.0
        self.d_4sigma_y = 0.0
        self.d_4sigma_dia = 0.0
        self.knifeedge_10_90_x = 0.0
        self.knifeedge_10_90_y = 0.0
        self.knifeedge_10_90_dia = 0.0
        self.knifeedge_16_84_x = 0.0
        self.knifeedge_16_84_y = 0.0
        self.knifeedge_16_84_dia = 0.0
        self.knifeedge_prog_x = 0.0
        self.knifeedge_prog_y = 0.0
        self.knifeedge_prog_dia = 0.0
        self.percent_peak_x = 0.0
        self.percent_peak_y = 0.0
        self.percent_peak_dia = 0.0
        self.moving_slit_x = 0.0
        self.moving_slit_y = 0.0
        self.percent_total_x = 0.0
        self.percent_total_y = 0.0
        self.percent_total_dia = 0.0
        self.d_epss_x = 0.0
        self.d_epss_y = 0.0
        self.d_epsa_86p5_dia = 0.0
        self.d_epsa_prog_dia = 0.0
        self.cross_sectional_area = 0.0
        self.cursor_to_crosshair = 0.0
        self.centroid_to_crosshair = 0.0
        self.orientation = 0.0
        self.ellipticity = 0.0
        self.eccentricity = 0.0

    def update(self):
        self.centroid_location_x = round(self.beamgage.SpatialResults.CentroidX, self.precision)
        self.centroid_location_y = round(self.beamgage.SpatialResults.CentroidY, self.precision)
        self.peak_location_x = round(self.beamgage.SpatialResults.PeakLocationX, self.precision)
        self.peak_location_x = round(self.beamgage.SpatialResults.PeakLocationY, self.precision)
        self.d_4sigma_x = round(self.beamgage.SpatialResults.D4SigmaMajor, self.precision)
        self.d_4sigma_y = round(self.beamgage.SpatialResults.D4SigmaMinor, self.precision)
        self.d_4sigma_dia = round(self.beamgage.SpatialResults.D4SigmaDiameter, self.precision)
        self.knifeedge_10_90_x = round(self.beamgage.SpatialResults.KnifeEdgeMajor_10_90, self.precision)
        self.knifeedge_10_90_y = round(self.beamgage.SpatialResults.KnifeEdgeMinor_10_90, self.precision)
        self.knifeedge_10_90_dia = round(self.beamgage.SpatialResults.KnifeEdgeDiameter_10_90, self.precision)
        self.knifeedge_16_84_x = round(self.beamgage.SpatialResults.KnifeEdgeMajor_16_84, self.precision)
        self.knifeedge_16_84_y = round(self.beamgage.SpatialResults.KnifeEdgeMinor_16_84, self.precision)
        self.knifeedge_16_84_dia = round(self.beamgage.SpatialResults.KnifeEdgeDiameter_16_84, self.precision)
        self.knifeedge_prog_x = round(self.beamgage.SpatialResults.KnifeEdgeMajorProgrammable, self.precision)
        self.knifeedge_prog_y = round(self.beamgage.SpatialResults.KnifeEdgeMinorProgrammable, self.precision)
        self.knifeedge_prog_dia = round(self.beamgage.SpatialResults.KnifeEdgeDiameterProgrammable, self.precision)
        self.percent_peak_x = round(self.beamgage.SpatialResults.PercentOfPeakMajor, self.precision)
        self.percent_peak_y = round(self.beamgage.SpatialResults.PercentOfPeakMinor, self.precision)
        self.percent_peak_dia = round(self.beamgage.SpatialResults.PercentOfPeakDiameter, self.precision)
        self.moving_slit_x = round(self.beamgage.SpatialResults.MovingSlitMajor, self.precision)
        self.moving_slit_y = round(self.beamgage.SpatialResults.MovingSlitMinor, self.precision)
        self.percent_total_x = round(self.beamgage.SpatialResults.PercentOfTotalMajor, self.precision)
        self.percent_total_y = round(self.beamgage.SpatialResults.PercentOfTotalMinor, self.precision)
        self.percent_total_dia = round(self.beamgage.SpatialResults.PercentOfTotalDiameter, self.precision)
        self.d_epss_x = round(self.beamgage.SpatialResults.DepssM_95Point4, self.precision)
        self.d_epss_y = round(self.beamgage.SpatialResults.Depssm_95Point4, self.precision)
        self.d_epsa_86p5_dia = round(self.beamgage.SpatialResults.Depsa_86Point5, self.precision)
        self.d_epsa_prog_dia = round(self.beamgage.SpatialResults.DepsaProgrammable, self.precision)
        self.cross_sectional_area = round(self.beamgage.SpatialResults.CrossSectionArea, self.precision)
        self.cursor_to_crosshair = round(self.beamgage.SpatialResults.CursorToCrosshair, self.precision)
        self.centroid_to_crosshair = round(self.beamgage.SpatialResults.CentroidToCrosshair, self.precision)
        self.orientation = round(self.beamgage.SpatialResults.Orientation, self.precision)
        self.ellipticity = round(self.beamgage.SpatialResults.Ellipticity, self.precision)
        self.eccentricity = round(self.beamgage.SpatialResults.Eccentricity, self.precision)

    def disable(self, result_name):
        self.beamgage.SpatialResults(result_name)


class DivergenceResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 5
        self.precision = precision
        self.angle_x_major = 0.0
        self.angle_y_minor = 0.0
        self.angle = 0.0
        self.radiant_intensity = 0.0
        self.angular_fluence = 0.0

    def update(self):
        self.angle_x_major = round(self.beamgage.DivergenceResults.AngleXMajor, self.precision)
        self.angle_y_minor = round(self.beamgage.DivergenceResults.AngleYMinor, self.precision)
        self.angle = round(self.beamgage.DivergenceResults.Angle, self.precision)
        self.radiant_intensity = round(self.beamgage.DivergenceResults.RadiantIntensity, self.precision)
        self.angular_fluence = round(self.beamgage.DivergenceResults.AngularFluence, self.precision)

    def disable(self, result_name):
        self.beamgage.DivergenceResults(result_name)


class GaussianResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 10
        self.precision = precision
        self.gauss_height_z = 0.0
        self.gauss_width_x = 0.0
        self.gauss_width_y = 0.0
        self.gauss_centroid_x = 0.0
        self.gauss_centroid_y = 0.0
        self.delta_centroid_x = 0.0
        self.delta_centroid_y = 0.0
        self.delta_centroid = 0.0
        self.goodness_of_fit = 0.0
        self.roughness_of_fit = 0.0

    def update(self):
        self.gauss_height_z = round(self.beamgage.GaussianResults.GaussHeightZg, self.precision)
        self.gauss_width_x = round(self.beamgage.GaussianResults.DgXMajor, self.precision)
        self.gauss_width_y = round(self.beamgage.GaussianResults.DgYMinor, self.precision)
        self.gauss_centroid_x = round(self.beamgage.GaussianResults.GaussCentroidX, self.precision)
        self.gauss_centroid_y = round(self.beamgage.GaussianResults.GaussCentroidY, self.precision)
        self.delta_centroid_x = round(self.beamgage.GaussianResults.DeltaCentroidX, self.precision)
        self.delta_centroid_y = round(self.beamgage.GaussianResults.DeltaCentroidY, self.precision)
        self.delta_centroid = round(self.beamgage.GaussianResults.DeltaCentroid, self.precision)
        self.goodness_of_fit = round(self.beamgage.GaussianResults.GoodnessOfFit, self.precision)
        self.roughness_of_fit = round(self.beamgage.GaussianResults.RoughnessOfFit, self.precision)

    def disable(self, result_name):
        self.beamgage.GaussianResults(result_name)


class GaussianResults1D:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 6
        self.precision = precision
        self.gauss_height = 0.0
        self.gauss_width = 0.0
        self.gauss_centroid = 0.0
        self.delta_centroid = 0.0
        self.goodness_of_fit = 0.0
        self.roughness_of_fit = 0.0

    def update(self, axis):
        self.gauss_height = round(self.beamgage.GaussianResults1D.GaussHeightZg(axis), self.precision)
        self.gauss_width = round(self.beamgage.GaussianResults1D.DgXMajor(axis), self.precision)
        self.gauss_centroid = round(self.beamgage.GaussianResults1D.GaussCentroidX(axis), self.precision)
        self.delta_centroid = round(self.beamgage.GaussianResults1D.DeltaCentroid(axis), self.precision)
        self.goodness_of_fit = round(self.beamgage.GaussianResults1D.GoodnessOfFit(axis), self.precision)
        self.roughness_of_fit = round(self.beamgage.GaussianResults1D.RoughnessOfFit(axis), self.precision)

    def disable(self, result_name):
        self.beamgage.GaussianResults1D(result_name)


class TopHatResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 8
        self.precision = precision
        self.flatness = 0.0
        self.effective_area = 0.0
        self.fractional_power = 0.0
        self.effective_average_fluence = 0.0
        self.uniformity = 0.0
        self.plateau_uniformity = 0.0
        self.edge_steepness = 0.0

    def update(self):
        self.flatness = round(self.beamgage.TopHatResults.Flatness, self.precision)
        self.effective_area = round(self.beamgage.TopHatResults.EffectiveArea, self.precision)
        self.fractional_power = round(self.beamgage.TopHatResults.Fractional, self.precision)
        self.effective_average_fluence = round(self.beamgage.TopHatResults.EffectiveAverageFluence, self.precision)
        self.uniformity = round(self.beamgage.TopHatResults.Uniformity, self.precision)
        self.plateau_uniformity = round(self.beamgage.TopHatResults.PlateauUniformity, self.precision)
        self.edge_steepness = round(self.beamgage.TopHatResults.EdgeSteepness, self.precision)

    def disable(self, result_name):
        self.beamgage.TopHatResults(result_name)


class TopHatResults1D:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 6
        self.precision = precision
        self.flatness = 0.0
        self.effective_power = 0.0
        self.fractional_power = 0.0
        self.uniformity = 0.0
        self.plateau_uniformity = 0.0
        self.edge_steepness = 0.0

    def update(self, axis):
        self.flatness = round(self.beamgage.TopHatResults1D.Flatness(axis), self.precision)
        self.effective_power = round(self.beamgage.TopHatResults1D.Effective(axis), self.precision)
        self.fractional_power = round(self.beamgage.TopHatResults1D.Fractional(axis), self.precision)
        self.uniformity = round(self.beamgage.TopHatResults1D.Uniformity(axis), self.precision)
        self.plateau_uniformity = round(self.beamgage.TopHatResults1D.PlateauUniformity(axis), self.precision)
        self.edge_steepness = round(self.beamgage.TopHatResults1D.EdgeSteepness(axis), self.precision)

    def disable(self, result_name):
        self.beamgage.TopHatResults1D(result_name)


class FrameResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 11  # type: int
        self.precision = precision  # type: int
        self.width = 0.0  # type: float
        self.height = 0.0  # type: float
        self.frame_timestamp = 0.0  # type: float
        self.bit_depth = 0  # type: int
        self.pixel_scale = 0.0  # type: float
        self.offset_x = 0.0  # type: float
        self.offset_y = 0.0  # type: float
        self.binning_x = 0.0  # type: float
        self.binning_y = 0.0  # type: float
        self.gamma = 0.0  # type: float
        self.frame_comment = ""  # type: str

    def update(self):
        self.width = round(self.beamgage.FrameInfoResults.Width, self.precision)
        self.height = round(self.beamgage.FrameInfoResults.Height, self.precision)
        self.frame_timestamp = self.beamgage.FrameInfoResults.Timestamp
        self.bit_depth = self.beamgage.FrameInfoResults.BitsPerPixel
        self.pixel_scale = self.beamgage.FrameInfoResults.ScaleMultiplier  # do not round to maintain full precision
        self.offset_x = round(self.beamgage.FrameInfoResults.OffsetX, self.precision)
        self.offset_y = round(self.beamgage.FrameInfoResults.OffsetY, self.precision)
        self.binning_x = round(self.beamgage.FrameInfoResults.BinningX, self.precision)
        self.binning_y = round(self.beamgage.FrameInfoResults.BinningY, self.precision)
        self.gamma = round(self.beamgage.FrameInfoResults.Gamma, self.precision)
        self.frame_comment = self.beamgage.FrameInfoResults.Comment

    def disable(self, result_name):
        self.beamgage.FrameInfoResults(result_name)


class CustomCalculationResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 0
        self.precision = precision
        self.custom_result = 0.0

    def update(self, result_name):
        self.custom_result = self.beamgage.CustomCalculationResults(result_name)

    def disable(self, result_name):
        self.beamgage.CustomCalculationResults(result_name)


class PositionalStabilityResults:
    def __init__(self, bg_instance, precision):
        self.beamgage = bg_instance
        self.count = 9
        self.precision = precision
        self.sample_size = 0
        self.center_x = 0.0
        self.center_y = 0.0
        self.last_x = 0.0
        self.last_y = 0.0
        self.azimuth = 0.0
        self.stability_x = 0.0
        self.stability_y = 0.0
        self.stability = 0.0

    def update(self):
        self.sample_size = self.beamgage.PositionalStabilityResults.SampleSize
        self.center_x = round(self.beamgage.PositionalStabilityResults.CenterX, self.precision)
        self.center_y = round(self.beamgage.PositionalStabilityResults.CenterY, self.precision)
        self.last_x = round(self.beamgage.PositionalStabilityResults.LastX, self.precision)
        self.last_y = round(self.beamgage.PositionalStabilityResults.LastY, self.precision)
        self.azimuth = round(self.beamgage.PositionalStabilityResults.Azimuth, self.precision)
        self.stability_x = round(self.beamgage.PositionalStabilityResults.PositionalStabilityX, self.precision)
        self.stability_y = round(self.beamgage.PositionalStabilityResults.PositionalStabilityY, self.precision)
        self.stability = round(self.beamgage.PositionalStabilityResults.PositionalStability, self.precision)

    def disable(self, result_name):
        self.beamgage.PositionalStabilityResults(result_name)

# enumerations listed alphabetically """

###
# These are likely unneeded now that the Python.Net enum handling has changed in v3.0 and later.
# It is possible to directly reference the enums from the Spiricon.Automation namespace.
###


class ApertureShape(enum.Enum):
    Rectangle = 0
    Ellipse = 1


class Axis(enum.Enum):
    X_Major = 0
    Y_minor = 1


class BaselineCalibrationStatus(enum.Enum):
    NotOn = 0
    Successful = 1
    AutoXAdjusting = 2
    Size = 3
    Location = 4
    Gain = 5
    Exposure = 6
    BlackLevel = 7
    BitsPerPixel = 8
    Binning = 9
    BitEncoding = 10
    FrameRate = 11
    Lens = 12


class CalibrationStatus(enum.Enum):
    Not_Supported = 0
    Failed = 1
    Calibrating = 2
    Ready = 3
    Beam_Detected = 4


class DataSourceStatus(enum.Enum):
    Unavailable = 0
    Running = 1
    Paused = 2


class EGBDesignator(enum.Enum):
    Exposure = 0
    Gain = 1
    BlackLevel = 2


class ExportFormat(enum.Enum):
    No_Format = 0  # Do Not Use! internal default only
    ASCII = 1
    BMP = 2
    GIF = 3
    JPG = 4
    PNG = 5
    TIFF = 6


class ExportStatus(enum.Enum):
    Good = 0
    Bad_File = 1
    Bad_Frame_Number = 2
    Base_File_Exists = 3
    Base_File_Is_Directory = 4
    No_File_Name = 5
    No_Image = 6
    Read_Only = 7


class FrameCalibrationStatus(enum.Enum):
    NoStatus = 0
    Manual = 1
    Interpolated = 2
    Power_Meter = 3


class LoggerSubscriptionTypes(enum.Enum):
    Continuous = 0
    Frames = 1
    Time = 2


class LoggerTypes(enum.Enum):
    Binary = 0
    Results = 1
    Sum = 2
    Cursor = 3
    ASCII = 4


class PowerEnergyUnitBase(enum.Enum):
    Joules = 0
    Watts = 1
    No_Units = 2


class PowerEnergyUnitQuantifier(enum.Enum):
    Kilo = 0
    No_Quantifier = 1
    Milli = 2
    Micro = 3
    Nano = 4
    Pico = 5


class ProgrammableSettingsNames(enum.Enum):
    ThresholdPEValue = 0
    ThresholdPEPercent = 1
    FocalLength = 2
    DetectorDistance = 3
    SeparationDistance = 4
    FirstPointX = 5
    FirstPointY = 6
    FirstPointD = 7
    PercentPeakClip = 8
    PercentTotalClip = 9
    KnifeEdgeLowClip = 10
    KnifeEdgeMultiplier = 11
    MovingSlit = 12
    SmallestAperture = 13


class SaveLoadStatus(enum.Enum):
    Success = 0
    Not_Exist = 1
    Persistence_Error = 2
    Read_Only = 3
    FileTypeError = 4


class TrackingTypes(enum.Enum):
    Disabled = 0
    Peak = 1
    Manual = 2
    Centroid = 3


class TriggerPolarity(enum.Enum):
    Negative = 0
    Positive = 1
