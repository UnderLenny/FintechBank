using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FintechBank.Models;

namespace FintechBank.Views.ViewsPages
{
    public partial class HistoryPage : Page
    {
        private readonly int _currentUserId;

        public HistoryPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadTransactionHistoryAsync();
        }

        private async void LoadTransactionHistoryAsync()
        {
            try
            {
                using (var context = new FintechBankEntities2())
                {
                    var transactions = await context.Transactions
                        .Include(t => t.Accounts) // Включение аккаунта отправителя
                        .Where(t => t.Accounts.UserID == _currentUserId || t.Accounts1.UserID == _currentUserId)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToListAsync();

                    var transactionHistory = transactions.Select(t => new
                    {
                        t.TransactionID,
                        t.Amount,
                        t.Description,
                        t.CreatedAt,
                        SenderAccountNumber = t.Accounts != null ? t.Accounts.AccountNumber : "N/A"
                    }).ToList();

                    TransactionHistoryListView.ItemsSource = transactionHistory;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки истории транзакций: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TransactionHistoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
