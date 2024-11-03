using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;

namespace Desktop_Frontend.Components
{
    /// <summary>
    /// Manages the "My Lists" feature, retrieves user lists, and displays them in collapsible menus.
    /// </summary>
    public class MyListsHandler
    {
        private readonly SolidColorBrush textColor;
        private readonly IBackend backend;
        private readonly IUser user;
        private StackPanel parentPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyListsHandler"/> class.
        /// This constructor sets up the handler with the provided backend service and user instance,
        /// and initializes the text color to be used in UI elements.
        /// </summary>
        /// <param name="backend">The backend service instance for handling data operations.</param>
        /// <param name="user">The user instance for user-specific actions.</param>
        public MyListsHandler(IBackend backend, IUser user)
        {
            textColor = (SolidColorBrush)App.Current.Resources["TertiaryBrush"]; 
            this.backend = backend; 
            this.user = user;
            parentPanel = null;
        }

        /// <summary>
        /// Displays the "My Lists" section with collapsible ingredient lists for each user list.
        /// Each list includes a search bar to filter ingredients.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        /// <summary>
        /// Displays the "My Lists" section with collapsible ingredient lists for each user list.
        /// Each list includes a search bar to filter ingredients.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        public async Task DisplayMyLists(StackPanel contentArea)
        {
            if (this.parentPanel == null)
            {
                this.parentPanel = contentArea;
            }
            contentArea.Children.Clear();

            // Create and add header
            TextBlock header = new TextBlock
            {
                Text = "My Lists",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = textColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };
            contentArea.Children.Add(header);

            // Retrieve user's lists from the backend
            List<UserList> myLists = await backend.GetMyLists(user);

            // Retrieve colors from App.xaml resources for consistent styling
            SolidColorBrush buttonBackgroundColor = (SolidColorBrush)App.Current.Resources["TertiaryBrush"];
            SolidColorBrush buttonTextColor = (SolidColorBrush)App.Current.Resources["SecondaryBrush"];

            // Display each list in a collapsible menu
            foreach (var userList in myLists)
            {
                Expander listExpander = new Expander
                {
                    Header = userList.GetListName(),
                    FontSize = 18,
                    Foreground = textColor,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                StackPanel ingredientPanel = new StackPanel();

                // Add search box for filtering ingredients
                TextBox searchBox = new TextBox
                {
                    Text = "Search ingredients...", // Initial placeholder text
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                // Placeholder behavior
                searchBox.GotFocus += (s, e) =>
                {
                    if (searchBox.Text == "Search ingredients...")
                    {
                        searchBox.Text = "";
                        searchBox.Foreground = Brushes.Black;
                    }
                };

                searchBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        searchBox.Text = "Search ingredients...";
                        searchBox.Foreground = Brushes.Gray;
                    }
                };

                ingredientPanel.Children.Add(searchBox);

                // Filter ingredients as user types in the search box
                searchBox.TextChanged += (s, e) =>
                {
                    string searchText = searchBox.Text.ToLower();
                    UpdateIngredientPanel(ingredientPanel, searchText, userList);
                };

                // Event to handle expanding and collapsing
                listExpander.Expanded += (s, e) =>
                {
                    ResetSearchAndDisplayIngredients(searchBox, ingredientPanel, userList);
                };
                listExpander.Collapsed += (s, e) =>
                {
                    ResetSearchAndDisplayIngredients(searchBox, ingredientPanel, userList);
                };

                // Display all ingredients initially
                UpdateIngredientPanel(ingredientPanel, "", userList);

                // Set the ingredient panel as the content of the expander
                listExpander.Content = ingredientPanel;

                // Add the expander to the main content area
                contentArea.Children.Add(listExpander);
            }
        }

        /// <summary>
        /// Updates the ingredient panel based on search text.
        /// </summary>
        /// <summary>
        /// Updates the ingredient panel based on search text.
        /// Ensures the search box is added first if it's missing.
        /// </summary>
        private void UpdateIngredientPanel(StackPanel ingredientPanel, string searchText, UserList userList)
        {
            // Check if the search box exists in the panel; if not, add it.
            if (ingredientPanel.Children.Count == 0 || !(ingredientPanel.Children[0] is TextBox))
            {
                TextBox searchBox = new TextBox
                {
                    Text = "Search ingredients...", // Initial placeholder text
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                // Placeholder behavior
                searchBox.GotFocus += (s, e) =>
                {
                    if (searchBox.Text == "Search ingredients...")
                    {
                        searchBox.Text = "";
                        searchBox.Foreground = Brushes.Black;
                    }
                };

                searchBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        searchBox.Text = "Search ingredients...";
                        searchBox.Foreground = Brushes.Gray;
                    }
                };

                // Search filter event
                searchBox.TextChanged += (s, e) =>
                {
                    string newSearchText = searchBox.Text.ToLower();
                    UpdateIngredientPanel(ingredientPanel, newSearchText, userList);
                };

                ingredientPanel.Children.Insert(0, searchBox);
            }

