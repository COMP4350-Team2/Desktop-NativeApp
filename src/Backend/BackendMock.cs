using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    public class BackendMock : IBackend
    {
        private IUser user;
        private List<Ingredient> ingredients;

        // Constructor initializes the list of ingredients
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

        public Task<List<Ingredient>> GetAllIngredients(IUser user)
        {
            return Task.FromResult(ingredients);
        }

        public async Task<bool> CreateUser(IUser user)
        {
            return await Task.FromResult(true);
        }
    }
}
