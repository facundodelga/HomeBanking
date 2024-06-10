using HomeBanking.DTOS;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.DTOS;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase {
        private ICardService cardService;

        public CardsController(ICardService cardService) { 
            this.cardService= cardService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get() {
            try {
                var cards = cardService.GetAllCards();

                var cardsdto = cardService.CardsToDTOs(cards);
                return Ok(cardsdto);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult Get(long id) {
            try {
                var card = cardService.FindById(id);
                if (card == null) {
                    return Forbid();
                }
                var cardDTO = cardService.CardToDTO(card);
                return Ok(cardDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }

}



