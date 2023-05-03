# dotnet-minimal-api-examples
API examples built in .NET to be used as a supplement to an article on all things REST and HTTP web API.

## Concepts
- Consistent method naming
- Consistent response structure
- Utilise HTTP ations
- Provide meaningful responses
- Avoid side effects
- Validate data
- Implement versioning
- Provide a specification

## Versioning
Different API versions have been implemented to showcase good and bad practices for each concept.

## Getting Started
### Prerequisites
- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Visual Studio Code](https://code.visualstudio.com/)

### Running the application
#### VSCode
- Open the root of the repository in VSCode
- Navigate to the 'Run and Debug' menu (CTRL + SHIFT + D)
- Ensure that 'Debug Application' is shown in the drop down to the right of the green 'Start Debugging' button
- Click the 'Start Debugging' button (F5)

#### Terminal
- Build the application
  - `dotnet build ./Application/Application.csproj`
- Run the application
  - `dotnet ./Application/bin/Debug/net7.0/Application.dll`

### Open API documentation
Navigate to `http://localhost:5050/swagger` (or whatever port the application is currently running on)
