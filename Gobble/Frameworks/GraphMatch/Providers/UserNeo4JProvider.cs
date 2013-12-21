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
    public class UserNeo4JProvider : EntityNeo4JProvider<User>
    {
        public void CreateUserIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :User(DocumentUserID)").ExecuteWithoutResults();
        }

        public override bool Insert(User newUser)
        {
            _graphClient.Cypher
                        .Merge("(user:User { DocumentUserID: {DocumentUserID} })")
                        .OnCreate()
                        .Set("user = {newUser}")
                        .WithParams(new
                        {
                            DocumentUserID = newUser.DocumentUserID,
                            newUser
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(User newUser)
        {
            _graphClient.Cypher
                        .Match("(user:User)")
                        .Where((User user) => user.DocumentUserID == newUser.DocumentUserID)
                        .Set("user = {update}")
                        .WithParam("update", newUser)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(User newUser)
        {
            _graphClient.Cypher
                        .Match("(user:User)")
                        .Where((User user) => user.DocumentUserID == newUser.DocumentUserID)
                        .Delete("user")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(User newUser)
        {
            _graphClient.Cypher
                        .Match("(user:User)<-[?:r]-()")
                        .Where((User user) => user.DocumentUserID == newUser.DocumentUserID)
                        .Delete("r, user")
                        .ExecuteWithoutResults();

            return true;
        }

        public override User Get(string documentUserID)
        {
            return _graphClient.Cypher
                .Match("(u:User)")
                .Where((User u) => u.DocumentUserID == documentUserID)
                .Return(u => u.As<User>())
                .Results
                .SingleOrDefault();
        }
    }
}
