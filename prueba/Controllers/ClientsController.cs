using HomeBanking.DTOS;
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

        public ClientsController(IClientRepository clientRepository) {
            _clientRepository = clientRepository;
        }

        [HttpGet]
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
        public IActionResult Get(long id) {
            try {
                var client = _clientRepository.FindById(id);
                if (client == null) {
                    return Forbid();
                }
                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
            }
            catch (Exception ex) {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent() {
            try {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null) {
                    return Forbid();
                }

                var clientDTO = new ClientDTO(client);
                return Ok(clientDTO);
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
                return Created("", newClient);

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }


    }
}