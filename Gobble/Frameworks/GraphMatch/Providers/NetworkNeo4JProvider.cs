using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using GraphMatch.Entities;
using Constraints;

namespace GraphMatch.Providers
{
    public class NetworkNeo4JProvider : EntityNeo4JProvider<Network>
    {
        public NetworkNeo4JProvider() : base()
        {
        }

        public void CreateNetworkIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :Network(DocumentNetworkID)").ExecuteWithoutResults();
        }

        public override bool Insert(Network newNetwork)
        {
            _graphClient.Cypher
                        .Merge("(n:Network { DocumentNetworkID: {DocumentNetworkID} })")
                        .OnCreate()
                        .Set("n = {newNetwork}")
                        .WithParams(new
                        {
                            DocumentNetworkID = newNetwork.DocumentNetworkID,
                            newNetwork
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(Network newNetwork)
        {
            _graphClient.Cypher
                        .Match("(n:Network)")
                        .Where((Network n) => n.DocumentNetworkID == newNetwork.DocumentNetworkID)
                        .Set("n = {update}")
                        .WithParam("update", newNetwork)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(Network newNetwork)
        {
            _graphClient.Cypher
                        .Match("(n:Network)")
                        .Where((Network n) => n.DocumentNetworkID == newNetwork.DocumentNetworkID)
                        .Delete("n")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(Network newNetwork)
        {
            _graphClient.Cypher
                        .Match("(n:Network)<-[?:r]-()")
                        .Where((Network n) => n.DocumentNetworkID == newNetwork.DocumentNetworkID)
                        .Delete("r, n")
                        .ExecuteWithoutResults();

            return true;
        }

        public override Network Get(string documentNetworkID)
        {
            return _graphClient.Cypher
                .Match("(n:Network)")
                .Where((Network n) => n.DocumentNetworkID == documentNetworkID)
                .Return(n => n.As<Network>())
                .Results
                .SingleOrDefault();
        }
    }
}
