using Application.DTO;
namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(UserTokenDataDto userData);
    }
}
