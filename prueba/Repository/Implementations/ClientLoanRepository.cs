using HomeBanking.Models;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Repository.Implementations {
    public class ClientLoanRepository : RepositoryBase<ClientLoan>, IClientLoanRepository {
        public ClientLoanRepository(HomeBankingContext repository) : base(repository) {
        }

        public void save(ClientLoan clientLoan) {
            Create(clientLoan);
            SaveChanges();
        }
    }
}
