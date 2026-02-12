# HDD Recovery Application

A comprehensive Windows Forms application for recovering files from hard drives with an advanced file preview feature.

## Features

### File Recovery
- Select source folder to recover from
- Select destination folder for recovered files
- Progress tracking during recovery
- Status updates and completion notifications

### File Preview Screen (NEW)
The application now includes a sophisticated file preview screen with the following features:

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

## Building the Project

```bash
dotnet build
```

## Running the Application

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

## Project Structure

```
HDDRecovery/
├── Models/
│   ├── RecoveredFileInfo.cs    # File information model
│   └── RecoveryStatus.cs       # Recovery status enumeration
├── Services/
│   └── FilePreviewService.cs   # File preview operations
├── Helpers/
│   └── FileHelper.cs           # File utility methods
├── Forms/
│   ├── MainForm.cs             # Main application form
│   ├── MainForm.Designer.cs
│   ├── MainForm.resx
│   ├── FilePreviewForm.cs      # File preview window
│   ├── FilePreviewForm.Designer.cs
│   └── FilePreviewForm.resx
├── Program.cs                  # Application entry point
└── HDDRecovery.csproj         # Project file
```

## Technical Details

### Models
- **RecoveredFileInfo**: Contains file metadata including path, size, dates, and recovery status
- **RecoveryStatus**: Enum with values Success, Skipped, Failed

### Services
- **FilePreviewService**: Handles file scanning, image loading, and folder tree building
  - Async file scanning
  - Image resizing
  - System icon retrieval
  - Folder hierarchy organization

### Helpers
- **FileHelper**: Utility methods for file operations
  - Image file detection
  - Safe image loading
  - Image resizing with aspect ratio
  - System file type icons
  - File size formatting

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

## Screenshots

*The file preview screen features a modern three-panel layout with folder navigation, file details, and image preview capabilities.*

## License

This project is provided as-is for educational and recovery purposes.

## Contributing

Contributions are welcome! Please ensure all code follows the existing patterns and includes proper XML documentation.