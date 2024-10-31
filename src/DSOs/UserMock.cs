namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Mock implementation of the <see cref="IUser"/> interface
    /// </summary>
    internal class UserMock : IUser
    {
        private string username;
        private bool loggedIn;
        private List<UserList> myLists;

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
        /// Retrieves the lists of a user
        /// </summary>
        /// <returns>
        /// A list of <see cref="UserList"/>
        /// </returns>
        public List<UserList> GetUserLists() { return myLists; }

        /// <summary>
        /// Sets the user's lists.
        /// </summary>
        /// <param name="userLists">The lists of <see cref="UserList"/>.</param>
        public void SetUserLists(List<UserList> userLists) { this.myLists = userLists; }
    }
}
