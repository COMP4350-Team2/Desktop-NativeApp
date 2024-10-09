using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    public interface IBackend
    {
        public Task<List<Ingredient>> GetAllIngredients();

        public Task<bool> CreateUser(IUser user);

    }
}
