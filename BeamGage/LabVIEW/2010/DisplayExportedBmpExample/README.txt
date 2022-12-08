NOTE: This is an UNSUPPORTED example which demonstrates how a BeamGage user might use LabVIEW to display the BeamGage 2D Display in a LabVIEW VI.

This example was built in LabVIEW 2010 and was known to work with BeamGage v5.7 Professional at the time of creation.  Depending on the version and product tier in use, it may be necessary to re-link the vi to the version and location of the Spiricon.BeamGage.Automation.dll that was installed with your purchased product.  This is in the BeamGage installation directory, C:\Program Files\Spiricon\BeamGage <producttier>\

Upon starting the vi, BeamGage will launch immediately and connect to the local camera connected to the PC.  The BeamGage GUI will be shown.  Once data frames begin to load into the BeamGage frame buffer the callback vi will respond to the OnNewFrame event and export a jpeg image to c:\temp\LabVIEW Images\exportImage_0002.jpeg.  If the directory does not exist then it will be created automatically.  The Callback vi will then load the image into LabVIEW and display it on the front panel.

It is important to note that the image follows the WYSIWYG principle in that what is visible in BeamGage, you will get.  Currently this also means that in order to export an image of the 2D Display with the overlays (i.e. cursor, crosshair, floating results, etc.), then the BeamGage GUI must also be shown.  If the ShowGUI boolean parameter is set to false, only the base 2D Beam Display image will be exported.

The process of saving an image file to the hard disk drive and reading it back is costly, resulting in a loss of frame rate in the vi compared to the frame rate in BeamGage.  Spiricon Engineering is aware of this and is planning to investigate a change in future versions of BeamGage.

To create a smaller image for your vi, resize the 2D Display window and save it in a setup file.  This will ensure that you always obtain a static image size.

Also included on the front panel is a single shot export example.  With the Data source stopped, this will produce a timestamped jpeg image and save it to c:\temp\LabVIEW Images\exportImage_%H%M%S.jpeg.