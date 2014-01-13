using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Providers;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using Neo4jClient;
using Neo4jClient.Cypher;
using Constraints;

namespace GraphMatch.Repositories
{
    public class AttributeRepository : EntityRepository<Entities.Attribute, AttributeNeo4JProvider>
    {
        public Entities.Attribute CreateAttribute(AttributeSource source)
        {
            Entities.Attribute a = new Entities.Attribute
            {
                DocumentAttributeID = null,
                IsActive = true,
                Source = source
            };
            return a;
        }

        public List<Entities.Attribute> GetAttributesForUser(User user, UserRelationships relationship)
        {
            return _provider.GetAttributesForUser(user, relationship);
        }

        public void InitalizeProvider(Dictionary<string, AttributeSource> documentAttributeIDs)
        {
            List<Entities.Attribute> attributes = PopulateAttributes(documentAttributeIDs);
            foreach (Entities.Attribute attr in attributes)
            {
                Insert(attr);
            }
        }

        public override bool Insert(Entities.Attribute attribute)
        {
            if (attribute.DocumentAttributeID == null)
                throw new InvalidOperationException("This Attribute does not exist");

            return base.Insert(attribute);
        }

        public override bool Update(Entities.Attribute attribute)
        {
            if (attribute.DocumentAttributeID == null)
                throw new InvalidOperationException("This Attribute does not exist");

            return base.Update(attribute);
        }

        public List<Entities.Attribute> PopulateAttributes(Dictionary<string, AttributeSource> documentAttributeIDs)
        {
            return documentAttributeIDs.Select(x => new Entities.Attribute() { DocumentAttributeID = x.Key, Source = x.Value, IsActive = true }).ToList();
        }
    }
}
