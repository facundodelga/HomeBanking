using HomeBanking.Exceptions;
using HomeBanking.Repository;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _accountRepository.GetAllAccounts();
        }

        public void Save(Account account)
        {
            _accountRepository.Save(account);
        }

        public Account FindById(long id)
        {
            return _accountRepository.FindById(id);
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return _accountRepository.GetAccountsByClient(clientId);
        }

        public Account FindByNumber(string num)
        {
            return _accountRepository.FindByNumber(num);
        }

        public Account CreateAccount(long clientId)
        {
            var accounts = GetAccountsByClient(clientId);
            if (accounts.Count() < 3)
            {
                var random = new Random();
                string accountNum;

                do
                {
                    string randomNumber = random.Next(10000000, 100000000).ToString();
                    accountNum = "VIN-" + randomNumber;

                } while (FindByNumber(accountNum) != null);

                Account newAccount = new Account
                {
                    CreationDate = DateTime.Now,
                    ClientId = clientId,
                    Balance = 0,
                    Number = accountNum
                };

                Save(newAccount);

                return newAccount;
            }
            else
            {
                throw new ThreeAccountsException("Cliente con 3 cuentas");
            }
        }
    }
}
