# Cupboard Desktop NativeApp Frontend
This repository contains the code for the Cupboard App's Desktop Native front-end. It was developed as a group project for the University of Manitoba Fall 2024 COMP 4350 class by Team 2 - Teacup.
<br>By running this code, you should be able to access Cupboard's native app from your computer (on localhost - see below). <br>
<br>The link to our other repositories can be found below:
- [Link to Mobile Webapp](https://github.com/COMP4350-Team2/Mobile-WebApp) <br/>
- [Link to Backend](https://github.com/COMP4350-Team2/Backend)

## Table of Contents
- [Background](#background)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Acceptance Tests](#acceptance-tests)
- [Contributors](#contributors)
- [License](#license)

## Background
The Cupboard App was developed to be the ultimate kitchen companian that takes the hassle out of meal planning and grocery management.

## Technologies Used

- **Languages:** C#
- **Framework:** WPF.NET

## Getting Started

### Requirements
In order to successfully run this app locally, the following requirements must be fulfilled.
### Platform
This is a Windows Native Application. To be run on Windows.

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
    MY_LISTS=
    ADD_INGREDIENT=
    GET_MEASUREMENTS=
    REMOVE_INGREDIENT=
    SET_INGREDIENT=
    DELETE_LIST=
    CREATE_LIST=
    RENAME_LIST=
    MOVE_INGREDIENT=
    CREATE_CUSTOM_ING=
    DELETE_CUSTOM_ING=
    GET_RECIPES=
    CREATE_RECIPE=
    DELETE_RECIPE=
    ADD_ING_RECIPE=
    DELETE_ING_RECIPE=
    ADD_STEP_RECIPE=
    DELETE_STEP_RECIPE=
    REFRESH-TOKEN=
    ```   

- Not having these files will run a Mock version of the system.
- Contact one of the team members for `.env` files if needed.

### Restore (Optional)
- Navigate to `src`
- Run `dotnet restore`

### Build
- Navigate to `src`
- Run `dotnet build` (indicates successful build)

### Run
- Navigate to `src`
- Run `dotnet run` 


## Acceptance Tests
The acceptance tests for the user stories covered can be found in the following document <br>
[Acceptance Tests](https://github.com/COMP4350-Team2/Desktop-NativeApp/blob/72ed87e7874b3b251d197723128221524b911904/ACCEPTANCE_TESTS.md)

## Contributors
<a href="https://github.com/COMP4350-Team2/Desktop-NativeApp/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=COMP4350-Team2/Desktop-NativeApp" />
</a>

Made with [contrib.rocks](https://contrib.rocks).

## License
[GNU GENERAL PUBLIC LICENSE](LICENSE) © Teacup (COMP4350-Team2)