using Desktop_Frontend.Auth0;

namespace Desktop_Frontend.DSOs
{
    /// <summary>
    /// A factory class for creating instances of <see cref="IUser"/>.
    /// </summary>
    internal static class UserFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IUser"/> based on the validity of the Auth0 configuration.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IUser"/>. Returns a real <see cref="User"/> instance if the 
        /// configuration is valid, otherwise returns a <see cref="UserMock"/> instance.
        /// </returns>
        public static IUser CreateUser()
        {
            Auth0Config config = new Auth0Config();

            if (config.ConfigValid)
            {
                // Load the configuration from the .env file and return a real User instance
                return new User(config);
            }
            else
            {
                // Return a mock user instance when .env is not present
                return new UserMock();
            }
        }
    }
}
