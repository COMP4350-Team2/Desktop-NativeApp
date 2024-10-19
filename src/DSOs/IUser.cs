namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Interface for User 
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Asynchronously logs the user into the application.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Login();

        /// <summary>
        /// Asynchronously logs the user out of the application.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Logout();

        /// <summary>
        /// Checks if the user is currently logged in.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the user is logged in.
        /// </returns>
        public bool LoggedIn();

        /// <summary>
        /// Retrieves the username of the logged-in user.
        /// </summary>
        /// <returns>
        /// The username of the user as a string.
        /// </returns>
        public string UserName();

        /// <summary>
        /// Retrieves a string representation of the user's lists.
        /// </summary>
        /// <returns>
        /// A string containing the user's lists.
        /// </returns>
        public string GetLists();

        /// <summary>
        /// Retrieves the access token for the logged-in user.
        /// </summary>
        /// <returns>
        /// The access token as a string.
        /// </returns>
        public string GetAccessToken();
    }
}
