---
uid: adaptive_sensing
---

# Adaptive Sensing
An essential trade off within all mobile sensing apps is between (a) data density (high sampling
rates) and continuity (no data gaps) and (b) battery drain. It is easy to configure Sensus to 
optimize either of these objectives in isolation, but striking a balance between them poses significant 
challenges. For example, one might wish to sample high-density accelerometry when the device
is likely to be used in particular ways (e.g., while walking). Similarly, one might wish to sample light 
and temperature data, but only in particular geographic locations. No single, static sensing configuration 
would satisfy such objectives while also having minimal impact on the battery. Such use cases require
dynamic sensing configurations that adapt to usage contexts.

## Adaptive Control
The figure below depicts concepts that drive adaptive sensing within Sensus.

![image](/sensus/images/adaptive-sensing-state-diagram.png)

The following concepts are indicated:

* Sensing agent:  An entity that controls sensing parameters (e.g., sampling rates and continuity) on
the basis of observed state (i.e., <xref:Sensus.Datum> objects).

* Opportunistic observation:  Sensing agents can be configured to observe state information actively 
or opportunistically. Opportunistic observation captures any <xref:Sensus.Datum> object generated 
by Sensus during normal operation. In this mode, the app will not take any extra actions to observe 
data. This has the effect of minimizing battery drain at the expense of weaker state estimates and 
sensing control.

* Opportunistic control:  In response to opportunistically observed data, the sensing agent
may decide to control sensing in a particular way (e.g., by enabling continuous sensing or
increasing sampling rates). Such decisions are only possible upon the arrival of opportunistic data.

* Action interval:  In contrast with opportunistic sensing, sensing agents can be configured to actively
seek out data. This has the effect of strengthening state estimates and sensing control at the expense 
of increased battery drain. The action interval indicates how frequently Sensus should actively observe 
data for state estimation.

* Active observation duration:  Once the action interval elapses, Sensus will begin to actively
observe each <xref:Sensus.Datum> generated by the app. This parameter governs how long active
observation should continue before checking the control criterion.

* Control criterion:  Regardless of whether Sensus is observing opportunistically or actively, 
the control criterion defines state estimates that trigger sensing control. For example, a 
criterion might specify that sensing control should occur when the average acceleration 
magnitude exceeds a critical threshold. This is demonstrated in the 
[example](https://github.com/predictive-technology-laboratory/sensus/blob/develop/ExampleSensingAgent.Shared/ExampleAccelerationSensingAgent.cs)
sensing agent. This agent also demonstrates a control criterion based on proximity of the phone
to a surface (e.g., face).

* Control completion check interval:  Once sensing control is invoked, the app will periodically 
recheck the control criterion to determine whether it is still met. If the criterion is not met, 
then sensing control ends, the sensing agent transitions sensing settings as needed (e.g., reducing
sampling rates), and the sensing agent returns to its idle state. If the criterion is still met, 
then sensing control continues unabated until the next completion check occurs. This parameter governs 
how long Sensus should wait between each completion check.

## Android

### Sensing Agent Plug-Ins
On Android, Sensus supports a plug-in architecture for modules (or agents) that control Sensing configuration.
This architecture is intended to support research into adaptive sensing by providing a simple interface
through which researchers can deploy agents that implement specific adaptation approaches.

### Implementing and Deploying a Sensing Agent Plug-In
Follow the steps below to implement and deploy a sensing agent within your Sensus study.

1. Create a new Android Class Library project in Visual Studio. In Visual Studio for Mac, the following image
shows the correct selection:

![image](/sensus/images/survey-agent-project.png)

1. Add a NuGet reference to [the Sensus package](https://www.nuget.org/packages/Sensus).

1. Add a new class that inherits from <xref:Sensus.SensingAgent>. Implement all abstract methods.

1. Build the library project, and upload the resulting .dll to a web-accessible URL. A convenient
solution is to upload the .dll to a Dropbox directory and copy the sharing URL for the .dll file.

1. Generate a QR code that points to your .dll (e.g., using [QR Code Generator](https://www.qr-code-generator.com/)).
The content of the QR code must be exactly as shown below:
```
sensing-agent:URL
```
where URL is the web-accessible URL that points to your .dll file. If you are using Dropbox, then the QR code
content will be similar to the one shown below (note the `dl=1` ending of the URL, and note that the following 
URL is only an example -- it is not actually valid):
```
sensing-agent:https://www.dropbox.com/s/dlaksdjfasfasdf/SensingAgent.dll?dl=1
```

1. In your <xref:Sensus.Protocol> settings, tap "Set Agent" and scan your QR code. Sensus will fetch your .dll file and 
extract any agent definitions contained therein. Select your desired agent.

1. Continue with [configuration](xref:protocol_creation) and [distribution](xref:protocol_distribution)
of your protocol.

### Example Sensing Agents
See the following implementations for example agents:

* [Acceleration](xref:ExampleSensingAgent.ExampleAccelerationSensingAgent) (code [here](https://github.com/predictive-technology-laboratory/sensus/blob/develop/ExampleSensingAgent.Shared/ExampleAccelerationSensingAgent.cs)):  A 
sensing agent that samples continuously if the device is moving or near a surface (e.g., face).

## iOS

In contrast with Android, iOS does not allow apps to load code (e.g., from the above .dll assembly) at
run time. Thus, adaptive sensing agents are more limited on iOS compared with Android. Here are the options:

* The app comes with one example sensing agent; however, it is simply for demonstration and is unlikely to work
well in practice. Nonetheless, the examples is:

  * [Acceleration](xref:ExampleSensingAgent.ExampleAccelerationSensingAgent) (code [here](https://github.com/predictive-technology-laboratory/sensus/blob/develop/ExampleSensingAgent.Shared/ExampleAccelerationSensingAgent.cs)):  A 
sensing agent that samples continuously if the device is moving or near a surface (e.g., face).

You can select this agent when configuring the <xref:Sensus.Protocol>.

* You can [redeploy](xref:redeploying) Sensus as your own app, to which you can add your own agent implementations.

* You can implement your own agent implementations following the instructions above for Android and email 
our team (uva.ptl@gmail.com) to include them in a future release.

## Testing and Debugging

Regardless of whether your sensing agent targets Android or iOS, there are a few ways to test and debug it:

* Monitor the agent state:  Within your <xref:Sensus.Protocol> settings, tap "View Agent State" to see a real-time
display of your agent's state. You will see it cycle through the state diagram shown above.

* Write to the log file:  See the code for the example agents above. You will see calls that write to the log file. Use similar
calls in your code to write information about the behavior of your agent to the log. Run your agent for a while in the app and
then share the log file with yourself from within the app. Note that the size of the log file is limited, so you might not be 
able to view the entire log history of your agent.

* Flash notifications on the screen:  On Android, you can flash notifications on the screen as shown in the example code. These
messages will appear for a short duration.

* Run your agent in the debugger:  By far the most useful approach is to [configure a development system](xref:dev_config) and
run Sensus in the debugger with your sensing agent. You will need to add your agent code to the Sensus app projects in order to 
step through it in the debugger.
