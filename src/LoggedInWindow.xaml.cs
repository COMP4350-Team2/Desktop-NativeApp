using System.Windows;

namespace Desktop_Frontend
{
    public partial class LoggedInWindow : Window
    {
        private IUser user;

        // Constructor accepting Auth0Client
        public LoggedInWindow(IUser user)
        {
            InitializeComponent(); 
            this.user = user;

            UsernameTextBox.Text = $"Username: {user.UserName()}";
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await user.Logout();

            //if user is logged out successfully
            if (!user.LoggedIn())
            {
                // Close the logged-in window and open the main window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            //if not
            else
            {
                MessageBox.Show("Something went wrong with logging out. Try again.");
            }

        }
    }
}
