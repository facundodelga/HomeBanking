using HomeBanking.DTOS;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientLoanService
    {
        public void save(ClientLoan clientLoan);
        public (ClientLoan cl, int status) MakeLoan(LoanApplicationDTO loanDTO, string email);
    }
}
