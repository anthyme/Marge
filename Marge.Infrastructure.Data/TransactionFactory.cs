using System.Transactions;

namespace Marge.Infrastructure.Data
{
    public class TransactionFactory : ITransactionFactory
    {
        public ITransaction Create()
        {
            return new Transaction();
        }
    }

    class Transaction : ITransaction
    {
        private readonly TransactionScope transaction;

        public Transaction()
        {
            transaction = new TransactionScope();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }

        public void Complete()
        {
            transaction.Complete();
        }
    }
}
