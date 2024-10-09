using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend
{
    public partial class LoggedInWindow : Window
    {
        private IUser user;
        private IBackend backend; // Declare backend variable

        // Constructor accepting Auth0Client
        public LoggedInWindow(IUser user, IBackend backend)
        {
            InitializeComponent();
            this.user = user;
            this.backend = backend;

            InitializeContentSpace(); // Call the method to initialize content
            UsernameTextBox.Text = $"{user.UserName()}"; // Set the username
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await user.Logout();

            // If user is logged out successfully
            if (!user.LoggedIn())
            {
                // Close the logged-in window and open the main window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong with logging out. Try again.");
            }
        }

        private void MyListsButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the ContentArea
            ContentArea.Children.Clear();
        }

        private async void AllIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the method to display ingredients
            await DisplayIngredients();
        }

        private async void InitializeContentSpace()
        {
            // Call the method to display ingredients on initialization
            await DisplayIngredients();
        }

        private async Task DisplayIngredients()
        {
            // Clear any existing content
            ContentArea.Children.Clear();

            // Create a StackPanel for the ingredients
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10);

            // Create the search TextBox with placeholder text
            TextBox searchBox = new TextBox
            {
                Width = double.NaN, // Set width to auto-fill available space
                Height = 30,
                Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)), // Brown text color
                Background = new SolidColorBrush(Color.FromRgb(237, 220, 126)), // Yellow background
                Margin = new Thickness(0, 0, 0, 10),
                Text = "Search ingredients..."
            };

            // Handle focus events for placeholder
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...")
                {
                    searchBox.Text = "";
                    searchBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            };
            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search ingredients...";
                    searchBox.Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)); // Brown color when placeholder
                }
            };

            // Add the search TextBox to the StackPanel
            stackPanel.Children.Add(searchBox);

            // Create a ListBox to display ingredients
            ListBox ingredientListBox = new ListBox
            {
                Margin = new Thickness(0, 5, 0, 0), // Space above the ListBox
                Background = new SolidColorBrush(Color.FromRgb(237, 220, 126)), // Yellow background
                Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)), // Brown text color
            };

            // Fetch the ingredients from the backend
            List<Ingredient> ingredients = await backend.GetAllIngredients();

            // Add each ingredient to the ListBox
            foreach (var ingredient in ingredients)
            {
                ingredientListBox.Items.Add(ingredient.GetName()); // Assuming GetName returns the ingredient name
            }

            // Add the search filter functionality
            searchBox.TextChanged += (s, e) =>
            {
                ingredientListBox.Items.Clear(); // Clear the ListBox

                string filter = searchBox.Text.ToLower(); // Get the filter text in lowercase
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.GetName().ToLower().Contains(filter))
                    {
                        ingredientListBox.Items.Add(ingredient.GetName()); // Add item if it matches the filter
                    }
                }
            };

            // Add the ListBox to the StackPanel
            stackPanel.Children.Add(ingredientListBox);

            // Add the StackPanel to the ContentArea
            ContentArea.Children.Add(stackPanel);
        }
    }
}
