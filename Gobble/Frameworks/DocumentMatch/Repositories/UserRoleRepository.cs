using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentMatch.Entities;
using DocumentMatch.Providers;

namespace DocumentMatch.Repositories
{
    public class UserRoleRepository : EntityRepository<UserRole, UserRoleMongoDBProvider>
    {
        public UserRole CreateUserRole(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("value cannot be null or empty", "name");

            UserRole uRole = new UserRole
            {
                Name = name
            };
            return uRole;
        }
    }
}
