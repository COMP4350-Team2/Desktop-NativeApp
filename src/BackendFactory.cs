namespace Desktop_Frontend
{
    public static class BackendFactory
    {
        public static IBackend CreateBackend(IUser user)
        {
            if (IsMockEnvironment(user))
            {
                return new BackendMock(user);
            }
            else
            {
                return new Backend(user);
            }
        }

        private static bool IsMockEnvironment(IUser user)
        {
            BackendConfig config = new BackendConfig();

            return (user is UserMock) || (!config.ConfigValid);
        }
    }
}
