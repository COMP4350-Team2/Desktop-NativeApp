using System.Windows;
using System.Windows.Controls;
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
            // Disable the button to prevent multiple clicks
            var button = sender as Button;
            button.IsEnabled = false;

            await user.Login();

            bool loginSuccessful = user.LoggedIn() && await UserRegistered();

            if (loginSuccessful)
            {
                var loggedInWindow = new LoggedInWindow(user, backend);
                loggedInWindow.Show();
                this.Close();
            }
            else
            {
                button.IsEnabled = true;
                MessageBox.Show("Something went wrong with logging in. Try again.");
            }          

        }

        private async Task<bool> UserRegistered()
        {
            return await backend.CreateUser(user);
        }
    }
}
