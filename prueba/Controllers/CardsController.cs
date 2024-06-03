using HomeBanking.DTOS;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.DTOS;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase {
        private ICardRepository cardRepository{ get; set; }

        public CardsController(ICardRepository cardRepository) { 
            this.cardRepository = cardRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            try {
                var cards = cardRepository.GetAllCards();

                var cardsdto = new List<CardDTO>();

                foreach (var card in cards) {
                    var carddto = new CardDTO(card);

                    cardsdto.Add(carddto);
                }

                return Ok(cardsdto);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id) {
            try {
                var card = cardRepository.FindById(id);
                if (card == null) {
                    return Forbid();
                }
                var cardDTO = new CardDTO(card);
                return Ok(cardDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }

}



