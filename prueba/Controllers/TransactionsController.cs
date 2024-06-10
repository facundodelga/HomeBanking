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
                var transactionsDTO = _transactionService.transactionsToDTOs(transactions);
                return Ok(transactionsDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Get(long id) {
            try {
                var transaction = _transactionService.FindById(id);
                if (transaction == null) {
                    return Forbid();
                }
                var transactionDTO = _transactionService.TransactionToDTO(transaction);
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

                //MakeTransaction va a devolver la transaccion de Debito y el status 
                var transactionReponse = _transactionService.MakeTransaction(email,transfer);
                if (transactionReponse.objectResponse == null) {
                    return StatusCode(transactionReponse.status, transactionReponse.message);
                }

                var transactionDTO = new TransactionDTO(transactionReponse.objectResponse);

                return StatusCode(transactionReponse.status, transactionDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
