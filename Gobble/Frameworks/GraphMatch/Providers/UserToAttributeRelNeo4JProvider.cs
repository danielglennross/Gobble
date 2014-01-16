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
    public class UserToAttributeRelNeo4JProvider : RelationshipNeo4JProvider<Entities.Attribute, User, UserToAttribute, UserAttributeRelationships>
    {
        public override bool Insert(UserAttributeRelationships relationship, Entities.Attribute attribute, User user, int weight = 0)
        {
            if (_queryLookup.DoesRelationshipHaveWeight((AllRelationshipTypes)relationship) && weight <= 0)
                throw new InvalidOperationException("Weight must be > than 0");

            _graphClient.Cypher
                .Match("(u:User)", "(attr:Attribute)")
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((Entities.Attribute attr) => attr.DocumentAttributeID == attribute.DocumentAttributeID)
                .CreateUnique(_queryLookup[(AllRelationshipTypes)relationship].RelationshipDefinitionWithParam)
                .WithParam("Data", new { Weight = weight })
                .ExecuteWithoutResults();
            return true;
        }

        public bool Update(UserAttributeRelationships relationship, Entities.Attribute attribute, User user, int weight)
        {
            if (weight <= 0)
                throw new InvalidOperationException("Weight must be > than 0");

            _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((Entities.Attribute attr) => attr.DocumentAttributeID == attribute.DocumentAttributeID)
                .Set("r.Weight = {Value}")
                .WithParam("Value", weight)
                .ExecuteWithoutResults();
            return true;
        }

        public override UserToAttribute Get(UserAttributeRelationships relationship, Entities.Attribute attribute, User user)
        {
            return _graphClient.Cypher
               .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
               .Where((User u) => u.DocumentUserID == user.DocumentUserID)
               .AndWhere((Entities.Attribute attr) => attr.DocumentAttributeID == attribute.DocumentAttributeID)
               .Return(r => r.As<UserToAttribute>())
               .Results
               .SingleOrDefault();
        }

        public override bool Delete(UserAttributeRelationships relationship, Entities.Attribute attribute, User user)
        {
            _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((Entities.Attribute attr) => attr.DocumentAttributeID == attribute.DocumentAttributeID)
                .Delete("r")
                .ExecuteWithoutResults();
            return true;
        }
    }
}
