using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class SecretController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecret()
    {

        var expClaim = User.FindFirst("exp");

        if (expClaim == null)
        {
            return Unauthorized("No exp claim found in token.");
        }

        // Convertir `exp` (en segundos desde Unix epoch) a DateTime
        var expSeconds = long.Parse(expClaim.Value);
        var expirationTimeUtc = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

        var nowUtc = DateTime.UtcNow;
        var remainingTime = expirationTimeUtc - nowUtc;

        if (remainingTime.TotalSeconds <= 0)
        {
            return Unauthorized("Token already expired.");
        }

        var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Can't get the user name";

        return Ok(new
        {
            Message = $"This content is fully protected. Hello user: {userName}",
            TokenExpiresAt = expirationTimeUtc,
            RemainingSeconds = (int)remainingTime.TotalSeconds
        });
    }
}
