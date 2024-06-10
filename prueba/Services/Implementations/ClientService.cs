using HomeBanking.DTOS;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;
using System.Security.Cryptography;
using System.Text;

namespace HomeBanking.Services.Implementations {

    public class ClientService : IClientService {
        private IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository) { 
            _clientRepository = clientRepository;
        }

        public List<ClientDTO> ClientsToDTOs(IEnumerable<Client> clients) {
            var clientsDTO = new List<ClientDTO>();
            foreach (Client client in clients) {

                var newClientDTO = new ClientDTO(client);

                clientsDTO.Add(newClientDTO);
            }

            return clientsDTO;
        }

        public ClientDTO ClientToDTO(Client client) {
            return new ClientDTO(client);
        }

        public ServiceResponse<Client> CreateClient(SignUpDTO signup) {

            if (String.IsNullOrEmpty(signup.Email) || String.IsNullOrEmpty(signup.Password) || String.IsNullOrEmpty(signup.FirstName) || String.IsNullOrEmpty(signup.LastName))
                return new ServiceResponse<Client>(null,403,"Algun campo vacio");
            
            //buscamos si ya existe el usuario
            Client user = _clientRepository.FindByEmail(signup.Email);

            if (user != null) {
                return new ServiceResponse<Client>(null, 403, "Usuario existente");
            }

            string passwordHash = PasswordHash(signup.Password);

            Client newClient = new Client {
                Email = signup.Email,
                Password = passwordHash,
                FirstName = signup.FirstName,
                LastName = signup.LastName,
            };
            //Guardo en la DB asi se genera la ID
            _clientRepository.Save(newClient);

            return new ServiceResponse<Client>(newClient, 403, "Usuario registrado con exito");  
        }

        public Client FindByEmail(string email) {
            return _clientRepository.FindByEmail(email);
        }

        public Client FindById(long id) {
            return _clientRepository.FindById(id);
        }

        public IEnumerable<Client> GetAllClients() {
            return _clientRepository.GetAllClients();
        }

        public void Save(Client client) {
            _clientRepository.Save(client);
        }

        public string PasswordHash(string password) {
            var hmac = new HMACSHA512();

            byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convierte el array de bytes a un string en formato hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
