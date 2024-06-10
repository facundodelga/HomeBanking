using HomeBanking.Models;

namespace HomeBanking.Repository {
    public interface IClientLoanRepository {
        public void save(ClientLoan clientLoan);
    }
}
