using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Backend
{
    public static class BackendFactory
    {
        public static BackendConfig config;
        public static IBackend CreateBackend(IUser user)
        {
            if (IsMockEnvironment(user))
            {
                return new BackendMock(user);
            }
            else
            {
                return new Backend(config);
            }
        }

        private static bool IsMockEnvironment(IUser user)
        {
            config = new BackendConfig();

            return user is UserMock || !config.ConfigValid;
        }
    }
}
