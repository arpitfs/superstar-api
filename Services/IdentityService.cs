using ApiWorld.Domain;
using ApiWorld.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiWorld.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationRequest> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "User does not exists" } };
            }

            var userPassword = await _userManager.CheckPasswordAsync(user, password);

            if(!userPassword)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "Username/password is incorrect" } };
            }

            return GetTokenForUser(user);
        }

        public async Task<AuthenticationRequest> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "User already exists" } };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationRequest { ErrorMessages = createdUser.Errors.Select(x => x.Description) };
            }

            return GetTokenForUser(newUser);
        }

        private AuthenticationRequest GetTokenForUser(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("id", newUser.Id),
                    }),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationRequest
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
