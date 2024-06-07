using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Services.Implementations {
    public class TransactionService : ITransactionService {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository) {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public Transaction FindById(long id) {
            return _transactionRepository.FindById(id);
        }

        public IEnumerable<Transaction> GetAllTransactions() {
            return _transactionRepository.GetAllTransactions();
        }

        public (Transaction response, int status) MakeTransaction(Account fromAccount, Account toAccount, TransferDTO transfer) {
            //si el monto es menor a 0 o si la cuenta no tiene los fondos
            if (transfer.Amount < 0 || fromAccount.Balance - transfer.Amount < 0) {
                return (null,403);
            }

            fromAccount.Balance -= transfer.Amount;
            toAccount.Balance += transfer.Amount;

            var newDebitT = new Transaction {
                Amount = transfer.Amount,
                Type = TransactionType.DEBIT,
                AccountId = fromAccount.Id,
                Description = transfer.Description,
                Date = DateTime.Now,
            };

            var newCreditT = new Transaction {
                Amount = transfer.Amount,
                Type = TransactionType.CREDIT,
                AccountId = toAccount.Id,
                Description = transfer.Description,
                Date = DateTime.Now,
            };

            _transactionRepository.Save(newCreditT);
            _transactionRepository.Save(newDebitT);

            _accountRepository.Save(fromAccount);
            _accountRepository.Save(toAccount);

            

            return (newDebitT, 201);
        }

        public void Save(Transaction transaction) {
            _transactionRepository.Save(transaction);
        }
    }
}
