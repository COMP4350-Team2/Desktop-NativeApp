namespace Desktop_Frontend.DSOs
{
    public class Ingredient
    {
        private string name;
        private string ingType;

        public Ingredient() : this("No Name", "No Type") { }

        public Ingredient(string name, string ingType)
        {
            this.name = name;
            this.ingType = ingType;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetIngType()
        {
            return ingType;
        }

        public void SetIngType(string ingType)
        {
            this.ingType = ingType;
        }

    }
}
