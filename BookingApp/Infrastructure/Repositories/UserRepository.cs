using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repositories;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<IdentityUser> , IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<string?> GetUsernameByIdAsync(string id)
        {
            return await _context.Users
                .Where(dbUser => dbUser.Id == id)
                .Select(x => x.UserName).FirstOrDefaultAsync();
        }
    }
}
