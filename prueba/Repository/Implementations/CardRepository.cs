using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;

namespace HomeBanking.Repository.Implementations {
    public class CardRepository : RepositoryBase<Card>, ICardRepository {
        public CardRepository(HomeBankingContext repository) : base(repository) {
        }

        public Card FindById(long id) {
            return FindByCondition(ac => ac.Id == id).FirstOrDefault();
        }

        public IEnumerable<Card> GetAllCards() {
            return FindAll().ToList();
        }

        public void Save(Card card) {
            Create(card);
            SaveChanges();
        }
    }
}
