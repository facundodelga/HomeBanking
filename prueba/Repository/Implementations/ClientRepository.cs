using prueba.Models;

namespace prueba.Repository.Implementations {
    public class ClientRepository : RepositoryBase<Client>, IClientRepository {

        public ClientRepository(HomeBankingContext repository) : base(repository) {

        }

        public Client FindById(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAllClients() {
            throw new NotImplementedException();
        }

        public void Save(Client client) {
            throw new NotImplementedException();
        }
    }
}
