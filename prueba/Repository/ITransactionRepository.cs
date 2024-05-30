using HomeBanking.Models;
using prueba.Models;

namespace HomeBanking.Repository {
    public interface ITransactionRepository {

        
            IEnumerable<Transaction> GetAllTransactions();
            void Save(Transaction account);
            Transaction FindById(long id);

        
    }
}
