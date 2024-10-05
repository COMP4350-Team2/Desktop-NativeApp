using System.Diagnostics;
using System.IO;

namespace Desktop_Frontend
{
    internal static class UserFactory
    {
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
