# HDD Recovery Application

A comprehensive file recovery application for .NET 8.0 with advanced features for recovering files from hard drives, preserving folder structures, and previewing recovered files.

## Features

- **File Recovery**: Recover files from source directories to destination locations
- **Folder Structure Preservation**: Maintain original folder hierarchy during recovery
- **File Filtering**: Support for file type filters (e.g., *.txt, *.jpg)
- **Progress Tracking**: Real-time progress updates during recovery operations
- **Logging**: Comprehensive logging of all operations
- **File Preview**: Preview recovered files including images
- **Batch Operations**: Recover multiple files efficiently

## Project Structure

```
HDDRecovery/
├── HDDRecovery/                    # Main application library
│   ├── Models/                     # Data models
│   ├── Services/                   # Business logic services
│   └── Utilities/                  # Helper utilities
├── HDDRecovery.Tests/              # Comprehensive test suite
│   ├── Models/                     # Model tests
│   ├── Services/                   # Service tests
│   ├── Utilities/                  # Utility tests
│   ├── IntegrationTests/           # End-to-end tests
│   └── TestHelpers/                # Test utilities
└── HDDRecovery.sln                 # Solution file
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code (optional)

### Building the Solution

```bash
dotnet build
```

### Running the Application

```bash
dotnet run --project HDDRecovery
```

## Testing

This project includes comprehensive unit tests covering all functionality.

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

- **Target**: 80%+ code coverage
- **Framework**: xUnit
- **Assertions**: FluentAssertions
- **Mocking**: Moq

For more details about testing, see [HDDRecovery.Tests/README.md](HDDRecovery.Tests/README.md).

## Usage Example

```csharp
using HDDRecovery.Models;
using HDDRecovery.Services;

// Initialize logging service
var logger = new LoggingService("Logs");

// Create recovery service
var recoveryService = new FileRecoveryService(logger);

// Configure recovery options
var options = new RecoveryOptions
{
    SourcePath = @"C:\SourceFolder",
    DestinationPath = @"D:\RecoveredFiles",
    PreserveFolderStructure = true,
    OverwriteExisting = false,
    FileFilter = "*.*"
};

// Subscribe to progress updates
recoveryService.ProgressChanged += (sender, progress) => 
    Console.WriteLine($"Progress: {progress}%");

// Perform recovery
var result = await recoveryService.RecoverFilesAsync(options);

Console.WriteLine($"Recovered: {result.SuccessfulFiles} files");
Console.WriteLine($"Failed: {result.FailedFiles} files");
Console.WriteLine($"Skipped: {result.SkippedFiles} files");
```

## Components

### Models

- **RecoveryOptions**: Configuration for recovery operations
- **RecoveredFileInfo**: Information about recovered files
- **FileStatus**: Status enumeration for file recovery

### Services

- **FileRecoveryService**: Main service for file recovery operations
- **LoggingService**: Handles logging of all operations
- **FilePreviewService**: Provides file preview functionality

### Utilities

- **FileHelper**: Static helper methods for file operations
  - Copy files with attribute preservation
  - Format file sizes
  - Get drive information
  - Check path accessibility

## Contributing

Contributions are welcome! Please ensure:

1. All tests pass
2. Code coverage remains above 80%
3. New features include corresponding tests
4. Code follows existing patterns and conventions

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on the GitHub repository.