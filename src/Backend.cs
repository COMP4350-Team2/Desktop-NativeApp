namespace Desktop_Frontend
{
    public class Backend : IBackend
    {
        private IUser user;
        // Constructor (empty for now)
        public Backend(IUser user) 
        { 
            this.user = user;
        }

        // Implementation of GetAllIngredients, returning an empty list for now
        public async Task<List<Ingredient>> GetAllIngredients()
        {
            // Returning an empty list as a placeholder
            return await Task.FromResult(new List<Ingredient>());
        }
    }
}
