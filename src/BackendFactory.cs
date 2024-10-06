namespace Desktop_Frontend
{
    public static class BackendFactory
    {
        public static IBackend CreateBackend(IUser user)
        {
            if (user is UserMock)
            {
                return new BackendMock(user);
            }
            else
            {
                return new Backend(user);
            }
        }
    }
}
