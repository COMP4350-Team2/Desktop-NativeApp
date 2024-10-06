namespace Desktop_Frontend
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

        // Implementation of GetAllIngredients using the initialized list
        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return ingredients;
        }
    }
}
