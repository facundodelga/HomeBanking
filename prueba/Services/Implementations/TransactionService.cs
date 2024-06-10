using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using Microsoft.IdentityModel.Tokens;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Services.Implementations {
    public class TransactionService : ITransactionService {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository,
            IClientRepository clientRepository) {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        public Transaction FindById(long id) {
            return _transactionRepository.FindById(id);
        }

        public IEnumerable<Transaction> GetAllTransactions() {
            return _transactionRepository.GetAllTransactions();
        }

        public (Transaction response, int status) MakeTransaction(string email,TransferDTO transfer) {

            if(transfer.ToAccountNumber.IsNullOrEmpty() || transfer.FromAccountNumber.IsNullOrEmpty()
                || transfer.Amount <= 0 || transfer.Description.IsNullOrEmpty()) 
                return (null, 403);
            
            Account fromAccount = _accountRepository.FindByNumber(transfer.FromAccountNumber);
            if (fromAccount == null) {
                return (null, 403);
            }

            //Voy a validar que la cuenta pertenezca al que hace la transferencia
            Client client = _clientRepository.FindById(fromAccount.ClientId);
            if (client == null || client.Email != email) {
                return (null, 403);
            }

            Account toAccount = _accountRepository.FindByNumber(transfer.ToAccountNumber);
            if (toAccount == null || fromAccount.Number == toAccount.Number) {
                return (null, 403);
            }

            //si la cuenta no tiene los fondos
            if (fromAccount.Balance - transfer.Amount < 0) {
                return (null,403);
            }

            fromAccount.Balance -= transfer.Amount;
            toAccount.Balance += transfer.Amount;

            var newDebitT = new Transaction {
                Amount = transfer.Amount,
                Type = TransactionType.DEBIT,
                AccountId = fromAccount.Id,
                Description = transfer.Description + " - To: " + toAccount.Number,
                Date = DateTime.Now,
            };

            var newCreditT = new Transaction {
                Amount = transfer.Amount,
                Type = TransactionType.CREDIT,
                AccountId = toAccount.Id,
                Description = transfer.Description + " - From: " + fromAccount.Number,
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

        public List<TransactionDTO> transactionsToDTOs(IEnumerable<Transaction> transactions) {
            var transactionsDTO = new List<TransactionDTO>();

            foreach (Transaction transaction in transactions) {

                var transactionDTO = new TransactionDTO(transaction);

                transactionsDTO.Add(transactionDTO);
            }
            return transactionsDTO;
        }

        public TransactionDTO TransactionToDTO(Transaction t) {
            return new TransactionDTO(t);
        }
    }
}
