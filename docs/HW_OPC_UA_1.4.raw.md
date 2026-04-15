# HiWatch OPC UA Data Communication - Raw Page-Faithful Extraction (V1.4)

- Source PDF: docs/.archivetempHW_OPC_UA_1.4.pdf
- Mode: Page-faithful raw extraction, markdown formatting only
- Total pages (PDF metadata): 21

## Front Matter

```text
HiWatch OPC UA data communication 
User documentation
V1.4

Table of Contents
1 SCOPE..............................................................................................................................................3
2 GENERAL INFORMATION.........................................................................................................3
3 USAGE.............................................................................................................................................3
3.1 REMOTE COMMAND AND CONTROLS...........................................................................................3
3.2 CONNECTION SECURITY..............................................................................................................4
4 OPC UA NODE STRUCTURE......................................................................................................4
4.1 ROOT LEVEL NODES....................................................................................................................4
4.2 DATA NODE FOLDERS..................................................................................................................4
4.2.1 Data node status codes........................................................................................................5
4.3 OPC UA SERVER NODE STRUCTURE REFERENCE.......................................................................5
4.3.1 ROOT FOLDER..................................................................................................................5
4.3.2 FOLDER ID: ns=2;s=Sensor_00........................................................................................6
4.3.3 FOLDER ID: ns=2;s=Sensor_00.CurrentMean.................................................................6
4.3.4 FOLDER ID: ns=2;s=Sensor_00.CurrentParticles...........................................................7
4.3.5 FOLDER ID: ns=2;s=Sensor_00.PlumeStatistics..............................................................7
4.3.6 FOLDER ID: ns=2;s=Sensor_00.SensorStatus..................................................................8
4.3.7 FOLDER ID: ns=2;s=Sensor_00.SessionData...................................................................9
4.3.8 FOLDER ID: ns=2;s=Sensor_00.OPERATIONS.............................................................10
4.3.9 FOLDER ID: ns=2;s=Sensor_00.CurrentMean...............................................................11
4.4 NOTES FOR SPECIFIC DATA NODES:...........................................................................................12
4.5 ALERT OBJECTS.........................................................................................................................12
5 COMMAND INTERFACE..........................................................................................................12
5.1 COMMAND STRUCTURE.............................................................................................................12
5.2 AVAILABLE COMMANDS...........................................................................................................12
5.2.1 SENSOR.............................................................................................................................13
5.2.2 PULSE...............................................................................................................................13
5.2.3 SESSION............................................................................................................................13
 LIMITATIONS................................................................................................................................14
6 INSTALLING HIWATCH OPC UA HMI SOFTWARE.........................................................15
6.1 RUNNING THE HIWATCH OPC UA INSTALLER........................................................................15
6.2 ACTIVATION OF OPC UA SERVER LICENSE..............................................................................19
6.3 RUNNING HIWATCH OPC UA SERVER.....................................................................................20
```

## Page 3

```text
HiWatch OPC UA3(21)
1Scope
This document is related to OPC UA data communication protocol as defined with HiWatch class spray particle 
sensors. 
OPC UA node tree structure is presented with usage description.Contents of individul nodes are described. Use of 
HiWatch sensor command interface is defined.
2General information
HiWatch sensor control software includes OPC UA server starting from Version 3.5 as an optional component. 
Running the server requires a National Instruments software license:
LabView OPC UA Toolkit deployment license, part no: 785292-35
One license per server computer is required. The installation of the OPC UA server components must be enabled in 
the main software installer. Normal shipment of the software includes a single permanent license. When shipped with 
a complete sensor system, the license is pre-installed on the sensor controller and is non-transferable.
The presence of the OPC UA server functionality can be verified in the main window title sting, and by the presence 
ot the Remote tab in camera control interface. The software version identifier contains the string 鈥漁PC_UA鈥?if the 
functionality is included.
3Usage
The OPC UA server process is started automatically when the HiWatch control software is run. Sensor nodes are 
populated only after the camera head is connected and initialized with a measurement profile.
OPC UA server accepts local and remote connections using OPC.TCP protocol. Other protocols are not supported. 
The URL for the server connection is displayed on the Remote tab in the device control section (Fig. 1). The user 
must arrange the network connection between OPC server and remote client, TCP/IP routing and name resolution in 
order to use remote clients.
If a network firewall is present on the server computer, TCP port 49580 must be open for incoming connections from 
the network to enable remote connections.
NOTE: TCP connections with the server may be interrupted when the sensor is connected or disconnected, or when 
the alert limit settings are changed. For availability reasons, enforcing automatic reconnection of the clients is 
encouraged.
3.1Remote command and controls
HiWatch OPC UA publishes the data of sensor status and measurement results over the OPC.TCP connection. The 
user may also enable remote command and control (C&C) operation. C&C is enabled by checking Remote control 
button in the tab.
The remote C&C mode has the following features:
漏 Oseir Ltd. 2026v. 1.4
Figure 1: OPC UA server connection details shown in HiWatch software. Host name in the URL 
should be substituted with the actual hostname,
```

