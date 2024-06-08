using HomeBanking.DTOS;
using HomeBanking.Models;

namespace HomeBanking.Services {
    public interface ICardService {
        IEnumerable<Card> GetAllCards();
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> FindByClient(long clientId);
        Card FindByNumber(string number);
        CardDTO CardToDTO(Card card);
        List<CardDTO> CardsToDTOs(IEnumerable<Card> cards);
        (Card card,int status) CreateCard(long clientId, string cardHolder, CreateCardDTO createCardDTO);
    }
}
