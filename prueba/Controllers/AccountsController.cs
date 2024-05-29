using HomeBanking.Repository;
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
    }
}
