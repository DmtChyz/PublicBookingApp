using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace API.Services.UrlBuilder
{
    public class UrlBuilder : IUrlBuilder
    {
        private readonly IConfiguration _configuration;

        public UrlBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratePasswordResetLink(string email, string token)
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var frontEndURL = _configuration.GetValue<string>("URL:FrontEnd");
            var passwordResetLink = $"{frontEndURL}/reset-password?email={email}&token={encodedToken}";
            return passwordResetLink;
        }
        public string DecodeToken(string encodedToken)
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(encodedToken);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}