## Page 4

```text
HiWatch OPC UA4(21)
鈥ontrol of camera and laser status is exclusively by the remote connection
鈥amera is switched to Standby/Idle state when the mode is entered
鈥aser trigger is enabled
鈥nalysis mode is activated
In remote operation, the status of the sensor should be monitored continuously over the TCP connection. CameraState, 
CameraTemp, FrameRate and SensorErrorCode are indicators of possible issues in sensor operation. Any error 
condition of the sensor must be handled in the local interface.
Refer to Section 5 for more information about the remote control implementation.
3.2Connection security
HiWatch   OPC   UA   server   uses   only   plaintext   communication.   No   encrypted 
connection is available in the present version. No user identification is made for the 
connection.   Using   an   isolated   network   between   server   and   client   is   strongly 
recommended if any network security issues are relevant.
4OPC UA Node structure
The node tree structure offered by the HiWatch OPC UA server is presented in figure 
2. All nodes are client readable, only COMMAND node is client writable. The 
details for each node are explained in the Node Reference section.
4.1Root level nodes
The HiWatch OPC UA server has root level data nodes that identify the software that 
runs the server.
鈥oftwareClass
鈥oftwareVersion
Refer to these nodes to resolve any compatibility issues with client software. This 
manual covers SoftwareVersion 3.5.
4.2Data node folders
The sensor data output is provided by the following node folders:
鈥urrentMean:   latest   filtered   mean   data   values   from   the   sensor.   These 
correspond to the items in the HiWatch  HMI History page.
鈥urrentParticles: data arrays that contain individual particle data from the 
most   recent   image   bundle   analyzed   by   the   software.   This   data   can   be 
assembled and compiled to form the equivalent of various data distributions 
in the HiWatch HMI.
鈥lumeStatistics: scalar data vaues complied from the current Particle dataset 
contained in the HMI. They represent data collected over a longer period of 
time, as specified in the Particle Collection bar of the HMI.
鈥ensorStatus: various items related to the physical state of the sensor. Data 
collection clients should monitor these nodes to be aware of the status of the 
sensor operation.
鈥essionData: various items related to the remote controlled data collection. 
Any client using the remote control functions should monitor the related 
漏 Oseir Ltd. 2026v. 1.4
Figure 2: 
HiWatch   CS 
OPC UA node 
tree
```

## Page 5

