using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Relationships;

namespace GraphMatch.Providers
{
    public class BookMarkNeo4JProvider : EntityNeo4JProvider<BookMark>
    {
        public void CreateAttributeIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :BookMark(DocumentBookMarkID)").ExecuteWithoutResults();
        }

        public List<BookMark> GetBookMarksForUser(User user, UserRelationships relationship)
        {
            return _graphClient.Cypher
                 .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                 .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                 .Return(b => b.As<BookMark>())
                 .Results
                 .ToList();
        }

        public override bool Insert(BookMark bookMark)
        {
            _graphClient.Cypher
                        .Merge("(b:BookMark { DocumentBookMarkID: {DocumentBookMarkID} })")
                        .OnCreate()
                        .Set("b = {newBookMark}")
                        .WithParams(new
                        {
                            DocumentBookMarkID = bookMark.DocumentBookMarkID,
                            newBookMark = bookMark
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(BookMark bookMark)
        {
            _graphClient.Cypher
                        .Match("(b:BookMark)")
                        .Where((BookMark b) => b.DocumentBookMarkID == bookMark.DocumentBookMarkID)
                        .Set("b = {update}")
                        .WithParam("update", bookMark)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(BookMark bookMark)
        {
            _graphClient.Cypher
                        .Match("(b:BookMark)")
                        .Where((BookMark b) => b.DocumentBookMarkID == bookMark.DocumentBookMarkID)
                        .Delete("b")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(BookMark bookMark)
        {
            _graphClient.Cypher
                        .Match("(b:BookMark)<-[?:r]-()")
                        .Where((BookMark b) => b.DocumentBookMarkID == bookMark.DocumentBookMarkID)
                        .Delete("r, b")
                        .ExecuteWithoutResults();

            return true;
        }

        public override BookMark Get(string documentBookMarkID)
        {
            return _graphClient.Cypher
                .Match("(b:BookMark)")
                .Where((BookMark b) => b.DocumentBookMarkID == documentBookMarkID)
                .Return(b => b.As<BookMark>())
                .Results
                .SingleOrDefault();
        }
    }
}
