using FintechBank.Models;
using FintechBank.Controllers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace FintechBank.Views.ViewsPages
{
    public partial class PaymentsPage : Page
    {
        private readonly TransactionsController _transactionsController;
        private readonly int _currentUserId;
        private Cards _card;
        private Accounts _account;
        private readonly Connection _dbConnection;

        public PaymentsPage(int userId)
        {
            InitializeComponent();
            _transactionsController = new TransactionsController();
            _currentUserId = userId;
            _dbConnection = new Connection();
            LoadCardAndAccountFromDatabase();
            var cardNumberParts = SplitCardNumberIntoParts(_card?.CardNumber);

            if (_card == null || _account == null)
            {
                MessageBox.Show("Не удалось загрузить данные карты или счета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataContext = new
            {
                NumberPart1 = cardNumberParts?[0],
                NumberPart2 = cardNumberParts?[1],
                NumberPart3 = cardNumberParts?[2],
                NumberPart4 = cardNumberParts?[3],
                Balance = _account.Balance
            };
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string receiverCardNumber = ReceiverCardNumberTextBox.Text;
            if (!decimal.TryParse(AmountTextBox.Text.Replace("₽", "").Trim(), out decimal amount))
            {
                MessageBox.Show("Неверная сумма.");
                return;
            }
            string message = MessageTextBox.Text;

            try
            {
                using (var context = new FintechBankEntities())
                {
                    var receiverAccount = context.Cards
                        .Where(c => c.CardNumber == receiverCardNumber)
                        .Select(c => c.AccountID)
                        .FirstOrDefault();

                    if (receiverAccount == 0)
                    {
                        MessageBox.Show("Счет получателя не найден.");
                        return;
                    }

                    var senderAccount = context.Accounts
                        .Where(a => a.UserID == _currentUserId)
                        .FirstOrDefault();

                    if (senderAccount == null)
                    {
                        MessageBox.Show("Счет отправителя не найден.");
                        return;
                    }

                    bool success = await _transactionsController.TransferAsync(senderAccount.AccountID, (int)receiverAccount, amount, message);

                    if (success)
                    {
                        MessageBox.Show("Перевод выполнен успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно средств на счете отправителя.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка перевода: " + ex.Message);
            }
        }

        private void LoadCardAndAccountFromDatabase()
        {
            try
            {
                _dbConnection.openConnection();
                using (SqlCommand command = new SqlCommand(
                    "SELECT TOP 1 Cards.CardNumber, Accounts.Balance " +
                    "FROM Cards " +
                    "INNER JOIN Accounts ON Cards.AccountID = Accounts.AccountID " +
                    "WHERE Accounts.UserID = @UserID", _dbConnection.getConnection()))
                {
                    command.Parameters.AddWithValue("@UserID", _currentUserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _card = new Cards
                            {
                                CardNumber = reader["CardNumber"].ToString()
                            };
                            _account = new Accounts
                            {
                                Balance = (decimal)reader["Balance"]
                            };
                        }
                    }
                }
                _dbConnection.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных из базы данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _dbConnection.closeConnection();
            }
        }

        private string[] SplitCardNumberIntoParts(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
            {
                MessageBox.Show("Некорректный номер карты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return new string[]
            {
                cardNumber.Substring(0, 4),
                cardNumber.Substring(4, 4),
                cardNumber.Substring(8, 4),
                cardNumber.Substring(12, 4)
            };
        }
    }
}
