namespace Desktop_Frontend.DSOs
{
    public interface IUser
    {
        public Task Login();

        public Task Logout();

        public bool LoggedIn();

        public string UserName();

        public string GetLists();

        public string GetAccessToken();
    }
}
