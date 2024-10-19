namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Mock implementation of the <see cref="IUser"/> interface
    /// </summary>
    internal class UserMock : IUser
    {
        private string username;
        private bool loggedIn;
        private string myListsJSON;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMock"/> class.
        /// </summary>
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

        /// <summary>
        /// Indicates whether the user is logged in.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the user is logged in; otherwise, <c>false</c>.
        /// </returns>
        public bool LoggedIn()
        {
            return loggedIn;
        }

        /// <summary>
        /// Simulates the login process for the mock user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Login()
        {
            loggedIn = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Simulates the logout process for the mock user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task Logout()
        {
            loggedIn = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves the username of the mock user.
        /// </summary>
        /// <returns>The username of the mock user.</returns>
        public string UserName()
        {
            return username;
        }

        /// <summary>
        /// Retrieves the JSON representation of the user's lists.
        /// </summary>
        /// <returns>A JSON string representing the user's lists.</returns>
        public string GetLists()
        {
            return myListsJSON;
        }

        /// <summary>
        /// Retrieves an access token for the mock user.
        /// </summary>
        /// <returns>An empty string as no access token is provided for the mock user.</returns>
        public string GetAccessToken()
        {
            return "";
        }
    }
}
