using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AuthentificationResultDTO
    {
        public string Token { get ; init; }

        public static AuthentificationResultDTO WriteToken(string token)
        {
            return new AuthentificationResultDTO {Token = token};
        }
    }
}
