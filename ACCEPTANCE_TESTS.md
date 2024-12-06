# Acceptance Tests

## Environment Setup
The team ran these tests on both the Production environment and a Mock environment. </br>

### Production
- Populated `AUTH0.env` and `BACKEND.env` as shown above.
- Run [Backend](https://github.com/COMP4350-Team2/Backend) locally.
- Run the app as above.

### Mock
- Remove `AUTH0.env` and `BACKEND.env`.
- No need to run the backend. Internal mock is used.
- Run the app as above.

## User Story: Login/Registration
**Description:** As a user, I want to be a able to log in using an existing account or register a new account.

**Acceptance Criteria:** 
- Given that I am on the log in/registration page
- I should be able log in with a previous account or register a new account.

### Test Steps
- On the main page, click on log in button. Mock users log in immediately. Bypasses login, single user.
- Production environment users are taken to an `Auth0` log in and registration page.
- Register a new account with an email. It will sign you in.
- Log out. User taken to home page.
- Log in with the registered credientials.
- User taken to logged in page.


## User Story: Common Ingredients
**Description:** As a user, I want to have a list of commonly available ingredients I can choose from.

**Acceptance Criteria:** 
- Given that I’m a logged in user and on my profile
- When I am on my profile, I should be able to see my list of ingredients available
- Upon clicking on my list, I should be able to click the “add ingredient” button
- Then, I should be presented with a list of commonly available ingredients that I can add to my own list.

### Test Steps
- After logging in, the homepage shows a list of ingredients. Alternatively click `All Ingredients` button.
- All ingredients are shown on the page. Each ingredient has a `+` button next to it. 
- Click the `+` button to add ingredient. Shows a message box saying this will be implemented in the future.


## User Story: Ingredient lists board
**Description:** As a user, I want to have a place to view all my lists.

**Acceptance Criteria:** 
- Given that I’m a logged in user
- When I click on my profile and click My Lists
- Then I should be able to see, add or remove all my lists with all the ingredients in them

### Test Steps
- After logging in, click on `My Lists` button.
- It shows a list of all the user's lists.
- Each list has an expand button which can be clicked to show the contents of that list.



## User Story: Remove Ingredients from List
**Description:** As a user, i should be able to remove an ingredient from my lists.

**Acceptance Criteria:** 
- Given that I am logged in and on My Lists page
- And I navigate to an ingredient on a list
- And I click on delete ingredient
- Then the ingredient should be removed from that list

### Test Steps
- On the `My Lists` page, click on a list to expand it. It shows ingredients in that list.
- Click on the delete button next on the ingredient.
- That should remove that ingredient from that list.


## User Story: Add Ingredient To List
**Description:** As a user, I want to search ingredients and add them to one of my lists.

**Acceptance Criteria:** 
- Given that I’m a logged in user and on my profile
- I should be able to see a list of my own ingredients that I can open
- When I click the “add ingredients” button upon opening my list
- Then I should be presented with a list of commonly available ingredients that I can search through using a search bar.

### Test Steps
- After logging in, the homepage shows a list of ingredients. Alternatively click `All Ingredients` button.
- All ingredients are shown on the page with a search bar. Each ingredient has a `+` button next to it. 
- Click the `+` button to add ingredient. I will then be able to specify an amount, the unit and the list to add to.
- After adding, click `My Lists` button to see all lists. Ingredient should be in the list specified.
- If that ingredient did not exist in that list, a new ingredient should be added.
- If the ingredient with the same name and unit existed in the list already, the amount of that ingredient should be increased.
- If the ingredient with the same name but different unit existed in the list already, a new ingredient should be added.

## User Story: Set Amount/Unit For Ingredients
**Description:** As a user, I want to be able to set the amounts/units of ingredients in a list

**Acceptance Criteria:** 
- Given that I’m in a list on My Lists page
- When I click on the edit button next to an ingredient.
- Then, I should be able to edit the amount/unit of ingredient in that list.

### Test Steps
- When on a list in `My Lists` page, click on the edit button next to an ingredient.
- Enter the amount and/or unit in the popup and click OK.
- If an ingredient with the same name and unit as the new ingredient exists, the amount entered should be added to that ingredient.
- If an ingredient with the same name and unit as the new ingredient does not exist, the ingredient should just be edited with the new values.

## User Story: Persistence User
**Description:** As a user, I want to see the ingredients I previously had in my list when I log in.

**Acceptance Criteria:** 
- Given that I’m a logged-out user
- When I enter my details correctly and get logged in
- Then, the app should keep my previous ingredients list.

### Test Steps
- Log into the application
- Click on `My Lists`
- All the lists should have the same ingredients as before the log-in.

## User Story: Move Ingredients
**Description:** As a user, when I'm on a list, I should be able to move any given ingredient to another list of mine.

**Acceptance Criteria:** 
- Given that I am a logged in user on My Lists page on a list
- Then I should be able to move an ingredient to another list.

### Test Steps
- Click on the `Move` button next to an ingredient.
- Select the list to move to from the pop-up and click OK.
- The ingredient should disappear from the current list.
- If the list to move to already has an ingredient with the same name and unit, the amount of that ingredient should increase. 
- If the list to move to does not have an ingredient with the same name and unit, a new ingredient should be added.

## User Story: Create custom ingredients

**Description:** As a user,I should be able to create my own custom ingredients.

**Acceptance Criteria:**

- Given that I’m a logged in user with my list of ingredients open

- When I click `Create Ingredient`

- Then I should be able to create a custom ingredient by entering a name and type of my choosing.

### Test Steps

- From the Home Page, click on `Ingredients`
- Click on the `Create Ingredient` button
- Enter the Name and Type of ingredient.
- The custom ingredient (with it's flag) should show up in the list.

## User Story: Add custom ingredients

**Description:** As a user, I should be able to add my custom ingredients to my lists

**Acceptance Criteria:**

- Given that I’m a logged in user on either the Ingredients page or when I'm on a certain list

- When I try to add ingredients to a certain list, I should be given the option to add a custom ingredient to a list.


### Test Steps (from Ingredients)

- From the Home Page, click on `Ingredients`
- Filter on "Custom"
- Click the `+` button
- Enter the amount and unit and click
- Choose one of your lists from the dropdown menu
- Click `Add` to add your ingredient to a list

## User Story: Remove Custom Ingredients from List

**Description:** As a user, i should be able to remove a custom ingredient from my lists.

**Acceptance Criteria:**

- Given that I am logged in and on My Lists page
- And I navigate to a custom ingredient on a list
- And I click on delete ingredient
- Then the custom ingredient should be removed from that list

### Test Steps

- On the `My Lists` page, click on a list to open it. It shows ingredients in that list.
- Navigate to the custom ingredient to delete.
- Click on the delete button next on the chosen custom ingredient.
- That custom ingredient (with that amount and unit) should no longer be in the list.

## User Story: Set Amount/Unit For Custom Ingredients

**Description:** As a user, I want to be able to set the amounts/units of custom ingredients in a list

**Acceptance Criteria:**

- Given that I’m a logged in user in a list on My Lists page
- When I click on the edit button next to a custom ingredient.
- Then, I should be able to edit the amount/unit of the custom ingredient in that list.

### Test Steps

- When on a list in `My Lists` page, click on the edit button next to a custom ingredient.
- Enter the amount and/or unit in the popup and click OK.
- If a custom ingredient with the same name and unit as the new ingredient exists, the amount entered should be added to that ingredient.
- If a custom ingredient with the same name and unit as the new ingredient does not exist, the ingredient should just be edited with the new values.

## User Story: Move Custom Ingredients

**Description:** As a user, when I'm on a list, I should be able to move any given custom ingredient to another list of mine.

**Acceptance Criteria:**

- Given that I am a logged in user on My Lists page on a list
- Then I should be able to move a custom ingredient to another list.

### Test Steps

- From the Home Page, click on `My Lists` and navigate to a certain List.
- Click on the `Move` button next to a custom ingredient.
- Select the list to move to from the pop-up and click Move.
- The ingredient should disappear from the current list.
- If the list to move to already has a custom ingredient with the same name and unit, the amount of that ingredient should increase.
- If the list to move to does not have a custom ingredient with the same name and unit, a new ingredient should be added.


## User Story: Create Recipe

**Description:** As a user, I should be able to create recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able to create a new `Recipe` with a name of my choice.

### Test Steps

- From the Home Page, click on `Recipes`.
- Click on the `Create Recipe` button
- Enter the name of your choosing.
- The recipe with your chosen name should show up on the screen.

## User Story: Delete Recipe

**Description:** As a user, I should be able to delete any of my recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able to delete any given `Recipe` 

### Test Steps

- From the Home Page, click on `Recipes`.
- Find the Recipe that you want to delete from the list provided
- Click on the delete icon in the same row as the list name
- Confirm the dialog
- The recipe should now be deleted.

## User Story: Add Ingredient to Recipe

**Description:** As a user, I should be able to add ingredients to my recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able navigate to a certain recipe and add ingredients to it. 

### Test Steps

- From the Home Page, click on `Recipes`.
- Find the Recipe that you want to add ingredients to and click on it.
- On the `Ingredients` section of the screen, press the `+` button.
- Find the ingredient of your choice from the provided popup.
- Specify an amount and unit and click `Add`.
- The chosen ingredient should now display on the `Ingredients` portion of the screen.

## User Story: Delete Ingredient from Recipe

**Description:** As a user, I should be able to delete ingredients from my recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able navigate to a certain recipe and delete ingredients from it. 

### Test Steps

- From the Home Page, click on `Recipes`.
- Find the Recipe that you want to delete ingredients from and click on it.
- On the `Ingredients` section of the screen, press the delete button next to the ingredient of your choice.
- Confirm the dialog.
- The chosen ingredient should now be deleted from the `Ingredients` portion of the screen.

## User Story: Add Steps to Recipe

**Description:** As a user, I should be able to add steps to my recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able navigate to a certain recipe and add steps to it. 

### Test Steps

- From the Home Page, click on `Recipes`.
- Find the Recipe that you want to add steps to and click on it.
- On the `Steps` section of the screen, press the `+` button.
- Type out your step and click the `Add` button shown.
- The entered step should now be the last step of the recipe.


## User Story: Delete Steps from Recipe

**Description:** As a user, I should be able to delete steps from my recipes.

**Acceptance Criteria:**

- Given that I am a logged in user and on the `Recipes` page.
- Then I should be able navigate to a certain recipe and delete steps from it. 

### Test Steps

- From the Home Page, click on `Recipes`.
- Find the Recipe that you want to delete steps from and click on it.
- On the `Steps` section of the screen, press the delete button next to any given step.
- Confirm the dialog.
- The chosen step should now be deleted from the recipe.