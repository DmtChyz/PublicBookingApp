using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUrlBuilder
    {
        public string GeneratePasswordResetLink(string email, string token);
        public string DecodeToken(string encodedToken);
    }
}