```text
HiWatch OPC UA5(21)
nodes to verify the status of the data collection.
4.2.1Data node status codes
OPC UA status codes are used to monitor the quality of collected data. The server may set the following status codes 
on the data nodes:
鈥OOD: normal state of operation
鈥AD: measurement was not successful
鈥NCERTAIN: can mean any of the following:
鈼n CurrentMean and CurrentParticles: the image has too few signals resolved, data is noisy or the particle 
density is too high for reliable measurement
鈻狢heck spray system status, sensor alignment and measurement parameters to ensure that good spray 
images are generated
鈼n PlumeStatistics: the particle count is too low for good statistic. If the status does not improve over time:
鈻猣irst check the sensor alignment and other possible issues in the measurement 
鈻猚onsider increasing the session length to collect more particles in the dataset.
鈥AD_WaitingForInitialData: the measurement has not yet started
Particle data displaying other than GOOD status should not be relied on in determining the spray properties.
4.3OPC UA server node structure reference
4.3.1ROOT FOLDER
4.3.1.1NODE ID: ns=2;s=SoftwareBuild
鈥rowse name: SoftwareBuild 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: The minor release (build) version of the server software. 
NODE ID: ns=2;s=SoftwareClass
鈥rowse name: SoftwareClass 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: OCP UA server software identifier. This value is related to sensor models supported by 
the software. 
NODE ID: ns=2;s=MaxSensorCount
鈥rowse name: MaxSensorCount 
鈥ata type: Int32 
鈥ccess level: Read 
鈥ariable description: Maximum nuber of sensors supported by the server. 
NODE ID: ns=2;s=SoftwareVersion
鈥rowse name: SoftwareVersion 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: The major release version of the server software. 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 6

```text
HiWatch OPC UA6(21)
4.3.2FOLDER ID: ns=2;s=Sensor_00
鈥rowse name: Sensor_00 
鈥older description: Sensor node folder. Folder internal structure and subfolder names defined by the sensor 
type. 
NODE ID: ns=2;s=Sensor_00.Location
鈥rowse name: Location 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: User defined location ID for the sensor device. 
NODE ID: ns=2;s=Sensor_00.SensorType
鈥rowse name: SensorType 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Name of the sensor device model. 
NODE ID: ns=2;s=Sensor_00.SensorSN
鈥rowse name: SensorSN 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Serial number of the sensor device. 
4.3.3FOLDER ID: ns=2;s=Sensor_00.CurrentMean
鈥rowse name: CurrentMean 
鈥older description: Nodes in this folder have historical access enabled with history length: 1000 points. 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleSpeed
鈥rowse name: ParticleSpeed 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleSpeed 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleDensity
鈥rowse name: ParticleDensity 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleDensity 
NODE ID: ns=2;s=Sensor_00.CurrentMean.SprayPos
鈥rowse name: SprayPos 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: SprayPos 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleCount
鈥rowse name: ParticleCount 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleCount 
NODE ID: ns=2;s=Sensor_00.CurrentMean.SprayWidth
鈥rowse name: SprayWidth 
鈥ata type: Double 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 7

```text
HiWatch OPC UA7(21)
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: SprayWidth 
NODE ID: ns=2;s=Sensor_00.CurrentMean.CurrentAlertMessage
鈥rowse name: CurrentAlertMessage 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Message specifying the most recent alert condition. 
4.3.4FOLDER ID: ns=2;s=Sensor_00.CurrentParticles
鈥rowse name: CurrentParticles 
鈥older description: Nodes in this folder have historical access enabled with history length: 1000 points. 
NODE ID: ns=2;s=Sensor_00.CurrentParticles.ParticleAxVel
鈥rowse name: ParticleAxVel 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description:  Array format data for single particle properties from most recent image bundle: 
ParticleAxVel 
NODE ID: ns=2;s=Sensor_00.CurrentParticles.ParticleLatVel
鈥rowse name: ParticleLatVel 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description:  Array format data for single particle properties from most recent image bundle: 
ParticleLatVel 
NODE ID: ns=2;s=Sensor_00.CurrentParticles.ParticleAxPos
鈥rowse name: ParticleAxPos 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description:  Array format data for single particle properties from most recent image bundle: 
ParticleAxPos 
NODE ID: ns=2;s=Sensor_00.CurrentParticles.ParticleLatPos
鈥rowse name: ParticleLatPos 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description:  Array format data for single particle properties from most recent image bundle: 
ParticleLatPos 
NODE ID: ns=2;s=Sensor_00.CurrentParticles.ParticleDiameter
鈥rowse name: ParticleDiameter 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description:  Array format data for single particle properties from most recent image bundle: 
ParticleDiameter 
4.3.5FOLDER ID: ns=2;s=Sensor_00.PlumeStatistics
鈥rowse name: PlumeStatistics 
鈥older description: Nodes in this folder have historical access enabled with history length: 1000 points. 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 8

```text
HiWatch OPC UA8(21)
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.PlumeCount
鈥rowse name: PlumeCount 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: PlumeCount 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.PeakDensityPos
鈥rowse name: PeakDensityPos 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: PeakDensityPos 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.PeakAxVelPos
鈥rowse name: PeakAxVelPos 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: PeakAxVelPos 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.PeakAxVel
鈥rowse name: PeakAxVel 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: PeakAxVel 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.SizeCount
鈥rowse name: SizeCount 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: SizeCount 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.SizeDV50
鈥rowse name: SizeDV50 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: SizeDV50 
NODE ID: ns=2;s=Sensor_00.PlumeStatistics.SizeDV90
鈥rowse name: SizeDV90 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Particle collection statistic for spray plume: SizeDV90 
4.3.6FOLDER ID: ns=2;s=Sensor_00.SensorStatus
鈥rowse name: SensorStatus 
鈥older description: Sensor status information. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.CalibrationDate
鈥rowse name: CalibrationDate 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Date of the presently loaded device calibration file. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.HardwareProfile
鈥rowse name: HardwareProfile 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 9

