using HomeBanking.DTOS;
using HomeBanking.Exceptions;
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

        public (Client client, int status) CreateClient(SignUpDTO signup) {

            if (String.IsNullOrEmpty(signup.Email) || String.IsNullOrEmpty(signup.Password) || String.IsNullOrEmpty(signup.FirstName) || String.IsNullOrEmpty(signup.LastName))
                return (null,403);
            
            //buscamos si ya existe el usuario
            Client user = _clientRepository.FindByEmail(signup.Email);

            if (user != null) {
                return (null,403);
            }

            Client newClient = new Client {
                Email = signup.Email,
                Password = signup.Password,
                FirstName = signup.FirstName,
                LastName = signup.LastName,
            };
            //Guardo en la DB asi se genera la ID
            _clientRepository.Save(newClient);

            return (newClient,201); 
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
