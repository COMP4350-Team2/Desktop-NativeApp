using System.Windows;

namespace Desktop_Frontend
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the second page
            var loggedInWindow = new LoggedInWindow();
            loggedInWindow.Show();
            this.Close(); // Close the current window if desired
        }
    }
}
