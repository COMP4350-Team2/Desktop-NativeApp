namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Mock implementation of the <see cref="IUser"/> interface
    /// </summary>
    internal class UserMock : IUser
    {
        private string ?username;
        private bool loggedIn;
        private List<UserList> ?myLists;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMock"/> class.
        /// </summary>
        public UserMock() { username = "Mock User"; }

        /// <summary>
        /// Indicates whether the user is logged in.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the user is logged in; otherwise, <c>false</c>.
        /// </returns>
        public bool LoggedIn() { return loggedIn; }

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
        public string UserName() { return username; }
 

        /// <summary>
        /// Retrieves an access token for the mock user.
        /// </summary>
        /// <returns>An empty string as no access token is provided for the mock user.</returns>
        public string GetAccessToken() { return "";}

        /// <summary>
        /// Setter for setting user's access token
        /// </summary>
        /// <param name="accessToken"> new token</param>
        public void SetAccessToken(string accessToken) { }

        /// <summary>
        /// Method to get refresh token of user
        /// </summary>
        /// <returns> string of refresh token </returns>
        public string GetRefreshToken() { return ""; }

        /// <summary>
        /// Setter for setting user's refresh token
        /// </summary>
        /// <param name="refreshToken"> new token</param>
        public void SetRefreshToken(string refreshToken) { }

        /// <summary>
        /// Getter for a user's client id
        /// </summary>
        /// <returns>string of client id</returns>
        public string GetClientId() { return ""; }
    }
}
