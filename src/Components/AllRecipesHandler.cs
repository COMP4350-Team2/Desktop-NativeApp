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
    /// Class to handle displaying all recipes
    /// </summary>
    public class AllRecipesHandler
    {
        private IUser user;
        private IBackend backend;
        private List<Recipe> recipes;
        private StackPanel parentPanel;
        private ScrollViewer scrollViewer;
        private UniformGrid recipeGrid;
        private StackPanel allRecipesButton;

        /// <summary>
        /// Constructor do initalizing the component handler
        /// </summary>
        /// <param name="backend"> backend to be used </param>
        /// <param name="user"> user making calls </param>
        /// <param name="parentPanel"> content panel in LoggedInWindow </param>
        public AllRecipesHandler(IBackend backend, IUser user, StackPanel parentPanel, StackPanel allRecipesButton)
        {
            this.backend = backend;
            this.user = user;
            this.parentPanel = parentPanel;
            recipes = null;
            scrollViewer = null;
            this.allRecipesButton = allRecipesButton;
        }

        /// <summary>
        /// Async method to display the recipes
        /// </summary>
        public async Task DisplayAllRecipes()
        {
            await InitRecipes();

            // Clear the parent panel
            this.parentPanel.Children.Clear();

            // Create and add header
            TextBlock header = CreateHeader();
            parentPanel.Children.Add(header);

            // Create the Create Recipe button
            Button createRecipeButton = new Button
            {
                Content = "Create Recipe",
                Margin = new Thickness(25, 30, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                Style = (Style)App.Current.Resources["ExpandButtonStyle"] 
            };
            createRecipeButton.Click += CreateRecipeButton_Click;
            parentPanel.Children.Add(createRecipeButton);
            

            // Create search bar and add it 
            Border searchBar = CreateSearchBox();
            StackPanel searchBarPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 0) };
            searchBarPanel.Children.Add(searchBar);
            parentPanel.Children.Add(searchBarPanel); 

            // Initialize the grid
            PopulateRecipeGrid(recipes);

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
                Text = "All Recipes",
                FontSize = headerFont,
                FontWeight = FontWeights.Bold,
                Foreground = headerText,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };

            return header;
        }

        /// <summary>
        /// Creates a TextBox for recipe search with placeholder text.
        /// </summary>
        /// <returns>A configured TextBox for searching recipes.</returns>
        private Border CreateSearchBox()
        {
            SolidColorBrush boxBackground = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush boxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            int boxFont = 24;

            // Create a border for rounded edges
            Border searchBoxBorder = new Border
            {
                Height = 50,
                Margin = new Thickness(25, 20, 20, 10),
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
                Text = "Search recipes...",
                FontSize = boxFont,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(10, 0, 0, 0)
            };

            // Clear placeholder text when the box is focused
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Search recipes...")
                {
                    searchBox.Text = string.Empty;
                }
            };

            // Restore placeholder text if the box is empty when focus is lost
            searchBox.LostFocus += async (s, e) =>
            {
                if (string.IsNullOrEmpty(searchBox.Text))
                {
                    searchBox.Text = "Search recipes...";
                    PopulateRecipeGrid(recipes);
                }
            };

            searchBox.TextChanged += (s, e) => PopulateRecipeGrid(recipes, searchBox.Text.Trim().ToString());

            searchBoxBorder.Child = searchBox;

            return searchBoxBorder;
        }


        /// <summary>
        /// Helper method to initalized recipes
        /// </summary>
        private async Task InitRecipes()
        {
            recipes = await backend.GetAllRecipes(user);
        }

        /// <summary>
        /// Method to populate the recipe grid 
        /// </summary>
        /// <param name="recipes"> List of recipes to be shown </param>
        private void PopulateRecipeGrid(List<Recipe> recipes, string searchText = "")
        {

            parentPanel.Children.Remove(scrollViewer);

            // Create grid for recipes
            recipeGrid = new UniformGrid
            {
                Rows = (int)Math.Ceiling((double)recipes.Count / 2),
                Columns = 2, // 2 items per row
                Margin = new Thickness(20, 10, 10, 10)
            };

            // Create a ScrollViewer to make the recipe grid scrollable
            scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Margin = new Thickness(0, 10, 0, 0)
            };

            scrollViewer.ScrollToVerticalOffset(0);
            recipeGrid.Children.Clear();

            // Populate grid with recipes
            foreach (Recipe recipe in recipes)
            {
                if (string.IsNullOrEmpty(searchText) || recipe.GetRecipeName()
                    .Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    Border recipeBox = CreateRecipeBox(recipe);
                    recipeGrid.Children.Add(recipeBox);
                }
            }


            // Add recipe grid to scroller
            if(scrollViewer.Content == null)
            {
                scrollViewer.Content = recipeGrid;
            }

            parentPanel.Children.Add(scrollViewer);

        }

        /// <summary>
        /// Creates a box with recipe name and delete button.
        /// </summary>
        /// <param name="recipe">The recipe to display in the box.</param>
        /// <returns>A styled Border containing the recipe box.</returns>
        private Border CreateRecipeBox(Recipe recipe)
        {
            // Define resource-based colors and font sizes
            SolidColorBrush boxTextCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush boxBorderCol = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            int recipeNameFont = 35;

            Border recipeBorder = new Border
            {
                BorderBrush = boxBorderCol,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Style = (Style)Application.Current.Resources["HoverableBorder"],
                ToolTip = "Open Recipe",
                Cursor = Cursors.Hand
            };


            DockPanel borderContent = new DockPanel { Margin = new Thickness(5) };

            TextBlock recipeName = new TextBlock
            {
                Text = recipe.GetRecipeName(),
                FontWeight = FontWeights.Bold,
                FontSize = recipeNameFont,
                Foreground = boxTextCol,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 0.4 * parentPanel.ActualWidth,
            };

            DockPanel.SetDock(recipeName, Dock.Left);
            borderContent.Children.Add(recipeName);

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
            deleteButton.Click += async (s, e) => await DeleteRecipePopup(recipe);
       

            DockPanel.SetDock(deleteButton, Dock.Right);
            borderContent.Children.Add(deleteButton);


            recipeBorder.Child = borderContent;

            recipeBorder.MouseLeftButtonDown += async (sender, e) =>
            {
                RecipeHandler recipePage = new RecipeHandler(backend, user, parentPanel, recipe);
                await recipePage.DisplayRecipe();
                allRecipesButton.IsEnabled = true;
            };

            return recipeBorder;
        }

        /// <summary>
        /// Shows a dialog box to confirm deleting a recipe
        /// </summary>
        /// <param name="recipe"> The recipe to be deleted </param>
        private async Task DeleteRecipePopup(Recipe recipe)
        {
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];

            // Create popup window for confirmation
            Window confirmationPopup = new Window
            {
                Title = "Deleting " + recipe.GetRecipeName(),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = background
            };

            // Stack panel for layout
            StackPanel panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock
            {
                Text = $"Are you sure you want to delete {recipe.GetRecipeName()}?",
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

                // Call backend to remove recipe
                bool success = await backend.DeleteRecipe(user, recipe.GetRecipeName());

                // Display result and close popup
                if (success)
                {
                    MessageBox.Show("Recipe deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    PopulateRecipeGrid(recipes);
                }
                else
                {
                    MessageBox.Show("Failed to delete recipe. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                confirmButton.IsEnabled = true;

                confirmationPopup.Close();
            };

            confirmationPopup.Content = panel;
            confirmationPopup.ShowDialog();
        }


        /// <summary>
        /// handler for create recipe button
        /// </summary>
        private async void CreateRecipeButton_Click(object sender, RoutedEventArgs e)
        {

            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["PrimaryBrushB"];
            SolidColorBrush textboxBackground = (SolidColorBrush)App.Current.Resources["SecondaryBrushB"];
            SolidColorBrush textboxForeground = (SolidColorBrush)App.Current.Resources["SecondaryBrushA"];


            int headingFont = 20;
            int recipeNameFont = 20;

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

            TextBlock createRecipeHeader = new TextBlock
            {
                Text = "Recipe Name:",
                FontSize = headingFont,
                Foreground = headerForeground,
                Background = background,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            TextBox recipeNameBox = new TextBox
            {
                Width = 300,
                Height = 30,
                FontSize = recipeNameFont,
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

                string newRecipeName = recipeNameBox.Text;

                if (!string.IsNullOrEmpty(newRecipeName))
                {
                    createButton.IsEnabled = false;
                    bool success = await backend.CreateRecipe(user, newRecipeName);

                    // Show result message
                    if (success)
                    {
                        MessageBox.Show("Recipe created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        PopulateRecipeGrid(recipes);
                    }
                    else
                    {
                        MessageBox.Show("Failed to create recipe. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    createButton.IsEnabled = true;
                    popup.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a list name", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            panel.Children.Add(createRecipeHeader);
            panel.Children.Add(recipeNameBox);
            panel.Children.Add(createButton);
            popup.Content = panel;
            popup.ShowDialog();
        }

    }
}
