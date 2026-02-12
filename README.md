# HDD Recovery Application

A comprehensive Windows Forms application for recovering files from hard drives with an advanced file preview feature and comprehensive test suite.

## Features

### File Recovery
- Recover files from source directories to destination locations
- Folder Structure Preservation: Maintain original folder hierarchy during recovery
- File Filtering: Support for file type filters (e.g., *.txt, *.jpg)
- Progress tracking during recovery
- Status updates and completion notifications
- Comprehensive logging of all operations
- Batch Operations: Recover multiple files efficiently

### File Preview Screen
The application includes a sophisticated file preview screen with the following features:

#### Explorer-Style Interface
- **Split-panel layout** with three sections:
  - **Left Panel**: Folder tree view showing directory hierarchy
  - **Right Top Panel**: Detailed file list view
  - **Right Bottom Panel**: Image preview with zoom controls

#### Folder Tree View
- Hierarchical display of folder structure
- Shows file count for each folder
- Expandable/collapsible nodes
- Context menu with "Open in Explorer" option
- Root node displays destination path

#### File List View
- Columns: Name, Type, Size, Date Modified, Status
- Support for sorting by clicking column headers
- Multiple file selection
- System icons for different file types
- Context menu with:
  - Open File
  - Open File Location
  - Copy Path
  - Properties

#### Image Preview
- Supports: jpg, jpeg, png, gif, bmp, ico
- Preview controls:
  - Fit to Window
  - Actual Size
  - Zoom In/Out
  - Mouse wheel zoom support
  - Drag to pan when zoomed
- Displays image dimensions and file size
- Shows placeholder message for non-image files

#### Additional Features
- **Search/Filter**: Real-time file filtering by name
- **Refresh**: Reload file list
- **Async Loading**: Non-blocking file scanning
- **Image Caching**: Improved performance for frequently viewed images
- **Status Bar**: Shows selected file count and total size
- **Double-click**: Open files with default application

## System Requirements
- .NET 8.0 or later
- Windows operating system
- Windows Forms support

## Project Structure

```
HDDRecovery/
├── Forms/                          # Windows Forms UI
│   ├── MainForm.cs                 # Main application form
│   ├── MainForm.Designer.cs
│   ├── MainForm.resx
│   ├── FilePreviewForm.cs          # File preview window
│   ├── FilePreviewForm.Designer.cs
│   └── FilePreviewForm.resx
├── Models/                         # Data models
│   ├── RecoveredFileInfo.cs        # File information model
│   └── RecoveryStatus.cs           # Recovery status enumeration
├── Services/                       # Business logic services
│   └── FilePreviewService.cs       # File preview operations
├── Helpers/                        # Helper utilities
│   └── FileHelper.cs               # File utility methods
├── Program.cs                      # Application entry point
├── HDDRecovery.csproj              # Project file
├── HDDRecovery/                    # Main application library (for testing)
│   ├── Models/                     # Data models
│   ├── Services/                   # Business logic services
│   └── Utilities/                  # Helper utilities
└── HDDRecovery.Tests/              # Comprehensive test suite
    ├── Models/                     # Model tests
    ├── Services/                   # Service tests
    ├── Utilities/                  # Utility tests
    ├── IntegrationTests/           # End-to-end tests
    └── TestHelpers/                # Test utilities
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code (optional)

### Building the Project

```bash
dotnet build
```

### Running the Application

```bash
dotnet run
```

## Usage

1. **Select Source Folder**: Click "Browse..." in the Source section and select the folder you want to recover files from.

2. **Select Destination Folder**: Click "Browse..." in the Destination section and choose where to save recovered files.

3. **Start Recovery**: Click "Start Recovery" to begin the file recovery process. Progress will be shown in the progress bar.

4. **View Recovered Files**: After recovery completes, click "View Recovered Files" to open the file preview screen.

5. **Browse Files**: 
   - Click folders in the tree view to see their contents
   - Use the search box to filter files
   - Click on image files to preview them
   - Right-click files or folders for additional options

6. **Image Preview**:
   - Select an image file to preview it
   - Use zoom controls to adjust the view
   - Scroll mouse wheel while hovering over image to zoom
   - Drag the image to pan when zoomed in

## Testing

This project includes a comprehensive unit test suite covering all functionality.

### Run All Tests

```bash
dotnet test
```

### Run Tests with Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Generate HTML Coverage Report

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=html /p:CoverletOutput=./coverage/
```

### Test Coverage

- **Current Coverage**: 90.1% line coverage, 92.3% branch coverage
- **Test Count**: 87 tests total (82 passing, 5 platform-specific skipped)
- **Framework**: xUnit
- **Assertions**: FluentAssertions
- **Mocking**: Moq

For more details about testing, see [HDDRecovery.Tests/README.md](HDDRecovery.Tests/README.md) and [HDDRecovery.Tests/COVERAGE.md](HDDRecovery.Tests/COVERAGE.md).

## Technical Details

### Models
- **RecoveredFileInfo**: Contains file metadata including path, size, dates, and recovery status
- **RecoveryStatus**: Enum with values Success, Skipped, Failed
- **RecoveryOptions**: Configuration for file recovery operations

### Services
- **FilePreviewService**: Handles file scanning, image loading, and folder tree building
  - Async file scanning
  - Image resizing
  - System icon retrieval
  - Folder hierarchy organization
- **FileRecoveryService**: Manages file recovery operations with progress tracking
- **LoggingService**: Thread-safe logging service

### Helpers/Utilities
- **FileHelper**: Utility methods for file operations
  - Image file detection
  - Safe image loading
  - Image resizing with aspect ratio
  - System file type icons
  - File size formatting
  - File attribute preservation

### Performance Optimizations
- Asynchronous file loading to prevent UI freezing
- Image caching (up to 50 images)
- Lazy loading for images
- Virtual mode support for large file lists
- Proper resource disposal

### Error Handling
- Graceful handling of missing files
- Corrupted image handling
- Access denied error management
- Error icons for failed items
- Detailed error messages

## License

This project is provided as-is for educational and recovery purposes.

## Contributing

Contributions are welcome! Please ensure all code follows the existing patterns and includes proper XML documentation. All new code should include comprehensive unit tests.