            // Clear everything except the search box and the add button
            for (int i = ingredientPanel.Children.Count - 1; i > 0; i--)
            {
                ingredientPanel.Children.RemoveAt(i);
            }

            // Filter and display ingredients that match the search
            foreach (var ingredient in userList.GetIngredients().Where(ing => ing.GetName().ToLower().Contains(searchText)))
            {
                ingredientPanel.Children.Add(CreateIngredientRow(ingredient, userList));
            }

            // Add the + button at the bottom
            Button addIngredientButton = new Button
            {
                Content = "+",
                Width = 30,
                Height = 30,
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(20, 20, 0, 0)
            };

            // Click event for adding an ingredient
            addIngredientButton.Click += async (s, e) =>
            {
                await ShowAddIngredientPopup(userList);
            };

            // Add the button to the ingredient panel
            ingredientPanel.Children.Add(addIngredientButton);
        }


        /// <summary>
        /// Resets the search box and displays all ingredients when the list is expanded or collapsed.
        /// </summary>
        private void ResetSearchAndDisplayIngredients(TextBox searchBox, StackPanel ingredientPanel, UserList userList)
        {
            searchBox.Text = "Search ingredients...";
            searchBox.Foreground = Brushes.Gray;
            UpdateIngredientPanel(ingredientPanel, "", userList);
        }

        /// <summary>
        /// Creates a row with ingredient details, including name, type, amount, and unit.
        /// </summary>
        /// <param name="ingredient">The ingredient to display.</param>
        /// <returns>A Border control containing the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient, UserList userList)
        {
            // Create a row to display ingredient details
            DockPanel ingredientRow = new DockPanel { Margin = new Thickness(0, 5, 0, 5) };

            // Ingredient details text
            TextBlock ingredientText = new TextBlock
            {
                Text = $"{ingredient.GetName()} - {ingredient.GetIngType()} | {ingredient.GetAmount()} {ingredient.GetUnit()}",
                Foreground = textColor,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            DockPanel.SetDock(ingredientText, Dock.Left);
            ingredientRow.Children.Add(ingredientText);

            // Panel to hold delete and edit buttons together, aligned to the right
            StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };

            // Delete button (Trash icon)
            Button deleteButton = new Button
            {
                Content = "\uD83D\uDDD1", // Trash can icon
                Width = 30,
                Height = 30,
                Background = Brushes.White,
                Margin = new Thickness(10, 0, 0, 0)
            };
            deleteButton.Click += async (s, e) => await ConfirmDeletion(ingredient, userList.GetListName());
            buttonPanel.Children.Add(deleteButton);

            // Edit button
            Button editButton = new Button
            {
                Content = "\u270E", // Edit button icon
                Width = 30,
                Height = 30,
                Background = Brushes.White,
                Margin = new Thickness(10, 0, 0, 0)
            };
            editButton.Click += (s, e) => MessageBox.Show("Coming soon: Editing Ingredients");
            buttonPanel.Children.Add(editButton);

            // Add the button panel to the DockPanel, aligned to the right
            ingredientRow.Children.Add(buttonPanel);

            // Create border styling for the ingredient row
            Border border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(10),
                Child = ingredientRow
            };

