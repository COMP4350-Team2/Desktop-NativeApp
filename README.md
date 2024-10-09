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


## Acceptance Tests

### Environment Setup
The team ran these tests on both the Production environment and a Mock environment. </br>
If you would like to run these test, use the Mock section.

#### Production
- Populated `AUTH0.env` and `BACKEND.env` as shown above.
- Run [Backend](https://github.com/COMP4350-Team2/Backend) locally.
- Run the app as above.

#### Mock
- Remove `AUTH0.env` and `BACKEND.env`.
- Run the app as above.

### User Story: Login/Registration
**Description:** As a user, I want to be a able to log in using an existing account or register a new account.

**Acceptance Criteria:** 
- Given that I am on the log in/registration page.
- I should be able log in with a previous account or register a new account.

#### Test Steps
- On the main page, click on log in button. Mock users log in immediately. Bypasses login, single user.
- Production environment users are taken to an `Auth0` log in and registration page.
- Register a new account with an email. It will sign you in.
- Log out. User taken to home page.
- Log in with the registered credientials.
- User taken to logged in page.


### User Story: Common Ingredients
**Description:** As a user, I want to have a list of commonly available ingredients I can choose from.

**Acceptance Criteria:** 
- Given that I’m a logged in user and on my profile
- When I am on my profile, I should be able to see my list of ingredients available
- Upon clicking on my list, I should be able to click the “add ingredient” button
- Then, I should be presented with a list of commonly available ingredients that I can add to my own list.

#### Test Steps
- After logging in, the homepage shows a list of ingredients. Alternatively click `All Ingredients` button.
- All ingredients are shown on the page. Each ingredient has a `+` button next to it. 
- Click the `+` button to add ingredient. Shows a message box saying this will be implemented in the future.
