using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

namespace prueba.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;

        public ClientsController(IClientRepository clientRepository,
            IAccountRepository accountRepository, 
            ICardRepository cardRepository) 
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository; 
            _cardRepository = cardRepository;
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
                //Guardo en la DB asi se genera la ID
                _clientRepository.Save(newClient);
                
                //empiezo a crear la cuenta
                Client client = _clientRepository.FindByEmail(signup.Email);

                var accounts = _accountRepository.GetAccountsByClient(client.Id);
                var random = new Random();
                string accountNum;

                do {
                    long randomNumber = random.Next(10000000,100000000);
                    accountNum = randomNumber.ToString("D8");
                    accountNum = "VIN-" + accountNum;
                } while (_accountRepository.FindByNumber(accountNum) != null);

                var newAccount = new Account {
                    CreationDate = DateTime.Now,
                    ClientId = client.Id,
                    Balance = 0,
                    Number = accountNum
                };

                _accountRepository.Save(newAccount);

                return Created("", new ClientDTO(signup));

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
                        string randomNumber = random.Next(10000000,100000000).ToString();
                        accountNum = "VIN-" + randomNumber;
                    
                    } while (_accountRepository.FindByNumber(accountNum) != null);

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

        [HttpPost("current/cards")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostCards([FromBody] CreateCardDTO createCardDTO) {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var cards = _cardRepository.FindByClient(client.Id);
                //busco las tarjetas que tengo para el tipo que viene en Body
                var cardsByType = cards.Where(card => card.Type.ToString() == createCardDTO.Type).ToList();
                //busco si existe una tarjeta con color ya existente
                bool hasCardWithColor = cardsByType.Any(card => card.Color.ToString() == createCardDTO.Color);
                
                if (cardsByType.Count() < 3 && !hasCardWithColor) {
                    var random = new Random();
                    string cardNum;

                    do {
                        cardNum = random.Next(1000, 10000).ToString();
                        for (var i = 0; i<3; i++) {
                            cardNum =  cardNum + "-" + random.Next(1000, 10000).ToString();
                        }
                        //Console.WriteLine(cardNum);
                    } while (_cardRepository.FindByNumber(cardNum) != null);
                    //Console.WriteLine(cardNum);
                    int cvv = random.Next(100, 1000);

                    //parseo los enum que vienen por body
                    CardType cardType = (CardType)Enum.Parse(typeof(CardType), createCardDTO.Type);
                    CardColor cardColor = (CardColor)Enum.Parse(typeof(CardColor), createCardDTO.Color);
                    
                    var newCard = new Card {
                        CardHolder = client.FirstName + " " + client.LastName,
                        FromDate = DateTime.Now,
                        ThruDate = DateTime.Now.AddYears(5),
                        ClientId = client.Id,
                        Cvv = cvv,
                        Number = cardNum,
                        Color = cardColor,
                        Type = cardType,
                    };

                    _cardRepository.Save(newCard);

                    return StatusCode(201, cardNum);
                }
                else {
                    if(cardsByType.Count() == 3)
                        return StatusCode(403, "Cliente con 3 tarjetas del mismo tipo");
                    else
                        return StatusCode(403, "Cliente con una tarjeta " + createCardDTO.Color + "existente");
                }

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }

        }

    }
}