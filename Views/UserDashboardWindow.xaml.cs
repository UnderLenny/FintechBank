using System;
using System.Windows;
using FintechBank.Views.ViewsPages;

namespace FintechBank.Views
{
    public partial class UserDashboardWindow : Window
    {
        private int _currentUserId;

        public UserDashboardWindow(int currentUserId)
        {
            InitializeComponent();
            MessageBox.Show($"UserDashboardWindow initialized with UserID: {currentUserId}", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
            MainFrame.Navigate(new WalletPage());
            _currentUserId = currentUserId;
        }

        private void WalletButton_Click(object sender, RoutedEventArgs e)
        {
            WalletButton.Style = (Style)FindResource("activeMenuButton");
            WalletButtonArrow.Visibility = Visibility.Visible;

            if (WalletButtonArrow.Visibility == Visibility.Visible)
            {
                PaymentsButtonArrow.Visibility = Visibility.Hidden;
                HistoryButtonArrow.Visibility = Visibility.Hidden;
            }
            else
            {
                WalletButtonArrow.Visibility = Visibility.Hidden;
            }

            HistoryButton.Style = (Style)FindResource("menuButton");
            PaymentsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new WalletPage());
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryButton.Style = (Style)FindResource("activeMenuButton");
            HistoryButtonArrow.Visibility = Visibility.Visible;

            if (HistoryButtonArrow.Visibility == Visibility.Visible)
            {
                PaymentsButtonArrow.Visibility = Visibility.Hidden;
                WalletButtonArrow.Visibility = Visibility.Hidden;
            }
            else
            {
                HistoryButtonArrow.Visibility = Visibility.Hidden;
            }

            WalletButton.Style = (Style)FindResource("menuButton");
            PaymentsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new HistoryPage(_currentUserId));
        }

        private void PaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaymentsButton.Style = (Style)FindResource("activeMenuButton");
                PaymentsButtonArrow.Visibility = Visibility.Visible;

                if (PaymentsButtonArrow.Visibility == Visibility.Visible)
                {
                    WalletButtonArrow.Visibility = Visibility.Hidden;
                    HistoryButtonArrow.Visibility = Visibility.Hidden;
                }
                else
                {
                    PaymentsButtonArrow.Visibility = Visibility.Hidden;
                }

                HistoryButton.Style = (Style)FindResource("menuButton");
                WalletButton.Style = (Style)FindResource("menuButton");
                MainFrame.Navigate(new PaymentsPage(_currentUserId));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to PaymentsPage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
