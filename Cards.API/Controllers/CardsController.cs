using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        // Get Single Card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await cardsDbContext.Cards.SingleOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                return Ok(card);
            }
            return NotFound("Card Not Found");
        }

        // Add Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card) 
        {
            card.Id = Guid.NewGuid();
            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), card.Id, card);
        }

        // Updating Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatingCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var query = cardsDbContext.Cards.FirstOrDefault(x => x.Id == id);
            if (query != null)
            {
                query.CardholderName = card.CardholderName;
                query.CardNumber = card.CardNumber;
                query.CVC = card.CVC;
                query.ExpiryMonth = card.ExpiryMonth;
                query.ExpiryYear = card.ExpiryYear;

                await cardsDbContext.SaveChangesAsync();
                return Ok(query);
            }

            return NotFound("Card Not Found");
        }


        // delete item
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute]Guid id)
        {
            var modelCard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (modelCard != null)
            {
                cardsDbContext.Remove(modelCard);
                await cardsDbContext.SaveChangesAsync();
                return Ok(modelCard);
            }
            return Ok("Card Not Found");
        }
    }
}
