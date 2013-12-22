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
    public class AttributeNeo4JProvider : EntityNeo4JProvider<Entities.Attribute>
    {
        public void CreateAttributeIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :Attribute(DocumentAttributeID)").ExecuteWithoutResults();
        }

        public List<Entities.Attribute> GetAttributesForUser(User user, UserRelationships relationship)
        {
           return  _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .Return(attr => attr.As<Entities.Attribute>())
                .Results
                .ToList();
        }

        public override bool Insert(Entities.Attribute newAttribute)
        {
            _graphClient.Cypher
                        .Merge("(attr:Attribute { DocumentAttributeID: {DocumentAttributeID} })")
                        .OnCreate()
                        .Set("attr = {newAttribute}")
                        .WithParams(new
                        {
                            DocumentAttributeID = newAttribute.DocumentAttributeID,
                            newAttribute
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(Entities.Attribute newAttribute)
        {
            _graphClient.Cypher
                        .Match("(attr:Attribute)")
                        .Where((Entities.Attribute attr) => attr.DocumentAttributeID == newAttribute.DocumentAttributeID)
                        .Set("attr = {update}")
                        .WithParam("update", newAttribute)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(Entities.Attribute newAttribute)
        {
            _graphClient.Cypher
                        .Match("(attr:Attribute)")
                        .Where((Entities.Attribute attr) => attr.DocumentAttributeID == newAttribute.DocumentAttributeID)
                        .Delete("attr")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(Entities.Attribute newAttribute)
        {
            _graphClient.Cypher
                        .Match("(attr:Attribute)<-[?:r]-()")
                        .Where((Entities.Attribute attr) => attr.DocumentAttributeID == newAttribute.DocumentAttributeID)
                        .Delete("r, attr")
                        .ExecuteWithoutResults();

            return true;
        }

        public override Entities.Attribute Get(string documentAttributeID)
        {
            return _graphClient.Cypher
                .Match("(a:Attribute)")
                .Where((Entities.Attribute a) => a.DocumentAttributeID == documentAttributeID)
                .Return(a => a.As<Entities.Attribute>())
                .Results
                .SingleOrDefault();
        }
    }
}
