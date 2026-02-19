using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class UserRegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
