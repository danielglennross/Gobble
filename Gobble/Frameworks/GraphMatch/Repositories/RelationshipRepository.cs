using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Providers;
using GraphMatch.Relationships;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace GraphMatch.Repositories
{
    public abstract class RelationshipRepository<T, G, H, E, F> where T : Entities.Entity
                                                                where G : Entities.Entity
                                                                where H : Relationships.Relationship
                                                                where E : Providers.RelationshipNeo4JProvider<T, G, H, F>, new()
    {
        protected E _provider;

        public RelationshipRepository()
        {
            _provider = new E();
        }

        public virtual bool Insert(F relationship, T t, G g, int weight = 0)
        {
            return _provider.Insert(relationship, t, g, weight);
        }

        public virtual H Get(F relationship, T t, G g)
        {
            return _provider.Get(relationship, t, g);
        }

        public virtual bool Delete(F relationship, T t, G g)
        {
            return _provider.Delete(relationship, t, g);
        }
    }
}
