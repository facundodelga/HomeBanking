﻿using prueba.Models;

namespace HomeBanking.Repository {
    public interface IAccountRepository {
        IEnumerable<Account> GetAllAccounts();
        void Save(Account account);
        Account FindById(long id);

        IEnumerable<Account> GetAccountsByClient(long clientId);
        Account FindByNumber(string num);
    
    }
}
