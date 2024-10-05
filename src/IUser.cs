
namespace Desktop_Frontend
{
    public interface IUser
    {
        public Task Login();

        public Task Logout();

        public bool LoggedIn();

        public string UserName();
    }
}