```text
HiWatch OPC UA9(21)
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Sensor hardware profile ID selected at startup. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.CameraState
鈥rowse name: CameraState 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Current operation state of the camera device. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.CameraTemp
鈥rowse name: CameraTemp 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Camera device internal temperature. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.LaserPulsing
鈥rowse name: LaserPulsing 
鈥ata type: Array of Double 
鈥ccess level: Read 
鈥ariable description: Laser pulse structure in microseconds. Empty if laser pulsing is disabled. Array items: 
0: pulse count 1: pulse duration 2: pulse interval 
NODE ID: ns=2;s=Sensor_00.SensorStatus.CameraGain
鈥rowse name: CameraGain 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Sensor camera device gain setting in dB 
NODE ID: ns=2;s=Sensor_00.SensorStatus.FrameRate
鈥rowse name: FrameRate 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Recent sensor frame rate as frames per second. 
NODE ID: ns=2;s=Sensor_00.SensorStatus.SensorErrorCode
鈥rowse name: SensorErrorCode 
鈥ata type: Int32 
鈥ccess level: Read 
鈥ariable description: Most recent error code given by the sensor 
NODE ID: ns=2;s=Sensor_00.SensorStatus.SensorErrorMessage
鈥rowse name: SensorErrorMessage 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Error message associated with the SensorErrorCode 
4.3.7FOLDER ID: ns=2;s=Sensor_00.SessionData
鈥rowse name: SessionData 
鈥older description: Measurement session related information. 
NODE ID: ns=2;s=Sensor_00.SessionData.SessionName
鈥rowse name: SessionName 
鈥ata type: String 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 10

```text
HiWatch OPC UA10(21)
鈥ccess level: Read 
鈥ariable description: User provided name for the measurement session 
NODE ID: ns=2;s=Sensor_00.SessionData.AlertProfileList
鈥rowse name: AlertProfileList 
鈥ata type: Array of String 
鈥ccess level: Read 
鈥ariable description: Names of available sensor alert setting profiles. 
NODE ID: ns=2;s=Sensor_00.SessionData.ActiveAlertProfile
鈥rowse name: ActiveAlertProfile 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Name of the currently active sensor alert profile. 
NODE ID: ns=2;s=Sensor_00.SessionData.MaxLength
鈥rowse name: MaxLength 
鈥ata type: Int32 
鈥ccess level: Read 
鈥ariable description: Maximum count of image bundles included in the session data set. 
NODE ID: ns=2;s=Sensor_00.SessionData.CurrentLength
鈥rowse name: CurrentLength 
鈥ata type: Int32 
鈥ccess level: Read 
鈥ariable description: Current count of image bundles included in the session data set. 
NODE ID: ns=2;s=Sensor_00.SessionData.OutputFolder
鈥rowse name: OutputFolder 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Folder base path for the output files generated when session data is stored. 
NODE ID: ns=2;s=Sensor_00.SessionData.LastOutputPath
鈥rowse name: LastOutputPath 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Absolute pathname of the folder where the last session data was stored. 
4.3.8FOLDER ID: ns=2;s=Sensor_00.OPERATIONS
鈥rowse name: OPERATIONS 
鈥older description: Node structure for issuing commands to the sensor control software. 
NODE ID: ns=2;s=Sensor_00.OPERATIONS.ENABLED
鈥rowse name: ENABLED 
鈥ata type: Boolean 
鈥ccess level: Read 
鈥ariable description: Node value informs if remote control is enabled for the sensor 
NODE ID: ns=2;s=Sensor_00.OPERATIONS.COMMAND
鈥rowse name: COMMAND 
鈥ata type: String 
鈥ccess level: Read&Write 
鈥ariable description: Writable node for issuing commands to the sensor software. 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 11

