# About
This is a sample wrapper class donated by a customer for the expressed purpose of helping other customers.  The rest of the implementation was not provided, but can be used to understand or boot strap a development effort with Python and BeamSquared.

As with all the code provided in the repo it is provided without warranty or guarantee.  Support or troubleshooting for this sample code will not be provided.

## Python and inter-process events
BeamSquared is a system in which motion components are orchestrated to create a compound measurement of laser propagation.  In an ideal state, one could simply initiate a measurement run and it would succeed.  However, this is rarely the case in a real world measurement environment.  Multiple events and status enumerations are provided to allow handling and response to conditions that arise during configuration and the measurement run.

Python natively lacks the ability to register for events across process boundaries.  The Python for .NET module (pythonnet/clr), has commented publicly that this is not a feature they plan on supporting for .NET events in another process.  We have not found a workaround for this limitation.  For these reasons, Python, or other languages with this limitation are not recommended for use with BeamSquared.
