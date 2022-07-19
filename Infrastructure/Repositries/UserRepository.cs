using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositries
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly EcommerceContext _context;
        public UserRepository(EcommerceContext context):base(context)
        {
            this._context = context;
        }

        public async Task<User> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
