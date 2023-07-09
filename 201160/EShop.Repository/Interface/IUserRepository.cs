using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository.Interface
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAll();
        public User Get(string id);
        void Insert(User entity);
        void Update(User entity);
        void Delete(User entity);
    }
}
