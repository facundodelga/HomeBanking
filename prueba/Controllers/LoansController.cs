using HomeBanking.DTOS;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase {
        public ILoanRepository loanRepository { get; set; }

        public LoansController(ILoanRepository loanRepository)
        {
            this.loanRepository = loanRepository;
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
    }
}
