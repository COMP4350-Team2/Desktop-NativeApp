namespace Desktop_Frontend
{
    public interface IBackend
    {
        public Task<List<Ingredient>> GetAllIngredients();

    }
}
