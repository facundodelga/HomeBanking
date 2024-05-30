using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
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

        public TransactionsController(ITransactionRepository transactionRepository) {
            _transactionRepository = transactionRepository;
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
    }
}
