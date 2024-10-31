namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Represents a user's list containing ingredients.
    /// </summary>
    public class UserList
    {
        private string ListName; // Name of the list
        private List<Ingredient> Ingredients; // List of ingredients in the user's list

        /// <summary>
        /// Initializes a new instance of the <see cref="UserList"/> class.
        /// </summary>
        /// <param name="listName">The name of the user list.</param>
        /// <param name="ingredients">The list of ingredients associated with the user list.</param>
        public UserList(string listName, List<Ingredient> ingredients)
        {
            SetListName(listName);
            SetIngredients(ingredients);
        }

        /// <summary>
        /// Gets the name of the user list.
        /// </summary>
        /// <returns>The name of the user list.</returns>
        public string GetListName() { return ListName; }

        /// <summary>
        /// Sets the name of the user list.
        /// </summary>
        /// <param name="name">The new name for the user list.</param>
        public void SetListName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.ListName = name;
            }
            else
            {
                this.ListName = "No Name";
            }
        }

        /// <summary>
        /// Gets the list of ingredients.
        /// </summary>
        /// <returns>A list of ingredients in the user list.</returns>
        public List<Ingredient> GetIngredients() { return Ingredients; }

        /// <summary>
        /// Sets the ingredients for the user list.
        /// </summary>
        /// <param name="ingredients">The new list of ingredients.</param>
        public void SetIngredients(List<Ingredient> ingredients) { Ingredients = ingredients; }

        /// <summary>
        /// Adds an ingredient to the user list.
        /// If the ingredient is already in the list, updates its amount.
        /// </summary>
        /// <param name="ingredient">The ingredient to add to the list.</param>
        public void AddIngToList(Ingredient ingredient)
        {
            // Check if the ingredient is already in the list
            if (IngInList(ingredient))
            {
                // Find the existing ingredient in the list
                Ingredient existingIngredient = Ingredients.First(i => i.IsEqual(ingredient));

                // Update its amount
                existingIngredient.SetAmount(existingIngredient.GetAmount() + ingredient.GetAmount());
            }
            else
            {
                // If the ingredient is not in the list, add it
                Ingredients.Add(ingredient);
            }
        }

        /// <summary>
        /// Removes an ingredient from the user list.
        /// </summary>
        /// <param name="ingredient">The ingredient to remove from the list.</param>
        public void RemIngFromList(Ingredient ingredient) { Ingredients.Remove(ingredient); }

        /// <summary>
        /// Checks if an ingredient is already in the user list.
        /// </summary>
        /// <param name="ingredient">The ingredient to check.</param>
        /// <returns>True if the ingredient is in the list, otherwise false.</returns>
        private bool IngInList(Ingredient ingredient)
        {
            bool found = false;
            for (int i = 0; i < Ingredients.Count && !found; i++)
            {
                found = Ingredients[i].IsEqual(ingredient);
            }

            return found;
        }
    }
}
