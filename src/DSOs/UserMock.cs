namespace Desktop_Frontend.DSOs
{
    internal class UserMock : IUser
    {
        private string username;
        private bool loggedIn;
        private string myListsJSON;
        public UserMock()
        {
            username = "Mock User";
            myListsJSON = @"
            {
                ""lists"": [
                    {
                        ""id"": 0,
                        ""name"": ""Grocery List"",
                        ""ingredients"": [
                            {
                                ""name"": ""Apples"",
                                ""amount"": 5,
                                ""unit"": ""count""
                            },
                            {
                                ""name"": ""Milk"",
                                ""amount"": 1000,
                                ""unit"": ""ml""
                            }
                        ]
                    },
                    {
                        ""id"": 1,
                        ""name"": ""Pantry List"",
                        ""ingredients"": [
                            {
                                ""name"": ""Rice"",
                                ""amount"": 200,
                                ""unit"": ""g""
                            }
                        ]
                    }
                ]
            }";
        }

        public bool LoggedIn()
        {
            return loggedIn;
        }

        public async Task Login()
        {
            loggedIn = true;
        }

        public async Task Logout()
        {
            loggedIn = false;
        }

        public string UserName()
        {
            return username;
        }

        public string GetLists()
        {
            return myListsJSON;
        }

        public string GetAccessToken()
        {
            return "";
        }
    }
}
