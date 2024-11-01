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
        private List<UserList> myLists;

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

            InitMyLists();
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
            List<Ingredient> copy = new List<Ingredient>();

            for (int i = 0; i < ingredients.Count; i++)
            {
                copy.Add(ingredients[i].CopyIngredient());
            }
            return Task.FromResult(copy);
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

        /// <summary>
        /// Asynchronously retrieves the user's lists of ingredients.
        /// This method creates two predefined ingredient lists: Grocery and Pantry, 
        /// each containing a set of ingredients.
        /// </summary>
        /// <param name="user">The user for whom the lists are retrieved.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of user lists.</returns>
        public async Task<List<UserList>> GetMyLists(IUser user)
        {
            List<UserList> copy = new List<UserList>();

           for (int i = 0; i < myLists.Count; i++)
           {
                copy.Add(myLists[i].CopyList());
           }
           return await Task.FromResult(myLists);
        }

        /// <summary>
        /// Method to initialize mocked backend lists
        /// </summary>
        private void InitMyLists()
        {
            myLists = new List<UserList>();

            List<Ingredient> groceryIngs = new List<Ingredient>();
            groceryIngs.Add(new Ingredient("Chicken", "Poultry", 2000, "g"));
            groceryIngs.Add(new Ingredient("Beef", "Meat", 250, "g"));
            groceryIngs.Add(new Ingredient("Rabbit", "Meat", 1, "count"));
            groceryIngs.Add(new Ingredient("Chicken", "Poultry", 8, "count"));


            List<Ingredient> pantryIngs = new List<Ingredient>();
            pantryIngs.Add(new Ingredient("Cheese", "Dairy", 100, "g"));
            pantryIngs.Add(new Ingredient("Milk", "Dairy", 250, "ml"));
            pantryIngs.Add(new Ingredient("Cereal", "Pantry", 500, "g"));
            pantryIngs.Add(new Ingredient("Carrot", "Produce", 4, "count"));

            UserList groceryList = new UserList("Grocery", groceryIngs);
            UserList pantryList = new UserList("Pantry", pantryIngs);

            myLists.Add(groceryList);
            myLists.Add(pantryList);

        }

        /// <summary>
        /// Adds an <see cref="Ingredient"/> to a <see cref="UserList"/> with the given name
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> who is adding.</param>
        /// <param name="ingredient">The <see cref="Ingredient"/> to be added.</param>
        /// <param name="listName">The name of the list to add to</param>
        public async Task<bool> AddIngredientToList(IUser user, Ingredient ingredient, string listName)
        {
            UserList listToBeModified = myLists.FirstOrDefault(list => list.GetListName() == listName);
               
            listToBeModified?.AddIngToList(ingredient);

            return true;

        }

        /// <summary>
        /// Returns a list of strings containing the measurement units
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> for authentication.</param>
        /// <returns>
        /// List of strings with the allowed measurement units
        /// </returns>
        public Task<List<string>> GetAllMeasurements(IUser user)
        {
            List<string> units = new List<string>();
            units.Add("count");
            units.Add("g");
            units.Add("kg");
            units.Add("lbs");
            units.Add("oz");
            units.Add("mL");
            units.Add("L");
            units.Add("gal");

            return Task.FromResult(units);
        }
    }
}
