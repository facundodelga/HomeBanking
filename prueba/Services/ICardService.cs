using HomeBanking.DTOS;
using HomeBanking.Models;

namespace HomeBanking.Services {
    public interface ICardService {
        IEnumerable<Card> GetAllCards();
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> FindByClient(long clientId);
        Card FindByNumber(string number);
        Card CreateCard(long clientId, string cardHolder, CreateCardDTO createCardDTO);
    }
}
