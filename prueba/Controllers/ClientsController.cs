using HomeBanking.DTOS;
using HomeBanking.Models;
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

                _clientRepository.Save(newClient);
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
                //busco las targetas que tengo para el tipo que viene en Body
                var cardsType = cards.Select(card => card.Type.ToString() == createCardDTO.Type);

                if (cardsType.Count() < 3) {
                    var random = new Random();
                    string cardNumAux;
                    string cardNum;

                    do {
                        long randomNumber = random.Next(10000);

                        cardNumAux = randomNumber.ToString("D4");
                        cardNum = cardNumAux;

                        for (int i = 0; i < 3; i++) {
                            randomNumber = random.Next(10000);

                            cardNumAux = randomNumber.ToString("D4");
                            cardNum = cardNum + "-" + cardNumAux;
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
                    return StatusCode(403, "Cliente con 3 tarjetas del mismo tipo");
                }

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }

        }

    }
}