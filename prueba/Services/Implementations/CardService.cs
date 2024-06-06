using HomeBanking.DTOS;
using HomeBanking.Exceptions;
using HomeBanking.Models;
using HomeBanking.Repository;
using prueba.Models;

namespace HomeBanking.Services.Implementations {
    public class CardService : ICardService{

        private ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository) {
            _cardRepository = cardRepository;
        }

        public IEnumerable<Card> FindByClient(long clientId) {
            return _cardRepository.FindByClient(clientId);
        }

        public Card FindById(long id) {
            return _cardRepository.FindById(id);
        }

        public Card FindByNumber(string number) {
            return (_cardRepository.FindByNumber(number)); 
        }

        public IEnumerable<Card> GetAllCards() {
            return _cardRepository.GetAllCards();
        }

        public void Save(Card card) {
            _cardRepository.Save(card);
        }
        public Card CreateCard(long clientId, string cardHolder, CreateCardDTO createCardDTO) {

            var cards = _cardRepository.FindByClient(clientId);
            //busco las tarjetas que tengo para el tipo que viene en Body
            var cardsByType = cards.Where(card => card.Type.ToString() == createCardDTO.Type).ToList();
            //busco si existe una tarjeta con color ya existente
            bool hasCardWithColor = cardsByType.Any(card => card.Color.ToString() == createCardDTO.Color);

            if (cardsByType.Count() < 3 && !hasCardWithColor) {
                var random = new Random();
                string cardNum;

                do {
                    cardNum = random.Next(1000, 10000).ToString();
                    for (var i = 0; i < 3; i++) {
                        cardNum = cardNum + "-" + random.Next(1000, 10000).ToString();
                    }
                    //Console.WriteLine(cardNum);
                } while (_cardRepository.FindByNumber(cardNum) != null);
                //Console.WriteLine(cardNum);
                int cvv = random.Next(100, 1000);

                //parseo los enum que vienen por body
                CardType cardType = (CardType)Enum.Parse(typeof(CardType), createCardDTO.Type);
                CardColor cardColor = (CardColor)Enum.Parse(typeof(CardColor), createCardDTO.Color);

                var newCard = new Card {
                    CardHolder = cardHolder,
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(5),
                    ClientId = clientId,
                    Cvv = cvv,
                    Number = cardNum,
                    Color = cardColor,
                    Type = cardType,
                };

                _cardRepository.Save(newCard);

                return newCard;
            }
            else {
                if (cardsByType.Count() == 3)

                    throw new CardException("Cliente con 3 tarjetas del mismo tipo");
                else
                    throw new CardException("Cliente con una tarjeta " + createCardDTO.Color + " existente");
            }
        }
    }
}
