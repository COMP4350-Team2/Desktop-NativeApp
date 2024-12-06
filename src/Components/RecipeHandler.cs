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
    /// Class for handling display of a singular recipe
    /// </summary>
    public class RecipeHandler
    {
        private IUser user;
        private IBackend backend;
        private Recipe recipe;
        private StackPanel? parentPanel;
        private Expander? ingExpander;
        private Expander? stepsExpander;
        private UniformGrid? ingGrid;
        private UniformGrid? stepsGrid;

        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeHandler(IBackend backend, IUser user, StackPanel parentPanel, Recipe recipe)
        {
            this.backend = backend;
            this.user = user;
            this.recipe = recipe;
            this.parentPanel = parentPanel;
            this.parentPanel.Children.Clear();
        }

        /// <summary>
        /// Method to display the recipe and its contents
        /// </summary>
        public async Task DisplayRecipe()
        {
            // Create and add header
            TextBlock header = CreateHeader();
            parentPanel?.Children.Add(header);

            // Populate ing and steps grids initally 
            PopulateIngGrid();
            PopulateStepsGrid();
        }


        /// <summary>
        /// Helper method to create page header
        /// </summary>
        /// <returns>TextBlock containing header</returns>
        private TextBlock CreateHeader()
        {
            SolidColorBrush headerText = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int headerFont = 34;

            // Create and add header
            TextBlock header = new TextBlock
            {
                Text = $"{recipe.GetRecipeName()}",
                FontSize = headerFont,
                FontWeight = FontWeights.Bold,
                Foreground = headerText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20),
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = parentPanel.ActualWidth - 100
            };

            return header;
        }


        /// <summary>
        /// Helper to create expanders for ingredients and steps
        /// </summary>
        /// <param name="name">Name of the expander</param>
        /// <param name="content">content of the expander</param>
        /// <returns>Styled expander to be added to parentPanel</returns>
        private Expander CreateExpander(string name, UIElement content)
        {
            Brush expanderBackground = (Brush)Application.Current.Resources["ExpanderBrushA"];
            Brush headerTextCol = (Brush)Application.Current.Resources["SecondaryBrushB"];
            int headerFont = 35;
            int buttonFont = 70;

            // Create grid for expander heading
            Grid headerGrid = new Grid
            {
                Margin = new Thickness(10)
            };

            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Auto) });

            TextBlock headerText = new TextBlock
            {
                Text = name,
                FontSize = headerFont,
                FontWeight = FontWeights.Bold,
                Foreground = headerTextCol,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10),
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                Width = parentPanel.ActualWidth
            };

            DockPanel.SetDock(headerText, Dock.Left);
            Grid.SetColumn(headerText, 0);
            headerGrid.Children.Add(headerText);

            // Create add button
            Button addButton = new Button
            {
                Width = 60,
                Height = 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5, 5, 5, 10),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Content = new TextBlock
                {
                    Text = "+",
                    Foreground = headerTextCol,
                    FontSize = buttonFont,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    ToolTip = $"Add {name}".Trim('s')
                },
                Cursor = Cursors.Hand,
                Style = (Style)Application.Current.Resources["NoHighlightButton"]
            };

            // Different on clicks for the 2 expanders
            if(name == "Ingredients")
            {
                addButton.Click += async (s, e) => await ShowAddIngredientPopup();
            }
            else
            {
                addButton.Click += async (s, e) => await ShowAddStepPopup();
            }

            // Set the add button in the second column
            DockPanel.SetDock(addButton, Dock.Right);
            Grid.SetColumn(addButton, 1);
            headerGrid.Children.Add(addButton);

            // Create the Expander for ingredients
            Expander expander = new Expander
            {
                Header = headerGrid,
                FontSize = 18,
                Margin = new Thickness(30),
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.White,
                Background = expanderBackground,
                Content = content
            };

            return expander;
        }


        /// <summary>
        /// Shows pop up window for adding an <see cref="Ingredient"/>
        /// </summary>
        private async Task ShowAddIngredientPopup()
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

            // Add radio buttons at the very top
            StackPanel radioPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            RadioButton allRadioButton = new RadioButton
            {
                Content = "All",
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = headerText,
                Margin = new Thickness(20, 10, 20, 10)
            };

            RadioButton commonRadioButton = new RadioButton
            {
                Content = "Common",
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20, 10, 20, 10),
                Foreground = headerText
            };

            RadioButton customRadioButton = new RadioButton
            {
                Content = "Custom",
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20, 10, 20, 10),
                Foreground = headerText
            };

            // Default to 'All' radio button selected
            allRadioButton.IsChecked = true;

            radioPanel.Children.Add(allRadioButton);
            radioPanel.Children.Add(commonRadioButton);
            radioPanel.Children.Add(customRadioButton);

            panel.Children.Add(radioPanel);

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
            void UpdateComboBoxItems(IEnumerable<Ingredient> filteredIngredients)
            {
                nameBox.Items.Clear();
                foreach (var ing in filteredIngredients)
                {
                    nameBox.Items.Add(ing.GetName());
                }

                if (nameBox.Items.Count > 0)
                {
                    nameBox.SelectedIndex = 0;
                }
            }

            // Filter ingredients based on radio button selection and search text
            void FilterIngredients()
            {
                string searchText = searchBox.Text.ToLower().Trim();
                IEnumerable<Ingredient> filteredIngredients = allIngredients;

                // Filter based on radio button selection
                if (commonRadioButton.IsChecked == true)
                {
                    filteredIngredients = filteredIngredients.Where(ing => !ing.IsCustom());
                }
                else if (customRadioButton.IsChecked == true)
                {
                    filteredIngredients = filteredIngredients.Where(ing => ing.IsCustom());
                }

                // Further filter based on search text
                if (!string.IsNullOrWhiteSpace(searchText) && searchBox.IsFocused)
                {
                    filteredIngredients = filteredIngredients.Where(ing => ing.GetName().ToLower().Contains(searchText));
                }

                UpdateComboBoxItems(filteredIngredients);
            }

            // Set initial ComboBox items and apply filters when needed
            UpdateComboBoxItems(allIngredients);

            searchBox.TextChanged += (s, e) => FilterIngredients();
            searchBox.GotFocus += (s, e) =>
            {
                searchBox.Text = "";
                searchBox.Foreground = Brushes.Black;
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search Ingredients...";
                    searchBox.Foreground = headerText;
                }
            };
            allRadioButton.Checked += (s, e) => FilterIngredients();
            commonRadioButton.Checked += (s, e) => FilterIngredients();
            customRadioButton.Checked += (s, e) => FilterIngredients();

            panel.Children.Add(new TextBlock { Text = "Ingredient Name:", Foreground = headerText, FontSize = 18, FontWeight = FontWeights.Bold });
            panel.Children.Add(searchBox);
            panel.Children.Add(nameBox);

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

            Button addButton = new Button
            {
                Content = "Add",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"]
            };

            addButton.Click += async (s, e) =>
            {
                if (!float.TryParse(amountBox.Text, out float amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount greater than 0.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

                addButton.IsEnabled = false;

                bool success = await backend.AddIngToRecipe(user, newIngredient, recipe.GetRecipeName());

                addButton.IsEnabled = true;

                if (success)
                {
                    MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    PopulateIngGrid();
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
        /// Popup for adding a step
        /// </summary>
        private async Task ShowAddStepPopup()
        {

            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textboxBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush textboxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushA"];


            int headingFont = 20;
            int stepFont = 20;

            SolidColorBrush buttonBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush buttonForeground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush headerForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create Popup window
            Window popup = new Window
            {
                Title = "Add Step",
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(10) };

            TextBlock createStepHeader = new TextBlock
            {
                Text = "New Step:",
                FontSize = headingFont,
                Foreground = headerForeground,
                Background = background,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            TextBox stepTextBox = new TextBox
            {
                Width = 500,
                Height = 200,
                FontSize = stepFont,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10),
                Background = textboxBackground,
                Foreground = textboxForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            // Create the Add button
            Button addButton = new Button
            {
                Content = "Add",
                Margin = new Thickness(10, 30, 10, 10),
                Style = (Style)Application.Current.Resources["ExpandButtonStyle"]
            };


            addButton.Click += async (s, e) =>
            {

               string newStep = stepTextBox.Text.Trim();

               if (!string.IsNullOrEmpty(newStep))
               {
                   addButton.IsEnabled = false;
                   bool success = await backend.AddStepToRecipe(user, newStep, recipe.GetRecipeName());

                   // Show result message
                   if (success)
                   {
                        MessageBox.Show("Recipe created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        PopulateStepsGrid();
                   }
                   else
                   {
                        MessageBox.Show("Failed to create recipe. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                   }
                   addButton.IsEnabled = true;
                   popup.Close();
               }
               else
               {
                   MessageBox.Show("Please enter a step", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
               }
            };

            panel.Children.Add(createStepHeader);
            panel.Children.Add(stepTextBox);
            panel.Children.Add(addButton);
            popup.Content = panel;
            popup.ShowDialog();
        }


        /// <summary>
        /// Helper method to refresh ingredient panel
        /// </summary>
        private void PopulateIngGrid()
        {
            bool wasExpanded = ingExpander?.IsExpanded ?? false;

            parentPanel?.Children.Remove(ingExpander);

            ingGrid?.Children.Clear();
            ingGrid = new UniformGrid
            {
                Rows = (int)Math.Ceiling((double)(recipe.GetRecipeIngList().GetIngredients().Count) / 2),
                Columns = 2, // 2 items per row
                Margin = new Thickness(20, 10, 10, 10)
            };
            foreach (Ingredient ing in recipe.GetRecipeIngList().GetIngredients())
            {
                Border ingBox = CreateIngBox(ing);
                ingGrid?.Children.Add(ingBox);
            }

            // Create ingredient scroller
            ScrollViewer ingScroller = CreateIngredientScroller();
            ingScroller.Content = ingGrid;


            // Create expander for ingredients
            ingExpander = CreateExpander("Ingredients", ingScroller);
            ingExpander.Content = ingScroller;
            ingExpander.IsExpanded = wasExpanded;

            parentPanel?.Children.Insert(1, ingExpander);

        }


        /// <summary>
        /// Helper method to inialize ingredient scroller (to be put in expander)
        /// </summary>
        /// <returns> A styled and populated scrollviewer </returns>
        private ScrollViewer CreateIngredientScroller()
        {
            // Create a ScrollViewer to make the ing grid scrollable
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0, 10, 0, 0),
                MaxHeight = parentPanel.ActualHeight/2 - 10
            };

            return scrollViewer;
        }

        /// <summary>
        /// Helper method to create an ingredient box
        /// </summary>
        /// <param name="ingredient"> Ingredient to be created </param>
        /// <returns>Styled border with the ing info, custom logo and del buttons</returns>
        private Border CreateIngBox(Ingredient ingredient)
        {
            // Define resource-based colors and font sizes
            SolidColorBrush boxTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int ingFont = 35;

            Border ingBorder = new Border
            {
                BorderBrush = boxBorderCol,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Style = (Style)Application.Current.Resources["HoverableBorder"],
                MaxHeight = parentPanel.ActualHeight / 4,
                ToolTip = ingredient.GetName()
            };

            // Use a vertical StackPanel to stack all elements
            StackPanel contentPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5)
            };

            // Add Ingredient Name
            TextBlock ingName = new TextBlock
            {
                Text = ingredient.GetName(),
                FontWeight = FontWeights.Bold,
                FontSize = ingFont,
                Foreground = boxTextCol,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 0.4 * parentPanel.ActualWidth,
                MaxHeight = 0.05 * parentPanel.ActualHeight,
                Margin = new Thickness(5)
            };
            contentPanel.Children.Add(ingName);

            // Add Ingredient Type
            TextBlock ingType = new TextBlock
            {
                Text = ingredient.GetIngType(),
                FontWeight = FontWeights.Bold,
                FontSize = ingFont - 5,
                Foreground = boxTextCol,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 0.4 * parentPanel.ActualWidth,
                MaxHeight = 0.05 * parentPanel.ActualHeight,
                Margin = new Thickness(5) 
            };
            contentPanel.Children.Add(ingType);

            // Add Ingredient Amount
            TextBlock ingAmount = new TextBlock
            {
                Text = $"{ingredient.GetAmount()} {ingredient.GetUnit()}",
                FontWeight = FontWeights.Bold,
                FontSize = ingFont - 5,
                Foreground = boxTextCol,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 0.4 * parentPanel.ActualWidth,
                MaxHeight = 0.05 * parentPanel.ActualHeight,
                Margin = new Thickness(5) 
            };
            contentPanel.Children.Add(ingAmount);

            // Bottom panel for icons and buttons
            DockPanel bottomPanel = new DockPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Add Custom Icon (if applicable)
            if (ingredient.IsCustom())
            {
                Image customIcon = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/custom_ing_icon_white.png")),
                    Width = 50,
                    Height = 50,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };
                DockPanel.SetDock(customIcon, Dock.Left);
                bottomPanel.Children.Add(customIcon);
            }

            // Add Delete Button (aligned to the right)
            Button deleteButton = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/del_icon_red.png")),
                    Height = 50,
                    Width = 50,
                    Stretch = Stretch.Uniform
                },
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(10),
                Cursor = Cursors.Hand,
                ToolTip = "Delete",
                Style = (Style)Application.Current.Resources["NoHighlightButton"],
                HorizontalAlignment = HorizontalAlignment.Right
            };
            deleteButton.Click += async (s, e) => await DeleteIngredientPopup(ingredient);

            DockPanel.SetDock(deleteButton, Dock.Right);
            bottomPanel.Children.Add(deleteButton);


            // Add bottom panel to content panel
            contentPanel.Children.Add(bottomPanel);

            // Set the content of the Border
            ingBorder.Child = contentPanel;

            return ingBorder;
        }

        /// <summary>
        /// Shows a dialog box to confirm deleting an ingredient from recipe
        /// </summary>
        /// <param name="ingredient"> The ingredient to be deleted </param>
        private async Task DeleteIngredientPopup(Ingredient ingredient)
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
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = parentPanel.ActualWidth - 200
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
                bool success = await backend.DeleteIngInRecipe(user, ingredient, recipe.GetRecipeName());   

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Ingredient deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    PopulateIngGrid();
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
        /// Helper method to create steps scroller
        /// </summary>
        /// <returns> Styled scrollviewer</returns>
        private ScrollViewer CreateStepsScroller()
        {
            // Create a ScrollViewer to make the steps grid scrollable
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0, 10, 0, 0),
                MaxHeight = parentPanel.ActualHeight / 2 - 10
            };

            return scrollViewer;

        }


        /// <summary>
        /// Helper to fill in the steps grid
        /// </summary>
        private void PopulateStepsGrid()
        {
            bool wasExpanded = stepsExpander?.IsExpanded ?? false;

            parentPanel?.Children.Remove(stepsExpander);

            stepsGrid?.Children.Clear();
            stepsGrid = new UniformGrid
            {
                Rows = recipe.GetRecipeSteps().Count,
                Columns = 1,
                Margin = new Thickness(20, 10, 10, 10)
            };
            foreach (string step in recipe.GetRecipeSteps())
            {
                Border stepBox = CreateStepBox(step);
                stepsGrid?.Children.Add(stepBox);
            }

            // Create steps scroller
            ScrollViewer stepsScroller = CreateStepsScroller();
            stepsScroller.Content = stepsGrid;


            // Create expander for steps
            stepsExpander = CreateExpander("Steps", stepsScroller);
            stepsExpander.Content = stepsScroller;
            stepsExpander.IsExpanded = wasExpanded;

            parentPanel?.Children.Insert(2, stepsExpander);

        }

        /// <summary>
        /// Helper to create box for each step
        /// </summary>
        /// <param name="step"> The step to make a box out of </param>
        /// <returns> Style border with step and delete button</returns>
        private Border CreateStepBox(string step)
        {
            // Define resource-based colors and font sizes
            SolidColorBrush boxTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int stepFont = 30;

            Border stepBorder = new Border
            {
                BorderBrush = boxBorderCol,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Style = (Style)Application.Current.Resources["HoverableBorder"],
                MaxHeight = parentPanel.ActualHeight / 4,
                ToolTip = step
            };

            DockPanel borderContent = new DockPanel();

            TextBlock stepText = new TextBlock
            {
                Text = step,
                FontSize = stepFont,
                Foreground = boxTextCol,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 0.7 * parentPanel.ActualWidth,
                MaxHeight = 0.4 * parentPanel.ActualHeight,
                Margin = new Thickness(5)
            };

            DockPanel.SetDock(stepText, Dock.Left);
            borderContent.Children.Add(stepText);

            Button deleteButton = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/del_icon_red.png")),
                    Height = 50,
                    Width = 50,
                    Stretch = Stretch.Uniform
                },
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(10),
                Cursor = Cursors.Hand,
                ToolTip = "Delete",
                Style = (Style)Application.Current.Resources["NoHighlightButton"],
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            deleteButton.Click += async (s, e) => await DeleteStepPopup(step);

            DockPanel.SetDock(deleteButton, Dock.Right);
            borderContent.Children.Add(deleteButton);

            stepBorder.Child = borderContent;

            return stepBorder;
        }

        /// <summary>
        /// Shows a dialog box to confirm deleting a step from recipe
        /// </summary>
        /// <param name="step"> The step to be deleted </param>
        private async Task DeleteStepPopup(string step)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Deleting Step",
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            // Stack panel for layout
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock
            {
                Text = $"Are you sure you want to delete {step}?",
                Margin = new Thickness(10),
                TextWrapping = TextWrapping.Wrap,
                Foreground = textForeground,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = parentPanel.ActualWidth - 200
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

                // Call backend to remove step
                int stepIndex = recipe.GetStepIndex(step);
                bool success = false;
                if (stepIndex > 0)
                {
                    success = await backend.DeleteStepFromRecipe(user, stepIndex, recipe.GetRecipeName());
                }

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Ingredient deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    PopulateStepsGrid();
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
