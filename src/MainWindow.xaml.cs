using System.Windows;

namespace Desktop_Frontend
{
    public partial class MainWindow : Window
    {
        private IUser user;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUser();
        }


        private void InitializeUser()
        {
            user = UserFactory.CreateUser();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await user.Login();

            if (user.LoggedIn())
            {
                var loggedInWindow = new LoggedInWindow(user);
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
