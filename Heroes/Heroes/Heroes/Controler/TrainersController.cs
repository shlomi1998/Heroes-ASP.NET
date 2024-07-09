using Heroes.Models;
using Heroes.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Heroes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IConfiguration _configuration;

        public TrainersController(ITrainerRepository trainerRepository, IConfiguration configuration)
        {
            _trainerRepository = trainerRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<Trainer>> GetAllTrainers()
        {
            return await _trainerRepository.GetAllTrainersAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainerById(int id)
        {
            var trainer = await _trainerRepository.GetTrainerByIdAsync(id);

            if (trainer == null)
            {
                return NotFound();
            }

            return trainer;
        }

        [HttpPost]
        public async Task<ActionResult<Trainer>> AddTrainer(TrainerWithHeroesModel trainerModel)
        {
            var trainer = new Trainer
            {
                Username = trainerModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(trainerModel.Password), // הצפנת הסיסמא
                Heroes = trainerModel.Heroes
            };

            await _trainerRepository.AddTrainerAsync(trainer);
            return CreatedAtAction(nameof(GetTrainerById), new { id = trainer.Id }, trainer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return BadRequest();
            }

            await _trainerRepository.UpdateTrainerAsync(trainer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            await _trainerRepository.DeleteTrainerAsync(id);
            return NoContent();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupModel signupModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usernameExists = await _trainerRepository.IsUsernameExistsAsync(signupModel.Username);
            if (!usernameExists)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var trainer = await _trainerRepository.GetTrainerByUsernameAsync(signupModel.Username);
            if (trainer == null || !BCrypt.Net.BCrypt.Verify(signupModel.Password, trainer.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(trainer);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trainer = await _trainerRepository.GetTrainerByUsernameAsync(loginModel.Username);
            if (trainer == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, trainer.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(trainer);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Trainer trainer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, trainer.Username),
                    new Claim("TrainerId", trainer.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
