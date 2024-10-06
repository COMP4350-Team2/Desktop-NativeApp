﻿using Auth0.OidcClient;
using System.Diagnostics;
namespace Desktop_Frontend
{
    internal class User : IUser
    {
        private Auth0Config config;
        private Auth0Client auth0Client;
        private bool loggedIn;
        private string username;

        public User() : this(new Auth0Config()) { }

        public User(Auth0Config config)
        {
            this.config = config;
            username = "Auth0 User";

            Auth0ClientOptions clientOptions = new Auth0ClientOptions
            {
                Domain = config.Domain,
                ClientId = config.ClientId
            };

            auth0Client = new Auth0Client(clientOptions);

            clientOptions.PostLogoutRedirectUri = clientOptions.RedirectUri;
        }

        // Async login method
        public async Task Login()
        {
            try
            {
                await auth0Client.LoginAsync();
                loggedIn = true;

            }
            catch (Exception e)
            {
                Debug.WriteLine("> Auth0 Logout Failed > >", e.ToString());
                loggedIn = false;
            }
        }

        // Async logout method 
        public async Task Logout()
        {
            try
            {
                await auth0Client.LogoutAsync();
                loggedIn = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("> Auth0 Login Failed > >", e.ToString());
                loggedIn = true;
            }
        }

        private bool ConfigValid()
        {
            return config != null && config.ConfigValid;
        }

        public bool LoggedIn()
        {
            return loggedIn;
        }

        public string UserName()
        {
            return username;
        }
    }
}