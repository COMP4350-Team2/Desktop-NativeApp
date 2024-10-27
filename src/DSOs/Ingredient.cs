namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// DSO class for Ingredient
    /// </summary>
    public class Ingredient
    {
        private string name;
        private string ingType;
        private float amount;
        private string unit;

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
            amount = 0;
            unit = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ingredient"/> class with specified details.
        /// </summary>
        /// <param name="name">The name of the ingredient.</param>
        /// <param name="ingType">The type of the ingredient.</param>
        /// <param name="amount">The amount of the ingredient.</param>
        /// <param name="unit">The unit of measurement for the ingredient amount.</param>
        public Ingredient(string name, string ingType, float amount, string unit) : this(name, ingType)
        {
            this.amount = amount;
            this.unit = unit;
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

        /// <summary>
        /// Gets the amount of the ingredient.
        /// </summary>
        /// <returns>The amount of the ingredient as a float.</returns>
        public float GetAmount() { return amount; }

        /// <summary>
        /// Sets the amount of the ingredient.
        /// </summary>
        /// <param name="amount">The amount to set for the ingredient.</param>
        public void SetAmount(float amount) { this.amount = amount; }

        /// <summary>
        /// Gets the unit of the ingredient.
        /// </summary>
        /// <returns>The unit of the ingredient as a string.</returns>
        public string GetUnit() { return unit; }


        /// <summary>
        /// Sets the unit of the ingredient.
        /// </summary>
        /// <param name="unit">The unit to set for the ingredient.</param>
        public void SetUnit(string unit) { this.unit = unit; }
    }
}
