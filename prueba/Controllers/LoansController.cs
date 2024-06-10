using HomeBanking.DTOS;
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

        public LoansController(ILoanRepository loanRepository, IAccountService accountService, IClientService clientService)
        {
            this.loanRepository = loanRepository;
            this._accountService = accountService;
            _clientService = clientService;
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
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                var loan = loanRepository.FindById(loanDTO.LoanId);
                if (loan == null) {
                    return BadRequest();
                }

                if(loanDTO.Amount <= 0 || loanDTO.Amount > loan.MaxAmount) 
                    return Forbid();

                if(loanDTO.Payments.IsNullOrEmpty())
                    return Forbid();

                var account = _accountService.FindByNumber(loanDTO.ToAccountNumber);
                if (account == null) {
                    return Forbid();
                }

                var client = _clientService.FindById(account.ClientId);
                if (client == null || client.Email != email) {
                    return NotFound();
                }




                return Ok(loanDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
