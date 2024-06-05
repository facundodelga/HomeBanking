using HomeBanking.DTOS;
using HomeBanking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

namespace prueba.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;

        public ClientsController(IClientRepository clientRepository,IAccountRepository accountRepository) {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository; 
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get() {
            try {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients) {

                    var newClientDTO = new ClientDTO(client);

                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get(long id) {
            try {
                var client = _clientRepository.FindById(id);
                if (client == null) {
                    return StatusCode(403, "Forbidden");
                }
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception ex) {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrent() {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403,"Forbidden");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null) {
                    return StatusCode(403,"Unauthorized");
                }

                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult GetCurrentAccounts() {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var accounts = _accountRepository.GetAccountsByClient(client.Id);

                var accountsDTO = new List<AccountDTO>();
                foreach (var account in accounts) { 
                    var accountDTO = new AccountDTO(account);
                    accountsDTO.Add(accountDTO);
                }

                return Ok(accountsDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] SignUpDTO signup) {
            try {
                //validamos datos antes
                if (String.IsNullOrEmpty(signup.Email) || String.IsNullOrEmpty(signup.Password) || String.IsNullOrEmpty(signup.FirstName) || String.IsNullOrEmpty(signup.LastName))
                    return StatusCode(403, "datos inválidos");

                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(signup.Email);

                if (user != null) {
                    return StatusCode(403, "Email está en uso");
                }

                Client newClient = new Client { 
                    Email = signup.Email,
                    Password = signup.Password,
                    FirstName = signup.FirstName,
                    LastName = signup.LastName,
                    };

                _clientRepository.Save(newClient);
                return Created("", new ClientDTO(signup));

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Post() {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var accounts = _accountRepository.GetAccountsByClient(client.Id);
                if (accounts.Count() < 3) {
                    var random = new Random();
                    string accountNum;

                    do {
                        long randomNumber = random.Next(100000000);
                        accountNum = randomNumber.ToString("D8");
                    } while (_accountRepository.FindByNumber(accountNum) != null);

                    accountNum = "VIN-" + accountNum;

                    var newAccount = new Account {
                        CreationDate = DateTime.Now,
                        ClientId = client.Id,
                        Balance = 0,
                        Number = accountNum
                    };

                    _accountRepository.Save(newAccount);

                    return StatusCode(201, accountNum);
                }
                else {
                    return StatusCode(403, "Cliente con 3 cuentas");
                }

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

    }
}