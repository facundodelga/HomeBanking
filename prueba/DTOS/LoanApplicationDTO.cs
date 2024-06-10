using System.Reflection.Metadata;

namespace HomeBanking.DTOS {
    public class LoanApplicationDTO {
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string ToAccountNumber { get; set; }
    }
}
