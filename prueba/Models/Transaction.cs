using prueba.Models;

namespace HomeBanking.Models {
    public class Transaction {
        
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public string Description{ get; set; }
        public DateTime Date { get; set; }
        public Account Account{ get; set; }
        public long AccountId { get; set; }

    }
}
