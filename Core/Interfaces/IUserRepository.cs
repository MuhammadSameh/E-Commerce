using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {


        public Task<User> GetById(string id);
    }
}
