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

        public PaymentsPage(int userId)
        {
            InitializeComponent();
            _transactionsController = new TransactionsController();
            _currentUserId = userId;
            LoadCardAndAccountFromDatabase();
            var cardNumberParts = SplitCardNumberIntoParts(_card.CardNumber);

            DataContext = new
            {
                NumberPart1 = cardNumberParts[0],
                NumberPart2 = cardNumberParts[1],
                NumberPart3 = cardNumberParts[2],
                NumberPart4 = cardNumberParts[3],
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
                using (var context = new FintechBankEntities2())
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
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-BO24OOP;Initial Catalog=FintechBank;Integrated Security=True"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "SELECT TOP 1 Cards.CardNumber, Accounts.Balance " +
                    "FROM Cards " +
                    "INNER JOIN Accounts ON Cards.AccountID = Accounts.AccountID " +
                    "WHERE Accounts.UserID = @UserID", connection))
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
            }
        }

        private string[] SplitCardNumberIntoParts(string cardNumber)
        {
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