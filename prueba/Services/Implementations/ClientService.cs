using HomeBanking.DTOS;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using prueba.DTOS;
using prueba.Models;
using prueba.Repository;

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

            Client newClient = new Client {
                Email = signup.Email,
                Password = signup.Password,
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

        
    }
}
