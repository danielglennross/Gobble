using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentMatch.Entities;
using DocumentMatch.Providers;

namespace DocumentMatch.Repositories
{
    public class UserRepository : EntityRepository<User, UserMongoDBProvider>
    {
        public User CreateUser(List<Account> accounts, List<string> userRoleIds, List<string> schoolIds)
        {
            User u = new User
            {
                Accounts = accounts,
                UserRole_Ids = userRoleIds,
                School_Ids = schoolIds,
                Orientation = Constraints.UserOrientation.NotSet,
                Gender = Constraints.UserGender.NotSet,
                Community_Ids = new List<string>(),
                QuestionAnswers = new List<QuestionAnswer>(),
                Email = "",
                FirstName = "",
                LastName = "",
                IsActive = true
            };
            return u;
        }
    }
}
