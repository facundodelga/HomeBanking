using HomeBanking.Models;
using prueba.Models;

namespace HomeBanking.DTOS {
    public class TransactionDTO {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public TransactionDTO(Transaction t)
        {
            Id = t.Id;
            Type = t.Type.ToString();
            Amount = t.Amount;
            Description = t.Description;
            Date = t.Date;
        }

    }
}
