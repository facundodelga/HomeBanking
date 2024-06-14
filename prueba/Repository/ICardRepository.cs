using HomeBanking.Models;

namespace HomeBanking.Repository {
    public interface ICardRepository {
        IEnumerable<Card> GetAllCards();
        void Save(Card card);
        Card FindById(long id);

        IEnumerable<Card> FindByClient(long clientId);

        Card FindByNumber(string number);
    }
}
