using FintechBank.Views.ViewsPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FintechBank.Views
{
    /// <summary>
    /// Логика взаимодействия для UserDashboardWindow.xaml
    /// </summary>
    public partial class UserDashboardWindow : Window
    {
        public UserDashboardWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new WalletPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void WalletButton_Click(object sender, RoutedEventArgs e)
        {
            WalletButton.Style = (Style)FindResource("activeMenuButton");
            WalletButtonArrow.Visibility = Visibility.Visible;

            if (WalletButtonArrow.Visibility == Visibility.Visible)
            {
                DepositsButtonArrow.Visibility = Visibility.Hidden;
                HistoryButtonArrow.Visibility = Visibility.Hidden;
                SettingsButtonArrow.Visibility = Visibility.Hidden;
            }
            else
            {
                WalletButtonArrow.Visibility = Visibility.Hidden;
            }

            HistoryButton.Style = (Style)FindResource("menuButton");
            DepositsButton.Style = (Style)FindResource("menuButton");
            SettingsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new WalletPage());
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryButton.Style = (Style)FindResource("activeMenuButton");
            HistoryButtonArrow.Visibility = Visibility.Visible;

            if (HistoryButtonArrow.Visibility == Visibility.Visible)
            {
                DepositsButtonArrow.Visibility = Visibility.Hidden;
                WalletButtonArrow.Visibility = Visibility.Hidden;
                SettingsButtonArrow.Visibility = Visibility.Hidden;
            }
            else
            {
                HistoryButtonArrow.Visibility = Visibility.Hidden; 
            }

            WalletButton.Style = (Style)FindResource("menuButton");
            DepositsButton.Style = (Style)FindResource("menuButton");
            SettingsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new HistoryPage());
        }

        private void DepositsButton_Click(object sender, RoutedEventArgs e)
        {

            DepositsButton.Style = (Style)FindResource("activeMenuButton");
            DepositsButtonArrow.Visibility = Visibility.Visible;

            if (DepositsButtonArrow.Visibility == Visibility.Visible)
            {
                WalletButtonArrow.Visibility = Visibility.Hidden;
                HistoryButtonArrow.Visibility = Visibility.Hidden;
                SettingsButtonArrow.Visibility = Visibility.Hidden;
            } else
            {
                DepositsButtonArrow.Visibility = Visibility.Hidden;
            }

            HistoryButton.Style = (Style)FindResource("menuButton");
            WalletButton.Style = (Style)FindResource("menuButton");
            SettingsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new DepositsPage());
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsButton.Style = (Style)FindResource("activeMenuButton");
            SettingsButtonArrow.Visibility = Visibility.Visible;


            if (SettingsButtonArrow.Visibility == Visibility.Visible)
            {
                DepositsButtonArrow.Visibility = Visibility.Hidden;
                WalletButtonArrow.Visibility = Visibility.Hidden;
                HistoryButtonArrow.Visibility = Visibility.Hidden;
            }
            else
            {
                SettingsButton.Visibility = Visibility.Hidden;
                
            }

            HistoryButton.Style = (Style)FindResource("menuButton");
            WalletButton.Style = (Style)FindResource("menuButton");
            DepositsButton.Style = (Style)FindResource("menuButton");
            MainFrame.Navigate(new SettingsPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
