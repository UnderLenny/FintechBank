using FintechBank.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FintechBank.Views.AdminViewsPages
{
    /// <summary>
    /// Логика взаимодействия для ClientListPage.xaml
    /// </summary>
    public partial class ClientListPage : Page
    {
        private List<Accounts> accounts;
        public ClientListPage()
        {
            InitializeComponent();
            LoadClients();
        }
        private void LoadClients()
        {
            try
            {
                // Подключение к базе данных
                using (var context = new FintechBankEntities())
                {
                    // Получение всех клиентов из базы данных
                    accounts = context.Accounts.ToList();
                }

                // Отображение клиентов на странице
                AccountsDataGrid.ItemsSource = accounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке клиентов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}