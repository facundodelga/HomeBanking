using System.IdentityModel.Tokens.Jwt;

namespace HomeBanking.Models {
    public class Loan {

        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }
        public ICollection<ClientLoan> ClientLoans { get; set; }
    }
}
