# HDDRecovery.Tests

Comprehensive unit test suite for the HDDRecovery application.

## Overview

This test project provides extensive test coverage for all components of the HDDRecovery application, including:
- File recovery operations
- Logging services
- File preview functionality
- Utility functions
- Model classes

## Test Structure

```
HDDRecovery.Tests/
├── Models/
│   ├── RecoveryOptionsTests.cs         # Tests for RecoveryOptions model
│   └── RecoveredFileInfoTests.cs       # Tests for RecoveredFileInfo model
├── Services/
│   ├── FileRecoveryServiceTests.cs     # Tests for file recovery operations
│   ├── LoggingServiceTests.cs          # Tests for logging functionality
│   └── FilePreviewServiceTests.cs      # Tests for file preview features
├── Utilities/
│   └── FileHelperTests.cs              # Tests for file utility functions
├── IntegrationTests/
│   ├── FileRecoveryIntegrationTests.cs # End-to-end recovery tests
│   └── FilePreviewIntegrationTests.cs  # End-to-end preview tests
└── TestHelpers/
    └── TestFileGenerator.cs            # Helper for generating test data
```

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Tests with Verbose Output
```bash
dotnet test --verbosity normal
```

### Run Specific Test Class
```bash
dotnet test --filter "FullyQualifiedName~FileHelperTests"
```

### Run Tests in Visual Studio
1. Open the solution in Visual Studio
2. Open Test Explorer (Test > Test Explorer)
3. Click "Run All" to execute all tests

## Code Coverage

### Generate Code Coverage Report
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Generate HTML Coverage Report
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=html /p:CoverletOutput=./coverage/
```

The HTML report will be generated in the `coverage` directory.

### Coverage Goals
- Target: **80%+ code coverage**
- Current coverage includes all public methods and key code paths

## Test Framework and Libraries

- **xUnit**: Testing framework
- **FluentAssertions**: Readable assertion syntax
- **Moq**: Mocking framework for dependencies
- **coverlet.collector**: Code coverage collection

## Test Categories

### Unit Tests
- Test individual methods in isolation
- Mock external dependencies
- Fast execution
- Follow AAA pattern (Arrange, Act, Assert)

### Integration Tests
- Test complete workflows
- Use real file system operations
- Verify end-to-end functionality

## Best Practices

1. **Test Naming**: Use descriptive names following the pattern `MethodName_Scenario_ExpectedBehavior`
2. **Test Isolation**: Each test is independent and can run in any order
3. **Cleanup**: Tests clean up temporary files using the `IDisposable` pattern
4. **Assertions**: Use FluentAssertions for readable test assertions
5. **Async Testing**: Properly test async methods with `async Task` signatures

## Adding New Tests

When adding new features to the main application:

1. Create corresponding test class in the appropriate folder
2. Follow the existing naming conventions
3. Include both success and failure scenarios
4. Test edge cases and boundary conditions
5. Update this README if adding new test categories

## Test Helpers

### TestFileGenerator
Provides utility methods for creating test files and folders:

```csharp
// Create a temporary test directory
var testDir = TestFileGenerator.CreateTestDirectory();

// Create test files
TestFileGenerator.CreateTestFiles(testDir, count: 10);

// Create nested folder structure
TestFileGenerator.CreateNestedFolders(testDir, depth: 3);

// Create test images
TestFileGenerator.CreateTestImage(imagePath, width: 800, height: 600);

// Cleanup
TestFileGenerator.Cleanup(testDir);
```

## Continuous Integration

Tests are automatically run on every commit. Build will fail if:
- Any test fails
- Code coverage drops below threshold
- Test warnings are detected

## Troubleshooting

### Tests Fail on Non-Windows Platforms
Some image and icon tests may behave differently on non-Windows platforms due to platform-specific APIs. These tests are designed to handle such cases gracefully.

### Cleanup Issues
If tests leave behind temporary files, check:
1. All tests properly implement `IDisposable`
2. `TestFileGenerator.Cleanup()` is called in `Dispose()`
3. File handles are properly closed before cleanup

### Slow Test Execution
- Use `dotnet test --logger "console;verbosity=detailed"` to identify slow tests
- Integration tests may take longer due to file I/O operations
- Consider running unit tests separately from integration tests

## Contributing

When contributing tests:
1. Ensure all tests pass locally before committing
2. Follow the established patterns and conventions
3. Add meaningful test descriptions
4. Keep test code maintainable and readable
5. Update coverage reports if adding significant new features

## Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/moq/moq4)
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
