# This is the README for the Desktop-WebApp

## Technologies To Be Used
- **Language:** C#
- **Framework:** WPF.NET


## Links To Other Repos
[Link to Mobile Webapp](https://github.com/COMP4350-Team2/Mobile-WebApp) <br/>
[Link to Backend](https://github.com/COMP4350-Team2/Backend)

## Requirements

### .NET Framework
This front end requires `.NET` Framework to be downloaded. [Download link.](https://dotnet.microsoft.com/en-us/download)

Type `dotnet --version` in terminal to check version on your system.

## Running Instructions

### Make & Populate .env Files
- Make a file file called `AUTH0.env` in the `./src/Auth0` directory. Below is the expected format:

    ```
    AUTH0_DOMAIN=
    AUTH0_API_IDENTIFIER=
    AUTH0_CLIENT_ID=
    AUTH0_CALLBACK_URL=
    AUTH0_AUDIENCE=
    ```
- Make a file file called `BACKEND.env` in the `./src/Backend` directory. Below is the expected format:

    ```
    BACKEND_URL=
    CREATE_USER=
    ALL_INGREDIENTS=
    ```   

- Not having these files will run a Mock version of the system.

### Restore (Optional)
- Navigate to `src`
- Run `dotnet restore`

### Build
- Navigate to `src`
- Run `dotnet build` (indicates successful build)

### Run
- Navigate to `src`
- Run `dotnet run` 


