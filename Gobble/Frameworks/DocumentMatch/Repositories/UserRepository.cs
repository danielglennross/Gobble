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
                UserRoleIds = userRoleIds,
                SchoolIds = schoolIds,
                Orientation = Constraints.UserOrientation.NotSet,
                Gender = Constraints.UserGender.NotSet,
                CommunityIds = new List<string>(),
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
