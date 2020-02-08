using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ParkingAPI.Data.IRepository;
using ParkingAPI.Dtos;
using ParkingAPI.Models;

namespace ParkingAPI.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {

        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        public IConfiguration Configuration { get; }
        public AuthController(IAuthRepository repo, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            Configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if (!string.IsNullOrEmpty(userForRegisterDto.Email))
                userForRegisterDto.Email = userForRegisterDto.Email.ToLower();
            if (await _repo.UserExists(userForRegisterDto.Email))
                ModelState.AddModelError("Email", "Email is already taken");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            var createUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            var userToReturn = _mapper.Map<UserForDetailDto>(createUser);
            return StatusCode(201);
            //return CreatedAtRoute("GetUser", new { controller = "Users", id = createUser.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Email.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username),
                    new Claim(ClaimTypes.Email, userFromRepo.Email)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new { tokenString, user });

        }
    }
}