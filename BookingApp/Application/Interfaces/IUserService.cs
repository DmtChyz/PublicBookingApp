using Application.Common;
using Application.DTO;
using Application.DTO.User;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetUsernameByIdAsync(string id);
        Task<bool> DoesUserExistAsync(string id);
        Task<Result<AuthentificationResultDTO>> RegisterUser(UserRegisterDTO userToRegister);
        Task<Result<AuthentificationResultDTO>> LoginAsync(UserLoginDTO userLogin);
        Task<Result<bool>> IsUserExistByNameAsync(string username);
        Task<Result<bool>> IsUserExistByEmailAsync(string email);

        Task<Result<bool>> ForgotPasswordAsync(string email);
        Task<Result<bool>> ResetPasswordAsync(string email,string token,string newPassword);
        Task<Result<UserProfileDTO>> GetUserProfileData(string id);
        Task<Result<UserProfileDTO>> UpdateUserProfileAsync(string userId, string newUserName);
    }
}
    