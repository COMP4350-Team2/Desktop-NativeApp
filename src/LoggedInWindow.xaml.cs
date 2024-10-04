using System.Windows;
using Auth0.OidcClient;

namespace Desktop_Frontend
{
    public partial class LoggedInWindow : Window
    {
        private Auth0Client auth0Client;

        // Constructor accepting Auth0Client
        public LoggedInWindow(Auth0Client client)
        {
            InitializeComponent();
            auth0Client = client; // Assign the passed instance
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Log the user out
            await auth0Client.LogoutAsync();

            // Close the logged-in window and open the main window
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
