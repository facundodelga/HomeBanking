
using prueba.DTOS;
using prueba.Models;

namespace HomeBanking.Services {
    public interface IAccountService {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);
        IEnumerable<Account> GetAccountsByClient(long clientId);
        Account FindByNumber(string num);
        (Account account, int status) CreateAccount(long clientId);
        
    }
}
