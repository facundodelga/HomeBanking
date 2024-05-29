using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Repository.Implementations {
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository {
        public AccountRepository(HomeBankingContext repository) : base(repository) {
        }

        public Account FindById(long id) {
            return FindByCondition(ac => ac.Id == id).Include(ac => ac.Transactions).FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts() {
            return FindAll().Include(ac => ac.Transactions).ToList();
        }

        public void Save(Account account) {
            Create(account);
            SaveChanges();
        }
    }
}
