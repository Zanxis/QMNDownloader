# QMN Downloader

```txt
   ___  __  __ _   _   ____                      _                 _
  / _ \|  \/  | \ | | |  _ \  _____      ___ __ | | ___   __ _  __| | ___ _ __
 | | | | |\/| |  \| | | | | |/ _ \ \ /\ / / '_ \| |/ _ \ / _` |/ _` |/ _ \ '__|
 | |_| | |  | | |\  | | |_| | (_) \ V  V /| | | | | (_) | (_| | (_| |  __/ |
  \__\_\_|  |_|_| \_| |____/ \___/ \_/\_/ |_| |_|_|\___/ \__,_|\__,_|\___|_|
```

## Introduction

QMN Downloader is an automated CLI tool that allows users to download content from the QueerMeNow blog website. With it, you can select any blog URL you desire and download all the main media content (including images and videos), saving them locally on your device in an organized, quick, and easy manner.

## Disclaimer

Due to the nature of this application being geared towards NSFW content, I declare that I am not responsible for anything that happens to you while using this tool. By downloading content, you are doing so at your own risk. If you are underage, please leave this repository.

Be aware that there may be issues that I will attempt to resolve gradually. Therefore, keep in mind the risks involved in using this application.

Although I strive to make it safe and responsible, it is important to mention this. This project is intended solely for the author's experience and studies, aiming to provide a useful tool for those seeking a solution that performs this function.

Thank you in advance for your understanding.

## Installation

To use the QMN Downloader tool, you need to install it in your environment. You can access the latest release page [by clicking here](https://github.com/Zanxis/QMNDownloader/releases) and download the latest portable version for your operating system.

After downloading the compressed file, choose a preferred location and extract it. When you open the directory, you should find the application's executable.

### Requirements

For the program to function correctly on your device, you need to have the [.NET Runtime 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed in your environment; without it, the application will not be able to initialize.

## Usage

To use the application, open a terminal of your choice (CMD, PowerShell, etc.) in the directory where the program's executable is located.

Then, run the application as follows:

```bash
./QMNDownloader
```

Remember to provide all the necessary arguments for everything to work correctly.

> [!NOTE]  
> You can use `./QMNDownloader --help` to see a list of all available arguments.

## Manual

Below, you will find all the arguments that can be used in the application, serving as a quick help guide for new users.

### Url (required)

#### Description

The URL of the blog page that will be downloaded. Make sure to pass a complete URL as the argument's value.

#### Example

```bash
./QMNDownloader --url "https://www.queermenow.net/blog/{name}/"
```

## Cloning and Building the Project

For those who wish to clone and compile the QMN Downloader project, follow the detailed steps below:

### Cloning the Repository

First, you need to clone the GitHub repository to your local environment. Open a terminal and execute the following command:

```bash
git clone https://github.com/Zanxis/QMNDownloader.git
```

This will create a local copy of the repository on your device.

### Navigating to the Project Directory

After cloning the repository, navigate to the project directory:

```bash
cd QMNDownloader
```

### Installing Dependencies

Before compiling the project, ensure that you have all the necessary dependencies installed. You will need .NET SDK 8.0.0 or higher. To install the .NET SDK, follow the instructions provided on the [official .NET page](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Verify that the .NET SDK is correctly installed by running the command:

```bash
dotnet --version
```

The command should return the installed version of the .NET SDK.

### Building the Project

With all dependencies installed, you can compile the project by running the following command in the root directory of the project:

```bash
dotnet build
```

This command will compile the project and generate the binaries in the `bin` directory.

You can also use the following command to compile and start the application (remember to include the necessary arguments if desired):

```bash
dotnet run --project QMNDownloader
```

### Running the Application

After compiling the project, you can navigate to the output directory and run the application directly with the following command:

```bash
./QMNDownloader
```

## Contributing

Feel free to explore and contribute to the project in any way you like! Create an issue if the application malfunctions or contribute by bringing a PR (pull request) to the project. Contributions will be reviewed and tested for approval. You can find more details on how to contribute [by clicking here](./CONTRIBUTING.md).

> [!IMPORTANT]  
> Make sure you have the [.NET SDK 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed in your environment.

## Credits

This tool was developed by the programmer [@Zanxis](https://github.com/Zanxis) as a theoretical and experimental study.

## License

The repository is licensed under the MIT license. See more details [by clicking here](./LICENSE).
