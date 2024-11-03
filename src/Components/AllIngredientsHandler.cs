using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Components
{
    /// <summary>
    /// Manages the "All Ingredients" feature, handling ingredient list display and search functionality.
    /// </summary>
    public class AllIngredientsHandler
    {
        private readonly IBackend backend;
        private readonly IUser user;
        private readonly SolidColorBrush ingredientTextColor;
        private readonly SolidColorBrush highlightedTextColor;
        private readonly SolidColorBrush highlightedBackgroundColor;
        private readonly SolidColorBrush buttonColor;

        /// <summary>
        /// Initializes an instance of the <see cref="AllIngredientsHandler"/> class.
        /// </summary>
        /// <param name="backend">Backend service for data retrieval.</param>
        /// <param name="user">Authenticated user instance.</param>
        public AllIngredientsHandler(IBackend backend, IUser user)
        {
            this.backend = backend;
            this.user = user;

            // Initialize colors from App.xaml resources
            ingredientTextColor = (SolidColorBrush)App.Current.Resources["TertiaryBrush"];
            highlightedTextColor = Brushes.Black;
            highlightedBackgroundColor = Brushes.White;
            buttonColor = Brushes.White;
        }

        /// <summary>
        /// Asynchronously displays all ingredients in the specified panel.
        /// Creates a search box for filtering the ingredients list in real-time.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        public async Task DisplayIngredientsAsync(StackPanel contentArea)
        {
            contentArea.Children.Clear();

            // Create and add header
            TextBlock header = new TextBlock
            {
                Text = "All Ingredients",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = ingredientTextColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };
            contentArea.Children.Add(header);

            StackPanel stackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };

            TextBox searchBox = CreateSearchBox();
            stackPanel.Children.Add(searchBox);

            StackPanel ingredientListPanel = new StackPanel();
            stackPanel.Children.Add(ingredientListPanel);

            // Retrieve ingredients and populate list
            List<Ingredient> ingredients = await backend.GetAllIngredients(user);
            PopulateIngredientList(ingredients, ingredientListPanel);

            // Update ingredients list based on search text
            searchBox.TextChanged += (s, e) =>
                FilterIngredients(ingredients, searchBox.Text, ingredientListPanel);

            contentArea.Children.Add(stackPanel);
        }

        /// <summary>
        /// Populates the list of ingredients in the specified panel.
        /// </summary>
        /// <param name="ingredients">List of ingredients to display.</param>
        /// <param name="ingredientListPanel">The panel to populate with ingredient rows.</param>
        private void PopulateIngredientList(List<Ingredient> ingredients, StackPanel ingredientListPanel)
        {
            ingredientListPanel.Children.Clear();
            foreach (var ingredient in ingredients)
            {
                ingredientListPanel.Children.Add(CreateIngredientRow(ingredient.CopyIngredient()));
            }
        }

        /// <summary>
        /// Filters and updates the ingredients list based on the provided search text.
        /// </summary>
        /// <param name="ingredients">Original list of ingredients.</param>
        /// <param name="filterText">Text to filter ingredients by.</param>
        /// <param name="ingredientListPanel">The panel to update with filtered ingredients.</param>
        private void FilterIngredients(List<Ingredient> ingredients, string filterText, StackPanel ingredientListPanel)
        {
            var filteredIngredients = ingredients
                .Where(i => i.GetName().Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                            i.GetIngType().Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            PopulateIngredientList(filteredIngredients, ingredientListPanel);
        }

        /// <summary>
        /// Creates a TextBox for ingredient search with placeholder text.
        /// </summary>
        /// <returns>A configured TextBox for searching ingredients.</returns>
        private TextBox CreateSearchBox()
        {
            TextBox searchBox = new TextBox
            {
                Height = 30,
                Foreground = Brushes.Black,
                Background = Brushes.White,
                Margin = new Thickness(0, 10, 0, 10),
                Text = "Search ingredients..."
            };

            // Clear placeholder on focus
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...") searchBox.Text = string.Empty;
            };
            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text)) searchBox.Text = "Search ingredients...";
            };

            return searchBox;
        }

        /// <summary>
        /// Creates a row with ingredient details and an add button, wrapped in a border.
        /// </summary>
        /// <param name="ingredient">The ingredient to display in the row.</param>
        /// <returns>A styled Border containing the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient)
        {
            DockPanel ingredientRow = new DockPanel { Margin = new Thickness(0, 5, 0, 5) };
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
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 0, 0)
            };
            addButton.Click += (s, e) => ShowAddIngredientPopup(ingredient.CopyIngredient());
            ingredientRow.Children.Add(addButton);

            Border border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10),
                Child = ingredientRow
            };
            border.MouseEnter += (s, e) =>
            {
                border.Background = highlightedBackgroundColor;
                ingredientText.Foreground = highlightedTextColor;
            };
            border.MouseLeave += (s, e) =>
            {
                border.Background = Brushes.Transparent;
                ingredientText.Foreground = ingredientTextColor;
            };

            return border;
        }

        /// <summary>
        /// Creates a popup for specifying ingredient amount, unit, and list name when the add button is clicked.
        /// </summary>
        /// <param name="ingredient">The ingredient to be added.</param>
        private async void ShowAddIngredientPopup(Ingredient ingredient)
        {
            // Create Popup window
            Window popup = new Window
            {
                Title = "Add Ingredient",
                Width = 300,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Amount input with placeholder 
            TextBox amountBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = "Enter amount", // Placeholder text initially
                Foreground = Brushes.Gray // Placeholder text color
            };

            // Placeholder functionality: clear on focus, restore on defocus if empty
            amountBox.GotFocus += (s, e) =>
            {
                if (amountBox.Text == "Enter amount")
                {
                    amountBox.Text = "";
                    amountBox.Foreground = Brushes.Black; 
                }
            };
            amountBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(amountBox.Text))
                {
                    amountBox.Text = "Enter amount";
                    amountBox.Foreground = Brushes.Gray; 
                }
            };
            panel.Children.Add(new TextBlock { Text = "Amount:" });
            panel.Children.Add(amountBox);

            // Get list of units and populate ComboBox
            ComboBox unitBox = new ComboBox { Margin = new Thickness(0, 10, 0, 10) };
            List<string> units = await backend.GetAllMeasurements(user);
            foreach (var unit in units)
            {
                unitBox.Items.Add(unit);
            }
            if (unitBox.Items.Count > 0) unitBox.SelectedIndex = 0; // Set default selection
            panel.Children.Add(new TextBlock { Text = "Unit:" });
            panel.Children.Add(unitBox);

            // Get list of user lists and populate ComboBox
            ComboBox listBox = new ComboBox { Margin = new Thickness(0, 10, 0, 10) };
            List<UserList> userLists = await backend.GetMyLists(user);
            foreach (var userList in userLists)
            {
                listBox.Items.Add(userList.GetListName());
            }
            if (listBox.Items.Count > 0) listBox.SelectedIndex = 0; // Set default selection
            panel.Children.Add(new TextBlock { Text = "List Name:" });
            panel.Children.Add(listBox);

            // Add button
            Button addButton = new Button
            {
                Content = "Add",
                Margin = new Thickness(0, 20, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            addButton.Click += async (s, e) =>
            {
                // Validate amount input
                if (!float.TryParse(amountBox.Text, out float amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount greater than 0.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Set ingredient properties
                ingredient.SetAmount(amount);
                ingredient.SetUnit(unitBox.SelectedItem.ToString());


                // Disable controls while processing
                addButton.IsEnabled = false;
                amountBox.IsEnabled = false;
                unitBox.IsEnabled = false;
                listBox.IsEnabled = false;

                // Call backend method
                string listName = listBox.SelectedItem.ToString();
                bool success = await backend.AddIngredientToList(user, ingredient, listName);

                // Re-enable controls
                addButton.IsEnabled = true;
                amountBox.IsEnabled = true;
                unitBox.IsEnabled = true;
                listBox.IsEnabled = true;


                // Show result message
                if (success)
                {
                    MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                popup.Close();
            };
            panel.Children.Add(addButton);

            popup.Content = panel;
            popup.ShowDialog();
        }
    }
}
