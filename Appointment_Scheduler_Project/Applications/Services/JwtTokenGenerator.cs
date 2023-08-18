using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Persistences.EF;
using Appointment_Scheduler_Project.Presentations.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Appointment_Scheduler_Project.Applications.Services
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApScDbContext _context;

        public JwtTokenGenerator(IConfiguration configuration, UserManager<ApplicationUser> userManager, ApScDbContext dbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = dbContext;
        }

        public async Task<AuthResult> GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString())
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }
            var claimsIdentity = new ClaimsIdentity(claims.ToArray());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration["Jwt:ExpiryTimeFrame"]!)),
                Audience = "FaghatKhooba", // specify the audience here
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);


            var result = new AuthResult()
            {
                Token = jwtToken,
                Result = true

            };
            return result;
        }
    }
}
