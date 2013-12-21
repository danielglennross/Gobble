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
    public class CommunityNeo4JProvider : EntityNeo4JProvider<Community>
    {
        public void CreateCommunityIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :Community(DocumentCommunityID)").ExecuteWithoutResults();
        }

        public override bool Insert(Community newCommunity)
        {
            _graphClient.Cypher
                        .Merge("(c:Community { DocumentCommunityID: {DocumentCommunityID} })")
                        .OnCreate()
                        .Set("c = {newCommunity}")
                        .WithParams(new
                        {
                            DocumentCommunityID = newCommunity.DocumentCommunityID,
                            newCommunity
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(Community newCommunity)
        {
            _graphClient.Cypher
                        .Match("(c:Community)")
                        .Where((Community c) => c.DocumentCommunityID == newCommunity.DocumentCommunityID)
                        .Set("c = {update}")
                        .WithParam("update", newCommunity)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(Community newCommunity)
        {
            _graphClient.Cypher
                        .Match("(c:Community)")
                        .Where((Community c) => c.DocumentCommunityID == newCommunity.DocumentCommunityID)
                        .Delete("c")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(Community newCommunity)
        {
            _graphClient.Cypher
                        .Match("(c:Community)<-[?:r]-()")
                        .Where((Community c) => c.DocumentCommunityID == newCommunity.DocumentCommunityID)
                        .Delete("r, c")
                        .ExecuteWithoutResults();

            return true;
        }

        public override Community Get(string documentCommunityID)
        {
            return _graphClient.Cypher
                .Match("(c:Community)")
                .Where((Community c) => c.DocumentCommunityID == documentCommunityID)
                .Return(c => c.As<Community>())
                .Results
                .SingleOrDefault();
        }
    }
}
