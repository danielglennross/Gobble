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
    public class SchoolNeo4JProvider : EntityNeo4JProvider<School>
    {
        public void CreateSchoolIndexs()
        {
            _graphClient.Cypher.Create("INDEX ON :School(DocumentSchoolID)").ExecuteWithoutResults();
        }

        public override bool Insert(School newSchool)
        {
            _graphClient.Cypher
                        .Merge("(sch:School { DocumentSchoolID: {DocumentSchoolID} })")
                        .OnCreate()
                        .Set("sch = {newSchool}")
                        .WithParams(new
                        {
                            DocumentSchoolID = newSchool.DocumentSchoolID,
                            newSchool
                        })
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Update(School newSchool)
        {
            _graphClient.Cypher
                        .Match("(sch:School)")
                        .Where((School sch) => sch.DocumentSchoolID == newSchool.DocumentSchoolID)
                        .Set("sch = {update}")
                        .WithParam("update", newSchool)
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool Delete(School newSchool)
        {
            _graphClient.Cypher
                        .Match("(sch:School)")
                        .Where((School sch) => sch.DocumentSchoolID == newSchool.DocumentSchoolID)
                        .Delete("sch")
                        .ExecuteWithoutResults();

            return true;
        }

        public override bool DeleteWithInboundRelationships(School newSchool)
        {
            _graphClient.Cypher
                        .Match("(sch:School)<-[?:r]-()")
                        .Where((School sch) => sch.DocumentSchoolID == newSchool.DocumentSchoolID)
                        .Delete("r, sch")
                        .ExecuteWithoutResults();

            return true;
        }

        public override School Get(string documentSchoolID)
        {
            return _graphClient.Cypher
                .Match("(s:School)")
                .Where((School s) => s.DocumentSchoolID == documentSchoolID)
                .Return(s => s.As<School>())
                .Results
                .SingleOrDefault();
        }
    }
}
