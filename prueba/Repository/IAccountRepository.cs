using prueba.Models;

namespace HomeBanking.Repository {
    public interface IAccountRepository {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
    
    }
}
