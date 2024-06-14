using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Repository.Implementations {
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository {
        public LoanRepository(HomeBankingContext repository) : base(repository) {
        }

        public Loan FindById(long id) {
            return FindByCondition(ac => ac.Id == id).FirstOrDefault();
        }

        public IEnumerable<Loan> GetAllLoans() {
            return FindAll().ToList();
        }

        public void Save(Loan loan) {
            Create(loan);
            this.RepositoryContext.SaveChanges();
        }
    }
}
