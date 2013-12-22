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
    public abstract class EntityNeo4JProvider<T>
    {
        private const string CONNECTION_STRING = "http://localhost:7474/db/data";

        protected GraphClientWrapper _graphClient;

        protected RelationshipQueryFetcher _queryLookup;

        public EntityNeo4JProvider()
        {
            _graphClient = new GraphClientWrapper(new Uri(CONNECTION_STRING));
            _graphClient.Connect();

            _queryLookup = new RelationshipQueryFetcher();
        }

        public abstract bool Insert(T g);
        public abstract bool Update(T g);
        public abstract T Get(string id);
        public abstract bool Delete(T g);
        public abstract bool DeleteWithInboundRelationships(T g);
    }
}
