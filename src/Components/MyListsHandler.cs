﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            this.backend = backend;
            this.user = user;
            parentPanel = null;
        }

        /// <summary>
        /// Displays the "My Lists" section with collapsible ingredient lists for each user list.
        /// Each list includes a search bar to filter ingredients.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        public async Task DisplayMyLists(StackPanel contentArea)
        {
            SolidColorBrush pageHeaderTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush listHeaderTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush ingredientButtonCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush ingredientTxtCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush searchBarBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush searchBarTxtCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush expanderIconCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush expanderBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush dropDownBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush dropDownForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int pageHeaderFont = 32;
            int listHeaderFont = 28;
            int ingredientButtonFont = 28;
            int searchBarFont = 24;
            int optionsFont = 20;


            double availableWidth = SystemParameters.PrimaryScreenWidth;
            double itemWidth = availableWidth / 2 - 200;

            if (this.parentPanel == null)
            {
                this.parentPanel = contentArea;
            }
            contentArea.Children.Clear();

            // Create and add header
            TextBlock header = new TextBlock
            {
                Text = "My Lists",
                FontSize = pageHeaderFont,
                FontWeight = FontWeights.Bold,
                Foreground = pageHeaderTextCol,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };
            contentArea.Children.Add(header);

            // Initialize create list button
            InitializeCreateListButton(contentArea);

            // Retrieve user's lists from the backend
            List<UserList> myLists = await backend.GetMyLists(user);

            // Display each list in a collapsible menu with a delete button
            foreach (var userList in myLists)
            {
                Grid headerGrid = new Grid
                {
                    Margin = new Thickness(10)
                };

                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) }); 
                headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Auto) }); 


                // Create the list name label
                TextBlock listHeader = new TextBlock
                {
                    Text = userList.GetListName(),
                    FontSize = listHeaderFont,
                    FontWeight = FontWeights.Bold,
                    Foreground = listHeaderTextCol,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(10),
                    Width = availableWidth 
                };

                // Set the list name to be in the first column
                Grid.SetColumn(listHeader, 0);
                headerGrid.Children.Add(listHeader);

                // Initialize the options button 
                Button optionsButton = new Button
                {
                    Content = new TextBlock
                    {
                        Text = "...",
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Margin = new Thickness(10, 10, 5, 20),
                    FontSize = ingredientButtonFont,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Foreground = ingredientButtonCol,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Cursor = Cursors.Hand
                };
                optionsButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];

                // Set the options button in the second column
                Grid.SetColumn(optionsButton, 1);
                headerGrid.Children.Add(optionsButton);

                // Create the dropdown menu 
                ContextMenu dropdownMenu = new ContextMenu { Background = dropDownBackground };

                // Add the options to the menu
                MenuItem addIngredientOption = new MenuItem { Header = "Add Ingredient" };
                addIngredientOption.Click += async (s, e) => await ShowAddIngredientPopup(userList);
                addIngredientOption.Foreground = dropDownForeground;
                addIngredientOption.FontSize = optionsFont;
                dropdownMenu.Items.Add(addIngredientOption);

                MenuItem renameListOption = new MenuItem { Header = "Rename List" };
                renameListOption.Click += (s, e) => { MessageBox.Show("To be implemented"); };
                renameListOption.Foreground = dropDownForeground;
                renameListOption.FontSize = optionsFont;
                dropdownMenu.Items.Add(renameListOption);

                MenuItem deleteListOption = new MenuItem { Header = "Delete List" };
                deleteListOption.Click += async (s, e) => await ConfirmDeleteList(userList.GetListName());
                deleteListOption.Foreground = Brushes.Red;
                deleteListOption.FontSize = optionsFont;
                deleteListOption.FontWeight = FontWeights.Bold;
                dropdownMenu.Items.Add(deleteListOption);

                // Show the dropdown menu when the options button is clicked
                optionsButton.Click += (sender, e) => { dropdownMenu.IsOpen = true; };

                // Create the Expander for the list, using the grid for the header
                Expander listExpander = new Expander
                {
                    Header = headerGrid,
                    FontSize = 18,
                    Foreground = expanderIconCol,
                    Margin = new Thickness(30),
                    BorderThickness = new Thickness(2),
                    BorderBrush = expanderBorderCol
                };

                StackPanel ingredientPanel = new StackPanel();

                // Create a border for rounded edges
                Border searchBoxBorder = new Border
                {
                    Height = 50,
                    Margin = new Thickness(12, 10, 10, 10),
                    CornerRadius = new CornerRadius(5),
                    BorderBrush = searchBarTxtCol,
                    BorderThickness = new Thickness(1),
                    Background = searchBarBackground,
                    Width = 2 * itemWidth + 50,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                // Add search box for filtering ingredients
                TextBox searchBox = new TextBox
                {
                    Text = "Search ingredients...",
                    Foreground = searchBarTxtCol,
                    Background = Brushes.Transparent,
                    FontSize = searchBarFont,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    VerticalContentAlignment = VerticalAlignment.Bottom,
                    
                };

                // Placeholder behavior
                searchBox.GotFocus += (s, e) =>
                {
                    if (searchBox.Text == "Search ingredients...")
                    {
                        searchBox.Text = "";
                        searchBox.Foreground = searchBarTxtCol;
                    }
                };

                searchBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(searchBox.Text))
                    {
                        searchBox.Text = "Search ingredients...";
                        searchBox.Foreground = searchBarTxtCol;
                    }
                };

                searchBoxBorder.Child = searchBox;
                ingredientPanel.Children.Add(searchBoxBorder);

                // Filter ingredients as user types in the search box
                searchBox.TextChanged += (s, e) =>
                {
                    string searchText = searchBox.Text.ToLower().Trim();
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
        /// Shows a pop up confirming list deletion
        /// </summary>
        private async Task ConfirmDeleteList(string listName)
        {
            // Define the brushes for the popup background and text
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int textFont = 18;

            // Create Popup window
            Window popup = new Window
            {
                Title = "Deleting " + listName,
                Width = 500,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            // Panel for the popup content
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Confirmation message text
            TextBlock confirmationText = new TextBlock
            {
                Text = "Are you sure you want to delete " + listName + "?",
                Foreground = textColor,
                FontSize = textFont,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };

            panel.Children.Add(confirmationText);

            // Create the delete confirmation button 
            Button deleteButton = new Button
            {
                Content = "Delete",
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"], 
                Margin = new Thickness(10, 30, 10, 10)
            };

            // Handle delete button click
            deleteButton.Click += async (s, e) =>
            {
                deleteButton.IsEnabled = false;

                bool success = await backend.DeleteList(user, listName);

                // Show result message
                if (success)
                {
                    MessageBox.Show("List deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete list. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                deleteButton.IsEnabled = true;

                await DisplayMyLists(parentPanel); 
                popup.Close();
            };

            panel.Children.Add(deleteButton);

            // Set the popup content
            popup.Content = panel;

            // Show the popup as a modal dialog
            popup.ShowDialog();
        }

        /// <summary>
        /// Updates the ingredient panel based on search text.
        /// Ensures the search box is added first if it's missing.
        /// </summary>
        private void UpdateIngredientPanel(StackPanel ingredientPanel, string searchText, UserList userList)
        {
            SolidColorBrush searchBoxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush searchBoxBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];

            int ingBoxHeight = (int) (SystemParameters.PrimaryScreenHeight/5);
            int maxRowsPerPanel = 3;
            int searchBarFont = 24;

            // Dynamically calculate item width based on the screen size 
            double availableWidth = SystemParameters.PrimaryScreenWidth;
            double itemWidth = availableWidth / 2 - 200; 

            // Check if the search box exists in the panel; if not, add it.
            if (ingredientPanel.Children.Count == 0 || !(ingredientPanel.Children[0] is Border))
            {
                // Create a border for rounded edges
                Border searchBoxBorder = new Border
                {
                    Height = 50,
                    Margin = new Thickness(12, 10, 10, 10),
                    CornerRadius = new CornerRadius(5),
                    BorderBrush = searchBoxForeground,
                    BorderThickness = new Thickness(1),
                    Background = searchBoxBackground,
                    Width = 2 * itemWidth + 50,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                // Add search box for filtering ingredients
                TextBox searchBox = new TextBox
                {
                    Text = "Search ingredients...",
                    Foreground = searchBoxForeground,
                    Background = Brushes.Transparent,
                    FontSize = searchBarFont,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    VerticalContentAlignment = VerticalAlignment.Bottom,

                };

                // Placeholder behavior for the search box
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
                        searchBox.Foreground = searchBoxForeground;
                    }
                };

                // Search filter event
                searchBox.TextChanged += (s, e) =>
                {
                    string newSearchText = searchBox.Text.ToLower();
                    UpdateIngredientPanel(ingredientPanel, newSearchText, userList);
                };

                searchBoxBorder.Child = searchBox;

                ingredientPanel.Children.Insert(0, searchBoxBorder);
            }

            // Clear everything except the search box and the add button
            for (int i = ingredientPanel.Children.Count - 1; i > 0; i--)
            {
                ingredientPanel.Children.RemoveAt(i);
            }

            // Create a ScrollViewer to make the grid scrollable
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto, 
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled, 
                MaxHeight = (maxRowsPerPanel * ingBoxHeight) + 10 
            };

            // Create a WrapPanel for ingredients
            WrapPanel ingredientGrid = new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5, 0, 0, 20)
            };


            // Filter and display ingredients that match the search
            foreach (var ingredient in userList.GetIngredients().Where(ing => ing.GetName().ToLower().Contains(searchText) ||
            ing.GetIngType().ToLower().Contains(searchText)))
            {
                Border ingRow = CreateIngredientRow(ingredient, userList, itemWidth, ingBoxHeight);
                ingredientGrid.Children.Add(ingRow);
            }

            scrollViewer.Content = ingredientGrid;

            // Add the WrapPanel to the ingredient panel
            ingredientPanel.Children.Add(scrollViewer);
        }



        /// <summary>
        /// Resets the search box and displays all ingredients when the list is expanded or collapsed.
        /// </summary>
        private void ResetSearchAndDisplayIngredients(TextBox searchBox, StackPanel ingredientPanel, UserList userList)
        {
            SolidColorBrush seachBoxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            searchBox.Text = "Search ingredients...";
            searchBox.Foreground = seachBoxForeground;
            UpdateIngredientPanel(ingredientPanel, "", userList);
        }

        /// <summary>
        /// Creates a row with ingredient details, including name, type, amount, and unit.
        /// </summary>
        /// <param name="ingredient">The ingredient to display.</param>
        /// <returns>A Border control containing the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient, UserList userList, double itemWidth, int ingBoxHeight)
        {
            SolidColorBrush ingTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush borderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int ingTextFont = 28;
            int buttonFont = 28;

            double availableWidth = SystemParameters.PrimaryScreenWidth - 200;

            // Create a DockPanel to display ingredient details
            DockPanel ingredientRow = new DockPanel
            {
                Margin = new Thickness(10),
                Width = itemWidth,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Ingredient name text
            TextBlock ingredientNameText = new TextBlock
            {
                Text = $"{ingredient.GetName()}",
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = ingTextFont,
                FontWeight = FontWeights.Bold,
            };

            // Ingredient type text 
            TextBlock ingredientTypeText = new TextBlock
            {
                Text = $"{ingredient.GetIngType()}",
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = ingTextFont,
            };

            // Ingredient amount, unit text 
            TextBlock ingredientAmountText = new TextBlock
            {
                Text = $"{ingredient.GetAmount()} {ingredient.GetUnit()}",
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = ingTextFont,
            };

            DockPanel.SetDock(ingredientNameText, Dock.Top);
            ingredientRow.Children.Add(ingredientNameText);
            DockPanel.SetDock(ingredientTypeText, Dock.Top);
            ingredientRow.Children.Add(ingredientTypeText);
            DockPanel.SetDock(ingredientAmountText, Dock.Top);
            ingredientRow.Children.Add(ingredientAmountText);

            // Panel to hold delete, edit, and move buttons
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            // Move button
            Button moveButton = new Button
            {
                Content = "\u21C4",
                FontSize = buttonFont,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = buttonCol,
                Cursor = Cursors.Hand,
            };
            moveButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            moveButton.Click += async (s, e) => await ShowMoveIngredientPopup(userList.GetListName(), ingredient);

            // Delete button
            Button deleteButton = new Button
            {
                Content = "\uD83D\uDDD1", // Trash can icon
                FontSize = buttonFont,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = buttonCol,
                Margin = new Thickness(10),
                Cursor = Cursors.Hand
            };
            deleteButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            deleteButton.Click += async (s, e) => await ConfirmDeletion(ingredient, userList.GetListName());

            // Edit button
            Button editButton = new Button
            {
                Content = "\u270E", // Edit icon
                FontSize = buttonFont,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = buttonCol,
                Margin = new Thickness(10),
                Cursor = Cursors.Hand
            };
            editButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            editButton.Click += async (s, e) => await ShowEditIngredientPopup(ingredient, userList);

            // Add buttons to the panel
            buttonPanel.Children.Add(deleteButton);
            buttonPanel.Children.Add(editButton);
            buttonPanel.Children.Add(moveButton);

            // Add the button panel to the DockPanel
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            ingredientRow.Children.Add(buttonPanel);

            // Create border styling for the ingredient row
            Border border = new Border
            {
                BorderBrush = borderCol,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Height = ingBoxHeight, 
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
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];

            // Create Popup window
            Window popup = new Window
            {
                Title = "Add Ingredient",
                Width = 400,
                Height = 450,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            double boxWidth = 350;
            double boxHeight = 30;
            double fontSize = 18;

            SolidColorBrush boxColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxTextColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushA"];
            SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            TextBox searchBox = new TextBox
            {
                Margin = new Thickness(10),
                Height = boxHeight,
                Width = boxWidth,
                Text = "Search ingredients...",
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = fontSize
            };

            // Box to show ingredient names
            ComboBox nameBox = new ComboBox
            {
                Margin = new Thickness(10),
                Height = boxHeight,
                Width = boxWidth,
                FontSize = fontSize,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            List<Ingredient> allIngredients = await backend.GetAllIngredients(user);

            // Populate ComboBox with all ingredients initially
            foreach (var ing in allIngredients)
            {
                nameBox.Items.Add(ing.GetName());
            }
            if (nameBox.Items.Count > 0) nameBox.SelectedIndex = 0;

            panel.Children.Add(new TextBlock { Text = "Ingredient Name:", Foreground = headerText, FontSize = 18, FontWeight = FontWeights.Bold });
            panel.Children.Add(searchBox);
            panel.Children.Add(nameBox);

            // Amount input with placeholder
            TextBox amountBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = "Enter amount",
                Foreground = boxTextColor,
                Background = boxColor,
                Margin = new Thickness(10),
                Height = boxHeight,
                Width = boxWidth,
                FontSize = fontSize,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Clear on focus, restore on defocus if empty
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
            panel.Children.Add(new TextBlock { Text = "Amount:", Foreground = headerText, FontSize = 18, FontWeight = FontWeights.Bold });
            panel.Children.Add(amountBox);

            // Unit input
            ComboBox unitBox = new ComboBox
            {
                Margin = new Thickness(10),
                Height = boxHeight,
                Width = boxWidth,
                FontSize = fontSize,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            List<string> units = await backend.GetAllMeasurements(user);
            foreach (var unit in units)
            {
                unitBox.Items.Add(unit);
            }
            if (unitBox.Items.Count > 0) unitBox.SelectedIndex = 0;

            panel.Children.Add(new TextBlock { Text = "Unit:", Foreground = headerText, FontSize = 18, FontWeight = FontWeights.Bold });
            panel.Children.Add(unitBox);

            // Filter ingredients as user types in the search box
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

                    // Repopulate the ComboBox with all ingredients when the search box is empty
                    nameBox.Items.Clear();
                    foreach (var ing in allIngredients)
                    {
                        nameBox.Items.Add(ing.GetName());
                    }
                    if (nameBox.Items.Count > 0) nameBox.SelectedIndex = 0; 
                }
            };

            searchBox.TextChanged += (s, e) =>
            {
                string searchText = searchBox.Text.ToLower();
                nameBox.Items.Clear(); 

                // If the search box is empty, show all ingredients
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (var ing in allIngredients)
                    {
                        nameBox.Items.Add(ing.GetName()); 
                    }
                }
                else
                {
                    // If there is search text, filter the ingredients
                    foreach (var ing in allIngredients)
                    {
                        if (ing.GetName().ToLower().Contains(searchText))
                        {
                            nameBox.Items.Add(ing.GetName()); 
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
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"] // Apply the pre-defined style
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

                addButton.IsEnabled = false;

                // Add ingredient to list via backend and check success
                bool success = await backend.AddIngredientToList(user, newIngredient, userList.GetListName());

                addButton.IsEnabled = true;
                
                // Show success
                if (success)
                {
                    MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await DisplayMyLists(parentPanel);
                }
                else
                {
                    MessageBox.Show("Failed to add ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    popup.Close();
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
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Deleting " + ingredient.GetName() + " from " + listName,
                Width = 500,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            // Stack panel for layout
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock
            {
                Text = $"Are you sure you want to delete {ingredient.GetName()}?",
                Margin = new Thickness(10),
                TextWrapping = TextWrapping.Wrap,
                Foreground = textForeground,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            // Confirmation button 
            Button confirmButton = new Button
            {
                Content = "Delete",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"] 
            };

            // Add the button to the panel
            panel.Children.Add(new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { confirmButton }
            });

            // Event handler for confirm button to initiate deletion
            confirmButton.Click += async (s, e) =>
            {
                confirmButton.IsEnabled = false;

                // Call backend to remove ingredient
                bool success = await backend.RemIngredientFromList(user, ingredient, listName);

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Ingredient deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                confirmButton.IsEnabled = true;

                // Update lists after deletion
                await DisplayMyLists(parentPanel);
                confirmationPopup.Close();
            };

            confirmationPopup.Content = panel;
            confirmationPopup.ShowDialog();
        }


        /// <summary>
        /// Opens a pop-up window to edit an ingredient's details (amount and unit).
        /// Validates input and updates the ingredient in the backend if valid.
        /// </summary>
        /// <param name="oldIngredient">The ingredient being edited.</param>
        /// <param name="userList">The list that contains the ingredient.</param>
        private async Task ShowEditIngredientPopup(Ingredient oldIngredient, UserList userList)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create new window
            Window popup = new Window
            {
                Title = "Edit Ingredient",
                Width = 400,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Amount
            TextBox amountBox = new TextBox
            {
                Text = oldIngredient.GetAmount().ToString(),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 350,
                Height = 40,
                FontSize = 20
            };
            amountBox.GotFocus += (s, e) =>
            {
                if (float.TryParse(amountBox.Text, out float amount) && amount <= 0)
                {
                    amountBox.Clear();
                }
            };

            panel.Children.Add(new TextBlock
            {
                Text = "Amount:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10),
                Foreground = textForeground
            });
            panel.Children.Add(amountBox);

            // Unit
            ComboBox unitBox = new ComboBox
            {
                Width = 350,
                Height = 40,
                FontSize = 20,
                Margin = new Thickness(10)
            };
            var units = await backend.GetAllMeasurements(user);
            foreach (var unit in units)
            {
                unitBox.Items.Add(unit);
            }
            unitBox.SelectedItem = oldIngredient.GetUnit();

            panel.Children.Add(new TextBlock
            {
                Text = "Unit:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10),
                Foreground = textForeground
            });
            panel.Children.Add(unitBox);

            // Confirmation button 
            Button confirmButton = new Button
            {
                Content = "Confirm",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"] 
            };

            confirmButton.Click += async (s, e) =>
            {
                if (!float.TryParse(amountBox.Text, out float newAmount) || newAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount greater than 0.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string newUnit = unitBox.SelectedItem?.ToString();

                Ingredient updatedIngredient = new Ingredient(oldIngredient.GetName(), oldIngredient.GetIngType(), newAmount, newUnit);
                bool success = await backend.SetIngredient(user, oldIngredient, updatedIngredient, userList.GetListName());

                if (success)
                {
                    MessageBox.Show("Ingredient edited successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await DisplayMyLists(this.parentPanel);
                }
                else
                {
                    MessageBox.Show("Failed to update ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    popup.Close();
                }
            };

            panel.Children.Add(confirmButton);
            popup.Content = panel;
            popup.ShowDialog();
        }


        /// <summary>
        /// handler for create list button
        /// </summary>
        private async void CreateListButton_Click(object sender, RoutedEventArgs e)
        {

            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textboxBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush textboxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushA"];


            int headingFont = 20;
            int listNameFont = 20;

            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush headerForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create Popup window
            Window popup = new Window
            {
                Title = "New List",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            TextBlock createListHeader = new TextBlock
            {
                Text = "List Name:",
                FontSize = headingFont,
                Foreground = headerForeground,
                Background = background,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            TextBox listNameBox = new TextBox
            {
                Width = 300,
                Height = 30,
                FontSize = listNameFont,
                Margin = new Thickness(10),
                Background = textboxBackground,
                Foreground = textboxForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            // Create the Add button
            Button createButton = new Button
            {
                Content = "Create",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"]
            };


            createButton.Click += async (s, e) =>
            {
                string newListName = listNameBox.Text;

                if (!string.IsNullOrEmpty(newListName))
                {
                    bool success = await backend.CreateList(user, newListName);

                    // Show result message
                    if (success)
                    {
                        MessageBox.Show("List created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        await DisplayMyLists(parentPanel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create list. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        popup.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a list name", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            panel.Children.Add(createListHeader);
            panel.Children.Add(listNameBox);
            panel.Children.Add(createButton);
            popup.Content = panel;
            popup.ShowDialog();
        }

        /// <summary>
        /// Method to make create list button
        /// </summary>
        private void InitializeCreateListButton(StackPanel contentArea)
        {
            // Create the Create List button
            Button createListButton = new Button
            {
                Content = "Create List",
                Margin = new Thickness(25, 30, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                Style = (Style)App.Current.Resources["ExpandButtonStyle"] 
            };

            // Attach the click event handler for the Create List button
            createListButton.Click += CreateListButton_Click;

            // Add the Create List button to the content area
            contentArea.Children.Add(createListButton);
        }


        /// <summary>
        /// Popup for moving an ingredient from list to list
        /// </summary>
        private async Task ShowMoveIngredientPopup(string currListName, Ingredient ingredient)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush foreground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int boxHeight = 30;
            int boxWidth = 400;
            int headingFont = 20;

            // Create a new Window for the Move Ingredient popup
            Window moveWindow = new Window
            {
                Title = "Moving " + ingredient.GetName(),
                Width = 500,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = background
            };

            StackPanel movePanel = new StackPanel { Margin = new Thickness(10) };

            // ComboBox for list selection
            ComboBox listComboBox = new ComboBox
            {
                Margin = new Thickness(10),
                Height = boxHeight,
                Width = boxWidth,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = headingFont
            };

            // Get lists from backed
            List<UserList> myLists = await backend.GetMyLists(user);

            // Populate the ComboBox with user's lists, excluding the current one
            foreach (var list in myLists)
            {
                if (list.GetListName() != currListName)
                {
                    listComboBox.Items.Add(list.GetListName());
                }
            }

            movePanel.Children.Add(new TextBlock
            {
                Text = "Select a list to move the ingredient to:",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = foreground

            });
            movePanel.Children.Add(listComboBox);

            // Create OK button
            Button okButton = new Button
            {
                Content = "Move",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)App.Current.Resources["ExpandButtonStyle"]
            };

            okButton.Click += async (s, e) =>
            {
                string newListName = (string)listComboBox.SelectedItem;
                if (newListName != null)
                {
                    okButton.IsEnabled = false;
                    bool success = await backend.MoveIngredient(user, currListName, newListName, ingredient);
                    okButton.IsEnabled = true;
                    if (success)
                    {
                        MessageBox.Show("Ingredient moved successfully!");
                        await DisplayMyLists(this.parentPanel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to move ingredient. Please try again.");
                        moveWindow.Close();
                    }
                }               
            };
            movePanel.Children.Add(okButton);

            moveWindow.Content = movePanel;
            moveWindow.ShowDialog();
            
        }

    }
}