using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    public class Backend : IBackend
    {
        private IUser user;

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

        public async Task<bool> CreateUser(IUser user)
        {
            return await Task.FromResult(true);
        }
    }
}
