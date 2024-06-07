using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase {
        private ITransactionRepository _transactionRepository;
        private IAccountService _accountService;

        public TransactionsController(ITransactionRepository transactionRepository, IAccountService accountService) {
            _transactionRepository = transactionRepository;
            _accountService = accountService; 
        }

        [HttpGet]
        public IActionResult Get() {
            try {
                var transactions = _transactionRepository.GetAllTransactions();
                var transactionsDTO = new List<TransactionDTO>();

                foreach (Transaction transaction in transactions) {

                    var transactionDTO = new TransactionDTO(transaction);

                    transactionsDTO.Add(transactionDTO);
                }
                return Ok(transactionsDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id) {
            try {
                var transaction = _transactionRepository.FindById(id);
                if (transaction == null) {
                    return Forbid();
                }
                var transactionDTO = new TransactionDTO(transaction);
                return Ok(transactionDTO);
            }
            catch (Exception ex) {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostTransaction([FromBody] TransferDTO transaction) {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                Account fromAccount = _accountService.FindByNumber(transaction.FromAccountNumber);
                if (fromAccount == null || fromAccount.Client.Email != email) {
                    return StatusCode(403, "From account not valid");
                }

                Account toAccount = _accountService.FindByNumber(transaction.ToAccountNumber);
                if (toAccount == null) {
                    return StatusCode(403, "To account not valid");
                }
                //si el monto es menor a 0 y si la cuenta no tiene los fondos
                if (transaction.Amount < 0 && fromAccount.Balance - transaction.Amount < 0) {
                    return StatusCode(403, "Amount not valid");
                }

                fromAccount.Balance -= transaction.Amount;
                toAccount.Balance += transaction.Amount;

                var newDebitT = new Transaction {
                    Amount = transaction.Amount,
                    Type = TransactionType.DEBIT,
                    AccountId = fromAccount.Id,
                    Description = transaction.Description,
                    Date = DateTime.Now,
                };

                var newCreditT = new Transaction {
                    Amount = transaction.Amount,
                    Type = TransactionType.CREDIT,
                    AccountId = toAccount.Id,
                    Description = transaction.Description,
                    Date = DateTime.Now,
                };

                _transactionRepository.Save(newCreditT);
                _transactionRepository.Save(newDebitT);

                _accountService.Save(fromAccount);
                _accountService.Save(toAccount);

                return StatusCode(201, "Transaccion realizada con exito");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
