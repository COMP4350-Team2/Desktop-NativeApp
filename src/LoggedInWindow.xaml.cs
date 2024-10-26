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
        private SolidColorBrush textColor = new SolidColorBrush(Colors.White); // Text color set to White
        private SolidColorBrush buttonColor = new SolidColorBrush(Colors.White); // Button color set to White
        private SolidColorBrush ingredientTextColor = new SolidColorBrush(Colors.White); // Ingredients list text color (White)
        private SolidColorBrush highlightedTextColor = new SolidColorBrush(Colors.Black); // Text color for highlighted background
        private SolidColorBrush highlightedBackgroundColor = new SolidColorBrush(Colors.White); // Background color for highlighted rows

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
            ContentArea.Children.Clear();
            TextBlock myListsHeader = CreateHeader("My Lists");
            ContentArea.Children.Add(myListsHeader);

            StackPanel centerPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 40, 0, 0)
            };

            TextBlock placeholderText = CreatePlaceholderText("Coming soon: A page for your lists");
            placeholderText.HorizontalAlignment = HorizontalAlignment.Center;
            centerPanel.Children.Add(placeholderText);

            ContentArea.Children.Add(centerPanel);
        }

        /// <summary>
        /// Event handler for the "All Ingredients" button click event.
        /// Calls the method to display the list of ingredients.
        /// </summary>
        private async void AllIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            await DisplayIngredients();
        }

        /// <summary>
        /// Initializes the content space by displaying ingredients on initialization.
        /// </summary>
        private async void InitializeContentSpace()
        {
            await DisplayIngredients();
        }

        /// <summary>
        /// Displays the list of ingredients fetched from the backend.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DisplayIngredients()
        {
            ContentArea.Children.Clear();
            TextBlock allIngredientsHeader = CreateHeader("All Ingredients");
            ContentArea.Children.Add(allIngredientsHeader);

            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(0, 40, 0, 0)
            };

            TextBox searchBox = CreateSearchBox();
            stackPanel.Children.Add(searchBox);

            StackPanel ingredientListPanel = new StackPanel();
            stackPanel.Children.Add(ingredientListPanel);

            List<Ingredient> ingredients = await backend.GetAllIngredients(user);
            PopulateIngredientList(ingredients, ingredientListPanel);

            searchBox.TextChanged += (s, e) => FilterIngredients(ingredients, searchBox.Text, ingredientListPanel);

            ContentArea.Children.Add(stackPanel);
        }

        /// <summary>
        /// Populates the ingredient list in the given panel.
        /// </summary>
        /// <param name="ingredients">The list of ingredients to display.</param>
        /// <param name="ingredientListPanel">The panel to populate with ingredients.</param>
        private void PopulateIngredientList(List<Ingredient> ingredients, StackPanel ingredientListPanel)
        {
            ingredientListPanel.Children.Clear();
            foreach (var ingredient in ingredients)
            {
                ingredientListPanel.Children.Add(CreateIngredientRow(ingredient));
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
                Width = double.NaN,
                Height = 30,
                Foreground = Brushes.Black,
                Background = Brushes.White,
                Margin = new Thickness(0, 40, 0, 10),
                Text = "Search ingredients..."
            };

            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...")
                {
                    searchBox.Text = string.Empty;
                    searchBox.Foreground = Brushes.Black;
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search ingredients...";
                    searchBox.Foreground = Brushes.Gray;
                }
            };

            return searchBox;
        }

        /// <summary>
        /// Creates an ingredient row with a "+" button wrapped in a border.
        /// </summary>
        /// <param name="ingredient">The ingredient to display.</param>
        /// <returns>The created Border for the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient)
        {
            DockPanel ingredientRow = new DockPanel
            {
                Margin = new Thickness(0, 5, 0, 5),
                Background = Brushes.Transparent
            };

            // Create the TextBlock for the ingredient text
            TextBlock ingredientText = new TextBlock
            {
                Text = $"{ingredient.GetName()} - {ingredient.GetIngType()}",
                Foreground = ingredientTextColor,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };

            ingredientRow.Children.Add(ingredientText);

            Button addButton = new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Background = buttonColor,
                Foreground = Brushes.Black, // Button text color set to black for visibility
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 0, 0)
            };

            addButton.Click += (s, e) =>
            {
                MessageBox.Show("Coming soon: Adding ingredients to your lists");
            };

            ingredientRow.Children.Add(addButton);

            // Wrap the ingredient row in a Border with padding
            Border border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10), // Added padding inside the border
                Child = ingredientRow
            };

            // Highlight behavior for the whole border
            border.MouseEnter += (s, e) =>
            {
                border.Background = highlightedBackgroundColor; // Highlight the entire box
                ingredientText.Foreground = highlightedTextColor; // Change text color on hover
            };

            border.MouseLeave += (s, e) =>
            {
                border.Background = Brushes.Transparent; // Reset background of the box
                ingredientText.Foreground = ingredientTextColor; // Reset text color to original
            };

            return border;
        }



        /// <summary>
        /// Filters the displayed ingredients based on the search text.
        /// </summary>
        /// <param name="ingredients">The original list of ingredients.</param>
        /// <param name="filterText">The text to filter ingredients by.</param>
        /// <param name="ingredientListPanel">The panel to update with filtered ingredients.</param>
        private void FilterIngredients(List<Ingredient> ingredients, string filterText, StackPanel ingredientListPanel)
        {
            var filteredIngredients = ingredients.Where(i =>
                i.GetName().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                i.GetIngType().IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            PopulateIngredientList(filteredIngredients, ingredientListPanel);
        }

        /// <summary>
        /// Creates a header TextBlock with specified text.
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
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20) 
            };
        }

        /// <summary>
        /// Creates a placeholder TextBlock for future functionality.
        /// </summary>
        /// <param name="placeholderText">The text for the placeholder.</param>
        /// <returns>The created TextBlock.</returns>
        private TextBlock CreatePlaceholderText(string placeholderText)
        {
            return new TextBlock
            {
                Text = placeholderText,
                FontSize = 18,
                Foreground = textColor,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }
    }
}
