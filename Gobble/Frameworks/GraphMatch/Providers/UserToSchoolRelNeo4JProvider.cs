using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using Constraints;

namespace GraphMatch.Providers
{
    public class UserToSchoolRelNeo4JProvider : RelationshipNeo4JProvider<School, User, UserToSchool, SchoolRelationships>
    {
        public override bool Insert(SchoolRelationships relationship, School school, User user, int weight = 0)
        {
            _graphClient.Cypher
                .Match("(u:User)", "(sch:School)")
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
                .CreateUnique(_queryLookup[(AllRelationshipTypes)relationship].RelationshipDefinitionWithoutParam)
                .ExecuteWithoutResults();
            return true;
        }

        public override UserToSchool Get(SchoolRelationships relationship, School school, User user)
        {
            return _graphClient.Cypher
               .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
               .Where((User u) => u.DocumentUserID == user.DocumentUserID)
               .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
               .Return(r => r.As<UserToSchool>())
               .Results
               .SingleOrDefault();
        }

        public override bool Delete(SchoolRelationships relationship, School school, User user)
        {
            _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((User u) => u.DocumentUserID == user.DocumentUserID)
                .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
                .Delete("r")
                .ExecuteWithoutResults();
            return true;
        }
    }
}
