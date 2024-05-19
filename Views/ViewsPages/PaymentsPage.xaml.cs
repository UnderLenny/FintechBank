using FintechBank.Models;
using FintechBank.Controllers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace FintechBank.Views.ViewsPages
{
    public partial class PaymentsPage : Page
    {
        private readonly TransactionsController _transactionsController;
        private readonly int _currentUserId;

        public PaymentsPage()
        {
            InitializeComponent();
            _transactionsController = new TransactionsController();
            _currentUserId = 1; // Замените на ID текущего пользователя после авторизации
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
                // Найти счет получателя по номеру карты
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
    }
}
