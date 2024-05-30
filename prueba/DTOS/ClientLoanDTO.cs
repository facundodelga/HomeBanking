using HomeBanking.Models;

namespace HomeBanking.DTOS {
    public class ClientLoanDTO {
        public long Id { get; set; }
        public long LoanId{ get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }

        public ClientLoanDTO(ClientLoan cl) {
            Id = cl.Id;
            LoanId = cl.LoanId;
            Name = cl.Loan.Name;
            Amount = cl.Amount;
            Payments = int.Parse(cl.Payments);
        }
    }
}
