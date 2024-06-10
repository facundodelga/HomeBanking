using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase {
        private ILoanRepository loanRepository;
        private IAccountService _accountService;
        private IClientService _clientService;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;

        public LoansController(ILoanRepository loanRepository, IAccountService accountService, IClientService clientService, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            this.loanRepository = loanRepository;
            this._accountService = accountService;
            _clientService = clientService;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        
        public IActionResult Get() {
            try {
                var loans = loanRepository.GetAllLoans();

                var loansDTO = new List<LoanDTO>();

                foreach (var loan in loans) {
                    var loandto = new LoanDTO(loan);

                    loansDTO.Add(loandto);
                }

                return Ok(loansDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get(long id) {
            try {
                var loan = loanRepository.FindById(id);
                if (loan == null) {
                    return Forbid();
                }
                var loanDTO = new LoanDTO(loan);
                return Ok(loanDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostLoan([FromBody] LoanApplicationDTO loanDTO) {
            try {
                //Que el usuario este autentificado
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }
                
                //Verificar que el prestamo exista
                var loan = loanRepository.FindById(loanDTO.LoanId);
                if (loan == null) {
                    return StatusCode(403, "No se encontro loan por ID");
                }

                //Que el monto NO sea 0 y que no sobrepase el maximo autorizado
                if (loanDTO.Amount <= 0 || loanDTO.Amount > loan.MaxAmount)
                    return StatusCode(403, "el monto es mas que lo maximo");

                //Que los payments no lleguen vacios.
                if (loanDTO.Payments.IsNullOrEmpty())
                    return StatusCode(403, "payment llego vacio");

                //si la cantidad de cuotas no esta disponible para el prestamo solicitado
                HashSet<string> payments = new HashSet<string>(loan.Payments.Split(','));
                if (!payments.Contains(loanDTO.Payments))
                    return StatusCode(403, "los payments no corresponden");

                //Que exista la cuenta de destino
                var account = _accountService.FindByNumber(loanDTO.ToAccountNumber);
                if (account == null) {
                    return StatusCode(403, "No se encontro cuenta");
                }

                //Que la cuenta de destino pertenezca al Cliente autentificado
                var client = _clientService.FindById(account.ClientId);
                if (client == null || client.Email != email) {
                    return StatusCode(403, "cliente distinto al autenticado");
                }

                var creditTransaction = new Transaction {
                    AccountId = account.Id,
                    Type = TransactionType.CREDIT,
                    Amount = loanDTO.Amount,
                    Date = DateTime.Now,
                    Description = loan.Name + " - Loan Approved"
                };

                _transactionRepository.Save(creditTransaction);

                var clientLoan = new ClientLoan() {
                    Amount = loanDTO.Amount * 1.20, //Cuando guardes clientLoan el monto debes multiplicarlo por el 20%
                    LoanId = loan.Id,
                    ClientId = client.Id,
                    Payments = loanDTO.Payments,
                };

                _clientLoanRepository.save(clientLoan);


                return Ok(loanDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
