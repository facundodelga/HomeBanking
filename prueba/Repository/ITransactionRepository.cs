using HomeBanking.Models;
using prueba.Models;

namespace HomeBanking.Repository {
    public interface ITransactionRepository {

        
            IEnumerable<Transaction> GetAllTransactions();
            void Save(Transaction transaction);
            Transaction FindById(long id);

        
    }
}
