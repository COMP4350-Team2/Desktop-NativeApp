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
        /// Retrieves the access token for the logged-in user.
        /// </summary>
        /// <returns>
        /// The access token as a string.
        /// </returns>
        public string GetAccessToken();

        /// <summary>
        /// Setter for setting user's access token
        /// </summary>
        /// <param name="accessToken"> new token</param>
        public void SetAccessToken(string accessToken);

        /// <summary>
        /// Method to get refresh token of user
        /// </summary>
        /// <returns> string of refresh token </returns>
        public string GetRefreshToken();


        /// <summary>
        /// Setter for setting user's refresh token
        /// </summary>
        /// <param name="refreshToken"> new token</param>
        public void SetRefreshToken(string refreshToken);

        /// <summary>
        /// Getter for a user's client id
        /// </summary>
        /// <returns>string of client id</returns>
        public string GetClientId();
    }
}
