using Desktop_Frontend.Backend;
using Desktop_Frontend.DSOs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Desktop_Frontend.Components
{
    /// <summary>
    /// Manages the "My Lists" feature and displays placeholder text.
    /// </summary>
    public class MyListsHandler
    {
        private readonly SolidColorBrush textColor;
        private IBackend backend;
        private IUser user;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyListsHandler"/> class.
        /// This constructor sets up the handler with the provided backend service and user instance,
        /// and initializes the text color to be used in UI elements.
        /// </summary>
        /// <param name="backend">The backend service instance for handling data operations.</param>
        /// <param name="user">The user instance for user-specific actions.</param>
        public MyListsHandler(IBackend backend, IUser user)
        {
            textColor = (SolidColorBrush)App.Current.Resources["TertiaryBrush"]; // Retrieve the text color from application resources
            this.backend = backend; // Store the backend instance
            this.user = user; // Store the user instance
        }


        /// <summary>
        /// Displays the "My Lists" section with placeholder text in the specified panel.
        /// </summary>
        /// <param name="contentArea">The panel to display content within.</param>
        public void DisplayMyLists(StackPanel contentArea)
        {
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

            StackPanel centerPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 40, 0, 0)
            };

            // Create and add placeholder text
            TextBlock placeholderText = new TextBlock
            {
                Text = "Coming soon: A page for your lists",
                FontSize = 18,
                Foreground = textColor,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            centerPanel.Children.Add(placeholderText);

            contentArea.Children.Add(centerPanel);
        }
    }
}
