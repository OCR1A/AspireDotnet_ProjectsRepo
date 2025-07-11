using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityManager.Services
{

    public class JwtService
    {

        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(string username, bool emailConfirmed)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),   //UserName claim
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //Guid Claim
            new Claim("email_confirmed", emailConfirmed.ToString().ToLower())
            };

            var jwtSection = _config.GetSection("Jwt");
            var configKey = jwtSection["Key"];
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}