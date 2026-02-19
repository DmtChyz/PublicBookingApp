using Application.Common;
using Application.DTO;
using Application.DTO;
using Application.DTO.User;
using Application.Interfaces;
using Infrastructure.RolesList;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailSender _emailService;
        private readonly IUrlBuilder _urlBuilder;

        public UserService(UserManager<IdentityUser> userManager,IJwtTokenGenerator jwtTokenGenerator, IEmailSender emailService, IUrlBuilder urlBuilder)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _urlBuilder = urlBuilder;
        }

        public async Task<bool> DoesUserExistAsync(string id)
        {
            var isUserExist = await _userManager.FindByIdAsync(id);
            return isUserExist != null
                ? true
                : false;
        }

        public async Task<string?> GetUsernameByIdAsync(string id)
        {
            var username = await _userManager.FindByIdAsync(id);
            return username != null 
                ? username.UserName 
                : null;
        }

        public async Task<Result<AuthentificationResultDTO>> LoginAsync(UserLoginDTO userData)
        {
            IdentityUser userToLogin = null;

            if (!string.IsNullOrEmpty(userData.Name))
            {
                userToLogin = await _userManager.FindByNameAsync(userData.Name);
            }
            else if(!string.IsNullOrEmpty(userData.Email) && userData.Email.Contains('@'))
            {
                userToLogin = await _userManager.FindByEmailAsync(userData.Email);
            }

            if (userToLogin == null)
            {
                return Result<AuthentificationResultDTO>.IsFailure("No user found.");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(userToLogin, userData.Password);

            if(isPasswordCorrect == false)
            {
                return Result<AuthentificationResultDTO>.IsFailure("Password is incorrect.");
            }

            var userRoles = new List<string>(await _userManager.GetRolesAsync(userToLogin));

            var token = _jwtTokenGenerator.GenerateToken(new UserTokenDataDto()
            {
                Username = userToLogin.UserName,
                Id = userToLogin.Id,
                Role = userRoles
            });

            var authentificationToken = AuthentificationResultDTO.WriteToken(token);
            return Result<AuthentificationResultDTO>.IsSuccess(authentificationToken);
        }

        public async Task<Result<AuthentificationResultDTO>> RegisterUser(UserRegisterDTO userToRegister)
        {

            var identityUserToCreate = new IdentityUser()
            {
                UserName = userToRegister.Username,
                Email = userToRegister.Email,
            };

            var result = await _userManager.CreateAsync(identityUserToCreate, userToRegister.Password);
            if (!result.Succeeded)
            {
                var error = string.Join( "," , result.Errors.Select(x => x.Description));
                return Result<AuthentificationResultDTO>.IsFailure(error);
            }

            var isRoleAdded = await _userManager.AddToRoleAsync(identityUserToCreate, Roles.User);

            if (!isRoleAdded.Succeeded)
            {
                var error = string.Join(",", isRoleAdded.Errors.Select(x => x.Description));
                return Result<AuthentificationResultDTO>.IsFailure("User role error.");
            }   

            var userRoles = new List<string>(await _userManager.GetRolesAsync(identityUserToCreate));

            
            var token = _jwtTokenGenerator.GenerateToken(new UserTokenDataDto()
            {
                Username = identityUserToCreate.UserName,
                Id = identityUserToCreate.Id,   
                Role = userRoles
            });

            try
            {
                await _emailService.SendGridEmailAsync(
                    new SendEmailDTO()
                    {
                        Email = identityUserToCreate.Email,
                        Subject = "Greetings",
                        Body = "Thank u for using our product!"
                    }
                );
            }
            catch (Exception ex)
            {
                // Logs is not implemented yet.
            }

            var authDto = AuthentificationResultDTO.WriteToken(token);
            return Result<AuthentificationResultDTO>.IsSuccess(authDto);
        }

        public async Task<Result<bool>> IsUserExistByEmailAsync(string email)
        {
            var isUserFound = await _userManager.FindByEmailAsync(email);   

            if(isUserFound == null)
            {
                return Result<bool>.IsFailure(default);
            }

            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<bool>> IsUserExistByNameAsync(string username)
        {
            var isUserFound = await _userManager.FindByNameAsync(username);

            if(isUserFound == null)
            {
                return Result<bool>.IsFailure(default);
            }

            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<bool>> ForgotPasswordAsync(string email)
        {
            var foundUser = await _userManager.FindByEmailAsync(email);
            if(foundUser == null)
            {   // also preventing valid email leak 
                return Result<bool>.IsSuccess(true);
            }
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(foundUser);
            var passwordResetLink = _urlBuilder.GeneratePasswordResetLink(email, passwordResetToken);

            var emailDTO = new SendEmailDTO
            {
                Email = email,
                Subject = "Password reset",
                Body = $"This is the password reset link if thats not yours please, let our support team know, if that was you click to reset password:${passwordResetLink}"
            };

            var isEmailSend = await _emailService.SendGridEmailAsync(emailDTO);
            if(isEmailSend.Success) return Result<bool>.IsSuccess(true);

            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var foundUser = await _userManager.FindByEmailAsync(email);
            if(foundUser == null)
            {
                return Result<bool>.IsFailure("Invalid Action.");
            }
            var decodedToken = _urlBuilder.DecodeToken(token);
            var resetResult = await _userManager.ResetPasswordAsync(foundUser, decodedToken, newPassword);
            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                return Result<bool>.IsFailure(errors);
            }
            return Result<bool>.IsSuccess(true);
        }

        public async Task<Result<UserProfileDTO>> GetUserProfileData(string id)
        {
            var foundUser = await _userManager.FindByIdAsync(id);

            if (foundUser == null)
            {
                return Result<UserProfileDTO>.IsFailure("User not found.");
            }

            var userProfileData = new UserProfileDTO()
            {
                Email = MaskEmail(foundUser.Email),
                Username = foundUser.UserName,
                Id = foundUser.Id,
            };

            return Result<UserProfileDTO>.IsSuccess(userProfileData);
        }

        public async Task<Result<UserProfileDTO>> UpdateUserProfileAsync(string userId, string newUserName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<UserProfileDTO>.IsFailure("User not found.");
            }

            if (user.UserName != newUserName)
            {
                var existingUser = await _userManager.FindByNameAsync(newUserName);

                if (existingUser != null)
                {
                    return Result<UserProfileDTO>.IsFailure("This username is already taken.");
                }

                user.UserName = newUserName;
                user.NormalizedUserName = newUserName.ToUpper();
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<UserProfileDTO>.IsFailure(errors);
            }

            return Result<UserProfileDTO>.IsSuccess(new UserProfileDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            });
        }

        private static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return "unknown";
            }

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 3)
            {
                return $"{name[0]}***@{domain}";
            }
            var firstPart = name.Substring(0, 3);
            var lastChar = name[name.Length - 1];

            return $"{firstPart}***{lastChar}@{domain}";
        }
    }
}
