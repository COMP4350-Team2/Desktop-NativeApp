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
        private IBackend backend;

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
        private void MyListsButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the ContentArea
            ContentArea.Children.Clear();

            // Create and add the "My Lists" header
            TextBlock myListsHeader = CreateHeader("My Lists");
            ContentArea.Children.Add(myListsHeader);

            // Create a StackPanel to center the content
            StackPanel centerPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 40, 0, 0) 
            };

            // Display a placeholder message for "My Lists" section
            TextBlock placeholderText = CreatePlaceholderText("Coming soon: A page for your lists");
            placeholderText.HorizontalAlignment = HorizontalAlignment.Center; // Center the text
            centerPanel.Children.Add(placeholderText);

            // Add the centered panel to the ContentArea
            ContentArea.Children.Add(centerPanel);
        }


        /// <summary>
        /// Event handler for the "All Ingredients" button click event.
        /// Calls the method to display the list of ingredients.
        /// </summary>
        private async void AllIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            await DisplayIngredients(); // Call the method to display ingredients
        }

        /// <summary>
        /// Initializes the content space by displaying ingredients on initialization.
        /// </summary>
        private async void InitializeContentSpace()
        {
            await DisplayIngredients(); // Call the method to display ingredients on initialization
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
            TextBlock allIngredientsHeader = CreateHeader("All Ingredients");
            ContentArea.Children.Add(allIngredientsHeader);

            // Create a StackPanel for the ingredients
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            // Create the search TextBox
            TextBox searchBox = CreateSearchBox();
            stackPanel.Children.Add(searchBox);

            // Create a StackPanel to display each ingredient with a "+" button
            StackPanel ingredientListPanel = new StackPanel();
            stackPanel.Children.Add(ingredientListPanel);

            // Fetch the ingredients from the backend
            List<Ingredient> ingredients = await backend.GetAllIngredients(user);
            PopulateIngredientList(ingredients, ingredientListPanel); // Populate the ingredient list

            // Add search filter functionality
            searchBox.TextChanged += (s, e) => FilterIngredients(ingredients, searchBox.Text, ingredientListPanel);

            ContentArea.Children.Add(stackPanel); // Add the main stack panel to the ContentArea
        }

        /// <summary>
        /// Populates the ingredient list in the given panel.
        /// </summary>
        /// <param name="ingredients">The list of ingredients to display.</param>
        /// <param name="ingredientListPanel">The panel to populate with ingredients.</param>
        private void PopulateIngredientList(List<Ingredient> ingredients, StackPanel ingredientListPanel)
        {
            ingredientListPanel.Children.Clear(); // Clear existing ingredients
            foreach (var ingredient in ingredients)
            {
                ingredientListPanel.Children.Add(CreateIngredientRow(ingredient)); // Create a row for each ingredient
            }
        }

        /// <summary>
        /// Creates a search TextBox with placeholder text.
        /// </summary>
        /// <returns>The created TextBox.</returns>
        private TextBox CreateSearchBox()
        {
            TextBox searchBox = new TextBox
            {
                Width = double.NaN, // Set width to auto-fill available space
                Height = 30,
                Foreground = textColor,
                Background = backgroundColor,
                Margin = new Thickness(0, 40, 0, 10),
                Text = "Search ingredients..."
            };

            // Handle focus events for placeholder
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...")
                {
                    searchBox.Text = string.Empty; // Clear placeholder
                    searchBox.Foreground = new SolidColorBrush(Colors.Black); // Change text color to black
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search ingredients..."; // Reset placeholder
                    searchBox.Foreground = new SolidColorBrush(Color.FromRgb(70, 48, 24)); // Brown color for placeholder
                }
            };

            return searchBox;
        }

        /// <summary>
        /// Creates an ingredient row with a "+" button.
        /// </summary>
        /// <param name="ingredient">The ingredient to display.</param>
        /// <returns>The created DockPanel for the ingredient.</returns>
        private DockPanel CreateIngredientRow(Ingredient ingredient)
        {
            DockPanel ingredientRow = new DockPanel
            {
                Margin = new Thickness(0, 5, 0, 5)
            };

            TextBlock ingredientText = new TextBlock
            {
                Text = $"{ingredient.GetName()} - {ingredient.GetIngType()}",
                Foreground = textColor,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };

            ingredientRow.Children.Add(ingredientText); // Add ingredient text to the row

            Button addButton = new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Background = backgroundColor,
                Foreground = buttonColor,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 0, 0)
            };

            addButton.Click += (s, e) =>
            {
                MessageBox.Show("Coming soon: Adding ingredients to your lists"); // Placeholder action
            };

            ingredientRow.Children.Add(addButton); // Add the button to the ingredient row
            return ingredientRow; // Return the complete row
        }

        /// <summary>
        /// Filters the displayed ingredients based on the search text.
        /// </summary>
        /// <param name="ingredients">The original list of ingredients.</param>
        /// <param name="filterText">The text to filter ingredients by.</param>
        /// <param name="ingredientListPanel">The panel to update with filtered ingredients.</param>
        private void FilterIngredients(List<Ingredient> ingredients, string filterText, StackPanel ingredientListPanel)
        {
            ingredientListPanel.Children.Clear(); // Clear for filtered results
            string filter = filterText.ToLower(); // Get the filter text in lowercase

            foreach (var ingredient in ingredients)
            {
                // Check if ingredient name or type contains the filter text
                if (ingredient.GetName().ToLower().Contains(filter) || ingredient.GetIngType().ToLower().Contains(filter))
                {
                    ingredientListPanel.Children.Add(CreateIngredientRow(ingredient)); // Add matching ingredients
                }
            }
        }

        /// <summary>
        /// Creates a header TextBlock.
        /// </summary>
        /// <param name="headerText">The text for the header.</param>
        /// <returns>The created TextBlock.</returns>
        private TextBlock CreateHeader(string headerText)
        {
            return new TextBlock
            {
                Text = headerText,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = textColor,
                Margin = new Thickness(10, 10, 0, 10)
            };
        }

        /// <summary>
        /// Creates a placeholder TextBlock for future content.
        /// </summary>
        /// <param name="placeholderText">The text for the placeholder.</param>
        /// <returns>The created TextBlock.</returns>
        private TextBlock CreatePlaceholderText(string placeholderText)
        {
            return new TextBlock
            {
                Text = placeholderText,
                FontSize = 16,
                Foreground = textColor,
                Margin = new Thickness(10, 10, 0, 10),
                FontStyle = FontStyles.Italic
            };
        }
    }
}

