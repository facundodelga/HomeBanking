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
        TransactionDTO TransactionToDTO(Transaction t);
        List<TransactionDTO> transactionsToDTOs(IEnumerable<Transaction> transactions);
        ServiceResponse<Transaction> MakeTransaction(string email, TransferDTO transfer);
    }
}
