# HDD File Recovery Application

A powerful and user-friendly Windows application for recovering files from external hard drives (HDDs), USB drives, and other storage devices. Built with C# and WinForms, this application provides an intuitive interface for file recovery operations while preserving file structure and metadata.

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
![License](https://img.shields.io/badge/license-MIT-green)

## 📋 Table of Contents

- [Features](#features)
- [System Requirements](#system-requirements)
- [Installation](#installation)
- [Usage Guide](#usage-guide)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Building from Source](#building-from-source)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## ✨ Features

### Core Functionality
- **Complete File Recovery**: Recover files from external HDDs, USB drives, and other storage devices
- **Folder Structure Preservation**: Maintain the original directory hierarchy of recovered files
- **Metadata Preservation**: Preserve file attributes including creation date, modification date, and file attributes
- **Selective File Filtering**: Filter files by type or extension (images, documents, audio, video, or all files)
- **Overwrite Control**: Choose whether to overwrite existing files at the destination

### User Interface
- **Intuitive Design**: Clean, modern interface suitable for both technical and non-technical users
- **Drive Information Display**: View drive name, total size, and available space for the source drive
- **Real-time Progress Tracking**: Live progress bar with percentage completion
- **File Count Display**: See how many files have been recovered out of the total
- **Current File Indicator**: View the name of the file currently being processed
- **Time Estimation**: Estimated time remaining for the recovery operation
- **Status Logging**: Comprehensive log view showing all operations and errors

### Technical Features
- **Asynchronous Operations**: Non-blocking UI with async/await pattern for responsive experience
- **Cancellation Support**: Cancel ongoing recovery operations safely
- **Robust Error Handling**: Graceful handling of file access errors, permissions issues, and locked files
- **Comprehensive Logging**: Detailed logs saved to application data folder for troubleshooting
- **Long Path Support**: Handles file paths longer than 260 characters (Windows limitation)

## 💻 System Requirements

### Operating System
- Windows 10 (Version 1809 or later)
- Windows 11
- Windows Server 2019 or later

### Software Requirements
- .NET 8.0 Runtime or later
  - Download from: https://dotnet.microsoft.com/download/dotnet/8.0

### Hardware Requirements
- **Processor**: 1 GHz or faster
- **RAM**: Minimum 512 MB (1 GB recommended for large file operations)
- **Disk Space**: 50 MB for application installation
- **Additional Space**: Sufficient free space on destination drive for recovered files

### Permissions
- Read access to source drive/folder
- Write access to destination folder
- Administrator privileges may be required for some system drives

## 📥 Installation

### Option 1: Download Pre-built Release (Recommended)
1. Go to the [Releases](../../releases) page
2. Download the latest `HDDRecovery.zip` file
3. Extract the ZIP file to your desired location
4. Run `HDDRecovery.exe`

### Option 2: Build from Source
See [Building from Source](#building-from-source) section below.

### First Run
1. If you don't have .NET 8.0 Runtime installed, you'll be prompted to download it
2. Allow the application through Windows Firewall if prompted (not typically required)
3. The application will create a log directory in `%APPDATA%\HDDRecovery\Logs`

## 📖 Usage Guide

### Step-by-Step Instructions

#### 1. Select Source Path
- Click the **"Browse..."** button in the "Source Path" section
- Navigate to and select the drive or folder containing files you want to recover
- The application will display drive information (name, total size, free space)

![Source Selection](docs/screenshots/source-selection.png)
*Screenshot placeholder - Select the source drive or folder*

#### 2. Select Destination Path
- Click the **"Browse..."** button in the "Destination Path" section
- Navigate to and select the folder where you want to save recovered files
- You can create a new folder if needed

![Destination Selection](docs/screenshots/destination-selection.png)
*Screenshot placeholder - Select the destination folder*

#### 3. Configure Recovery Options

**Preserve Folder Structure** (Checked by default)
- When checked: Maintains the original directory structure from the source
- When unchecked: All files are copied to the destination root folder (flattened)

**Overwrite Existing Files**
- When checked: Replaces files at the destination if they already exist
- When unchecked: Skips files that already exist at the destination

**File Filter** (Dropdown)
- `*.*` - All files (default)
- `*.jpg;*.jpeg;*.png;*.gif;*.bmp` - Image files only
- `*.doc;*.docx;*.pdf;*.txt` - Document files only
- `*.mp3;*.wav;*.flac` - Audio files only
- `*.mp4;*.avi;*.mkv;*.mov` - Video files only

![Recovery Options](docs/screenshots/recovery-options.png)
*Screenshot placeholder - Configure recovery options*

#### 4. Start Recovery
- Click the **"Start Recovery"** button
- Review the confirmation dialog showing all settings
- Click **"Yes"** to begin the recovery process

#### 5. Monitor Progress
The Progress section displays:
- **Progress Bar**: Visual representation of completion percentage
- **Percentage**: Numeric completion percentage
- **File Count**: "X/Y files recovered" showing progress
- **Current File**: Name of the file currently being processed
- **Time Remaining**: Estimated time to completion

![Recovery in Progress](docs/screenshots/recovery-progress.png)
*Screenshot placeholder - Monitor recovery progress*

#### 6. View Status Log
The Status Log section shows:
- Timestamp for each operation
- Files successfully recovered
- Files that were skipped
- Any errors encountered
- Final summary upon completion

#### 7. Cancel Operation (Optional)
- Click **"Cancel Recovery"** button during operation
- Confirm cancellation when prompted
- Already recovered files remain at the destination

#### 8. Clear and Start New Recovery
- Click **"Clear"** button to reset all selections
- The form will be cleared and ready for a new recovery operation

### Best Practices

1. **Before Starting Recovery**
   - Ensure you have sufficient disk space on the destination drive
   - Verify the source drive is properly connected and accessible
   - Close any applications that might be accessing files on the source drive

2. **During Recovery**
   - Don't disconnect the source drive
   - Avoid putting the computer to sleep
   - Monitor the Status Log for any errors

3. **After Recovery**
   - Review the completion summary
   - Check the log file for detailed information
   - Verify recovered files at the destination
   - Safely eject the source drive if it's external

## 🏗️ Architecture

### Technical Overview

The application follows a clean separation of concerns with distinct layers:

#### Presentation Layer (UI)
- **MainForm.cs**: Main user interface and event handlers
- **MainForm.Designer.cs**: Auto-generated UI component definitions
- Implements responsive async/await patterns to keep UI responsive

#### Business Logic Layer
- **FileRecoveryService.cs**: Core recovery logic with progress reporting
- **LoggingService.cs**: Comprehensive logging functionality
- Event-driven architecture for progress updates

#### Data/Model Layer
- **RecoveryOptions.cs**: Configuration model for recovery operations
- **RecoveryResult.cs**: Result model containing operation statistics
- **RecoveryProgressEventArgs.cs**: Progress event data

#### Utilities Layer
- **FileHelper.cs**: Helper methods for file operations, size formatting, and drive information

### Key Design Patterns

1. **Service Pattern**: Business logic encapsulated in service classes
2. **Event-Driven**: Progress updates via events to decouple UI from business logic
3. **Async/Await**: All I/O operations are asynchronous for responsiveness
4. **Dependency Injection**: Services injected into forms for testability

### File Recovery Algorithm

```
1. Validate source and destination paths
2. Scan source directory recursively
3. Apply file filter to get matching files
4. For each file:
   a. Check if operation is cancelled
   b. Determine destination path (preserve structure or flatten)
   c. Check if file should be copied (based on overwrite setting)
   d. Create destination directory if needed
   e. Copy file with all attributes
   f. Update progress and log status
   g. Handle any errors gracefully
5. Generate final statistics and log results
```

### Error Handling Strategy

- **File-level errors**: Logged and counted, but operation continues
- **Critical errors**: Operation halts with user notification
- **User cancellation**: Graceful shutdown with partial results preserved
- **All errors logged**: Detailed logging for troubleshooting

## 📁 Project Structure

```
HDDRecovery/
├── HDDRecovery.sln                    # Visual Studio solution file
├── README.md                          # This file
├── LICENSE                            # MIT License
├── .gitignore                         # Git ignore rules
│
└── HDDRecovery/                       # Main project directory
    ├── HDDRecovery.csproj             # Project file
    ├── Program.cs                     # Application entry point
    │
    ├── MainForm.cs                    # Main UI form (code-behind)
    ├── MainForm.Designer.cs           # Main UI form (designer)
    ├── MainForm.resx                  # Main UI form (resources)
    │
    ├── Models/                        # Data models
    │   └── RecoveryOptions.cs         # Recovery configuration model
    │
    ├── Services/                      # Business logic services
    │   ├── FileRecoveryService.cs     # Core recovery logic
    │   └── LoggingService.cs          # Logging functionality
    │
    └── Utilities/                     # Helper utilities
        └── FileHelper.cs              # File operation helpers
```

### Key Classes and Responsibilities

#### Program.cs
- Application entry point
- Initializes the Windows Forms application
- Launches MainForm

#### MainForm.cs
- Main user interface
- Handles all user interactions
- Coordinates recovery operations
- Updates UI based on progress events

#### RecoveryOptions.cs
- Stores user-selected recovery configuration
- Source and destination paths
- Boolean flags for preservation and overwrite options
- File filter pattern

#### FileRecoveryService.cs
- Scans source directory for files
- Performs file copy operations
- Preserves file attributes and metadata
- Reports progress via events
- Handles cancellation
- Generates recovery statistics

#### LoggingService.cs
- Writes timestamped log entries
- Supports multiple log levels (Info, Warning, Error)
- Thread-safe file writing
- Stores logs in application data folder

#### FileHelper.cs
- Copies files while preserving attributes
- Formats file sizes for display
- Retrieves drive information
- Validates path accessibility

## 🔨 Building from Source

### Prerequisites
1. **Visual Studio 2022** (Community, Professional, or Enterprise)
   - Download from: https://visualstudio.microsoft.com/
   - Workload required: ".NET Desktop Development"

2. **.NET 8.0 SDK** (usually installed with Visual Studio)
   - Verify installation: Open Command Prompt and run `dotnet --version`

### Build Steps

#### Using Visual Studio
1. Clone or download the repository:
   ```bash
   git clone https://github.com/damodaranrr/HDDRecovery.git
   ```

2. Open `HDDRecovery.sln` in Visual Studio 2022

3. Restore NuGet packages:
   - Visual Studio should automatically restore packages
   - Or manually: Right-click solution → "Restore NuGet Packages"

4. Build the solution:
   - Press `Ctrl+Shift+B` or
   - Menu: Build → Build Solution

5. Run the application:
   - Press `F5` to run with debugging, or
   - Press `Ctrl+F5` to run without debugging

#### Using Command Line
1. Clone or download the repository:
   ```bash
   git clone https://github.com/damodaranrr/HDDRecovery.git
   cd HDDRecovery
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build --configuration Release
   ```

4. Run the application:
   ```bash
   dotnet run --project HDDRecovery\HDDRecovery.csproj
   ```

### Build Output
- Debug build: `HDDRecovery\bin\Debug\net8.0-windows\`
- Release build: `HDDRecovery\bin\Release\net8.0-windows\`

### Creating a Release Package
1. Build in Release mode
2. Navigate to the Release output folder
3. Copy all files to a distribution folder
4. Create a ZIP file for distribution

## 🔧 Troubleshooting

### Common Issues and Solutions

#### Issue: Application won't start
**Solution:**
- Ensure .NET 8.0 Runtime is installed
- Check Windows Event Viewer for error details
- Run as Administrator if accessing system drives

#### Issue: "Access Denied" errors during recovery
**Solution:**
- Verify you have read permissions on source files
- Verify you have write permissions on destination folder
- Run the application as Administrator for system folders
- Close any applications that might be locking files

#### Issue: Some files are being skipped
**Solution:**
- Check if "Overwrite existing files" is unchecked (files may already exist)
- Review the Status Log for specific error messages
- Check the detailed log file in `%APPDATA%\HDDRecovery\Logs\`
- Verify source files are not locked by another process

#### Issue: Recovery is very slow
**Solution:**
- Large files or many small files can slow down the process
- USB 2.0 drives are slower than USB 3.0 or internal drives
- Network drives are typically slower than local drives
- Antivirus software may slow down file operations - consider temporarily disabling
- Check if the source or destination drive is nearly full

#### Issue: Application freezes or becomes unresponsive
**Solution:**
- This shouldn't happen due to async operations, but if it does:
- Wait a moment as large file operations may take time
- Check Task Manager to see if the process is actually running
- Force close and restart the application
- Report the issue with log files

#### Issue: Can't find log files
**Location:**
- Windows 10/11: `C:\Users\[YourUsername]\AppData\Roaming\HDDRecovery\Logs\`
- Access via Run dialog: Press `Win+R`, type `%APPDATA%\HDDRecovery\Logs`, press Enter

#### Issue: Drive information not showing
**Solution:**
- Ensure the selected source is a valid drive (not a network path or special folder)
- Try selecting a different folder on the same drive
- Drive information only works for local drives with drive letters

### Getting Help

If you encounter issues not listed here:

1. **Check the logs**: Review the detailed log file in the AppData folder
2. **Search Issues**: Check [GitHub Issues](../../issues) for similar problems
3. **Create an Issue**: If not found, create a new issue with:
   - Detailed description of the problem
   - Steps to reproduce
   - Log file contents
   - System information (Windows version, .NET version)

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

### Reporting Bugs
1. Check if the bug has already been reported in [Issues](../../issues)
2. If not, create a new issue with:
   - Clear title and description
   - Steps to reproduce
   - Expected vs actual behavior
   - Log files and screenshots if applicable
   - System information

### Suggesting Enhancements
1. Check existing [Issues](../../issues) for similar suggestions
2. Create a new issue describing:
   - The feature and its benefits
   - Possible implementation approach
   - Use cases

### Pull Requests
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Make your changes
4. Follow C# coding conventions
5. Add XML documentation to public methods
6. Test thoroughly
7. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
8. Push to the branch (`git push origin feature/AmazingFeature`)
9. Open a Pull Request

### Code Guidelines
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Write clean, readable code
- Test your changes thoroughly

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### MIT License Summary
- ✅ Commercial use
- ✅ Modification
- ✅ Distribution
- ✅ Private use
- ❌ Liability
- ❌ Warranty

## 📧 Contact

**Project Maintainer**: Damodaran RR

- GitHub: [@damodaranrr](https://github.com/damodaranrr)
- Project Link: [https://github.com/damodaranrr/HDDRecovery](https://github.com/damodaranrr/HDDRecovery)

### Support

- **Bug Reports**: [GitHub Issues](../../issues)
- **Feature Requests**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)

---

## 🙏 Acknowledgments

- Built with [.NET 8.0](https://dotnet.microsoft.com/)
- Windows Forms for the UI framework
- Community feedback and contributions

---

**Made with ❤️ for the data recovery community**

*Last Updated: 2024*