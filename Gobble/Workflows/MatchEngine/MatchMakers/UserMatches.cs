using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchEngine.Providers;
using MatchEngine.Results;
using GraphMatch.Entities;

namespace MatchEngine.MatchMakers
{
    public class UserMatches
    {
        private UserMatchesNeo4JProvider _provider;

        public UserMatches()
        {
            _provider = new UserMatchesNeo4JProvider();
        }

        public List<UserToUserResults> GetGeneralUserMatches(User user)
        {
            return _provider.GetGeneralUserMatchesWithDislikes(user);
        }
    }
}
