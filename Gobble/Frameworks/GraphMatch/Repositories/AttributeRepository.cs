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
        public Entities.Attribute CreateAttribute()
        {
            Entities.Attribute a = new Entities.Attribute
            {
                DocumentAttributeID = null,
                IsActive = true
            };
            return a;
        }

        public List<Entities.Attribute> GetAttributesForUser(User user, UserAttributeRelationships relationship)
        {
            return _provider.GetAttributesForUser(user, relationship);
        }

        public void InitalizeProvider(List<string> documentAttributeIDs)
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

        public List<Entities.Attribute> PopulateAttributes(List<string> documentAttributeIDs)
        {
            return documentAttributeIDs.Select(x => new Entities.Attribute() { DocumentAttributeID = x, IsActive = true }).ToList();
        }
    }
}
