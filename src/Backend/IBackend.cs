using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    /// <summary>
    /// Interface for the backend functionality
    /// </summary>
    public interface IBackend
    {
        /// <summary>
        /// Retrieves all available ingredients from the backend.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> requesting the ingredient list.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a list of <see cref="Ingredient"/> 
        /// objects as the result.
        /// </returns>
        public Task<List<Ingredient>> GetAllIngredients(IUser user);

        /// <summary>
        /// Creates a new user in the backend.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> to be created.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a value indicating
        /// whether the user creation was successful.
        /// </returns>
        public Task<bool> CreateUser(IUser user);

        /// <summary>
        /// Returns a list of <see cref="UserList"/> objects
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> whose lists are being returned.</param>
        /// <returns>
        /// List of <see cref="UserList"/> objects
        /// </returns>
        public Task<List<UserList>> GetMyLists(IUser user);

        /// <summary>
        /// Adds an <see cref="Ingredient"/> to a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is adding.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be added.</param>
        /// <param name="listName">The name of the list to add to</param>
        /// <returns>A bool indicating whether addition was successfull.</returns>
        public Task<bool> AddIngredientToList(IUser user, Ingredient ingredient, string listName);

        /// <summary>
        /// Returns a list of strings containing the measurement units
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> for authentication.</param>
        /// <returns>
        /// List of strings with the allowed measurement units
        /// </returns>
        public Task<List<string>> GetAllMeasurements(IUser user);

        /// <summary>
        /// Removes an <see cref="Ingredient"/> from a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be removed.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public Task<bool> RemIngredientFromList(IUser user, Ingredient ingredient, string listName);

        /// <summary>
        /// Edits the amount and/or unit <see cref="Ingredient"/> in <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is setting the ingredient.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be removed.</param>
        /// <param name="listName">The name of the list to remove from</param>
        /// <returns>A bool indicating whether edit was successfull.</returns>
        public Task<bool> SetIngredient(IUser user, Ingredient oldIng, Ingredient newIng, string listName);

        /// <summary>
        /// Removes a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is removing a list.</param>
        /// <param name="listName">The name of the list to be deleted</param>
        /// <returns>A bool indicating whether deletion was successfull.</returns>
        public Task<bool> DeleteList(IUser user, string listName);

        /// <summary>
        /// Creates a <see cref="UserList"/> with provided name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="listName">The name of the list to be created</param>
        /// <returns>A bool indicating whether creation was successfull.</returns>
        public Task<bool> CreateList(IUser user, string listName);

        /// <summary>
        /// Moves an ingredient from one list to another
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is creating a list.</param>
        /// <param name="currListName">The name of the original list where the ingredient is from</param>
        /// <param name="newListName">The name of the new list where the ingredient is to be moved</param>
        /// /// <param name="ingredient">The <see cref="Ingredient"></see> to be moved</param>
        /// <returns>A bool indicating whether moving was successfull.</returns>
        public Task<bool> MoveIngredient(IUser user, string currListName, string newListName, Ingredient ingredient);

        /// <summary>
        /// Method to rename a list
        /// </summary>
        /// <param name="user"> The user who is renaming lists</param>
        /// <param name="currListName"> Current name of the list to change </param>
        /// <param name="newListName"> New name for the list </param>
        /// <returns></returns>
        public Task<bool> RenameList(IUser user, string currListName, string newListName);

        /// <summary>
        /// Method to create a custom ingredient
        /// </summary>
        /// <param name="user"> User creating the ingredient</param>
        /// <param name="customIng"> The ingredient to be created </param>
        /// <returns> True on success, false on failure </returns>
        public Task<bool> CreateCustomIngredient(IUser user, Ingredient customIng);

        /// <summary>
        /// Backend method to delete a custom ingredient
        /// </summary>
        /// <param name="user"> User who is removing ingredient </param>
        /// <param name="ingredient"> The ingredient to be removed</param>
        /// <returns></returns>
        public Task<bool> DeleteCustomIngredient(IUser user, Ingredient ingredient);

        /// <summary>
        /// Method to get all recipes of a user
        /// </summary>
        /// <param name="user"> User making the request </param>
        /// <returns> List of Recipe objects </returns>
        public Task<List<Recipe>> GetAllRecipes(IUser user);

        /// <summary>
        /// Method to create a new recipe
        /// </summary>
        /// <param name="user"> User creating the recipe </param>
        /// <param name="recipeName"> Name of the new recipe </param>
        /// <returns> True on success, false on failure </returns>
        public Task<bool> CreateRecipe(IUser user, string recipeName);

        /// <summary>
        /// Method to delete a recipe
        /// </summary>
        /// <param name="user"> User deleting recipe </param>
        /// <param name="recipeName"> Name of recipe to be deleted </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> DeleteRecipe(IUser user, string recipeName);

        /// <summary>
        /// Method to add an ingredient to a recipe
        /// </summary>
        /// <param name="user"> User making the request </param>
        /// <param name="ingredient"> Ingredient to add </param>
        /// <param name="recipeName"> Name of recipe to be added to </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> AddIngToRecipe(IUser user, Ingredient ingredient, string recipeName);
        
        /// <summary>
        /// Method to delete an ingredient from a recipe
        /// </summary>
        /// <param name="user"> User deleting the ingredient </param>
        /// <param name="ingredient"> Ingredient being deleted </param>
        /// <param name="recipeName"> Name of recipe </param>
        /// <returns></returns>
        public Task<bool> DeleteIngInRecipe(IUser user, Ingredient ingredient, string recipeName);

        /// <summary>
        /// Method to add step to a recipe
        /// </summary>
        /// <param name="user"> User adding step </param>
        /// <param name="step"> The step to add </param>
        /// <param name="recipeName"> The name of the recipe </param>
        /// <returns>True on success, false on failure </returns>
        public Task<bool> AddStepToRecipe(IUser user, string step, string recipeName);

        /// <summary>
        /// Method to delete a step from a recipe
        /// </summary>
        /// <param name="user"> User deleting step </param>
        /// <param name="stepNum"> Index of step (1 to N) </param>
        /// <param name="recipeName"> Name of recipe </param>
        /// <returns>True on success, false on failure</returns>
        public Task<bool> DeleteStepFromRecipe(IUser user, int stepNum, string recipeName);
    }
}
