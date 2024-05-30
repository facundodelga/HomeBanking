using HomeBanking.Models;
using prueba.Models;

namespace HomeBanking.Repository {
    public interface ILoanRepository {
        IEnumerable<Loan> GetAllLoans();
        void Save(Loan loan);
        Loan FindById(long id);
    }
}
