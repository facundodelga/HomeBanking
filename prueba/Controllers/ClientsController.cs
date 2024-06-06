﻿using HomeBanking.DTOS;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repository;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

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
                    return StatusCode(403,"Forbidden");
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
                    return StatusCode(403, "Forbidden");
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


        [HttpPost]
        public IActionResult Post([FromBody] SignUpDTO signup) {
            try {
                
                var clientResponse = _clientService.CreateClient(signup);
                
                if (clientResponse.client == null) { 
                    return StatusCode(clientResponse.status,"Error al crear cliente");
                }
                var accountResponse = _accountService.CreateAccount(clientResponse.client.Id);

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

                Client client = _clientService.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var accountResponse = _accountService.CreateAccount(client.Id);
                if(accountResponse.account == null) {
                    return StatusCode(accountResponse.status, "Cliente con 3 cuentas");
                }

                var accountdto = new AccountClientDTO(accountResponse.account);
                
                return StatusCode(201, accountdto);
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

                Client client = _clientService.FindByEmail(email);
                if (client == null) {
                    return StatusCode(403, "Unauthorized");
                }

                var cardResponse = _cardService.CreateCard(client.Id,client.FirstName +" "+ client.LastName,createCardDTO);
                if (cardResponse.card == null) {
                    return StatusCode(cardResponse.status, "Error al crear la Tarjeta");
                }


                return Created("",new CardDTO(cardResponse.card));
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