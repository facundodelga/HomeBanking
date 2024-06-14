using HomeBanking.Models;
using HomeBanking.Repository;

namespace HomeBanking.Services.Implementations {
    public class LoanService : ILoanService {

        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository) {
            _loanRepository = loanRepository;
        }

        public Loan FindById(long id) {
            return _loanRepository.FindById(id);
        }

        public IEnumerable<Loan> GetAllLoans() {
            return _loanRepository.GetAllLoans();
        }

        public void Save(Loan loan) {
            _loanRepository.Save(loan);
        }
    }
}
