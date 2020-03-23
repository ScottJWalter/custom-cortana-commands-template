## Custom Cortana Commands Template

This is a simple template for how to register custom commands with Cortana on Windows 10.

#### Included Examples:
- Opening a website
- Opening a file
- Creating/modifiying a file
- Sending serial data
- Sending simple Pushover.Net notifications

***Feel free to create pull requests with additional command templates!***

### Prerequisites

1. Install [**Visual Studio Community Edition**](https://visualstudio.microsoft.com/vs/community/), with the
   [**Windows Universal Platform application template**](https://visualstudio.microsoft.com/vs/features/universal-windows-platform/)
2. Enable [**Windows Developer Mode**](https://docs.microsoft.com/en-us/windows/uwp/get-started/enable-your-device-for-development)

### Usage

#### Setup
1. Open Visual Studio
1. Select "Clone an existing repo", specifying this repo as the one to clone
1. Build, deploy, etc. (cleaning up any missing dependencies you need to install)

#### Customization

To add or edit a command, you need to make changes in the following two (2) files:

- [`CustomVoiceCommandDefinitions.xml`](CustomCortanaCommands/CustomVoiceCommandDefinitions.xml) &mdash;
  Add/Edit a `<Command>...</Command>` block that defines your command.
- [`CortanaFunctions.cs`](CustomCortanaCommands/CortanaFunctions.cs) &mdash; Add/Edit the command handler
  code according to the in-code examples.

### Tutorials

#### How to create commands

[![How to Create Custom Cortana Commands Tutorial](http://img.youtube.com/vi/0Wcn-ZK9mi4/0.jpg)](https://youtu.be/0Wcn-ZK9mi4 "Tutorial Video")

#### How to use this repository

[![How to Create Custom Cortana Commands Tutorial 2](http://img.youtube.com/vi/GICF03UAOcQ/0.jpg)](https://youtu.be/GICF03UAOcQ "Tutorial Video 2")
