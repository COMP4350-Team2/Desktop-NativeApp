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

            // Display a placeholder message for "My Lists" section
            TextBlock placeholderText = new TextBlock
            {
                Text = "Coming soon: A page for your lists",
                FontSize = 18,
                Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)), // Brown text color
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            ContentArea.Children.Add(placeholderText);
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
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

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
                    searchBox.Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)); // Brown color for placeholder
                }
            };

            // Add the search TextBox to the StackPanel
            stackPanel.Children.Add(searchBox);

            // Create a StackPanel to display each ingredient with a "+" button
            StackPanel ingredientListPanel = new StackPanel();

            // Fetch the ingredients from the backend
            List<Ingredient> ingredients = await backend.GetAllIngredients(user);

            // Add each ingredient with a "+" button anchored to the right
            foreach (var ingredient in ingredients)
            {
                // Create a DockPanel for each ingredient row
                DockPanel ingredientRow = new DockPanel
                {
                    Margin = new Thickness(0, 5, 0, 5)
                };

                // Create a TextBlock for the ingredient name and type
                TextBlock ingredientText = new TextBlock
                {
                    Text = $"{ingredient.GetName()} - {ingredient.GetIngType()}",
                    Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)), // Brown text color
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                // Add the ingredient text to the left side of the DockPanel
                DockPanel.SetDock(ingredientText, Dock.Left);
                ingredientRow.Children.Add(ingredientText);

                // Create the "+" button and anchor it to the right
                Button addButton = new Button
                {
                    Content = "+",
                    Width = 30,
                    Height = 30,
                    Background = new SolidColorBrush(Color.FromRgb(237, 220, 126)), // Yellow background
                    Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)), // Brown text color
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                addButton.Click += (s, e) =>
                {
                    MessageBox.Show("Coming soon: Adding ingredients to your lists");
                };

                // Add the button to the right side of the DockPanel
                DockPanel.SetDock(addButton, Dock.Right);
                ingredientRow.Children.Add(addButton);

                // Add the DockPanel to the main StackPanel
                ingredientListPanel.Children.Add(ingredientRow);
            }

            // Add the search filter functionality
            searchBox.TextChanged += (s, e) =>
            {
                ingredientListPanel.Children.Clear(); // Clear for filtered results

                string filter = searchBox.Text.ToLower(); // Get the filter text in lowercase
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.GetName().ToLower().Contains(filter) || ingredient.GetIngType().ToLower().Contains(filter))
                    {
                        // Create a new DockPanel row for each filtered ingredient
                        DockPanel filteredRow = new DockPanel
                        {
                            Margin = new Thickness(0, 5, 0, 5)
                        };

                        TextBlock filteredText = new TextBlock
                        {
                            Text = $"{ingredient.GetName()} - {ingredient.GetIngType()}",
                            Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)),
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 0, 10, 0)
                        };

                        Button filteredAddButton = new Button
                        {
                            Content = "+",
                            Width = 30,
                            Height = 30,
                            Background = new SolidColorBrush(Color.FromRgb(237, 220, 126)),
                            Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)),
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(10, 0, 0, 0)
                        };

                        filteredAddButton.Click += (s, e) =>
                        {
                            MessageBox.Show("Coming soon: Adding ingredients to your lists");
                        };

                        DockPanel.SetDock(filteredText, Dock.Left);
                        DockPanel.SetDock(filteredAddButton, Dock.Right);

                        filteredRow.Children.Add(filteredText);
                        filteredRow.Children.Add(filteredAddButton);

                        ingredientListPanel.Children.Add(filteredRow);
                    }
                }
            };

            // Add the ListPanel to the StackPanel
            stackPanel.Children.Add(ingredientListPanel);

            // Add the StackPanel to the ContentArea
            ContentArea.Children.Add(stackPanel);
        }

    }
}

