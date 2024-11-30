using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Desktop_Frontend.Components
{
    /// <summary>
    /// Manages the "All Ingredients" feature, handling ingredient list display and search functionality.
    /// </summary>
    public class AllIngredientsHandler
    {
        private readonly IBackend backend;
        private readonly IUser user;
        private StackPanel parentPanel;
        private ScrollViewer scrollViewer;
        private string selectedCatagory;

        /// <summary>
        /// Initializes an instance of the <see cref="AllIngredientsHandler"/> class.
        /// </summary>
        /// <param name="backend">Backend service for data retrieval.</param>
        /// <param name="user">Authenticated user instance.</param>
        public AllIngredientsHandler(IBackend backend, IUser user)
        {
            this.backend = backend;
            this.user = user;
            parentPanel = null;
            scrollViewer = null;
            selectedCatagory = "All";
        }

        /// <summary>
        /// Asynchronously displays all ingredients in the specified panel.
        /// Creates a search box for filtering the ingredients list in real-time.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        //public async Task DisplayIngredientsAsync(StackPanel contentArea)
        //{
        //    SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
        //    int headerFont = 34;

        //    if (parentPanel == null)
        //    {
        //        parentPanel = contentArea;
        //    }

        //    parentPanel.Children.Clear();

        //    // Create and add header
        //    TextBlock header = new TextBlock
        //    {
        //        Text = "All Ingredients",
        //        FontSize = headerFont,
        //        FontWeight = FontWeights.Bold,
        //        Foreground = headerText,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        Margin = new Thickness(0, 20, 0, 20)
        //    };
        //    parentPanel.Children.Add(header);

        //    // Create a StackPanel to hold the selection, the search box and scrollable content
        //    StackPanel stackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };

        //    // NEW COMPONENT HERE

        //    // Create and add the search box
        //    Border searchBox = CreateSearchBox();
        //    stackPanel.Children.Add(searchBox);

        //    // Retrieve ingredients
        //    List<Ingredient> ingredients = await backend.GetAllIngredients(user);

        //    // Create the ingredient grid
        //    UniformGrid ingredientGrid = new UniformGrid
        //    {
        //        Rows = (int)Math.Ceiling((double)ingredients.Count / 3), // Calculate rows dynamically
        //        Columns = 3, // 3 items per row
        //        Margin = new Thickness(20, 10, 10, 10)
        //    };

        //    // Create a ScrollViewer to make the ingredient grid scrollable
        //    scrollViewer = new ScrollViewer
        //    {
        //        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        //        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
        //        Margin = new Thickness(0, 10, 0, 0)
        //    };

        //    // Populate the grid with ingredients
        //    PopulateIngredientGrid(ingredients, ingredientGrid);

        //    // Set the ingredient grid as the content of the ScrollViewer
        //    scrollViewer.Content = ingredientGrid;

        //    // Calculate the available height for the scrollable area by subtracting the height of the header and search box
        //    double availableHeight = parentPanel.ActualHeight - header.ActualHeight - searchBox.ActualHeight - parentPanel.ActualHeight / 7;

        //    // Set the height of the ScrollViewer dynamically based on available space
        //    scrollViewer.Height = availableHeight > 0 ? availableHeight : 300; // Default to 300 if available space is too small

        //    // Add the ScrollViewer to the StackPanel
        //    stackPanel.Children.Add(scrollViewer);

        //    // Add the StackPanel to the parent panel
        //    parentPanel.Children.Add(stackPanel);

        //    // Update ingredients list based on search text
        //    ((TextBox)searchBox.Child).TextChanged += (s, e) =>
        //        FilterIngredients(ingredients, ((TextBox)searchBox.Child).Text.Trim(), ingredientGrid);
        //}

        public async Task DisplayIngredientsAsync(StackPanel contentArea)
        {
            SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush radioButtonCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int headerFont = 34;
            int radioButtonFont = 24;

            if (parentPanel == null)
            {
                parentPanel = contentArea;
            }

            parentPanel.Children.Clear();

            int radioButtonSize = (int)(parentPanel.ActualWidth / 5);

            // Create and add header
            TextBlock header = new TextBlock
            {
                Text = "All Ingredients",
                FontSize = headerFont,
                FontWeight = FontWeights.Bold,
                Foreground = headerText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };
            parentPanel.Children.Add(header);

            // Create a StackPanel to hold the selection, the search box, and scrollable content
            StackPanel stackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };

            // Add the radio button group above the search bar
            StackPanel radioButtonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10),
                Background = Brushes.Transparent
            };


            // Create radio buttons
            RadioButton allButton = new RadioButton
            {
                Content = "All",
                IsChecked = true,
                Margin = new Thickness(10, 0, 10, 0),
                Foreground = radioButtonCol,
                FontSize = radioButtonFont,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                //Height = radioButtonSize,
                Width = radioButtonSize
            };
            RadioButton commonButton = new RadioButton
            {
                Content = "Common Only",
                Margin = new Thickness(10, 0, 10, 0),
                Foreground = radioButtonCol,
                FontSize = radioButtonFont,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                //Height = radioButtonSize,
                Width = radioButtonSize
            };
            RadioButton customButton = new RadioButton
            {
                Content = "Custom Only",
                Margin = new Thickness(10, 0, 10, 0),
                Foreground = radioButtonCol,
                FontSize = radioButtonFont,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                //Height = radioButtonSize,
                Width = radioButtonSize
            };

            // Add radio buttons to the panel
            radioButtonPanel.Children.Add(allButton);
            radioButtonPanel.Children.Add(commonButton);
            radioButtonPanel.Children.Add(customButton);

            // Add the radio button panel to the stack panel
            stackPanel.Children.Add(radioButtonPanel);

            // Add the search box
            Border searchBox = CreateSearchBox();
            stackPanel.Children.Add(searchBox);

            // Retrieve ingredients
            List<Ingredient> ingredients = await backend.GetAllIngredients(user);

            // Create the ingredient grid
            UniformGrid ingredientGrid = new UniformGrid
            {
                Rows = (int)Math.Ceiling((double)ingredients.Count / 3), // Calculate rows dynamically
                Columns = 3, // 3 items per row
                Margin = new Thickness(20, 10, 10, 10)
            };

            // Create a ScrollViewer to make the ingredient grid scrollable
            scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // Populate the grid with ingredients
            PopulateIngredientGrid(ingredients, ingredientGrid);

            // Set the ingredient grid as the content of the ScrollViewer
            scrollViewer.Content = ingredientGrid;

            // Calculate the available height for the scrollable area by subtracting the height of the header and search box
            double availableHeight = parentPanel.ActualHeight - header.ActualHeight - searchBox.ActualHeight - parentPanel.ActualHeight / 7;

            // Set the height of the ScrollViewer dynamically based on available space
            scrollViewer.Height = availableHeight > 0 ? availableHeight : 300; // Default to 300 if available space is too small

            // Add the ScrollViewer to the StackPanel
            stackPanel.Children.Add(scrollViewer);

            // Add the StackPanel to the parent panel
            parentPanel.Children.Add(stackPanel);

            // Update the ingredients list based on the selected radio button and search text
            allButton.Checked += (s, e) =>
            {
                this.selectedCatagory = "All";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            commonButton.Checked += (s, e) =>
            {
                this.selectedCatagory = "Common";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            customButton.Checked += (s, e) =>
            {
                this.selectedCatagory = "Custom";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            // Update ingredients list based on search text
            ((TextBox)searchBox.Child).TextChanged += (s, e) =>
                FilterIngredients(ingredients, ((TextBox)searchBox.Child).Text.Trim(), ingredientGrid);
        }





        /// <summary>
        /// Populates the grid with ingredients, ensuring 4 items per row.
        /// </summary>
        /// <param name="ingredients">List of ingredients to display.</param>
        /// <param name="ingredientGrid">The grid to populate with ingredient rows.</param>
        private void PopulateIngredientGrid(List<Ingredient> ingredients, UniformGrid ingredientGrid)
        {
            scrollViewer.ScrollToVerticalOffset(0);

            ingredientGrid.Children.Clear();

            // Populate grid with ingredients
            foreach (var ingredient in ingredients)
            {
                Border ingredientBorder = CreateIngredientRow(ingredient.CopyIngredient());
                ingredientGrid.Children.Add(ingredientBorder); // Add ingredient to grid
            }
        }


        /// <summary>
        /// Filters and updates the ingredients list based on the provided search text.
        /// </summary>
        /// <param name="ingredients">Original list of ingredients.</param>
        /// <param name="filterText">Text to filter ingredients by.</param>
        /// <param name="ingredientGrid">The grid to update with filtered ingredients.</param>
        private void FilterIngredients(List<Ingredient> ingredients, string filterText, UniformGrid ingredientGrid)
        {
            var filteredIngredients = ingredients
                .Where(i => i.GetName().Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                            i.GetIngType().Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (selectedCatagory == "Custom")
            {
                filteredIngredients = filteredIngredients.Where(i => i.IsCustom()).ToList();
            }
            else if (selectedCatagory == "Common")
            {
                filteredIngredients = filteredIngredients.Where(i => !i.IsCustom()).ToList();
            }

            PopulateIngredientGrid(filteredIngredients, ingredientGrid);
        }


        /// <summary>
        /// Creates a TextBox for ingredient search with placeholder text.
        /// </summary>
        /// <returns>A configured TextBox for searching ingredients.</returns>
        private Border CreateSearchBox()
        {
            SolidColorBrush boxBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush boxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int boxFont = 24;

            // Create a border for rounded edges
            Border searchBoxBorder = new Border
            {
                Height = 50,
                Margin = new Thickness(25, 10, 30, 10),
                CornerRadius = new CornerRadius(5),
                BorderBrush = boxForeground,
                BorderThickness = new Thickness(1),
                Background = boxBackground
            };

            // Create the search box
            TextBox searchBox = new TextBox
            {
                Height = 50,
                Foreground = boxForeground,
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = "Search ingredients...",
                FontSize = boxFont,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(10, 0, 0, 0)
            };

            // Clear placeholder text when the box is focused
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search ingredients...")
                {
                    searchBox.Text = string.Empty;
                }
            };

            // Restore placeholder text if the box is empty when focus is lost
            searchBox.LostFocus += async (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search ingredients...";
                    await DisplayIngredientsAsync(parentPanel);
                }
            };

            searchBoxBorder.Child = searchBox;

            return searchBoxBorder;
        }


        /// <summary>
        /// Creates a row with ingredient details and an add button, wrapped in a border.
        /// </summary>
        /// <param name="ingredient">The ingredient to display in the row.</param>
        /// <returns>A styled Border containing the ingredient row.</returns>
        private Border CreateIngredientRow(Ingredient ingredient)
        {
            // Define resource-based colors and font sizes
            SolidColorBrush boxTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int ingNameFont = 28;
            int ingTypeFont = 26;

            int boxButtonFont = 45;

            // Create ingredient row 
            DockPanel ingredientRow = CreateIngredientRowPanel();

            // Create a container for the text blocks
            StackPanel textContainer = new StackPanel
            {
                Orientation = Orientation.Vertical,
                MaxWidth = parentPanel.ActualWidth / 4,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            // Create and add the ingredient name TextBlock
            TextBlock ingredientNameText = CreateTextBlock($"{ingredient.GetName()}", boxTextCol, ingNameFont, FontWeights.Bold);
            textContainer.Children.Add(ingredientNameText);

            // Create and add the ingredient type TextBlock
            TextBlock ingredientTypeText = CreateTextBlock($"{ingredient.GetIngType()}", boxTextCol, ingTypeFont);
            textContainer.Children.Add(ingredientTypeText);

            // Add text container to the row
            DockPanel.SetDock(textContainer, Dock.Top);
            ingredientRow.Children.Add(textContainer);

            // Create a Grid to align the "+" button and custom icon
            Grid bottomGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            // Define two columns: one for the icon and one for the button
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Icon column
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Spacer
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Button column

            // Add custom icon if needed
            if (ingredient.IsCustom())
            {
                // Create an Image control for the custom icon
                Image customIcon = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/custom_ing_icon_white.png")),
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                // Place the icon in the first column
                Grid.SetColumn(customIcon, 0);
                bottomGrid.Children.Add(customIcon);
                ingredientRow.Tag = "custom";
            }

            // Create and add the "+" button
            Button addButton = CreateAddButton(boxTextCol, boxButtonFont);
            addButton.Click += (s, e) => ShowAddIngredientPopup(ingredient.CopyIngredient());

            // Place the button in the last column
            Grid.SetColumn(addButton, 2);
            bottomGrid.Children.Add(addButton);

            // Add the bottomGrid to the DockPanel
            DockPanel.SetDock(bottomGrid, Dock.Bottom);
            ingredientRow.Children.Add(bottomGrid);

            // Wrap the ingredient row in a border
            Border border = CreateIngredientBorder(boxBorderCol, ingredientRow);

            if (ingredient.IsCustom())
            {
                border.ToolTip = "Custom Ingredient";
            }

            return border;
        }

        /// <summary>
        /// Creates a panel to put each ingredient info row in
        /// </summary>
        /// <returns>A DockPanel with the correct sizing and configuration.</returns>
        private DockPanel CreateIngredientRowPanel()
        {
            return new DockPanel { Margin = new Thickness(5) };
        }

        /// <summary>
        /// Creates a TextBlock pertaining to info given (used to populate ingredient borders)
        /// </summary>
        /// <returns>A texblock to filled in ingredient border.</returns>
        private TextBlock CreateTextBlock(string text, SolidColorBrush foreground, int fontSize, FontWeight fontWeight = default)
        {
            // Reduce font size by 2 for every 10 characters (with fontSize/2 limit)
            fontSize = int.Max(fontSize - 2 * (text.Length / 10), fontSize / 2);
            return new TextBlock
            {
                Text = text,
                Foreground = foreground,
                FontSize = fontSize,
                FontWeight = fontWeight,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10)
            };
        }

        /// <summary>
        /// Creates a + button (used to populate ingredient borders)
        /// </summary>
        /// <returns>A + button with the appropriate configuration.</returns>
        private Button CreateAddButton(SolidColorBrush foreground, int fontSize)
        {
            return new Button
            {
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Content = new TextBlock
                {
                    Text = "+",
                    Foreground = foreground,
                    FontSize = fontSize,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    ToolTip = "Add to list"
                },
                Cursor = Cursors.Hand,
                Style = (Style)Application.Current.Resources["NoHighlightButton"]
            };
        }

        /// <summary>
        /// Creates a Border for an ingredient (used to populate ingredient borders)
        /// </summary>
        /// <param name="child">The UI to be bordered.</param>
        /// <returns>A texblock to filled in ingredient border.</returns>
        private Border CreateIngredientBorder(SolidColorBrush borderBrush, UIElement child)
        {
            return new Border
            {
                BorderBrush = borderBrush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Child = child,
                Style = (Style)Application.Current.Resources["HoverableBorder"]
            };
        }


        /// <summary>
        /// Creates a popup for specifying ingredient amount, unit, and list name when the add button is clicked.
        /// </summary>
        /// <param name="ingredient">The ingredient to be added.</param>
        private async void ShowAddIngredientPopup(Ingredient ingredient)
        {
            int boxWidth = 300;
            int boxHeight = 30;
            int boxFont = 18;
            int headingFont = 20;

            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush boxColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxTextColor = (SolidColorBrush)App.Current.Resources["SecondaryBrushA"];
            SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];

            // Create Popup window
            Window popup = new Window
            {
                Title = "Adding " + ingredient.GetName(),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            // Amount input with placeholder
            TextBox amountBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = "Enter amount",
                Foreground = boxTextColor,
                Background = boxColor,
                Margin = new Thickness(10),
                FontSize = boxFont,
                Height = boxHeight,
                Width = boxWidth,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Placeholder functionality: clear on focus, restore on defocus if empty
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
                    amountBox.Foreground = boxTextColor;
                }
            };
            panel.Children.Add(new TextBlock
            {
                Text = "Amount:",
                Foreground = headerText,
                Margin = new Thickness(10),
                FontSize = headingFont,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            panel.Children.Add(amountBox);

            // Get list of units and populate ComboBox
            ComboBox unitBox = new ComboBox
            {
                Margin = new Thickness(10),
                Foreground = boxTextColor,
                FontSize = boxFont,
                Height = boxHeight,
                Width = boxWidth,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            List<string> units = await backend.GetAllMeasurements(user); // Async call for units
            foreach (var unit in units)
            {
                unitBox.Items.Add(unit);
            }
            if (unitBox.Items.Count > 0) unitBox.SelectedIndex = 0; // Set default selection
            panel.Children.Add(new TextBlock
            {
                Text = "Unit:",
                Foreground = headerText,
                Margin = new Thickness(10),
                FontSize = headingFont,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            panel.Children.Add(unitBox);

            // Get list of user lists and populate ComboBox
            ComboBox listBox = new ComboBox
            {
                Margin = new Thickness(10),
                Foreground = boxTextColor,
                FontSize = boxFont,
                Height = boxHeight,
                Width = boxWidth,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            List<UserList> userLists = await backend.GetMyLists(user); // Async call for user lists
            foreach (var userList in userLists)
            {
                listBox.Items.Add(userList.GetListName());
            }
            if (listBox.Items.Count > 0) listBox.SelectedIndex = 0; // Set default selection
            panel.Children.Add(new TextBlock
            {
                Text = "List Name:",
                Foreground = headerText,
                Margin = new Thickness(10),
                FontSize = headingFont,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            panel.Children.Add(listBox);

            // Create the Add button
            Button addButton = new Button
            {
                Content = "Add",
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"],
                Margin = new Thickness(10, 30, 10, 10),
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
