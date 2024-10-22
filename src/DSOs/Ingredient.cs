namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// DSO class for Ingredient
    /// </summary>
    public class Ingredient
    {
        private string name;
        private string ingType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class
        /// with default values for name and type.
        /// </summary>
        public Ingredient() : this("No Name", "No Type") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class
        /// with the specified name and type.
        /// </summary>
        /// <param name="name">The name of the ingredient.</param>
        /// <param name="ingType">The type of the ingredient.</param>
        public Ingredient(string name, string ingType)
        {
            this.name = name;
            this.ingType = ingType;
        }

        /// <summary>
        /// Gets the name of the ingredient.
        /// </summary>
        /// <returns>The name of the ingredient as a string.</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Sets the name of the ingredient.
        /// </summary>
        /// <param name="name">The name to set for the ingredient.</param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the type of the ingredient.
        /// </summary>
        /// <returns>The type of the ingredient as a string.</returns>
        public string GetIngType()
        {
            return ingType;
        }

        /// <summary>
        /// Sets the type of the ingredient.
        /// </summary>
        /// <param name="ingType">The type to set for the ingredient.</param>
        public void SetIngType(string ingType)
        {
            this.ingType = ingType;
        }
    }
}
