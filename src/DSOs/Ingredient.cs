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
            SetName(name);
            SetIngType(ingType);
            SetAmount(1);
            SetUnit("No Unit");
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

            SetAmount(amount);
            SetUnit(unit);
        }


        /// <summary>
        /// Gets the name of the ingredient.
        /// </summary>
        /// <returns>The name of the ingredient as a string.</returns>
        public string GetName() { return name; }

        /// <summary>
        /// Sets the name of the ingredient.
        /// </summary>
        /// <param name="name">The name to set for the ingredient.</param>
        public void SetName(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                this.name = name;
            }
            else
            {
                this.name = "No Name";
            }
            
        }

        /// <summary>
        /// Gets the type of the ingredient.
        /// </summary>
        /// <returns>The type of the ingredient as a string.</returns>
        public string GetIngType() { return ingType; }


        /// <summary>
        /// Sets the type of the ingredient.
        /// </summary>
        /// <param name="ingType">The type to set for the ingredient.</param>
        public void SetIngType(string ingType)
        {
            if (!string.IsNullOrEmpty(ingType))
            {
                this.ingType = ingType;
            }
            else
            {
                this.ingType = "No Type";
            }
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
        public void SetAmount(float amount) 
        {
            if (amount > 0)
            {
                this.amount = amount;
            } 
        }

        /// <summary>
        /// Gets the unit of the ingredient.
        /// </summary>
        /// <returns>The unit of the ingredient as a string.</returns>
        public string GetUnit() { return unit; }


        /// <summary>
        /// Sets the unit of the ingredient.
        /// </summary>
        /// <param name="unit">The unit to set for the ingredient.</param>
        public void SetUnit(string unit) 
        {
            string[] acceptableUnits = ["count", "g", "kg", "lb", "oz", "mL", "L", "gal"];

            if (!string.IsNullOrEmpty(name) && acceptableUnits.Contains(unit))
            {
                this.unit = unit;
            }
            else
            {
                this.unit = "count";
            }
        }


        /// <summary>
        /// Returns if 2 ingredients are the same
        /// </summary>
        /// <param name="other">The other ingredient to be compared to.</param>
        /// <returns>The true if their name, type and unit are the same.</returns>
        public bool IsEqual(Ingredient other)
        {
            return (name == other.GetName() && ingType == other.GetIngType() && unit == other.GetUnit());
        }

        /// <summary>
        /// Returns a copy of the ingredient
        /// </summary>
        /// <returns>Deep copy of this ingredient.</returns>
        public Ingredient CopyIngredient()
        {
            Ingredient copy = new Ingredient(this.GetName(), this.GetIngType(), this.GetAmount(), this.GetUnit());
            return copy;
        }
    }
}
