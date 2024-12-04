namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// DSO class for User's Recipes
    /// </summary>
    public class Recipe
    {
        private string Name;
        private UserList IngList;
        private List<string> Steps;

        /// <summary>
        /// Constructor to initialize an empty recipe
        /// </summary>
        /// <param name="name"> Name of recipe to be created </param>
        public Recipe(string name) : 
            this(name, new UserList("Ingredients", new List<Ingredient>()), new List<string>())
        { }

        /// <summary>
        /// Constructor for a filled recipe
        /// </summary>
        /// <param name="name"> Name of recipe to be created </param>
        /// <param name="ingList"> <see cref="UserList"/> of ingredients in recipe</param>
        /// <param name="steps"> List of strings containing the recipe steps</param>
        public Recipe(string name, UserList ingList, List<string> steps)
        {
            SetRecipeName(name);
            SetRecipeIngList(ingList);
            SetRecipeSteps(steps);
        }

        /// <summary>
        /// Getter for recipe name
        /// </summary>
        /// <returns>string of recipe name </returns>
        public string GetRecipeName() {  return Name; }

        /// <summary>
        /// Setter for recipe name
        /// </summary>
        /// <param name="name"> New name for recipe </param>
        public void SetRecipeName(string name) 
        { 
            Name = name ?? "No Name"; 
        }

        /// <summary>
        /// Getter for the ingredients of a recipe
        /// </summary>
        /// <returns> UserList containing ingredients with amounts and unit </returns>
        public UserList GetRecipeIngList() { return IngList; }

        /// <summary>
        /// Setter for setting the ingredients of a recipe
        /// </summary>
        /// <param name="ingList"> The list of ingredients with amounts and units</param>
        public void SetRecipeIngList(UserList ingList) { IngList = ingList; }

        /// <summary>
        /// Getter for all steps in a recipe
        /// </summary>
        /// <returns> List of strings containing all steps </returns>
        public List<string> GetRecipeSteps() { return Steps; }

        /// <summary>
        /// Setter for setting the steps of recipe 
        /// </summary>
        /// <param name="steps"> The steps of the recipe </param>
        public void SetRecipeSteps(List<string> steps) { Steps = steps; }

    }
}
