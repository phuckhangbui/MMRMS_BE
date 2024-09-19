using DTOs.Account;
using System.Security.Claims;

namespace Service.Interface
{
    public interface ITokenService
    {
        string CreateToken(AccountBaseDto accountBaseDto);
        string CreateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
