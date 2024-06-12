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
using FintechBank.Views.AdminViewsPages;

namespace FintechBank.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminDashboardWindow.xaml
    /// </summary>
    public partial class AdminDashboardWindow : Window
    {
        private int _currentUserId;
        public AdminDashboardWindow(int currentUserId)
        {
            InitializeComponent();
            _currentUserId = currentUserId;
        }

        private void RegisterClientButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RegisterClientPage());
        }

        private void ClientListButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientListPage()); 
        }

        private void ExportTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ExportTransactionsPage()); 
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}