# -*- coding: utf-8 -*-
""" A Python 3.7 example client for BeamGage Professional v6.16

This module is intended as a client program for the purposes of utilizing the BeamGage Professional
Automation Interface.  The BeamGage Automation Interface is an API which interfaces with the BeamGage beam profiling
application.  This module does not provide direct camera API access and such access is not provided or supported with
Ophir brand beam profiling cameras.  Note that the bulk of laser beam profiling is performed in the beam profiler
application, not the camera, and direct camera access is not useful.  For custom machine vision applications
alternative products are available commercially.

The BeamGagePy wrapper class and all associated subclasses, enumerations, and example code are provided as-is
with no express guarantee or warranty of functionality and may be provided and updated independently of BeamGage
Professional releases.

This module may be adapted as needed without attribution.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
https://www.ophiropt.com/laser--measurement/software-download

Professional tier licenses for Ophir brand beam profiling cameras may be purchased by contacting:
    Ophir USA Product Support\n
    1-800-383-0814\n
    service.ophir.usa@mksinst.com

MKS | Ophir  The True Measure of Laser Performance™

Todo:
    * Flush out inline documentation
    * Consider additional scenarios using more features and best practices
"""


import math
import statistics
import time

import beamgagepy

__author__ = "Russ Leikis"
__copyright__ = "Copyright 2022, Ophir-Spiricon, LLC"
__credits__ = ["Russ Leikis"]
__license__ = "MIT"
__version__ = "0.3"
__maintainer__ = "Russ Leikis"
__email__ = "russ.leikis@mksinst.com"
__status__ = "Alpha"


def main():
    # BeamGage Client
    beamgage = beamgagepy.BeamGagePy("camera", True)  # instantiate BeamGagePy class with instance name and GUI visible

    # Client will startup automatically acquiring data
    beamgage.data_source.stop()

    source_name = beamgage.data_source.current_source
    print("Source: %s" % source_name)

    exp_range = beamgage.data_source.exposure_range
    print("Exposure Range: %s" % exp_range)

    # Background calibrate the camera (don't forget to block the beam)
    beamgage.data_source.ultracal()
    cal_status = beamgage.data_source.ultracal_status()
    print(cal_status.name)

    # todo this isn't getting called despite getting registered to the event
    # def newframe_handler(source, args):
    #     print('my_handler called!')
    #
    #     # print('\n*Spatial Results from Event*')
    #     # beamgage.spatial_results.update()
    #     # print("D4S (X,Y): %.3f, %.3f" % (beamgage.spatial_results.d_4sigma_x, beamgage.spatial_results.d_4sigma_y))
    #     # print("D4S Dia: %.3f" % beamgage.spatial_results.d_4sigma_dia)
    #
    # d = beamgage.frameevents.OnNewFrame(newframe_handler)
    #
    # # Register event handler with OnNewFrame Event
    # beamgage.frameevents.OnNewFrame += d

    # Acquire data
    beamgage.data_source.start()
    time.sleep(5)
    status = beamgage.data_source.status
    print(status)

    # Obtaining results and frame data should be event driven or with the data source stopped
    data = beamgage.get_frame_data()
    print("Pixel Value (top-left) %.3f: " % (data.DoubleData[0]))

    print("\n-Spatial Results-")
    beamgage.spatial_results.update()
    print("D4S (X,Y): %.3f, %.3f" % (beamgage.spatial_results.d_4sigma_x, beamgage.spatial_results.d_4sigma_y))
    print("D4S Dia: %.3f" % beamgage.spatial_results.d_4sigma_dia)

    print("\n-Frame Results-")
    beamgage.frame_results.update()
    print("Width: %d" % beamgage.frame_results.width)
    print("Height: %d" % beamgage.frame_results.height)

    # Create a Partition to obtain results for a unique area of the sensor and monitor baseline noise
    beamgage.partition.create_partition(250, 250, 250, 250, 'baseline')

    # Get list of current partitions and select the first custom partition
    partition_list = beamgage.partition.get_partition_names()
    beamgage.partition.set_active_partition(partition_list[1])
    beamgage.power_energy_results.update()

    # Calculate dynamic range and print baseline stats
    max_ad = math.pow(2, beamgage.frame_results.bit_depth) - beamgage.power_energy_results.min
    baseline = beamgage.get_frame_data()
    mean = statistics.mean(baseline.DoubleData)

    noise_bias = (mean / max_ad) * 100
    noise_range = ((beamgage.power_energy_results.peak - beamgage.power_energy_results.min) / max_ad) * 100

    print("\n-Baseline-")
    print("Max A/D Range: %.3f" % max_ad)
    print("Bias: %.3f%% Max A/D" % noise_bias)
    print("Range: %.3f%% Max A/D" % noise_range)

    # Unregister from events and shutdown
    # beamgage.frameevents.OnNewFrame -= newframe_handler
    beamgage.shutdown()


if __name__ == "__main__":
    main()
