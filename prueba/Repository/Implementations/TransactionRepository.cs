using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Repository.Implementations {
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository {
        public TransactionRepository(HomeBankingContext repository) : base(repository) {
        }

        public Transaction FindById(long id) {
            return FindByCondition(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetAllTransactions() {
            return FindAll().ToList();
        }

        public void Save(Transaction transaction) {
            Create(transaction);
            SaveChanges();
        }
    }
}
