using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using Constraints;

namespace GraphMatch.Providers
{
    public class UserToBookMarkRelNeo4JProvider : RelationshipNeo4JProvider<BookMark, User, UserToBookMark, UserBookMarkRelationships>
    {
        public override bool Insert(UserBookMarkRelationships relationship, BookMark bookMark, User user, int weight = 0)
        {
            if (weight != 0)
                throw new InvalidOperationException("Weight does not exist on this relationship type");

            _graphClient.Cypher
                .Match("(u:User)", "(b:BookMark)")
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((BookMark b) => b.DocumentBookMarkID == bookMark.DocumentBookMarkID)
                .CreateUnique(_queryLookup[(AllRelationshipTypes)relationship].RelationshipDefinitionWithParam)
                .WithParam("Data", new { Weight = weight })
                .ExecuteWithoutResults();
            return true;
        }

        public override UserToBookMark Get(UserBookMarkRelationships relationship, BookMark bookMark, User user)
        {
            return _graphClient.Cypher
               .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
               .Where((User u) => u.DocumentUserID == user.DocumentUserID)
               .AndWhere((BookMark b) => b.DocumentBookMarkID == bookMark.DocumentBookMarkID)
               .Return(r => r.As<UserToBookMark>())
               .Results
               .SingleOrDefault();
        }

        public override bool Delete(UserBookMarkRelationships relationship, BookMark attribute, User user)
        {
            _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((BookMark b) => b.DocumentBookMarkID == b.DocumentBookMarkID)
                .Delete("r")
                .ExecuteWithoutResults();
            return true;
        }
    }
}
