using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class UserLoginDTO
    {
        public string? Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
    }
}