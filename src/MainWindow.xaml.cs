using System.Windows;
using Auth0.OidcClient;

namespace Desktop_Frontend
{
    public partial class MainWindow : Window
    {
        private Auth0Client auth0Client;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAuth0Client(); // Initialize Auth0 client
        }

        private void InitializeAuth0Client()
        {
            //var config = new Auth0Config(); // Load Auth0 configuration

            //auth0Client = new Auth0Client(new Auth0ClientOptions
            //{
            //    Domain = config.Domain,
            //    ClientId = config.ClientId,
            //    RedirectUri = config.CallbackUrl
            //});

            var config = new Auth0Config();

            Auth0ClientOptions clientOptions = new Auth0ClientOptions
            {
                Domain = config.Domain,
                ClientId = config.ClientId
            };

            auth0Client = new Auth0Client(clientOptions);

            clientOptions.PostLogoutRedirectUri = clientOptions.RedirectUri;

        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginResult = await auth0Client.LoginAsync();

            if (loginResult.IsError)
            {
                MessageBox.Show($"Error: {loginResult.Error}");
                return;
            }

            //MessageBox.Show($"Logged in! Access Token: {loginResult.AccessToken}");

            // Open the logged-in window and pass the auth0Client instance
            var loggedInWindow = new LoggedInWindow(auth0Client);
            loggedInWindow.Show();

            // Optionally, close the main window
            this.Close();
        }
    }
}
