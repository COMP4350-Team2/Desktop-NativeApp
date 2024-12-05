using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    /// <summary>
    /// A mock implementation of the <see cref="IBackend"/> interface
    /// </summary>
    public class BackendMock : IBackend
    {
        private IUser user;
        private List<Ingredient> ?ingredients;
        private List<UserList> ?myLists;
        private List<Recipe> ?recipes;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackendMock"/> class and populates the list 
        /// of ingredients with predefined values.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> associated with this mock backend.</param>
        public BackendMock(IUser user)
        {
            ingredients = new List<Ingredient>
            {
                new Ingredient("Apple", "Fruit"),
                new Ingredient("Milk", "Dairy"),
                new Ingredient("Rice", "Grain"),
                new Ingredient("Eggs", "Protein"),
                new Ingredient("Bread", "Grain"),
                new Ingredient("Beans", "Pantry"),
                new Ingredient("Owl", "Poultry"),
                new Ingredient("Chicken", "Poultry"),
                new Ingredient("Oats", "Pantry"),
                new Ingredient("Oranges", "Produce"),
                new Ingredient("Carrot", "Produce"),
                new Ingredient("Wheat", "Grain"),
                new Ingredient("Lentils", "Grain"),
                new Ingredient("Cookies", "Snacks"),
                new Ingredient("Chips", "Snacks"),
                new Ingredient("LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG", "long long long long long long long long"),
                new Ingredient("Custom Ing 1", "custom type 1", true),
                new Ingredient("Custom Ing 2", "custom type 2", true),
                new Ingredient("Custom Ing 3", "custom type 3", true),
                new Ingredient("Custom Ing 4", "custom type 4", true),
                new Ingredient("Custom Ing 5", "custom type 5", true)

            };
            ingredients.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));
            this.user = user;

            InitMyLists();

            InitRecipes();
        }

        /// <summary>
        /// Retrieves all ingredients from the mock backend.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> requesting the ingredients.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a list of <see cref="Ingredient"/> '
        /// objects as the result.
        /// </returns>
        public Task<List<Ingredient>> GetAllIngredients(IUser user)
        {
            return Task.FromResult(ingredients);
        }

        /// <summary>
        /// Creates a new user in the mock backend.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/>  to be created.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a value indicating
        /// whether the user creation was successful. Always returns true.
        /// </returns>
        public Task<bool> CreateUser(IUser user)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Asynchronously retrieves the user's lists of ingredients.
        /// This method creates two predefined ingredient lists: Grocery and Pantry, 
        /// each containing a set of ingredients.
        /// </summary>
        /// <param name="user">The user for whom the lists are retrieved.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of user lists.</returns>
        public async Task<List<UserList>> GetMyLists(IUser user)
        {
           return await Task.FromResult(myLists);
        }

        /// <summary>
        /// Method to initialize mocked backend lists
        /// </summary>
        private void InitMyLists()
        {
            myLists = new List<UserList>();

            List<Ingredient> groceryIngs = new List<Ingredient>();
            groceryIngs.Add(new Ingredient("LONG LONG LONG LONG LONG LONG LONG", "LONG LONG LONG LONG LONG LONG LONG", 1, "count"));
            groceryIngs.Add(new Ingredient("Chicken", "Poultry", 2000, "g"));
            groceryIngs.Add(new Ingredient("Beef", "Meat", 250, "g"));
            groceryIngs.Add(new Ingredient("Rabbit", "Meat", 1, "count"));
            groceryIngs.Add(new Ingredient("Chicken", "Poultry", 8, "count"));
            groceryIngs.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));



            List<Ingredient> pantryIngs = new List<Ingredient>();
            pantryIngs.Add(new Ingredient("Cheese", "Dairy", 100, "g"));
            pantryIngs.Add(new Ingredient("Milk", "Dairy", 250, "ml"));
            pantryIngs.Add(new Ingredient("Cereal", "Pantry", 500, "g"));
            pantryIngs.Add(new Ingredient("Carrot", "Produce", 4, "count"));
            pantryIngs.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));


            UserList groceryList = new UserList("Grocery", groceryIngs);
            UserList pantryList = new UserList("Pantry", pantryIngs);

            myLists.Add(groceryList);
            myLists.Add(pantryList);

        }

        /// <summary>
        /// Adds an <see cref="Ingredient"/> to a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is adding.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be added.</param>
        /// <param name="listName">The name of the list to add to</param>
        /// <returns>A bool indicating whether addition was successfull.</returns>
        public Task<bool> AddIngredientToList(IUser user, Ingredient ingredient, string listName)
        {
            bool added = false;

            UserList listToBeModified = null;

            for (int i = 0; i < myLists.Count; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    listToBeModified = myLists[i];
                }
            }

            if (listToBeModified != null)
            {
                listToBeModified.AddIngToList(ingredient);
                added = true;
            }

            return Task.FromResult(added);

        }

        /// <summary>
        /// Returns a list of strings containing the measurement units
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> for authentication.</param>
        /// <returns>
        /// List of strings with the allowed measurement units
        /// </returns>
        public Task<List<string>> GetAllMeasurements(IUser user)
        {
            List<string> units = new List<string>();
            units.Add("count");
            units.Add("g");
            units.Add("kg");
            units.Add("lb");
            units.Add("oz");
            units.Add("mL");
            units.Add("L");
            units.Add("gal");

            return Task.FromResult(units);
        }

        /// <summary>
        /// Removes an <see cref="Ingredient"/> from a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be removed.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public Task<bool> RemIngredientFromList(IUser user, Ingredient ingredient, string listName)
        {
            bool removed = false;

            for (int i = 0; i < myLists.Count && !removed; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    myLists[i].RemIngFromList(ingredient);
                    removed = true;
                }
            }

            return Task.FromResult(removed);
        }

        /// <summary>
        /// Edits the amount and/or unit <see cref="Ingredient"/> in <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be removed.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether edit was successfull.</returns>
        public Task<bool> SetIngredient(IUser user, Ingredient oldIng, Ingredient newIng, string listName)
        {
            bool added = false;

            for(int i = 0; i < myLists.Count && !added; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    added = myLists[i].EditIngredientInList(oldIng, newIng);

                }
            }

            return Task.FromResult(added);
        }

        /// <summary>
        /// Removes a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing a list.</param>
        /// <param name="listName">The name of the list to be deleted</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public Task<bool> DeleteList(IUser user, string listName)
        {
            bool deleted = false;

            for(int i = 0; i < myLists.Count && !deleted; i++)
            {
                if (myLists[i].GetListName() == listName)
                {
                    myLists.RemoveAt(i);
                    deleted = true;
                }
            }

            return Task.FromResult(deleted);
        }

        /// <summary>
        /// Creates a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="listName">The name of the list to be created</param>
        /// <returns>A bool indicating whether creation was successfull.</returns>
        public Task<bool> CreateList(IUser user, string listName)
        {
            bool created = false;

            bool alreadyExists = false;

            for(int i = 0; i < myLists.Count && !alreadyExists; i++)
            {
                alreadyExists = (myLists[i].GetListName() == listName);
            }

            if (!alreadyExists)
            {
                List<Ingredient> emptyIngs = new List<Ingredient>();
                UserList newList = new UserList(listName, emptyIngs);
                myLists.Add(newList);
                created = true;
            }

            return Task.FromResult(created);
        }

        /// <summary>
        /// Moves an ingredient from one list to another
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="currListName">The name of the original list where the ingredient is from</param>
        /// <param name="newListName">The name of the new list where the ingredient is to be moved</param>
        /// /// <param name="ingredient">The <see cref="Ingredient"></see> to be moved</param>
        /// <returns>A bool indicating whether moving was successfull.</returns>
        public async Task<bool> MoveIngredient(IUser user, string currListName, string newListName, Ingredient ingredient)
        {
            bool moved = false;

            bool addedToNewList = AddIngredientToList(user, ingredient, newListName).Result;

            // If addition to new list was successful
            if (addedToNewList)
            {
                // Try to remove from old list
                bool remFromOldList = RemIngredientFromList(user, ingredient, currListName).Result;

                // If remove from old list successful
                if (remFromOldList)
                {
                    // It has moved
                    moved = true;
                }
                // If remove failed
                else
                {
                    // Add it back to old list
                    await AddIngredientToList(user, ingredient, currListName);

                    // Remove the previously added 
                    await RemIngredientFromList(user, ingredient, newListName);

                    moved = false;
                }
            }

            return await Task.FromResult(moved);
        }

        /// <summary>
        /// Method to rename a list
        /// </summary>
        /// <param name="user"> The user who is renaming lists</param>
        /// <param name="currListName"> Current name of the list to change </param>
        /// <param name="newListName"> New name for the list </param>
        /// <returns></returns>
        public Task<bool> RenameList(IUser user, string currListName, string newListName)
        {
            bool renamed = false;

            for (int i = 0; i < myLists.Count && !renamed; i++)
            {
                if (myLists[i].GetListName() == currListName)
                {
                    myLists[i].SetListName(newListName);
                    renamed = true;
                }
            }

            return Task.FromResult(renamed);
        }

        /// <summary>
        /// Method to create a custom ingredient
        /// </summary>
        /// <param name="user"> User creating the ingredient</param>
        /// <param name="customIng"> The ingredient to be created </param>
        /// <returns> True on success, false on failure </returns>
        public Task<bool> CreateCustomIngredient(IUser user, Ingredient customIng)
        {
            ingredients.Add(customIng);

            ingredients.Sort((x, y) => string.Compare(x.GetName(), y.GetName(), StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(true);
        }

        /// <summary>
        /// Backend method to delete a custom ingredient
        /// </summary>
        /// <param name="user"> User who is removing ingredient </param>
        /// <param name="ingredient"> The ingredient to be removed</param>
        /// <returns></returns>
        public Task<bool> DeleteCustomIngredient(IUser user, Ingredient ingredient)
        {
            bool deleted = false;

            for(int i = 0; i < ingredients.Count && !deleted; i++)
            {
                Ingredient curr = ingredients[i];
                if (curr.GetName() == ingredient.GetName() && 
                    curr.GetIngType() == ingredient.GetIngType() && 
                    curr.IsCustom() == ingredient.IsCustom())
                {
                    ingredients.Remove(curr);
                    deleted = true;
                }
            }

            for(int i = 0; i < myLists.Count; i++)
            {
                myLists[i].CascadeDelCustomIng(ingredient);
            }

            return Task.FromResult(deleted);
        }

        /// <summary>
        /// Method to get all recipes of a user
        /// </summary>
        /// <param name="user"> User making the request </param>
        /// <returns> List of Recipe objects </returns>
        public Task<List<Recipe>> GetAllRecipes(IUser user)
        {
            return Task.FromResult(recipes);
        }

        /// <summary>
        /// Method to initialize recipes
        /// </summary>
        private void InitRecipes()
        {
            recipes = new List<Recipe>();

            Recipe r1 = new Recipe("Mock Recipe 1");
            UserList r1Ings = myLists.Find(i => i.GetListName() == "Grocery").CopyList();
            List<String> r1Steps = new List<String>();
            r1Steps.Add("r1 step 1 - Some words some words some words some words");
            r1Steps.Add("r1 step 2 - Some words some words some words some words");
            r1Steps.Add("r1 step 3 - Some words some words some words some words");
            r1Steps.Add("r1 step 4 - Some words some words some words some words");


            Recipe r2 = new Recipe("Mock Recipe 2");
            UserList r2Ings = myLists.Find(i => i.GetListName() == "Pantry").CopyList();
            List<String> r2Steps = new List<String>();
            r2Steps.Add("r2 step 1 - Some words some words some words some words");
            r2Steps.Add("r2 step 2 - Some words some words some words some words");
            r2Steps.Add("r2 step 3 - Some words some words some words some words");
            r2Steps.Add("r2 step 4 - Some words some words some words some words");

            recipes.Add(r1);
            recipes.Add(r2);
        }

        /// <summary>
        /// Method to create a new recipe
        /// </summary>
        /// <param name="user"> User creating the recipe </param>
        /// <param name="recipeName"> Name of the new recipe </param>
        /// <returns> True on success, false on failure </returns>
        public Task<bool> CreateRecipe(IUser user, string recipeName)
        {
            bool created = false;
            Recipe newRecipe = new Recipe(recipeName);

            bool found = false;
            for(int i = 0; i  < recipes?.Count && !found; i++)
            {
                if (recipes[i].GetRecipeName() == recipeName)
                {
                    found = true;
                }
            }

            if (!found)
            {
                recipes?.Add(newRecipe);
                created = true;
            }            
            return Task.FromResult(created);
        }


        /// <summary>
        /// Method to delete a recipe
        /// </summary>
        /// <param name="user"> User deleting recipe </param>
        /// <param name="recipeName"> Name of recipe to be deleted </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> DeleteRecipe(IUser user, string recipeName)
        {
            bool deleted = false;

            bool found = false;
            Recipe toDel = null;
            for (int i = 0; i < recipes?.Count && !found; i++)
            {
                if (recipes[i].GetRecipeName() == recipeName)
                {
                    toDel = recipes[i];
                    found = true;
                }
            }

            if (found)
            {
                recipes?.Remove(toDel);
                deleted = true;
            }


            return Task.FromResult(deleted);
        }

        /// <summary>
        /// Method to add an ingredient to a recipe
        /// </summary>
        /// <param name="user"> User making the request </param>
        /// <param name="ingredient"> Ingredient to add </param>
        /// <param name="recipeName"> Name of recipe to be added to </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> AddIngToRecipe(IUser user, Ingredient ingredient, string recipeName)
        {
            bool added = false;

            for(int i = 0; i < recipes?.Count && !added; i++)
            {
                Recipe curr = recipes[i];

                if(curr.GetRecipeName() == recipeName)
                {
                    curr.GetRecipeIngList().AddIngToList(ingredient);
                    added = true;
                }
            }

            return Task.FromResult(added);
        }

        /// <summary>
        /// Method to delete an ingredient from a recipe
        /// </summary>
        /// <param name="user"> User deleting the ingredient </param>
        /// <param name="ingredient"> Ingredient being deleted </param>
        /// <param name="recipeName"> Name of recipe </param>
        /// <returns></returns>
        public Task<bool> DeleteIngInRecipe(IUser user, Ingredient ingredient, string recipeName)
        {
            bool deleted = false;

            for (int i = 0; i < recipes?.Count && !deleted; i++)
            {
                Recipe curr = recipes[i];

                if (curr.GetRecipeName() == recipeName)
                {
                    curr.GetRecipeIngList().RemIngFromList(ingredient);
                    deleted = true;
                }
            }

            return Task.FromResult(deleted);
        }

        /// <summary>
        /// Method to add step to a recipe
        /// </summary>
        /// <param name="user"> User adding step </param>
        /// <param name="step"> The step to add </param>
        /// <param name="recipeName"> The name of the recipe </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> AddStepToRecipe(IUser user, string step, string recipeName)
        {
            bool added = false;

            for(int i = 0; i < recipes?.Count && !added; i++)
            {
                Recipe curr = recipes[i];

                if(curr.GetRecipeName() == recipeName)
                {
                    curr.GetRecipeSteps().Add(step);
                    added = true;
                }
            }

            return Task.FromResult(added);
        }
    }
}
