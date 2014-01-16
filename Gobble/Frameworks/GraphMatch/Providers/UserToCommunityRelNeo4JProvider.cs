using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using Constraints;

namespace GraphMatch.Providers
{
    public class UserToCommunityRelNeo4JProvider : RelationshipNeo4JProvider<Community, User, UserToCommunity, UserCommunityRelationships>
    {
        public override bool Insert(UserCommunityRelationships relationship, Community community, User user, int weight = 0)
        {
            _graphClient.Cypher
                .Match("(u:User)", "(c:Community)")
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((Community c) => c.DocumentCommunityID == community.DocumentCommunityID)
                .CreateUnique(_queryLookup[(AllRelationshipTypes)relationship].RelationshipDefinitionWithoutParam)
                .ExecuteWithoutResults();
            return true;
        }

        public override UserToCommunity Get(UserCommunityRelationships relationship, Community community, User user)
        {
            return _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((Community c) => c.DocumentCommunityID == community.DocumentCommunityID)
                .Return(r => r.As<UserToCommunity>())
                .Results
                .SingleOrDefault();
        }

        public override bool Delete(UserCommunityRelationships relationship, Community community, User user)
        {
            _graphClient.Cypher
                 .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                 .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                 .AndWhere((Community c) => c.DocumentCommunityID == community.DocumentCommunityID)
                 .Delete("r")
                 .ExecuteWithoutResults();
            return true;
        }
    }
}
