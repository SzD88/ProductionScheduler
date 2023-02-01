using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductionScheduler.Application.DTO;
using ProductionScheduler.Application.Security;
using ProductionScheduler.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductionScheduler.Infrastructure.Auth
{
    internal sealed class Authenticator : IAuthenticator
    {
        private readonly IClock _clock;
        private readonly string _issuer;
        private string _audience;
        private TimeSpan _expiry;
        private SigningCredentials _singingCredentials;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
        public Authenticator(IOptions<AuthOptions> options, IClock clock)
        {
            _clock = clock;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
            _singingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
                SecurityAlgorithms.HmacSha256);
        }
        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = _clock.Current();
            var expires = now.Add(_expiry);
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim( ClaimTypes.Role, role)
            };

            var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _singingCredentials);

            var accessToken = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JwtDto
            {
                AccessToken = accessToken,
            };

        }
    }
}
