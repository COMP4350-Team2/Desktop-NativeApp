using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private List<Expander> ingExpanders;
        private ScrollViewer parentScroller;

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
            ingExpanders  = new List<Expander>();
        }

        /// <summary>
        /// Displays the "My Lists" section with collapsible ingredient lists for each user list.
        /// Each list includes a search bar to filter ingredients.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        public async Task DisplayMyLists(StackPanel contentArea, ScrollViewer parentScroller)
        {
            SolidColorBrush pageHeaderTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int pageHeaderFont = 34;


            if (this.parentPanel == null)
            {
                this.parentPanel = contentArea;
            }

            if (this.parentScroller == null)
            {
                this.parentScroller = parentScroller;
            }

            ingExpanders.Clear();

            parentPanel.Children.Clear();

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
            parentPanel.Children.Add(header);

            // Initialize create list button
            InitializeCreateListButton();

            // Retrieve user's lists from the backend
            List<UserList> myLists = await backend.GetMyLists(user);

            // Display each list in a collapsible menu with a delete button
            foreach (var userList in myLists)
            {
                AddListExpander(userList);
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
                SizeToContent = SizeToContent.WidthAndHeight,
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
                Margin = new Thickness(10, 30, 10, 10),
                Background = Brushes.Red,
                Foreground = Brushes.White
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

                    for(int i = 0; i < ingExpanders.Count; i++)
                    {
                        if (ingExpanders[i].Tag.ToString() ==  listName)
                        {
                            Expander toRemove = ingExpanders[i];
                            ingExpanders.Remove(toRemove);
                            parentPanel.Children.Remove(toRemove);
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to delete list. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                deleteButton.IsEnabled = true;

                //await DisplayMyLists(parentPanel); 
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
        ///</summary>
        /// <param name="ingredientPanel">The panel to update.</param>
        /// <param name="searchText">The panel to display content within.</param>
        /// <param name="userList">The <see cref="UserList"/> to user for population.</param>
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
                Border ingRow = CreateIngredientRow(ingredient, userList, itemWidth, ingBoxHeight, ingredientPanel);
                ingredientGrid.Children.Add(ingRow);
            }

            scrollViewer.Content = ingredientGrid;

            // Attach the PreviewMouseWheel and ScrollChanged event handlers
            scrollViewer.PreviewMouseWheel += InnerScrollViewer_PreviewMouseWheel;

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
        private Border CreateIngredientRow(Ingredient ingredient, UserList userList, double itemWidth, int ingBoxHeight, StackPanel ingPanel)
        {
            SolidColorBrush ingTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush borderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int ingTextFont = 34;

            int ingButtonsDim = ingBoxHeight / 6;
            Thickness ingButtonPadding = new Thickness(10, 10, 10, 2);

            // Create a DockPanel to display ingredient details
            DockPanel ingredientRow = new DockPanel
            {
                Margin = new Thickness(10),
                Width = itemWidth,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            // Ingredient name text
            // Reduce font size by 4 for every 10 characters (with fontSize/2 limit)
            int nameFont = int.Max(ingTextFont - 4 * (ingredient.GetName().Length / 10), ingTextFont / 2);
            TextBlock ingredientNameText = new TextBlock
            {
                Text = $"{ingredient.GetName()}",
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = nameFont,
                FontWeight = FontWeights.Bold,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
            };

            // Ingredient type text 
            // Reduce font size by 4 for every 10 characters (with fontSize/2 limit)
            int typeFont = int.Max(ingTextFont - 4 * (ingredient.GetIngType().Length / 10), ingTextFont / 2);
            TextBlock ingredientTypeText = new TextBlock
            {
                Text = $"{ingredient.GetIngType()}",
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = typeFont - 4,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
            };

            // Ingredient amount, unit text 
            // Reduce font size by 4 for every 10 characters (with fontSize/2 limit)
            string amountText = $"{ingredient.GetAmount()} {ingredient.GetUnit()}";
            int amountFont = int.Max(ingTextFont - 4 * (amountText.Length / 10), ingTextFont / 2);
            TextBlock ingredientAmountText = new TextBlock
            {
                Text = amountText,
                Foreground = ingTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = amountFont - 4,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
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
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/move_ing_icon_white.png")),
                    Height = ingButtonsDim, 
                    Width = ingButtonsDim,  
                    Stretch = Stretch.Uniform 
                },
                Margin = ingButtonPadding,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Cursor = Cursors.Hand,
                ToolTip = "Move"
            };
            moveButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            moveButton.Click += async (s, e) => await ShowMoveIngredientPopup(userList, ingredient, ingPanel);

            // Delete button
            Button deleteButton = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/del_ing_icon_white.png")),
                    Height = ingButtonsDim,
                    Width = ingButtonsDim,
                    Stretch = Stretch.Uniform
                },
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = ingButtonPadding,
                Cursor = Cursors.Hand,
                ToolTip = "Delete"
            };
            deleteButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            deleteButton.Click += async (s, e) => await ConfirmIngDeletion(ingredient, userList, ingPanel);

            // Edit button
            Button editButton = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/edit_ing_icon_white.png")),
                    Height = ingButtonsDim,
                    Width = ingButtonsDim,
                    Stretch = Stretch.Uniform
                },
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = ingButtonPadding,
                Cursor = Cursors.Hand,
                ToolTip = "Edit"
            };
            editButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];
            editButton.Click += async (s, e) => await ShowEditIngredientPopup(ingredient, userList, ingPanel);

            // Add buttons to the panel
            buttonPanel.Children.Add(moveButton);
            buttonPanel.Children.Add(editButton);
            buttonPanel.Children.Add(deleteButton);


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
                Child = ingredientRow,
                Style = (Style)Application.Current.Resources["HoverableBorder"]
            };

            return border;
        }



        /// <summary>
        /// Shows pop up window for adding an <see cref="Ingredient"/>
        /// </summary>
        /// <param name="userList">The <see cref="UserList"/> to be added to.</param>
        private async Task ShowAddIngredientPopup(UserList userList, StackPanel ingPanel)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];

            // Create Popup window
            Window popup = new Window
            {
                Title = "Add Ingredient",
                SizeToContent = SizeToContent.WidthAndHeight,
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
                Foreground = Brushes.Gray,
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
                    amountBox.Foreground = boxTextColor;
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
                string searchText = searchBox.Text.ToLower().Trim();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    nameBox.Items.Clear();

                    // If there is search text, filter the ingredients
                    foreach (var ing in allIngredients)
                    {
                        if (ing.GetName().ToLower().Contains(searchText))
                        {
                            nameBox.Items.Add(ing.GetName()); 
                        }
                    }
                    if (nameBox.Items.Count > 0)
                    {
                        nameBox.SelectedIndex = 0;
                    }
                }
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
                Ingredient newIngredient = allIngredients.First(ing => ing.GetName() == name).CopyIngredient();
                newIngredient.SetAmount(amount);
                newIngredient.SetUnit(unit);
                //string type = allIngredients.First(ing => ing.GetName() == name).GetIngType();
                

                // Create the ingredient with specified values
                //Ingredient newIngredient = new Ingredient(name, type, amount, unit);

                addButton.IsEnabled = false;

                // Add ingredient to list via backend and check success
                bool success = await backend.AddIngredientToList(user, newIngredient, userList.GetListName());

                addButton.IsEnabled = true;
                
                // Show success
                if (success)
                {
                    MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //userList.AddIngToList(newIngredient);
                    UpdateIngredientPanel(ingPanel, "", userList);
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


        /// <summary>
        /// Displays a confirmation popup for deleting an ingredient.
        /// </summary>
        /// <param name="ingredient">The <see cref="Ingredient"/> to delete.</param>
        /// <param name="listName">The list name from which the ingredient should be removed.</param>
        private async Task ConfirmIngDeletion(Ingredient ingredient, UserList userList, StackPanel ingPanel)
        {
            string listName = userList.GetListName();
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Deleting " + ingredient.GetName() + " from " + listName,
                SizeToContent = SizeToContent.WidthAndHeight,
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
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"],
                Background = Brushes.Red,
                Foreground = Brushes.White
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
                    userList.RemIngFromList(ingredient);
                    UpdateIngredientPanel(ingPanel, "", userList);
                }
                else
                {
                    MessageBox.Show("Failed to delete ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                confirmButton.IsEnabled = true;

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
        private async Task ShowEditIngredientPopup(Ingredient oldIngredient, UserList userList, StackPanel ingPanel)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create new window
            Window popup = new Window
            {
                Title = "Edit Ingredient",
                SizeToContent = SizeToContent.WidthAndHeight,
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

                confirmButton.IsEnabled = false;

                //Ingredient updatedIngredient = new Ingredient(oldIngredient.GetName(), oldIngredient.GetIngType(), newAmount, newUnit);
                Ingredient updatedIngredient = oldIngredient.CopyIngredient();
                updatedIngredient.SetUnit(newUnit);
                updatedIngredient.SetAmount(newAmount);
                bool success = await backend.SetIngredient(user, oldIngredient, updatedIngredient, userList.GetListName());

                confirmButton.IsEnabled = true;

                if (success)
                {
                    MessageBox.Show("Ingredient edited successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //userList.RemIngFromList(oldIngredient);
                    //userList.AddIngToList(updatedIngredient);
                    UpdateIngredientPanel(ingPanel, "", userList);
                }
                else
                {
                    MessageBox.Show("Failed to update ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                popup.Close();
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
                SizeToContent = SizeToContent.WidthAndHeight,
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
                string newListName = listNameBox.Text.Trim();

                if (!string.IsNullOrEmpty(newListName))
                {
                    createButton.IsEnabled = false;
                    bool success = await backend.CreateList(user, newListName);

                    // Show result message
                    if (success)
                    {
                        MessageBox.Show("List created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        UserList newList = new UserList(newListName, new List<Ingredient>());
                        AddListExpander(newList);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create list. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    createButton.IsEnabled = true;
                    popup.Close();
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
        private void InitializeCreateListButton()
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
            parentPanel.Children.Add(createListButton);
        }


        /// <summary>
        /// Popup for moving an ingredient from list to list
        /// </summary>
        private async Task ShowMoveIngredientPopup(UserList currList, Ingredient ingredient, StackPanel ingPanel)
        {
            string currListName = currList.GetListName();
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
                SizeToContent = SizeToContent.WidthAndHeight,
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
            UserList toList;

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
                toList = myLists.FirstOrDefault(u => u.GetListName() == newListName);

                if (newListName != null && toList != null)
                {
                    okButton.IsEnabled = false;
                    bool success = await backend.MoveIngredient(user, currListName, newListName, ingredient);
                    okButton.IsEnabled = true;
                    if (success)
                    {
                        MessageBox.Show("Ingredient moved successfully!");

                        //Rem from curr list and add to new list
                        //currList.RemIngFromList(ingredient);
                        //toList.AddIngToList(ingredient);

                        //Update both
                        UpdateIngredientPanel(ingPanel, "", currList);
                        //UpdateIngredientPanel(FindIngPanel(newListName), "", toList);

                        // Fully rebuild the target `Expander`
                        Expander targetExpander = ingExpanders.FirstOrDefault(e => e.Tag.ToString() == newListName);
                        if (targetExpander != null)
                        {
                            // Clear the current content
                            targetExpander.Content = null;

                            // Create a new StackPanel and populate it with ingredients
                            StackPanel ingredientPanel = new StackPanel();
                            UpdateIngredientPanel(ingredientPanel, "", toList);

                            // Assign the new panel as the `Expander`'s content
                            targetExpander.Content = ingredientPanel;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Failed to move ingredient. Please try again.");
                    }
                    moveWindow.Close();
                }               
            };
            movePanel.Children.Add(okButton);

            moveWindow.Content = movePanel;
            moveWindow.ShowDialog();
            
        }

        /// <summary>
        /// Method to add new expander for a list being created
        /// </summary>
        /// <param name="userList"> The list to populate expander with</param>
        private void AddListExpander(UserList userList)
        {
            SolidColorBrush listHeaderTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush ingredientButtonCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush searchBarBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush searchBarTxtCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush expanderIconCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush expanderBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush dropDownBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush dropDownForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int listHeaderFont = 28;
            int ingredientButtonFont = 28;
            int searchBarFont = 24;
            int optionsFont = 20;
            int dropDownOptHeight = 50;

            double availableWidth = SystemParameters.PrimaryScreenWidth;
            double itemWidth = availableWidth / 2 - 200;

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

            StackPanel ingredientPanel = new StackPanel();

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
                Cursor = Cursors.Hand,
                ToolTip = "Options"
            };
            optionsButton.Style = (Style)Application.Current.Resources["NoHighlightButton"];

            // Set the options button in the second column
            Grid.SetColumn(optionsButton, 1);
            headerGrid.Children.Add(optionsButton);

            // Create the Expander for the list, using the grid for the header
            Expander listExpander = new Expander
            {
                Header = headerGrid,
                FontSize = 18,
                Foreground = expanderIconCol,
                Margin = new Thickness(30),
                BorderThickness = new Thickness(2),
                BorderBrush = expanderBorderCol,
                Background = (Brush)Application.Current.Resources["ExpanderBrushA"],
                Tag = userList.GetListName()
            };

            // Create the dropdown menu 
            ContextMenu dropdownMenu = new ContextMenu { Background = dropDownBackground };

            // Add the options to the menu
            MenuItem addIngredientOption = new MenuItem { Header = "Add Ingredient" };
            addIngredientOption.Click += async (s, e) => await ShowAddIngredientPopup(userList, ingredientPanel);
            addIngredientOption.Foreground = dropDownForeground;
            addIngredientOption.FontSize = optionsFont;
            addIngredientOption.Height = dropDownOptHeight;
            dropdownMenu.Items.Add(addIngredientOption);

            MenuItem renameListOption = new MenuItem { Header = "Rename List" };
            renameListOption.Click += async (s, e) => await RenameList(listExpander.Tag.ToString(), listHeader, userList);
            renameListOption.Foreground = dropDownForeground;
            renameListOption.FontSize = optionsFont;
            renameListOption.Height = dropDownOptHeight;
            dropdownMenu.Items.Add(renameListOption);

            MenuItem deleteListOption = new MenuItem { Header = "Delete List"};
            deleteListOption.Click += async (s, e) => await ConfirmDeleteList(userList.GetListName());
            deleteListOption.Background = Brushes.Red;
            deleteListOption.Foreground = dropDownForeground;
            deleteListOption.FontSize = optionsFont;
            deleteListOption.FontWeight = FontWeights.Bold;
            deleteListOption.Style = (Style)App.Current.Resources["CustomMenuItemStyle"];
            deleteListOption.Height = dropDownOptHeight;
            dropdownMenu.Items.Add(deleteListOption);

            // Show the dropdown menu when the options button is clicked
            optionsButton.Click += (sender, e) => { dropdownMenu.IsOpen = true; };

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
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0),
                BorderThickness = new Thickness(0)

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
            parentPanel.Children.Add(listExpander);

            ingExpanders.Add(listExpander);

        }
        /// <summary>
        /// Method to handle bubbling scroll event for list scrollers
        /// </summary>
        private void InnerScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int delta = e.Delta;
            var scv = sender as ScrollViewer;
            if (scv == null) return;

            // Check if we're at the bottom or top of the inner ScrollViewer
            bool atBottom = scv.VerticalOffset >= scv.ExtentHeight - scv.ViewportHeight;
            bool atTop = scv.VerticalOffset <= 0.0;


            // If at the bottom and scrolling up, transfer control to the parent
            if (delta < 0 && atBottom)
            {
                // Transfer control to the parent ScrollViewer
                double offset = double.MinNumber(parentScroller.MaxHeight, parentScroller.VerticalOffset - delta/2);
                parentScroller.ScrollToVerticalOffset(offset);
                e.Handled = false;  // Let the parent handle the event (smooth bubbling)
            }
            // If at the top and scrolling down, transfer control to the parent
            else if (delta > 0 && atTop)
            {
                // Transfer control to the parent ScrollViewer
                double offset = double.MaxNumber(0.0, parentScroller.VerticalOffset - delta/2);
                parentScroller.ScrollToVerticalOffset(offset);
                e.Handled = false;  // Let the parent handle the event (smooth bubbling)
            }
            else
            {
                // Let the inner ScrollViewer handle scrolling normally
                e.Handled = false;  // Allow inner ScrollViewer to handle the event
            }
        }

        /// <summary>
        /// Method called when rename list is clicked from options dropdown
        /// </summary>
        /// <param name="listName"> Name of the list being renamed</param>
        /// <param name="listHeader"> Header containing the list name (for editing)</param>
        /// <param name="userList"> UserList to be renamed (for editing)</param>
        /// <returns></returns>
        private async Task RenameList(string listName, TextBlock listHeader, UserList userList)
        {
            // Define the brushes for the popup background and text
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int textFont = 18;

            // Create Popup window
            Window popup = new Window
            {
                Title = "Renaming " + listName,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            // Panel for the popup content
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Confirmation message text
            TextBlock confirmationText = new TextBlock
            {
                Text = "Enter new name for list",
                Foreground = textColor,
                FontSize = textFont,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };

            panel.Children.Add(confirmationText);

            // Amount
            TextBox newNameBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Width = 400,
                Height = 50,
                Margin = new Thickness(10)
            };

            panel.Children.Add(newNameBox);

            // Create the delete confirmation button 
            Button renameButton = new Button
            {
                Content = "Rename",
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"],
                Margin = new Thickness(10, 30, 10, 10),
            };

            // Handle delete button click
            renameButton.Click += async (s, e) =>
            {
                renameButton.IsEnabled = false;

                newNameBox.IsEnabled = false;

                string newListName = newNameBox.Text.ToString().Trim();

                List<UserList> allLists = await backend.GetMyLists(user);

                bool alreadyExists = false;

                for(int i = 0; i < allLists.Count && !alreadyExists; i++)
                {
                    if (allLists[i].GetListName() == newListName)
                    {
                        MessageBox.Show($"{newListName} already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        alreadyExists = true;
                    }
                }

                if (!alreadyExists)
                {

                    bool success = await backend.RenameList(user, listName, newListName);

                    // Show result message
                    if (success)
                    {
                        MessageBox.Show($"List renamed successfully to {newListName}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        listHeader.Text = newListName;
                        userList.SetListName(newListName);

                        for (int i = 0; i < ingExpanders.Count; i++)
                        {
                            if (ingExpanders[i].Tag.ToString() == listName)
                            {
                                ingExpanders[i].Tag = newListName;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to rename list. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    popup.Close();

                }


                renameButton.IsEnabled = true;
                newNameBox.IsEnabled = true;

            };

            panel.Children.Add(renameButton);

            // Set the popup content
            popup.Content = panel;

            // Show the popup as a modal dialog
            popup.ShowDialog();
        }
    }
}