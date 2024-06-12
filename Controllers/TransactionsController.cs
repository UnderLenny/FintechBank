using System;
using System.Linq;
using System.Threading.Tasks;
using FintechBank.Models;

namespace FintechBank.Controllers
{
    public class TransactionsController
    {
        private readonly FintechBankEntities _context;

        public TransactionsController()
        {
            _context = new FintechBankEntities();
        }

        public async Task<bool> TransferAsync(int senderAccountId, int receiverAccountId, decimal amount, string description)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var senderAccount = _context.Accounts.SingleOrDefault(a => a.AccountID == senderAccountId);
                    var receiverAccount = _context.Accounts.SingleOrDefault(a => a.AccountID == receiverAccountId);

                    if (senderAccount == null || receiverAccount == null)
                    {
                        return false;
                    }

                    if (senderAccount.Balance < amount)
                    {
                        return false; // Недостаточно средств на счете отправителя
                    }

                    senderAccount.Balance -= amount;
                    receiverAccount.Balance += amount;

                    var transactionRecord = new Transactions
                    {
                        SenderAccountID = senderAccountId,
                        ReceiverAccountID = receiverAccountId,
                        Amount = amount,
                        TransactionTypeID = 1, 
                        Description = description,
                        CreatedAt = DateTime.Now
                    };

                    _context.Transactions.Add(transactionRecord);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error processing transaction", ex);
                }
            }
        }

    }
}