```text
HiWatch OPC UA11(21)
NODE ID: ns=2;s=Sensor_00.OPERATIONS.RESULT
鈥rowse name: RESULT 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Node contains the issued command and the result code of the command processor. 
NODE ID: ns=2;s=Sensor_00.OPERATIONS.USERPROMPT
鈥rowse name: USERPROMPT 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Node contains a display string for the user's attention. 
4.3.9FOLDER ID: ns=2;s=Sensor_00.CurrentMean
鈥rowse name: CurrentMean 
鈥older description: Nodes in this folder have historical access enabled with history length: 1000 points. 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleSpeed
鈥rowse name: ParticleSpeed 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleSpeed 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleDensity
鈥rowse name: ParticleDensity 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleDensity 
NODE ID: ns=2;s=Sensor_00.CurrentMean.SprayPos
鈥rowse name: SprayPos 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: SprayPos 
NODE ID: ns=2;s=Sensor_00.CurrentMean.ParticleCount
鈥rowse name: ParticleCount 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: ParticleCount 
NODE ID: ns=2;s=Sensor_00.CurrentMean.SprayWidth
鈥rowse name: SprayWidth 
鈥ata type: Double 
鈥ccess level: Read 
鈥ariable description: Most recent mean value for measurement: SprayWidth 
NODE ID: ns=2;s=Sensor_00.CurrentMean.CurrentAlertMessage
鈥rowse name: CurrentAlertMessage 
鈥ata type: String 
鈥ccess level: Read 
鈥ariable description: Message specifying the most recent alert condition. 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 12

```text
HiWatch OPC UA12(21)
4.4Notes for specific data nodes:  
鈥lumeStatistics.SizeCount; all sizing measurements remain inactive unless Sizing is enabled in the HMI.
鈥lumeStatistics.SizeDV50  and  PlumeStatistics.SizeDV90  require   that   corresponding   sizing   function   is 
enabled in the HMI.
鈥lumeStatistics.PeakAxVelPos and PlumeStatistics.PeakAxVel need that 鈥淐rossplots鈥?display in the HMI 
has the 鈥淟ateral V鈥?output setting enabled.
鈥urrentMean.CurrentAlertMessage is offered for the convenience of the clients that do not support OPC 
UA Events. Monitoring the Events is recommended implementation of system feedback. See Section 4.5 for 
more information.
4.5Alert objects
The sensor offers Alert node for monitoring user set level limits for user specified CurrentMean values. The values 
and limits for alert generation are defined in the HiWatch software user interface.
鈥rowse name: CurrentMeanAlertmon 
鈥vent description: Nonexclusive Level Alert for all defined CurrentMean variables
The OPC UA client software can subscribe to Alert node to receive OPC UA Events when the given variable exceeds 
the defined range limits.
The alert settings are determined and enabled in the HiWatch HMI screen. Refer to the software documentation for 
details.
NOTE: The OPC UA TCP session is disconnected when alert levels are changed. The client should try to reconnect in 
shorrt notice. A refresh request may be necessary from the client side to avoid missing condition events during 
reconnect.
5Command interface
The OPC UA server publishes a COMMAND variable node for remote control of the sensor. The client must write a 
String value to this variable to use the remote functionality. The RESULT node shows the processed command with 
result code after the processing is finished.
The ENABLED node indicates that the server will accept remote commands. Check the value of this node before 
attempting remote operations. The ENABLED condition is controlled by the operator of the server software.
NOTE: the result code is only related to the command syntax and structure information, not the actual successful or 
unsuccessful completion of the operation or its validity. The user should read the related SensorStatus node to verify 
the outcome of the command execution.
5.1Command structure
The ASCII command string value written to the OPC UA node COMMAND must have the following structure:
cmdstring {parameter}
cmdstring is one of the implemented commands, optional parameter is determined by the command specification.
5.2Available commands
The SprayWatch OPC UA server implements the following user commands:
鈥ENSOR: controls the operation of the sensor
鈼eprecated CAMERA command is still available and works with the same parameters
漏 Oseir Ltd. 2026v. 1.4
```

## Page 13

```text
HiWatch OPC UA13(21)
鈥ULSE: controls the pulsing state of the strobe laser
鈥ESSION: controls the remote data handling and storage operations
The usage, relevant parameters and options for each command are listed below.
5.2.1SENSOR
Syntax:
SENSOR [ACTIVE|IDLE]
Sets camera state to Active or Idle as specified by the mandatory parameter.
NOTE: Automatic state changes or manual mode switch by the user are still possible.
The current state of the camera is observed with SensorStatus.CameraState node. Before issuing SENSOR ACTIVE 
command, check that the state is Standby. In case of any other status code, the state of the sensor must be corrected 
before SENSOR ACTIVE will be successful.
SENSOR IDLE command will work only when the status is Active, the sensor will enter Standby state when 
completed.
NOTE: Sensor will exit Active state if operating conditions
5.2.2PULSE
Syntax:
PULSE [CT|DUR|INT] [value (floating point)]
Adjusts the laser strobe pulsing parameter listed. Other parameters will remain untouched. One value parameter must 
be given. The value uses point (.) as the decimal separator.
Switches (exactly one must be selected):
鈥T: Sets the number of strobe pulses per frame to value (rounded down).
鈥UR: Sets the time duration of strobe pulses value in microseconds.
鈥NT: Sets the time interval of strobe pulses value in microseconds.
Any value will be accepted in the command. Configuring the strobe laser will be attempted with the given value, 
however the actual result may be different. Read the LaserPulsing  node for information of the current pulsing values. 
Up to 1 s delay may be expected before the PULSE command takes effect and is shown in the LaserPulsing node. 
NOTE: HiWatch HR2 sensor will accept only INT switch for this command.
5.2.3SESSION
Syntax: SESSION [RESET|STORE|FOLDER|MAXLENGTH] [parameter]
Switches explained:
鈥ESSION RESET: clears the current particle dataset at the start of new measurement session.
鈥ESSION STORE [name]: Stores the current particle dataset to the Data table in the HMI, and to a file in the 
Autosave folder. The parameter name must be 
鈼rintable US-ASCII string (no control characters)
鈼aximum 64 bytes length
鈼ust contain no whitespace. 
The name of the output file will also include the Location parameter set for the sensor.
NOTE: the user must monitor the available storage space for new datasets.
鈥ESSION FOLDER [foldername]: defines the output folder for the session autosave data. The parameter 
foldername must be 
鈼rintable US-ASCII string (no control characters)
鈼aximum 256 bytes length
漏 Oseir Ltd. 2026v. 1.4
```

## Page 14

```text
HiWatch OPC UA14(21)
鈼ust be valid Windows pathname, to a path writable by the HMI software
鈼ust contain no whitespace, unless entirely in double quotes (")
鈥ESSION MAXLENGTH [number]: sets the maximum count of image bundles included in the particle data 
collection. 
The data count will be zero after SESSION RESET command and increase when new particle data is 
available. If the MAXLENGTH value is exceeded, the oldest data in the particle dataset will be dropped.
The success of any SESSION command should be verified by checking the value of any corresponding SessionData 
node after issuing the command. The nodes for respective switches include:
鈥ESET: SessionStatus.CurrentLength should go to zero until more particle data arrives
鈥TORE: SessionStatus.SessionName, SessionStatus.LastOutputPath will be properly set
鈥OLDER: SessionStatus.OutputFolder will be properly set
鈥AXLENGTH: SessionStatus.MaxLength will be properly set
In addition, SessionStatus.ActiveAlertProfile will show if there is an active level alert profile that has been enabled 
from the HMI.
 Limitations
The OPC UA server offers no support for the following features of OPC UA standard:
鈥ethods
鈥iews
Any equivalent functionality must be implemented by the user in the client software. 
漏 Oseir Ltd. 2026v. 1.4
```

## Page 15

```text
HiWatch OPC UA15(21)
6Installing HiWatch OPC UA HMI software
Installation of HiWatch OPC UA software is very similar to the standard version, with the following additional steps:
鈥ctivation of OPC UA server license
鈥ranting permission of OPC UA network traffic in the system firewall.
6.1Running the HiWatch OPC UA installer
The HiWatch OPC UA software is delivered on a physical medium with the HiWatch system, or a downloadable 
upgrade in compressed format. The compressed ZIP archive must be completely extracted to a local disk before 
starting the upgrade.  The upgrade will work only on a system that has a working installation of HiWatch 
software 3.5 or later. 
The installation/upgrade is started by running setup.exe program on the delivery media. The installing user must have 
administration privileges on the target system.
Starting from version 1.1, all HiWatch software will be digitally signed to enhance software security. Please verify 
that the User Account Control popup has 鈥漁seir Ltd鈥?as the 鈥漋erified publisher鈥?
漏 Oseir Ltd. 2026v. 1.4
```

## Page 16

```text
HiWatch OPC UA16(21)
After accepting the privileges of the installer, the following choices must be made:
鈥oftware destination
鈥ccepting the related NI licenses
The default values are strongly recommended for destination choices.
漏 Oseir Ltd. 2026v. 1.4
```

## Page 17

```text
HiWatch OPC UA17(21)
The upgrade installer will add new version of HiWatch software and OPC UA toolkit as below:
漏 Oseir Ltd. 2026v. 1.4
```

## Page 18

```text
HiWatch OPC UA18(21)
After the installation is complete, the system must be rebooted to finalize the process.
漏 Oseir Ltd. 2026v. 1.4
```

## Page 19

```text
HiWatch OPC UA19(21)
6.2Activation of OPC UA server license
The final step of the upgrade is to activate the OPC UA server software. 
Run 鈥漀I License Manager鈥?from the system menu, and select 鈥滾ocal Licenses鈥?
The actual components displayed may vary according to the local software installed.
The 鈥漁PC UA Deployment鈥?item must be activated. For that, the user will need 
鈼nique 鈥滳omputer ID鈥?of the target system
鈼erial number of the license, supplied with the system or upgrade delivery.
The Computer ID is revealed by clicking the 鈥滳omputer Information鈥?item.
The activation can be made at the Web address:
https://www.ni.com/nilg-activate/jsp/
customer_activate_details.jsp
This can be done in another computer. The link contains a Web form that will ask for: 
鈼omputer OS version
鈼ctivated software component name and version
鈼icense Serial number
鈼estination Computer ID
鈼ome personal information, but its veracity may not be checked. 
Entering a valid email address may be convenient, as a copy of the activation information will be sent to that address.
For HiWatch software version 3.5, use the following information:
Product:                      OPC UA Deployment
漏 Oseir Ltd. 2026v. 1.4
```

## Page 20

```text
HiWatch OPC UA20(21)
Version:                      2019
After entering the necessary details correctly, the following Web page will reveal the correct Activation Code for the 
software.
Next, click 鈥滱ctivate software鈥?on the License manager. In the popup window, select 鈥滶nter activation codes鈥?
method.
Enter the 20-character activation code to the box and click 鈥滱ctivate鈥?button. The activated component will be 
selected automatically.
Finally, the user may check that the 鈥漁PC UA Deployment鈥?component is correctly activated.  If positive, the 
SprayWatch OPC UA Server is now functional.
6.3Running HiWatch OPC UA server
After correct upgrade and activation, the system menu will contain following items:
漏 Oseir Ltd. 2026v. 1.4
```

## Page 21

```text
HiWatch OPC UA21(21)
After the upgrade, the original HiWatch HMI software version will still be available and can be used without OPC UA 
activation. Select the CS2/HR2_OPCUA item to start the OPC UA version.
When running the OPC UA version for the first time, the system may require opening of the firewall. In a typical case, 
the service should be allowed for both Private and Public networks for correct operation.
The final step is to connect a HiWatch sensor to the software and load the initial profile for it. After that the user may 
check that the correct software version is running and the OPC server URL is displayed on the Remote page (Fig.1). 
After this, the user may connect any compatible OPC UA client software using the displayed server URL and obtain 
the required data using the connection during a measurement session.
漏 Oseir Ltd. 2026v. 1.4
```


