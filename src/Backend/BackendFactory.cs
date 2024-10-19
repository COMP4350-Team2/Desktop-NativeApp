using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    /// <summary>
    /// Factory class for creating instances of the <see cref="IBackend"/> implementation.
    /// </summary>
    public static class BackendFactory
    {
        /// <summary>
        /// The configuration for the backend. 
        /// May be <c>null</c> if not initialized.
        /// </summary>
        public static BackendConfig? config;

        /// <summary>
        /// Creates an instance of the <see cref="IBackend"/> based on the environment.
        /// If in a mock environment, it returns a <see cref="BackendMock"/>; otherwise, it returns a real <see cref="Backend"/>.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> used to determine the environment.</param>
        /// <returns>
        /// An instance of <see cref="IBackend"/> that represents the backend service.
        /// </returns>
        public static IBackend CreateBackend(IUser user)
        {
            if (IsMockEnvironment(user))
            {
                return new BackendMock(user);
            }
            else
            {
                if (config == null)
                {
                    return new Backend();
                }
                else
                {
                    return new Backend(config);
                }
            }
        }

        /// <summary>
        /// Determines whether the current environment is a mock environment.
        /// </summary>
        /// <param name="user">The user of type <see cref="IUser"/> to check against.</param>
        /// <returns>
        /// <c>true</c> if the environment is a mock or if the configuration is invalid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsMockEnvironment(IUser user)
        {
            config = new BackendConfig();

            return user is UserMock || !config.ConfigValid;
        }
    }
}
