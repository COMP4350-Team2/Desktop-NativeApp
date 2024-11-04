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
    }
}
