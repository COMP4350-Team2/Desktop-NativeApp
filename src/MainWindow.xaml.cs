using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend
{
    public partial class MainWindow : Window
    {
        private IUser user;
        private IBackend backend;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUser();
            InitializeBackend();
        }


        private void InitializeUser()
        {
            user = UserFactory.CreateUser();
        }

        private void InitializeBackend()
        {
            backend = BackendFactory.CreateBackend(user);
        }


        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await user.Login();

            if (user.LoggedIn())
            {
                var loggedInWindow = new LoggedInWindow(user, backend);
                loggedInWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong with logging in. Try again.");
            }          

        }
    }
}
