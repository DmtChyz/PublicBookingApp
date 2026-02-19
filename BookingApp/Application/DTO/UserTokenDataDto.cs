using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserTokenDataDto
    {
        public string Username { get; set; }
        public string Id { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}
