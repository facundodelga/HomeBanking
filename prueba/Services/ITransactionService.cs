using HomeBanking.DTOS;
using HomeBanking.Models;
using prueba.Models;

namespace HomeBanking.Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetAllTransactions();
        void Save(Transaction transaction);
        Transaction FindById(long id);

        (Transaction response, int status) MakeTransaction(Account fromAccount, Account toAccount, TransferDTO transfer);
    }
}
