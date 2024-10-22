using System.Windows;
using System.Windows.Controls;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend
{
    /// <summary>
    /// Main window for the application, responsible for user login and initializing backend services.
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUser user;
        private IBackend backend;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeUser();
            InitializeBackend();
        }

        /// <summary>
        /// Initializes the user instance using the <see cref="UserFactory"/>.
        /// </summary>
        private void InitializeUser()
        {
            user = UserFactory.CreateUser();
        }

        /// <summary>
        /// Initializes the backend instance using the <see cref="BackendFactory"/>.
        /// </summary>
        private void InitializeBackend()
        {
            backend = BackendFactory.CreateBackend(user);
        }

        /// <summary>
        /// Event handler for the login button click event.
        /// Disables the button to prevent multiple clicks and performs user login.
        /// If the login is successful, opens the logged-in window; otherwise, shows an error message.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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

        /// <summary>
        /// Checks if the user is registered by attempting to create a user through the backend.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, with a result indicating
        /// whether the user registration was successful.
        /// </returns>
        private async Task<bool> UserRegistered()
        {
            return await backend.CreateUser(user);
        }
    }
}
