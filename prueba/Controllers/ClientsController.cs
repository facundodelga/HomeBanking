using HomeBanking.DTOS;
using HomeBanking.Exceptions;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using prueba.DTOS;
using prueba.Models;

namespace prueba.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase {
        private IClientService _clientService;
        private IAccountService _accountService;
        private ICardService _cardService;

        public ClientsController(IClientService clientService,
            IAccountService accountService, 
            ICardService cardService) 
        {
            _clientService = clientService;
            _accountService = accountService; 
            _cardService = cardService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get() {
            try {
                var clients = _clientService.GetAllClients();
                var clientsDTO = _clientService.ClientsToDTOs(clients);

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
                var client = _clientService.FindById(id);

                if (client == null) {
                    return StatusCode(403, "Forbidden");
                }
                var clientDTO = _clientService.ClientToDTO(client);

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
                    return StatusCode(401,"Forbidden");
                }

                Client client = _clientService.FindByEmail(email);

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
                if (email.IsNullOrEmpty()) {
                    return StatusCode(401, "Forbidden");
                }

                Client client = _clientService.FindByEmail(email);

                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var accounts = _accountService.GetAccountsByClient(client.Id);

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


        [HttpPost] //registro de nuevo cliente
        public IActionResult SignUp([FromBody] SignUpDTO signup) {
            try {
                
                var clientResponse = _clientService.CreateClient(signup);
                
                if (clientResponse.objectResponse == null) { 
                    return StatusCode(clientResponse.status,clientResponse.message);
                }
                var accountResponse = _accountService.CreateAccount(clientResponse.objectResponse.Id);

                return StatusCode(clientResponse.status,new ClientDTO(signup));

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("current/accounts")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostAccounts() {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(401, "Forbidden");
                }

                Client client = _clientService.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var accountResponse = _accountService.CreateAccount(client.Id);
                if(accountResponse.objectResponse == null) {
                    return StatusCode(accountResponse.status, accountResponse.message);
                }

                var accountdto = new AccountClientDTO(accountResponse.objectResponse);
                
                return StatusCode(accountResponse.status, accountdto);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostCards([FromBody] CreateCardDTO createCardDTO) {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(401, "Forbidden");
                }

                Client client = _clientService.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var cardResponse = _cardService.CreateCard(client.Id,client.FirstName +" "+ client.LastName,createCardDTO);
                if (cardResponse.objectResponse == null) {
                    return StatusCode(cardResponse.status, cardResponse.message);
                }


                return Created("",new CardDTO(cardResponse.objectResponse));
            }
            catch (CardException ex) {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }

        }

    }
}