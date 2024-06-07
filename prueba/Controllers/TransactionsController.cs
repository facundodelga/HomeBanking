using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.Models;


namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase {
        private ITransactionService _transactionService;
        private IAccountService _accountService;
        private IClientService _clientService;

        public TransactionsController(ITransactionService transactionService, IAccountService accountService, 
            IClientService clientService) {
            _transactionService = transactionService;
            _accountService = accountService; 
            _clientService = clientService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get() {
            try {
                var transactions = _transactionService.GetAllTransactions();
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
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get(long id) {
            try {
                var transaction = _transactionService.FindById(id);
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
        public IActionResult PostTransaction([FromBody] TransferDTO transfer) {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                Account fromAccount = _accountService.FindByNumber(transfer.FromAccountNumber);
                if (fromAccount == null) {
                    return StatusCode(403, "From account not valid");
                }

                //Voy a validar que la cuenta pertenezca al que hace la transferencia
                Client client = _clientService.FindById(fromAccount.ClientId);
                if (client == null || client.Email != email) {
                    return StatusCode(403, "From account is not yours");
                }

                Account toAccount = _accountService.FindByNumber(transfer.ToAccountNumber);
                if (toAccount == null) {
                    return StatusCode(403, "To account not valid");
                }
                
                var transactionReponse = _transactionService.MakeTransaction(fromAccount, toAccount,transfer);
                if (transactionReponse.response == null) {
                    return StatusCode(403, "Not enough founds");
                }

                var transactionDTO = new TransactionDTO(transactionReponse.response);

                return StatusCode(transactionReponse.status, transactionDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
