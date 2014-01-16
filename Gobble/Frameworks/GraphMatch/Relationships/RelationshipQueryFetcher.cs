using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMatch.Relationships
{
    public enum AllRelationshipTypes
    {
        UserHasAttribute,
        UserLikesAttribute,
        UserDislikesAttribute,
        UserMarksAttribute, // for external attributes, e.g FB likes

        UserAttendsSchool,
        UserMemberOfCommunity,
        SchoolExistsInNetwork
    }
    public enum UserRelationships
    {
        UserHasAttribute = AllRelationshipTypes.UserHasAttribute,
        UserLikesAttribute = AllRelationshipTypes.UserLikesAttribute,
        UserDislikesAttribute = AllRelationshipTypes.UserDislikesAttribute,
        UserMarksAttribute = AllRelationshipTypes.UserMarksAttribute
    }
    public enum SchoolRelationships
    {
        UserAttendsSchool = AllRelationshipTypes.UserAttendsSchool
    }
    public enum CommunityRelationships
    {
        UserMemberOfCommunity = AllRelationshipTypes.UserMemberOfCommunity
    }
    public enum NetworkRelationships
    {
        SchoolExistsInNetwork = AllRelationshipTypes.SchoolExistsInNetwork
    }

    public class RelationshipQueryFetcher
    {
        public class RelationshipQuery
        {
            public string RelationshipDefinitionWithParam { get; set; }
            public string RelationshipDefinitionWithoutParam { get; set; }
            public string RelationshipMatch { get; set; }
        }

        private Dictionary<AllRelationshipTypes, RelationshipQuery> _innerDictionary = new Dictionary<AllRelationshipTypes, RelationshipQuery>();

        public RelationshipQueryFetcher()
        {
            _innerDictionary.Add(AllRelationshipTypes.UserHasAttribute, new RelationshipQuery 
            { 
                RelationshipDefinitionWithParam = "u-[r:HAS {Data}]->attr", 
                RelationshipDefinitionWithoutParam = "u-[r:HAS]->attr",
                RelationshipMatch = "(u:User)-[r:HAS]->(attr:Attribute)"
            });

            _innerDictionary.Add(AllRelationshipTypes.UserLikesAttribute, new RelationshipQuery 
            { 
                RelationshipDefinitionWithParam = "u-[r:LIKES {Data}]->attr", 
                RelationshipDefinitionWithoutParam = "u-[r:LIKES]->attr",
                RelationshipMatch = "(u:User)-[r:LIKES]->(attr:Attribute)"
            });

            _innerDictionary.Add(AllRelationshipTypes.UserDislikesAttribute, new RelationshipQuery 
            { 
                RelationshipDefinitionWithParam = "u-[r:DISLIKES {Data}]->attr", 
                RelationshipDefinitionWithoutParam = "u-[r:DISLIKES]->attr",
                RelationshipMatch = "(u:User)-[r:DISLIKES]->(attr:Attribute)"
            });

            _innerDictionary.Add(AllRelationshipTypes.UserMarksAttribute, new RelationshipQuery
            {
                RelationshipDefinitionWithoutParam = "u-[r:MARKS]->attr",
                RelationshipMatch = "(u:User)-[r:MARKS]->(attr:Attribute)"
            });

            _innerDictionary.Add(AllRelationshipTypes.UserAttendsSchool, new RelationshipQuery
            {
                RelationshipDefinitionWithParam = "u-[r:ATTENDS {Data}]->sch",
                RelationshipDefinitionWithoutParam = "u-[r:ATTENDS]->sch",
                RelationshipMatch = "(u:User)-[r:ATTENDS]->(sch:School)"
            });

            _innerDictionary.Add(AllRelationshipTypes.UserMemberOfCommunity, new RelationshipQuery
            {
                RelationshipDefinitionWithParam = "u-[r:MEMBER {Data}]->c",
                RelationshipDefinitionWithoutParam = "u-[r:MEMBER]->c",
                RelationshipMatch = "(u:User)-[r:MEMBER]->(c:Community)"
            });

            _innerDictionary.Add(AllRelationshipTypes.SchoolExistsInNetwork, new RelationshipQuery
            {
                RelationshipDefinitionWithParam = "sch-[r:EXISTS {Data}]->n",
                RelationshipDefinitionWithoutParam = "sch-[r:EXISTS]->n",
                RelationshipMatch = "(sch:School)-[r:EXISTS]->(n:Network)"
            });
        }

        public RelationshipQuery this[AllRelationshipTypes key]
        {
            get { return _innerDictionary[key]; }
        }

        public bool DoesRelationshipHaveWeight(AllRelationshipTypes key)
        {
            return this[key].RelationshipDefinitionWithoutParam != null;
        }
    }
}
