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
    public abstract class RelationshipNeo4JProvider<T, G, H, F> where T : Entities.Entity
                                                                where G : Entities.Entity
                                                                where H : Relationships.Relationship
    {
        private const string CONNECTION_STRING = "http://localhost:7474/db/data";

        protected GraphClient _graphClient;

        protected RelationshipQueryFetcher _queryLookup;

        public RelationshipNeo4JProvider()
        {
            _graphClient = new GraphClient(new Uri(CONNECTION_STRING));
            _graphClient.Connect();

            _queryLookup = new RelationshipQueryFetcher();
        }

        public abstract bool Insert(F relationship, T t, G g, int weight = 0);

        public abstract H Get(F relationship, T t, G g);

        public abstract bool Delete(F relationship, T t, G g);
    }
}
