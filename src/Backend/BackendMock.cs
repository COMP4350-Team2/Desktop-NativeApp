using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    /// <summary>
    /// A mock implementation of the <see cref="IBackend"/> interface
    /// </summary>
    public class BackendMock : IBackend
    {
        private IUser user;
        private List<Ingredient> ingredients;

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
                new Ingredient("Bread", "Grain")
            };
            this.user = user;
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
        public async Task<bool> CreateUser(IUser user)
        {
            return await Task.FromResult(true);
        }
    }
}
