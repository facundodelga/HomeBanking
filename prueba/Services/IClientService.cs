using HomeBanking.DTOS;
using prueba.Controllers;
using prueba.DTOS;
using prueba.Models;

namespace HomeBanking.Services {
    public interface IClientService {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
        Client FindByEmail(string email);

        Client CreateClient(SignUpDTO signup);

        List<ClientDTO> ClientsToDTOs(IEnumerable<Client> clients);
        ClientDTO ClientToDTO(Client client);

    }
}
