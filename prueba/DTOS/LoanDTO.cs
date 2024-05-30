using HomeBanking.Models;

namespace HomeBanking.DTOS {
    public class LoanDTO {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }

        public ICollection<ClientLoanDTO> ClientLoans { get; set; }

    }
}
