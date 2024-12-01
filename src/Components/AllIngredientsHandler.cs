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
        private List<Ingredient> ingredients;

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
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        //public async Task DisplayIngredientsAsync(StackPanel contentArea)
        //{
        //    SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
        //    int headerFont = 34;

        //    // Retrieve ingredients
        //    //List<Ingredient> ingredients = await backend.GetAllIngredients(user);
        //    ingredients = await backend.GetAllIngredients(user);

        //    // Create the ingredient grid
        //    UniformGrid ingredientGrid = new UniformGrid
        //    {
        //        Rows = (int)Math.Ceiling((double)ingredients.Count / 3), // Calculate rows dynamically
        //        Columns = 3, // 3 items per row
        //        Margin = new Thickness(20, 10, 10, 10)
        //    };

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

        //    // Create a StackPanel to hold the toggle button group and the rest of the UI
        //    StackPanel stackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };

        //    // Create a Grid with 3 columns: first fixed width for the button, second auto width for the toggle panel, third fixed width for spacing
        //    Grid grid = new Grid
        //    {
        //        HorizontalAlignment = HorizontalAlignment.Stretch,
        //        Margin = new Thickness(0, 10, 0, 10)
        //    };

        //    // Define column definitions
        //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45 + (parentPanel.ActualWidth / 5)) }); // Fixed width for the Create button
        //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Auto width for the toggle panel
        //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(parentPanel.ActualWidth / 5) }); // Fixed width for spacing

        //    // Create the Create Ingredient button
        //    Button createIngButton = new Button
        //    {
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        Margin = new Thickness(25, 5, 10, 10),
        //        Style = (Style)App.Current.Resources["ExpandButtonStyle"],
        //        Background = Brushes.Transparent,
        //        BorderBrush = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"],
        //        BorderThickness = new Thickness(2),
        //        Width = 150,
        //        Height = 60,
        //        ToolTip = "Create Custom Ingredient"
        //    };

        //    createIngButton.Click += async (s, e) => await CreateCustomIngredientPopop(ingredientGrid); 



        //    // Create a StackPanel to hold the icon and the "+" text inside the button
        //    StackPanel buttonContent = new StackPanel
        //    {
        //        Orientation = Orientation.Horizontal,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Center
        //    };

        //    // Add the icon
        //    Image icon = new Image
        //    {
        //        Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/custom_ing_icon_white.png")),
        //        Height = 30,
        //        Width = 30,
        //        VerticalAlignment = VerticalAlignment.Center
        //    };
        //    buttonContent.Children.Add(icon);

        //    // Add the "+" sign after the icon
        //    TextBlock plusSign = new TextBlock
        //    {
        //        Text = "+",
        //        VerticalAlignment = VerticalAlignment.Center,
        //        FontSize = 32,
        //        FontWeight = FontWeights.Bold,
        //        Foreground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"]
        //    };
        //    buttonContent.Children.Add(plusSign);

        //    // Set the content of the button to the stack with icon and text
        //    createIngButton.Content = buttonContent;

        //    // Place the Create Ingredient button in the first column
        //    Grid.SetColumn(createIngButton, 0);
        //    grid.Children.Add(createIngButton);

        //    // Create the toggle button group (this will go in the second column)
        //    StackPanel toggleButtonGroup = new StackPanel
        //    {
        //        Orientation = Orientation.Horizontal,
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        Margin = new Thickness(10, 0, 10, 0) // Add some margin for separation
        //    };

        //    // Define toggle buttons for each option
        //    ToggleButton allButton = CreateToggleButton("All", isChecked: true);
        //    ToggleButton commonButton = CreateToggleButton("Common");
        //    ToggleButton customButton = CreateToggleButton("Custom");

        //    // Group toggle buttons to allow only one active at a time
        //    allButton.Checked += (s, e) =>
        //    {
        //        commonButton.IsChecked = false;
        //        customButton.IsChecked = false;
        //        this.selectedCatagory = "All";
        //        FilterIngredients(ingredients, "", ingredientGrid);
        //    };

        //    commonButton.Checked += (s, e) =>
        //    {
        //        allButton.IsChecked = false;
        //        customButton.IsChecked = false;
        //        this.selectedCatagory = "Common";
        //        FilterIngredients(ingredients, "", ingredientGrid);
        //    };

        //    customButton.Checked += (s, e) =>
        //    {
        //        allButton.IsChecked = false;
        //        commonButton.IsChecked = false;
        //        this.selectedCatagory = "Custom";
        //        FilterIngredients(ingredients, "", ingredientGrid);
        //    };

        //    // Add buttons to the group
        //    toggleButtonGroup.Children.Add(allButton);
        //    toggleButtonGroup.Children.Add(commonButton);
        //    toggleButtonGroup.Children.Add(customButton);

        //    // Place the toggle button group in the second column (centered)
        //    Grid.SetColumn(toggleButtonGroup, 1);
        //    grid.Children.Add(toggleButtonGroup);

        //    // Add the grid to the main StackPanel (stackPanel)
        //    stackPanel.Children.Add(grid);

        //    // Add the search box and the rest of the content below the toggle button group
        //    Border searchBox = CreateSearchBox();
        //    stackPanel.Children.Add(searchBox);

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
            int headerFont = 34;

            // Retrieve ingredients
            ingredients = await backend.GetAllIngredients(user);

            // Create the ingredient grid
            UniformGrid ingredientGrid = new UniformGrid
            {
                Rows = (int)Math.Ceiling((double)ingredients.Count / 3), // Calculate rows dynamically
                Columns = 3, // 3 items per row
                Margin = new Thickness(20, 10, 10, 10)
            };

            if (parentPanel == null)
            {
                parentPanel = contentArea;
            }

            parentPanel.Children.Clear();

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

            // Create a StackPanel to hold the toggle button group and the rest of the UI
            StackPanel stackPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };

            // Create a Grid with 3 columns: first fixed width for the button, second auto width for the toggle panel, third fixed width for spacing
            Grid grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 10, 0, 10)
            };

            // Define column definitions
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(parentPanel.ActualWidth / 5) }); // Fixed width for the Create button
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // This column will resize with the screen size
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(parentPanel.ActualWidth / 5) }); // Fixed width for spacing

            grid.LayoutUpdated += (s, e) =>
            {
                // Recalculate column widths when layout updates
                grid.ColumnDefinitions[0].Width = new GridLength(parentPanel.ActualWidth / 5); 
                grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);  
                grid.ColumnDefinitions[2].Width = new GridLength(parentPanel.ActualWidth / 5); 
            };

            // Create the Create Ingredient button
            Button createIngButton = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(25, 5, 10, 10),
                Style = (Style)App.Current.Resources["ExpandButtonStyle"],
                Background = Brushes.Transparent,
                BorderBrush = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"],
                BorderThickness = new Thickness(2),
                Width = 150,
                Height = 60,
                ToolTip = "Create Custom Ingredient"
            };

            createIngButton.Click += async (s, e) => await CreateCustomIngredientPopop(ingredientGrid);

            // Create a StackPanel to hold the icon and the "+" text inside the button
            StackPanel buttonContent = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Add the icon
            Image icon = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/custom_ing_icon_white.png")),
                Height = 30,
                Width = 30,
                VerticalAlignment = VerticalAlignment.Center
            };
            buttonContent.Children.Add(icon);

            // Add the "+" sign after the icon
            TextBlock plusSign = new TextBlock
            {
                Text = "+",
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 32,
                FontWeight = FontWeights.Bold,
                Foreground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"]
            };
            buttonContent.Children.Add(plusSign);

            // Set the content of the button to the stack with icon and text
            createIngButton.Content = buttonContent;

            // Place the Create Ingredient button in the first column
            Grid.SetColumn(createIngButton, 0);
            grid.Children.Add(createIngButton);

            // Create the toggle button group (this will go in the second column)
            StackPanel toggleButtonGroup = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0) // Add some margin for separation
            };

            // Define toggle buttons for each option
            ToggleButton allButton = CreateToggleButton("All", isChecked: true);
            ToggleButton commonButton = CreateToggleButton("Common");
            ToggleButton customButton = CreateToggleButton("Custom");

            // Group toggle buttons to allow only one active at a time
            allButton.Checked += (s, e) =>
            {
                commonButton.IsChecked = false;
                customButton.IsChecked = false;
                this.selectedCatagory = "All";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            commonButton.Checked += (s, e) =>
            {
                allButton.IsChecked = false;
                customButton.IsChecked = false;
                this.selectedCatagory = "Common";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            customButton.Checked += (s, e) =>
            {
                allButton.IsChecked = false;
                commonButton.IsChecked = false;
                this.selectedCatagory = "Custom";
                FilterIngredients(ingredients, "", ingredientGrid);
            };

            // Add buttons to the group
            toggleButtonGroup.Children.Add(allButton);
            toggleButtonGroup.Children.Add(commonButton);
            toggleButtonGroup.Children.Add(customButton);

            // Place the toggle button group in the second column (centered)
            Grid.SetColumn(toggleButtonGroup, 1);
            grid.Children.Add(toggleButtonGroup);

            // Add the grid to the main StackPanel (stackPanel)
            stackPanel.Children.Add(grid);

            // Add the search box and the rest of the content below the toggle button group
            Border searchBox = CreateSearchBox();
            stackPanel.Children.Add(searchBox);

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

            // Update ingredients list based on search text
            ((TextBox)searchBox.Child).TextChanged += (s, e) =>
                FilterIngredients(ingredients, ((TextBox)searchBox.Child).Text.Trim(), ingredientGrid);
        }

        /// <summary>
        /// Helper method to create toggle buttons on top of page
        /// </summary>
        /// <param name="text"> Text in the button </param>
        /// <param name="isChecked"> bool indicating whether it is checked </param>
        /// <returns> ToggleButton created </returns>
        private ToggleButton CreateToggleButton(string text, bool isChecked = false)
        {
            SolidColorBrush toggleForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create the toggle button first
            ToggleButton toggleButton = new ToggleButton
            {
                Content = text,
                IsChecked = isChecked,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10),
                Padding = new Thickness(10, 5, 10, 5),
                Style = (Style)App.Current.Resources["RoundedToggleButtonStyle"],
                Foreground = toggleForeground,
                Cursor = Cursors.Hand
            };

            // Initially set the width and height
            toggleButton.Width = parentPanel.ActualWidth / 6;
            toggleButton.Height = parentPanel.ActualWidth / 32;

            // Bind to LayoutUpdated to handle resizing dynamically when parent size changes
            parentPanel.LayoutUpdated += (s, e) =>
            {
                toggleButton.Width = parentPanel.ActualWidth / 6;
            };

            return toggleButton;
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
                Border ingredientBorder = CreateIngredientRow(ingredient.CopyIngredient(), ingredientGrid);
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
        private Border CreateIngredientRow(Ingredient ingredient, UniformGrid ingGrid)
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

                // Delete button
                Button deleteButton = new Button
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/del_icon_red.png")),
                        Height = 30,
                        Width = 30,
                        Stretch = Stretch.Uniform
                    },
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Cursor = Cursors.Hand,
                    ToolTip = "Delete",
                    Style = (Style)Application.Current.Resources["NoHighlightButton"],
                    Margin = new Thickness(0)
                };

                deleteButton.Click += async (s, e) => await DeleteCustomIngredientPopup(ingredient, ingGrid);

                // Place the button in the last column
                Grid.SetColumn(deleteButton, 1);
                bottomGrid.Children.Add(deleteButton);

            }

            // Create and add the "+" button
            Button addButton = CreateAddButton(boxTextCol, boxButtonFont);
            addButton.Click += async (s, e) =>
            {
                addButton.IsEnabled = false;
                await ShowAddIngredientPopup(ingredient.CopyIngredient());
                addButton.IsEnabled = true;
            };

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
                Width = 40,
                Height = 40,
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
                    VerticalAlignment = VerticalAlignment.Bottom,
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
        private async Task ShowAddIngredientPopup(Ingredient ingredient)
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

        /// <summary>
        /// Shows a dialog box when creating a custom ingredient
        /// </summary>
        private async Task CreateCustomIngredientPopop(UniformGrid ingGrid)
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
                Title = "Create Custom Ingredient",
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            TextBlock ingNameHeader = new TextBlock
            {
                Text = "Name:",
                FontSize = headingFont,
                Foreground = headerForeground,
                Background = background,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            TextBox ingNameBox = new TextBox
            {
                Width = 300,
                Height = 30,
                FontSize = listNameFont,
                Margin = new Thickness(10),
                Background = textboxBackground,
                Foreground = textboxForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            TextBlock ingTypeHeader = new TextBlock
            {
                Text = "Type:",
                FontSize = headingFont,
                Foreground = headerForeground,
                Background = background,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            TextBox ingTypeBox = new TextBox
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
                string ingName = ingNameBox.Text.Trim();
                string ingType = ingTypeBox.Text.Trim();


                if (!string.IsNullOrEmpty(ingName) && !string.IsNullOrEmpty(ingType))
                {
                    if (ingredients.FindIndex(i => i.GetName() ==  ingName) != -1)
                    {
                        MessageBox.Show("Ingredient already exists", "Already Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                        popup.Close();
                        return;
                    }
                    Ingredient newCustom = new Ingredient(ingName, ingType, true);
                    createButton.IsEnabled = false;
                    bool success = await backend.CreateCustomIngredient(user, newCustom);

                    // Show result message
                    if (success)
                    {
                        MessageBox.Show("Custom ingredient created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        FilterIngredients(ingredients, "", ingGrid);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create custom ingredient. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    createButton.IsEnabled = true;
                    popup.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a name and type", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            panel.Children.Add(ingNameHeader);
            panel.Children.Add(ingNameBox);
            panel.Children.Add(ingTypeHeader);
            panel.Children.Add(ingTypeBox);
            panel.Children.Add(createButton);
            popup.Content = panel;
            popup.ShowDialog();
        }


        /// <summary>
        /// Shows a dialog box to confirm deleting a custom ingredient
        /// </summary>
        /// <param name="ingredient"> The ingredient to be deleted </param>
        /// <param name="ingredient"> The ingredient to be deleted </param>
        private async Task DeleteCustomIngredientPopup(Ingredient ingredient, UniformGrid ingGrid)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Deleting " + ingredient.GetName(),
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
                bool success = await backend.DeleteCustomIngredient(user, ingredient);

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Ingredient deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    FilterIngredients(ingredients, "", ingGrid);
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

    }
}
