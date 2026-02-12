using WebApi.Models;

namespace WebApi.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}