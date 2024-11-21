using System.Windows;
using System.Windows.Controls;
using Desktop_Frontend.Backend;
using Desktop_Frontend.Components;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend
{
    /// <summary>
    /// Represents the logged-in window of the application. User's home page.
    /// </summary>
    public partial class LoggedInWindow : Window
    {
        private readonly IUser user; // The authenticated user instance
        private readonly IBackend backend; // The backend service instance
        private readonly AllIngredientsHandler allIngredientsHandler; // Handler for all ingredients
        private readonly MyListsHandler myListsHandler; // Handler for my lists

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInWindow"/> class.
        /// </summary>
        /// <param name="user">The authenticated <see cref="IUser"/> instance.</param>
        /// <param name="backend">The <see cref="IBackend"/> service instance.</param>
        public LoggedInWindow(IUser user, IBackend backend)
        {
            InitializeComponent();
            this.user = user; // Store the user instance
            this.backend = backend; // Store the backend instance
            allIngredientsHandler = new AllIngredientsHandler(backend, user); // Instantiate the ingredients handler
            myListsHandler = new MyListsHandler(backend, user); // Instantiate the lists handler

            InitializeContentSpace(); // Call the method to initialize content
            UsernameTextBox.Text = user.UserName(); // Set the username
        }

        /// <summary>
        /// Event handler for the logout button click event.
        /// Logs out the user and returns to the main window if successful.
        /// </summary>
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await user.Logout();

            // If user is logged out successfully
            if (!user.LoggedIn())
            {
                var mainWindow = new MainWindow();
                mainWindow.Show(); // Show the main window
                this.Close(); // Close the logged-in window
            }
            else
            {
                MessageBox.Show("Something went wrong with logging out. Try again."); // Error message
            }
        }

        /// <summary>
        /// Event handler for the "My Lists" button click event.
        /// Displays the "My Lists" section with placeholder content.
        /// </summary>
        private async void MyListsButton_Click(object sender, RoutedEventArgs e)
        {
            // Temporarily disable all buttons
            SetButtonsEnabled(false);

            // Enable the scrollbar
            ParentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            // Clear current content and display my lists section
            await myListsHandler.DisplayMyLists(ContentArea);

            // Enable buttons again
            SetButtonsEnabled(true);
        }

        /// <summary>
        /// Event handler for the "All Ingredients" button click event.
        /// Calls the method to display the list of ingredients.
        /// </summary>
        private async void AllIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Temporarily disable all buttons
            SetButtonsEnabled(false);

            // Remove the scrollbar
            ParentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

            // Display all ingredients using the handler
            await allIngredientsHandler.DisplayIngredientsAsync(ContentArea);

            // Enable buttons again
            SetButtonsEnabled(true);
            
        }

        /// <summary>
        /// Initializes the content space by displaying ingredients on initialization.
        /// </summary>
        private async void InitializeContentSpace()
        {
            // Remove the scrollbar
            ParentScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

            // Display all ingredients when the window is initialized
            await allIngredientsHandler.DisplayIngredientsAsync(ContentArea);
        }

        /// <summary>
        /// Enables or disables all buttons in the window.
        /// </summary>
        /// <param name="isEnabled">Indicates whether buttons should be enabled or disabled.</param>
        private void SetButtonsEnabled(bool isEnabled)
        {
            AllIngredientsButton.IsEnabled = isEnabled;
            MyListsButton.IsEnabled = isEnabled;
            LogoutButton.IsEnabled = isEnabled;
        }

    }
}
