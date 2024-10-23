using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend
{
    /// <summary>
    /// Represents the logged-in window of the application. User's home page.
    /// </summary>
    public partial class LoggedInWindow : Window
    {
        // Define color variables
        private SolidColorBrush primaryColor = new SolidColorBrush(Color.FromRgb(27, 38, 44)); // Dark color
        private SolidColorBrush secondaryColor = new SolidColorBrush(Color.FromRgb(15, 76, 117)); // Medium color
        private SolidColorBrush tertiaryColor = new SolidColorBrush(Color.FromRgb(50, 130, 184)); // Light color
        private SolidColorBrush backgroundColor = new SolidColorBrush(Color.FromRgb(187, 225, 250)); // Light background color
        private SolidColorBrush textColor = new SolidColorBrush(Color.FromRgb(187, 225, 250)); // Text color set to #BBE1FA
        private SolidColorBrush buttonColor = new SolidColorBrush(Color.FromRgb(27, 38, 44)); // Button color set to #1B262C

        private IUser user;
        private IBackend backend; // Declare backend variable

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInWindow"/> class.
        /// </summary>
        /// <param name="user">The authenticated <see cref="IUser"/> instance.</param>
        /// <param name="backend">The <see cref="IBackend"/> service instance.</param>
        public LoggedInWindow(IUser user, IBackend backend)
        {
            InitializeComponent();
            this.user = user;
            this.backend = backend;

            InitializeContentSpace(); // Call the method to initialize content
            UsernameTextBox.Text = $"{user.UserName()}"; // Set the username
        }

        /// <summary>
        /// Event handler for the logout button click event.
        /// Logs out the user and returns to the main window if successful.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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

        /// <summary>
        /// Event handler for the "My Lists" button click event.
        /// Displays a placeholder message indicating that the functionality is coming soon.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void MyListsButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the ContentArea
            ContentArea.Children.Clear();

            // Create and add the "My Lists" header
            TextBlock myListsHeader = new TextBlock
            {
                Text = "My Lists",
                FontSize = 24,
                Foreground = textColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10)
            };
            ContentArea.Children.Add(myListsHeader);

            // Display a placeholder message for "My Lists" section
            TextBlock placeholderText = new TextBlock
            {
                Text = "Coming soon: A page for your lists",
                FontSize = 18,
                Foreground = textColor, // Use textColor for visibility
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            ContentArea.Children.Add(placeholderText);
        }

        /// <summary>
        /// Event handler for the "All Ingredients" button click event.
        /// Calls the method to display the list of ingredients.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void AllIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the method to display ingredients
            await DisplayIngredients();
        }

        /// <summary>
        /// Initializes the content space by displaying ingredients on initialization.
        /// </summary>
        private async void InitializeContentSpace()
        {
            // Call the method to display ingredients on initialization
            await DisplayIngredients();
        }

        /// <summary>
        /// Displays the list of ingredients fetched from the backend.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DisplayIngredients()
        {
            // Clear any existing content
            ContentArea.Children.Clear();

            // Create and add the "All Ingredients" header
            TextBlock allIngredientsHeader = new TextBlock
            {
                Text = "All Ingredients",
                FontSize = 24,
                Foreground = textColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10) // Margin above header
            };
            ContentArea.Children.Add(allIngredientsHeader);

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
                Foreground = textColor, // Use textColor
                Background = backgroundColor, // Use backgroundColor
                Margin = new Thickness(0, 40, 0, 10), 
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
                    Foreground = textColor, // Use textColor for visibility
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
                    Background = backgroundColor, // Use backgroundColor
                    Foreground = buttonColor, // Use buttonColor set to #1B262C
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
                            Foreground = textColor, // Use textColor for visibility
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 0, 10, 0)
                        };

                        // Add the filtered ingredient text to the left side of the DockPanel
                        DockPanel.SetDock(filteredText, Dock.Left);
                        filteredRow.Children.Add(filteredText);

                        // Create the "+" button for the filtered ingredient
                        Button filteredAddButton = new Button
                        {
                            Content = "+",
                            Width = 30,
                            Height = 30,
                            Background = backgroundColor, // Use backgroundColor
                            Foreground = buttonColor, // Use buttonColor set to #1B262C
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Margin = new Thickness(10, 0, 0, 0)
                        };

                        filteredAddButton.Click += (s, e) =>
                        {
                            MessageBox.Show("Coming soon: Adding ingredients to your lists");
                        };

                        // Add the button to the right side of the DockPanel
                        DockPanel.SetDock(filteredAddButton, Dock.Right);
                        filteredRow.Children.Add(filteredAddButton);

                        // Add the filtered row to the ingredient list panel
                        ingredientListPanel.Children.Add(filteredRow);
                    }
                }
            };

            // Add the ingredient list panel to the main StackPanel
            stackPanel.Children.Add(ingredientListPanel);
            ContentArea.Children.Add(stackPanel);
        }
    }
}

