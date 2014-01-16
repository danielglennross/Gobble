using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using GraphMatch.Providers;
using Constraints;

namespace GraphMatch.Repositories
{
    public class UserToAttributeRepository : RelationshipRepository<Entities.Attribute, User, UserToAttribute, UserToAttributeRelNeo4JProvider, UserAttributeRelationships>
    {
        // specific to this class
        public bool Update(UserAttributeRelationships relationship, Entities.Attribute attribute, User user, int weight)
        {
            return _provider.Update(relationship, attribute, user, weight);
        }
    }
}
