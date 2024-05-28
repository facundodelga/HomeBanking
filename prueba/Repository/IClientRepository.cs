using prueba.Models;

namespace prueba.Repository {
    public interface IClientRepository {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
    }
}
