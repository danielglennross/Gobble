using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Providers;
using GraphMatch.Entities;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace GraphMatch.Repositories
{
    public class UserRepository : EntityRepository<User, UserNeo4JProvider>
    {
        public void InitalizeProvider()
        {
        }

        public User CreateUser()
        {
            User u = new User
            {
                DocumentUserID = null,
                Gender = Constraints.UserGender.NotSet,
                Orientation = Constraints.UserOrientation.NotSet,
                DateOfBirth = Int32.Parse(DateTime.MinValue.ToString("yyyyMMdd")),
                IsActive = true
            };
            return u;
        }

        private bool IsUserValid(User user, out string error)
        {
            error = "";
            if (user.Gender == Constraints.UserGender.NotSet)
                error += "Gender Not Set";
            if (user.Orientation == Constraints.UserOrientation.NotSet)
                error += "\nOrientation Not Set";
            if (user.DateOfBirth == Int32.Parse(DateTime.MinValue.ToString("yyyyMMdd")))
                error += "\nDate Of Birth Not Set";

            error = error.TrimStart("\n".ToCharArray());
            return error == "";
        }

        public override bool Insert(User user)
        {
            if (user.DocumentUserID == null)
                throw new InvalidOperationException("This User does not exist");
            
            string error;
            if (!IsUserValid(user, out error))
                throw new InvalidOperationException(error);

            return base.Insert(user);
        }

        public override bool Update(User user)
        {
            if (user.DocumentUserID == null)
                throw new InvalidOperationException("This User does not exist");

            string error;
            if (!IsUserValid(user, out error))
                throw new InvalidOperationException(error);

            return base.Update(user);
        }
    }
}
