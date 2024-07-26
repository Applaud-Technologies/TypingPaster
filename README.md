# TypeyClip

TypeyClip is a lightweight, customizable clipboard manager that simulates typing out the contents of your clipboard. It's designed for scenarios where traditional pasting methods are restricted or when you want to simulate manual typing.

## Features

- **Simulated Typing**: Automatically "types" out the content of your clipboard.
- **Adjustable Typing Speed**: Customize the typing speed to match your preferences or requirements.
- **Dynamic Speed Adjustment**: Increase or decrease typing speed on the fly using hotkeys.
- **Special Character Handling**: Correctly handles newlines, tabs, and other special characters.
- **Text Cleanup**: Automatically removes redundant whitespace and normalizes line breaks.
- **Global Hotkeys**: Easy-to-use hotkeys for pasting, quitting the application, and adjusting speed.
- **Cross-Application Compatibility**: Works across various applications, including complex IDEs.

## Requirements

- .NET Core (version compatible with your development environment)
- Windows operating system

## Installation

1. Clone this repository or download the source code.
2. Open a terminal and navigate to the project directory.
3. Run the following command to restore the required packages:
   ```
   dotnet restore
   ```

## Usage

1. Build and run the application using:
   ```
   dotnet run
   ```
2. When prompted, enter your desired typing speed in Words Per Minute (WPM) or press Enter to use the default speed (60 WPM).
3. The application will run in the background, waiting for hotkey inputs.

### Hotkeys

- **Ctrl+Alt+V**: Activate TypeyClip to paste by simulating typing.
- **Ctrl+Alt+Q**: Quit the application.
- **Ctrl+Alt+Up Arrow**: Increase typing speed by 10 WPM.
- **Ctrl+Alt+Down Arrow**: Decrease typing speed by 10 WPM.

## How It Works

TypeyClip uses the Windows Input Simulator to mimic keyboard input. When activated, it retrieves the text from your clipboard, performs some cleanup (removing redundant whitespace and normalizing line breaks), and then simulates typing each character, including special characters like newlines and tabs.

## Customization

You can modify the source code to change the hotkeys, adjust the speed increment, or add additional features as needed. The main logic is contained within the `Program` class in the `TypingPaster` namespace.

## Contributing

Contributions, issues, and feature requests are welcome. Feel free to check [issues page](link-to-your-issues-page) if you want to contribute.


## Acknowledgements

- [TextCopy](https://github.com/CopyText/TextCopy) for clipboard access
- [InputSimulator](https://github.com/michaelnoonan/inputsimulator) for simulating keyboard input