            return border;
        }

        /// <summary>
        /// Shows pop up window for adding an <see cref="Ingredient"/>
        /// </summary>
        /// <param name="userList">The <see cref="UserList"/> to be added to.</param>
        private async Task ShowAddIngredientPopup(UserList userList)
        {
            // Create Popup window
            Window popup = new Window
            {
                Title = "Add Ingredient",
                Width = 300,
                Height = 400, // Increased height to accommodate search
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Ingredient name input with search functionality
            TextBox searchBox = new TextBox
            {
                Margin = new Thickness(0, 10, 0, 10),
                Text = "Search ingredients...",
                Foreground = Brushes.Gray
            };

            ComboBox nameBox = new ComboBox { Margin = new Thickness(0, 10, 0, 10) };
            List<Ingredient> allIngredients = await backend.GetAllIngredients(user);

            // Populate ComboBox with all ingredients initially
            foreach (var ing in allIngredients)
            {
                nameBox.Items.Add(ing.GetName());
            }
            if (nameBox.Items.Count > 0) nameBox.SelectedIndex = 0;

            panel.Children.Add(new TextBlock { Text = "Ingredient Name:" });
            panel.Children.Add(searchBox);
            panel.Children.Add(nameBox);

            // Amount input with placeholder
            TextBox amountBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = "Enter amount",
                Foreground = Brushes.Gray
            };
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

            // Unit input
            ComboBox unitBox = new ComboBox { Margin = new Thickness(0, 10, 0, 10) };
            List<string> units = await backend.GetAllMeasurements(user);
            foreach (var unit in units)
            {
                unitBox.Items.Add(unit);
            }
            if (unitBox.Items.Count > 0) unitBox.SelectedIndex = 0;
            panel.Children.Add(new TextBlock { Text = "Unit:" });
            panel.Children.Add(unitBox);

            // Filter ingredients as user types in the search box
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...")
                {
                    searchBox.Text = "";
                    searchBox.Foreground = Brushes.Black; // Change text color to black for user input
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Search ingredients...";
                    searchBox.Foreground = Brushes.Gray; // Change text color back to gray

                    // Repopulate the ComboBox with all ingredients when the search box is empty
                    nameBox.Items.Clear();
                    foreach (var ing in allIngredients)
                    {
                        nameBox.Items.Add(ing.GetName());
                    }
                    if (nameBox.Items.Count > 0) nameBox.SelectedIndex = 0; // Select the first ingredient
                }
            };

            searchBox.TextChanged += (s, e) =>
            {
                string searchText = searchBox.Text.ToLower();
                nameBox.Items.Clear(); // Clear the ComboBox items

                // If the search box is empty, show all ingredients
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (var ing in allIngredients)
                    {
                        nameBox.Items.Add(ing.GetName()); // Add all ingredients back to the ComboBox
                    }
                }
                else
                {
                    // If there is search text, filter the ingredients
                    foreach (var ing in allIngredients)
                    {
                        if (ing.GetName().ToLower().Contains(searchText))
                        {
                            nameBox.Items.Add(ing.GetName()); // Add matching ingredients
                        }
                    }
                }

                // Select the first matching item if available
                if (nameBox.Items.Count > 0)
                    nameBox.SelectedIndex = 0;
            };

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

                // Retrieve values
                string name = nameBox.SelectedItem?.ToString();
                string unit = unitBox.SelectedItem?.ToString();
                if (name == null || unit == null)
                {
                    MessageBox.Show("Please select a valid ingredient and unit.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string type = allIngredients.First(ing => ing.GetName() == name).GetIngType();

                // Create the ingredient with specified values
                Ingredient newIngredient = new Ingredient(name, type, amount, unit);

                // Add ingredient to list via backend and check success
                bool success = await backend.AddIngredientToList(user, newIngredient, userList.GetListName());
                if (success)
                {
                    await DisplayMyLists(parentPanel);
                    MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    popup.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            panel.Children.Add(addButton);

            popup.Content = panel;
            popup.ShowDialog();
        }

        /// <summary>
        /// Displays a confirmation popup for deleting an ingredient.
        /// </summary>
        /// <param name="ingredient">The <see cref="Ingredient"/> to delete.</param>
        /// <param name="listName">The list name from which the ingredient should be removed.</param>
        private async Task ConfirmDeletion(Ingredient ingredient, string listName)
        {
            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Delete Ingredient",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            // Stack panel for layout
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock
            {
                Text = $"Are you sure you want to delete {ingredient.GetName()}?",
                Margin = new Thickness(0, 10, 0, 20),
                TextWrapping = TextWrapping.Wrap
            });

            // Confirmation buttons
            Button confirmButton = new Button { Content = "OK", Width = 60, Margin = new Thickness(5) };
            Button cancelButton = new Button { Content = "Cancel", Width = 60, Margin = new Thickness(5) };
            panel.Children.Add(new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { confirmButton, cancelButton }
            });

            // Event handler for cancel button to close the popup
            cancelButton.Click += (s, e) => confirmationPopup.Close();

            // Event handler for confirm button to initiate deletion
            confirmButton.Click += async (s, e) =>
            {
                // Disable buttons during backend call
                confirmButton.IsEnabled = false;
                cancelButton.IsEnabled = false;

                // Call backend to remove ingredient
                bool success = await backend.RemIngredientFromList(user, ingredient, listName);

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Ingredient deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await DisplayMyLists(parentPanel); // Refresh the list
                }
                else
                {
                    MessageBox.Show("Failed to delete ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                confirmationPopup.Close();
            };

            confirmationPopup.Content = panel;
            confirmationPopup.ShowDialog();
        }


    }
}
