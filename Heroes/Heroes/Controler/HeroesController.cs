using Heroes.Models;
using Heroes.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heroes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroRepository _heroRepository;

        public HeroesController(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        // קבל את כל הגיבורים
        [HttpGet]
        public async Task<IEnumerable<Hero>> GetAllHeroes()
        {
            return await _heroRepository.GetAllHeroesAsync();
        }

        // קבל גיבור לפי ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHeroById(int id)
        {
            var hero = await _heroRepository.GetHeroByIdAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return hero;
        }

        // קבל גיבורים ממיונים לפי כוח
        [HttpGet("sorted-by-power")]
        public async Task<IEnumerable<Hero>> GetHeroesSortedByPower()
        {
            return await _heroRepository.GetHeroesSortedByPowerAsync();
        }

        // אימון גיבור
        [HttpPost("{id}/train")]
        public async Task<IActionResult> TrainHero(int id)
        {
            var hero = await _heroRepository.GetHeroByIdAsync(id);

            if (hero == null)
            {
                return NotFound(new { message = "Hero not found" });
            }

            DateTime today = DateTime.Today;
            if (hero.LastTrainingDate != today)
            {
                hero.TrainingCountToday = 0;
                hero.LastTrainingDate = today;
            }

            if (hero.TrainingCountToday >= 5)
            {
                return BadRequest(new { message = "Hero has already trained 5 times today. Try again tomorrow." });
            }

            Random random = new Random();
            decimal growthPercentage = (decimal)(random.NextDouble() * 10); // generates a number between 0 and 10
            decimal growthAmount = hero.CurrentPower * growthPercentage / 100;
            hero.CurrentPower += growthAmount;
            hero.TrainingCountToday++;

            await _heroRepository.UpdateHeroAsync(hero);

            return Ok(new { message = $"Hero {hero.Name} trained. Power increased by {growthPercentage:F2}%. New power is {hero.CurrentPower:F2}." });
        }
    }
}
