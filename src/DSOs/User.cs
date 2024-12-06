using Auth0.OidcClient;
using Desktop_Frontend.Auth0;
using System.Windows;

namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// Represents a user that interacts with Auth0 for authentication.
    /// Implements the <see cref="IUser"/> interface.
    /// </summary>
    internal class User : IUser
    {
        private Auth0Config config;
        private Auth0Client auth0Client;
        private bool loggedIn;
        private string username;
        private string accessToken;
        private string refreshToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// with default configuration.
        /// </summary>
        public User() : this(new Auth0Config()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// with the specified Auth0 configuration.
        /// </summary>
        /// <param name="config">The Auth0 configuration of type <see cref="Auth0Config"/></param>
        public User(Auth0Config config)
        {
            this.config = config;
            username = "Auth0 User";
            accessToken = "";

            Auth0ClientOptions clientOptions = new Auth0ClientOptions
            {
                Domain = config.Domain,
                ClientId = config.ClientId,
                Scope = "openid profile email offline_access"
            };

            auth0Client = new Auth0Client(clientOptions);
            clientOptions.PostLogoutRedirectUri = clientOptions.RedirectUri;
        }

        /// <summary>
        /// Asynchronously logs the user into the Auth0 authentication system.
        /// </summary>
        /// <returns>A task representing the asynchronous login operation.</returns>
        public async Task Login()
        {
            try
            {
                var loginResult = await auth0Client.LoginAsync(new
                {
                    audience = config.Audience
                });

                accessToken = loginResult.AccessToken;
                refreshToken = loginResult.RefreshToken;

                if (!loginResult.IsError && 
                    !string.IsNullOrEmpty(accessToken) && 
                    !string.IsNullOrEmpty(refreshToken))
                {
                    loggedIn = true;
                }
            }
            catch (Exception)
            {
                accessToken = "";
                refreshToken = "";
                loggedIn = false;
            }
        }

        /// <summary>
        /// Asynchronously logs the user out of the Auth0 authentication system.
        /// </summary>
        /// <returns>A task representing the asynchronous logout operation.</returns>
        public async Task Logout()
        {
            try
            {
                await auth0Client.LogoutAsync();
                loggedIn = false;
                accessToken = "";
                refreshToken = "";
            }
            catch (Exception)
            {
                loggedIn = true;
            }
        }

        /// <summary>
        /// Checks if the user is currently logged in.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the user is logged in.
        /// </returns>
        public bool LoggedIn() { return loggedIn; }


        /// <summary>
        /// Retrieves the username of the authenticated user.
        /// </summary>
        /// <returns>
        /// The username as a string.
        /// </returns>
        public string UserName() { return username; }


        /// <summary>
        /// Retrieves the access token for the logged-in user.
        /// </summary>
        /// <returns>
        /// The access token as a string.
        /// </returns>
        public string GetAccessToken() { return accessToken; }

        /// <summary>
        /// Setter for setting user's access token
        /// </summary>
        /// <param name="accessToken"> new token</param>
        public void SetAccessToken(string accessToken) { this.accessToken = accessToken; }

        /// <summary>
        /// Method to get refresh token of user
        /// </summary>
        /// <returns> string of refresh token </returns>
        public string GetRefreshToken() { return refreshToken; }

        /// <summary>
        /// Setter for setting user's refresh token
        /// </summary>
        /// <param name="refreshToken"> new token</param>
        public void SetRefreshToken(string refreshToken) { this.refreshToken = refreshToken; }

        /// <summary>
        /// Getter for a user's client id
        /// </summary>
        /// <returns>string of client id</returns>
        public string GetClientId() { return config.ClientId; }
    }
}
