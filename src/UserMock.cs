
using System.Diagnostics;

namespace Desktop_Frontend
{
    internal class UserMock: IUser
    {
        private string username;
        private bool loggedIn;
        public UserMock() 
        {
            username = "Mock User";
        }

        public bool LoggedIn()
        {
            return loggedIn;
        }

        public async Task Login()
        {
            loggedIn = true;
        }

        public async Task Logout()
        {
            loggedIn = false;
        }

        public string UserName()
        {
            return username;
        }
    }
}
