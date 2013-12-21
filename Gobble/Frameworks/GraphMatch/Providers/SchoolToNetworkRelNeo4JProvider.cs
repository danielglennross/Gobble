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
    public class SchoolToNetworkRelNeo4JProvider : RelationshipNeo4JProvider<Network, School, SchoolToNetwork, NetworkRelationships>
    {
        public override bool Insert(NetworkRelationships relationship, Network network, School school, int weight = 0)
        {
            _graphClient.Cypher
                .Match("(n:Network)", "(sch:School)")
                .Where((Network n) => n.DocumentNetworkID == network.DocumentNetworkID)
                .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
                .CreateUnique(_queryLookup[(AllRelationshipTypes)relationship].RelationshipDefinitionWithoutParam)
                .ExecuteWithoutResults();
            return true;
        }

        public override SchoolToNetwork Get(NetworkRelationships relationship, Network network, School school)
        {
            return _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((Network n) => n.DocumentNetworkID == network.DocumentNetworkID)
                .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
                .Return(r => r.As<SchoolToNetwork>())
                .Results
                .SingleOrDefault();
        }

        public override bool Delete(NetworkRelationships relationship, Network network, School school)
        {
            _graphClient.Cypher
                .Match(_queryLookup[(AllRelationshipTypes)relationship].RelationshipMatch)
                .Where((Network n) => n.DocumentNetworkID == network.DocumentNetworkID)
                .AndWhere((School sch) => sch.DocumentSchoolID == school.DocumentSchoolID)
                .Delete("r")
                .ExecuteWithoutResults();
            return true;
        }
    }
}
