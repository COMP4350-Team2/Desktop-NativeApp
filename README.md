# This is the README for the Desktop-WebApp

## Technologies To Be Used
- **Language:** C#
- **Framework:** WPF.NET


## Links To Other Repos
[Link to Mobile Webapp](https://github.com/COMP4350-Team2/Mobile-WebApp) <br/>
[Link to Backend](https://github.com/COMP4350-Team2/Backend)

## Requirements

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

### Environment Setup
The team ran these tests on both the Production environment and a Mock environment. </br>

#### Production
- Populated `AUTH0.env` and `BACKEND.env` as shown above.
- Run [Backend](https://github.com/COMP4350-Team2/Backend) locally.
- Run the app as above.

#### Mock
- Remove `AUTH0.env` and `BACKEND.env`.
- No need to run the backend. Internal mock is used.
- Run the app as above.

### User Story: Login/Registration
**Description:** As a user, I want to be a able to log in using an existing account or register a new account.

**Acceptance Criteria:** 
- Given that I am on the log in/registration page
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


### User Story: Ingredient lists board
**Description:** As a user, I want to have a place to view all my lists.

**Acceptance Criteria:** 
- Given that I’m a logged in user
- When I click on my profile and click My Lists
- Then I should be able to see, add or remove all my lists with all the ingredients in them

#### Test Steps
- After logging in, click on `My Lists` button.
- It shows a list of all the user's lists.
- Each list has an expand button which can be clicked to show the contents of that list.



### User Story: Remove Ingredients from List
**Description:** As a user, i should be able to remove an ingredient from my lists.

**Acceptance Criteria:** 
- Given that I am logged in and on My Lists page
- And I navigate to an ingredient on a list
- And I click on delete ingredient
- Then the ingredient should be removed from that list

#### Test Steps
- On the `My Lists` page, click on a list to expand it. It shows ingredients in that list.
- Click on the delete button next on the ingredient.
- That should remove that ingredient from that list.


### User Story: Add Ingredient To List
**Description:** As a user, I want to search ingredients and add them to one of my lists.

**Acceptance Criteria:** 
- Given that I’m a logged in user and on my profile
- I should be able to see a list of my own ingredients that I can open
- When I click the “add ingredients” button upon opening my list
- Then I should be presented with a list of commonly available ingredients that I can search through using a search bar.

#### Test Steps
- After logging in, the homepage shows a list of ingredients. Alternatively click `All Ingredients` button.
- All ingredients are shown on the page with a search bar. Each ingredient has a `+` button next to it. 
- Click the `+` button to add ingredient. I will then be able to specify an amount, the unit and the list to add to.
- After adding, click `My Lists` button to see all lists. Ingredient should be in the list specified.
- If that ingredient did not exist in that list, a new ingredient should be added.
- If the ingredient with the same name and unit existed in the list already, the amount of that ingredient should be increased.
- If the ingredient with the same name but different unit existed in the list already, a new ingredient should be added.
