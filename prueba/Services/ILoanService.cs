using HomeBanking.Models;

namespace HomeBanking.Services {
    public interface ILoanService {
        IEnumerable<Loan> GetAllLoans();
        void Save(Loan loan);
        Loan FindById(long id);
    }
}
