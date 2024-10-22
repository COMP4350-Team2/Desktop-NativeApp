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
    }
}
