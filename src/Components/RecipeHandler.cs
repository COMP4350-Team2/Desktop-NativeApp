

using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            // Create expander for ingredients
            ingExpander = CreateExpander("Ingredients", new UIElement());
            parentPanel?.Children.Add(ingExpander);

            // Create expander for steps
            stepsExpander = CreateExpander("Steps", new UIElement());
            parentPanel?.Children.Add(stepsExpander);
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
            int headerFont = 30;
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

            //ON CLICK HERE

            // Set the options button in the second column
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
    }
}
