using ApiWorld.Data;
using ApiWorld.Domain;
using ApiWorld.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dbContext = dbContext;
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

            return await GetTokenForUser(user);
        }

        public async Task<AuthenticationRequest> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "Invalid Token" } };
            }

            var expiryTimeUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
                .AddSeconds(expiryTimeUnix);

            if(expiryTime > DateTime.Now)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "Token not expired" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _dbContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            
            if(storedRefreshToken ==  null || DateTime.Now > storedRefreshToken.ExpirationDate ||
                storedRefreshToken.Invalidated || storedRefreshToken.Used || storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "Token invalid" } };
            }

            storedRefreshToken.Used = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GetTokenForUser(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if(!isValidSignedToken(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool isValidSignedToken(SecurityToken token)
        {
            return (token is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationRequest> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationRequest { ErrorMessages = new[] { "User already exists" } };
            }

            var userId = Guid.NewGuid();
            var newUser = new IdentityUser
            {
                Id = userId.ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationRequest { ErrorMessages = createdUser.Errors.Select(x => x.Description) };
            }

            await _userManager.AddClaimAsync(newUser, new Claim("manager", "true"));

            return await GetTokenForUser(newUser);
        }

        private async Task<AuthenticationRequest> GetTokenForUser(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", newUser.Id),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var customClaim = await _userManager.GetClaimsAsync(newUser);
            claims.AddRange(customClaim);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = newUser.Id,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(6)
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            _dbContext.SaveChanges();

            return new AuthenticationRequest
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
