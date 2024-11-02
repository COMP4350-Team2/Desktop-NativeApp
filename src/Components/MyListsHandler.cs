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

                // Add each ingredient in the UserList to the ingredient panel
                foreach (var ingredient in userList.GetIngredients())
                {
                    ingredientPanel.Children.Add(CreateIngredientRow(ingredient));
                }

                // Create and add "Add Ingredient" button at the bottom of each ingredient panel
                Button addIngredientButton = new Button
                {
                    Content = "+",
                    Width = 30,
                    Height = 30,
                    Background = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(20, 20, 0, 0)
                };

                // Click event 
                addIngredientButton.Click += async (s, e) =>
                {
                    await ShowAddIngredientPopup(userList);
                };

                // Add the button to the ingredient panel
                ingredientPanel.Children.Add(addIngredientButton);

                // Set the ingredient panel as the content of the expander
                listExpander.Content = ingredientPanel;

                // Add the expander to the main content area
                contentArea.Children.Add(listExpander);
            }
        }


        /// <summary>
        /// Creates a row with ingredient details, including name, type, amount, and unit.
        /// </summary>
        /// <param name="ingredient">The ingredient to display.</param>
        /// <returns>A Border control containing the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient)
        {
            // Create a row to display ingredient details
            DockPanel ingredientRow = new DockPanel { Margin = new Thickness(0, 5, 0, 5) };
            TextBlock ingredientText = new TextBlock
            {
                Text = $"{ingredient.GetName()} - {ingredient.GetIngType()} | {ingredient.GetAmount()} {ingredient.GetUnit()}",
                Foreground = textColor,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };
            ingredientRow.Children.Add(ingredientText);

            // Edit button
            Button editButton = new Button
            {
                Content = "\u270E", //edit button icon
                Width = 30,
                Height = 30,
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10, 0, 0, 0)
            };
            editButton.Click += (s, e) => MessageBox.Show("Coming soon: Editing Ingredients");
            ingredientRow.Children.Add(editButton);

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
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Ingredient name input
            ComboBox nameBox = new ComboBox { Margin = new Thickness(0, 10, 0, 10) };
            List<Ingredient> allIngredients = await backend.GetAllIngredients(user);
            foreach (var ing in allIngredients)
            {
                nameBox.Items.Add(ing.GetName());
            }
            if (nameBox.Items.Count > 0) nameBox.SelectedIndex = 0;
            panel.Children.Add(new TextBlock { Text = "Ingredient Name:" });
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
                string name = nameBox.SelectedItem.ToString();
                string unit = unitBox.SelectedItem.ToString();
                string type = allIngredients.First(ing => ing.GetName() == name).GetIngType();

                // Create the ingredient with specified values
                Ingredient newIngredient = new Ingredient(name, type, amount, unit);

                // Add ingredient to list via backend and check success
                bool success = await backend.AddIngredientToList(user, newIngredient, userList.GetListName());
                if (success)
                {
                    // Update the local user list and UI
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


    }
}
