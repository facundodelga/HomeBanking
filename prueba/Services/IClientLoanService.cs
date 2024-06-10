using HomeBanking.DTOS;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientLoanService
    {
        void save(ClientLoan clientLoan);
        ServiceResponse<ClientLoan> MakeLoan(LoanApplicationDTO loanDTO, string email);
    }
}
