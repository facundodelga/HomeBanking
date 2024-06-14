using HomeBanking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.DTOS;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository){
            _accountRepository = accountRepository;
        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get() {
            try {
                var accounts = _accountRepository.GetAllAccounts();

                var accountsdto = new List<AccountDTO>();

                foreach (var account in accounts) {
                    var accountdto = new AccountDTO(account);

                    accountsdto.Add(accountdto);
                }

                return Ok(accountsdto);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Get(long id) {
            try {
                var account = _accountRepository.FindById(id);
                if (account == null) {
                    return StatusCode(403, "Forbidden");
                }
                var accountDTO = new AccountDTO(account);
                return Ok(accountDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
