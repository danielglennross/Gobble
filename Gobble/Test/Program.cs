using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Neo4jClient;
using Neo4jClient.Cypher;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };
            //var attr = new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Funny" };
            //GraphMatch.Repositories.UserToAttributeRepository r = new GraphMatch.Repositories.UserToAttributeRepository();
            //r.InsertRelationship(GraphMatch.Relationships.UserRelationships.UserDislikesAttribute, attr, user, 5);

            //InsertAllTest();
            //UpdateUserTest();
            //InsertAttributeThenDeleteTest();
            //UpdateAttributeRelationship();
            //InsertRelationshipThenDelete();
            //CreateAnotherNetworkOfResults();
            //MatchEngineTest();
            //GetAttributesForUser();
            InsertUserToMongo();
        }

        public static void InsertUserToMongo()
        {
            DocumentMatch.Entities.Account account1; 
            DocumentMatch.Entities.Account account2; 
            account1 = new DocumentMatch.Entities.FacebookAccount() { OAuthToken = "testOAuth", UserId = "testUserID", Type = DocumentMatch.Entities.Account.AccountType.Facebook };
            account2 = new DocumentMatch.Entities.GobbleAccount() {UserLogon = "testLogon", UserPassword = "testPassword", Type = DocumentMatch.Entities.Account.AccountType.Gobble };

            DocumentMatch.Repositories.UserRepository uRepo = new DocumentMatch.Repositories.UserRepository();
            var user = uRepo.CreateUser(new List<DocumentMatch.Entities.Account> { account1, account2 }, new List<string>() { "testRoles" }, new List<string>() { "testSchools" });

            DocumentMatch.Entities.GobbleAccount pass = (DocumentMatch.Entities.GobbleAccount)user.Accounts[1];
            string password = pass.UserPassword;
            string decryptPassword = pass.GetDecryptedPassword();

            string id = uRepo.Insert(user);

            user.LastName = "Ross";
            uRepo.Update(user);
            var newUser = uRepo.Get(id);
            uRepo.Delete(user);
        }

        public static void GetAttributesForUser()
        {
            var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };
            GraphMatch.Repositories.AttributeRepository aRepo = new GraphMatch.Repositories.AttributeRepository();
            var attributes = aRepo.GetAttributesForUser(user, GraphMatch.Relationships.UserRelationships.UserHasAttribute);
        }

        public static void MatchEngineTest()
        {
            var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };
            MatchEngine.MatchMakers.UserMatches userMatches = new MatchEngine.MatchMakers.UserMatches();
            var results = userMatches.GetGeneralUserMatches(user);

            user = new GraphMatch.Entities.User() { DocumentUserID = "Graeme" };
            results = userMatches.GetGeneralUserMatches(user);
        }

        public static void UpdateUserTest()
        {
            var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };
            user.DateOfBirth = 19890528;

            GraphMatch.Repositories.UserRepository userRepo = new GraphMatch.Repositories.UserRepository();
            userRepo.Update(user);
        }

        public static void InsertAttributeThenDeleteTest()
        {
            var attr = new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sad" };

            GraphMatch.Repositories.AttributeRepository attrRepo = new GraphMatch.Repositories.AttributeRepository();
            attrRepo.Insert(attr);
            attrRepo.Delete(attr);
        }

        public static void UpdateAttributeRelationship()
        {
            var attr = new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sweet" };
            var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };

            GraphMatch.Repositories.UserToAttributeRepository uToARepo = new GraphMatch.Repositories.UserToAttributeRepository();
            uToARepo.Update(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attr, user, 10);
        }

        public static void InsertRelationshipThenDelete()
        {
            var attr = new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Pleasant" };
            var user = new GraphMatch.Entities.User() { DocumentUserID = "Daniel" };

            GraphMatch.Repositories.UserToAttributeRepository uToARepo = new GraphMatch.Repositories.UserToAttributeRepository();
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attr, user, 1);
            uToARepo.Delete(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attr, user);
        }

        public static void CreateAnotherNetworkOfResults()
        {
            List<string> networkDocIDs = new List<string>() { "Leeds" };
            List<GraphMatch.Entities.Network> networks = new List<GraphMatch.Entities.Network>();
            networks.Add(new GraphMatch.Entities.Network() { DocumentNetworkID = "Leeds" });

            List<string> schoolDocIDs = new List<string>() { "LeedsMet", };
            List<GraphMatch.Entities.School> schools = new List<GraphMatch.Entities.School>();
            schools.Add(new GraphMatch.Entities.School() { DocumentSchoolID = "LeedsMet" });

            GraphMatch.Repositories.SchoolRepository schoolRepo = new GraphMatch.Repositories.SchoolRepository();
            GraphMatch.Repositories.NetworkRepository networkRepo = new GraphMatch.Repositories.NetworkRepository();
            GraphMatch.Repositories.UserRepository userRepo = new GraphMatch.Repositories.UserRepository();

            GraphMatch.Repositories.UserToAttributeRepository uToARepo = new GraphMatch.Repositories.UserToAttributeRepository();
            GraphMatch.Repositories.UserToSchoolRepository uToSRepo = new GraphMatch.Repositories.UserToSchoolRepository();
            GraphMatch.Repositories.UserToCommunityRepository uToCRepo = new GraphMatch.Repositories.UserToCommunityRepository();
            GraphMatch.Repositories.SchoolToNetworkRepository sToNRepo = new GraphMatch.Repositories.SchoolToNetworkRepository();

            networkRepo.InitalizeProvider(networkDocIDs);
            schoolRepo.InitalizeProvider(schoolDocIDs);

            GraphMatch.Entities.User user1 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Bill",
                DateOfBirth = Int32.Parse(new DateTime(1989, 5, 26).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Male,
                Orientation = Constraints.UserOrientation.Straight
            };

            GraphMatch.Entities.User user2 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Cal",
                DateOfBirth = Int32.Parse(new DateTime(1989, 1, 10).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Male,
                Orientation = Constraints.UserOrientation.Straight
            };

            GraphMatch.Entities.User user3 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Jenny",
                DateOfBirth = Int32.Parse(new DateTime(1989, 9, 1).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Female,
                Orientation = Constraints.UserOrientation.Straight
            };

            userRepo.Insert(user1);
            userRepo.Insert(user2);
            userRepo.Insert(user3);

            // school to networks
            sToNRepo.Insert(GraphMatch.Relationships.NetworkRelationships.SchoolExistsInNetwork, networks[0], schools[0]);

            // users to schools
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user1);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user2);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user3);

            // users to attributes
            List<string> attributeDocIDs = new List<string>() { "Sweet", "Funny", "Sarcastic", "Cheap", "Pleasant" };
            List<GraphMatch.Entities.Attribute> attributes = new List<GraphMatch.Entities.Attribute>();
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sweet" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Funny" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sarcastic" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Cheap" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Pleasant" });

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[1], user1, 3);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user1, 4);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[0], user1, 5);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[4], user1, 5);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user2, 7);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[3], user2, 2);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[0], user2, 8);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[3], user2, 9);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[1], user3, 10);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[4], user3, 1);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[2], user3, 4);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[1], user3, 5);
        }

        public static void InsertAllTest()
        {
            List<string> networkDocIDs = new List<string>() { "NewcastleUponTyne" };
            List<GraphMatch.Entities.Network> networks = new List<GraphMatch.Entities.Network>();
            networks.Add(new GraphMatch.Entities.Network() { DocumentNetworkID = "NewcastleUponTyne" });

            List<string> schoolDocIDs = new List<string>() { "Newcastle", "Northumbria" };
            List<GraphMatch.Entities.School> schools = new List<GraphMatch.Entities.School>();
            schools.Add(new GraphMatch.Entities.School() { DocumentSchoolID = "Newcastle" });
            schools.Add(new GraphMatch.Entities.School() { DocumentSchoolID = "Northumbria" });

            List<string> attributeDocIDs = new List<string>() { "Sweet", "Funny", "Sarcastic", "Cheap", "Pleasant" };
            List<GraphMatch.Entities.Attribute> attributes = new List<GraphMatch.Entities.Attribute>();
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sweet" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Funny" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Sarcastic" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Cheap" });
            attributes.Add(new GraphMatch.Entities.Attribute() { DocumentAttributeID = "Pleasant" });

            GraphMatch.Repositories.SchoolRepository schoolRepo = new GraphMatch.Repositories.SchoolRepository();
            GraphMatch.Repositories.NetworkRepository networkRepo = new GraphMatch.Repositories.NetworkRepository();
            GraphMatch.Repositories.AttributeRepository attributeRepo = new GraphMatch.Repositories.AttributeRepository();
            GraphMatch.Repositories.UserRepository userRepo = new GraphMatch.Repositories.UserRepository();

            GraphMatch.Repositories.UserToAttributeRepository uToARepo = new GraphMatch.Repositories.UserToAttributeRepository();
            GraphMatch.Repositories.UserToSchoolRepository uToSRepo = new GraphMatch.Repositories.UserToSchoolRepository();
            GraphMatch.Repositories.UserToCommunityRepository uToCRepo = new GraphMatch.Repositories.UserToCommunityRepository();
            GraphMatch.Repositories.SchoolToNetworkRepository sToNRepo = new GraphMatch.Repositories.SchoolToNetworkRepository();

            networkRepo.InitalizeProvider(networkDocIDs);
            schoolRepo.InitalizeProvider(schoolDocIDs);
            attributeRepo.InitalizeProvider(attributeDocIDs);

            GraphMatch.Entities.User user1 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Daniel",
                DateOfBirth = Int32.Parse(new DateTime(1989, 5, 26).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Male,
                Orientation = Constraints.UserOrientation.Straight
            };

            GraphMatch.Entities.User user2 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Graeme",
                DateOfBirth = Int32.Parse(new DateTime(1989, 1, 10).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Male,
                Orientation = Constraints.UserOrientation.Straight
            };

            GraphMatch.Entities.User user3 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Katie",
                DateOfBirth = Int32.Parse(new DateTime(1989, 9, 1).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Female,
                Orientation = Constraints.UserOrientation.Straight
            };

            GraphMatch.Entities.User user4 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Ben",
                DateOfBirth = Int32.Parse(new DateTime(1989, 8, 15).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Male,
                Orientation = Constraints.UserOrientation.Gay
            };

            GraphMatch.Entities.User user5 = new GraphMatch.Entities.User
            {
                DocumentUserID = "Cathleen",
                DateOfBirth = Int32.Parse(new DateTime(1989, 3, 20).ToString("yyyyMMdd")),
                Gender = Constraints.UserGender.Female,
                Orientation = Constraints.UserOrientation.Straight
            };

            userRepo.Insert(user1);
            userRepo.Insert(user2);
            userRepo.Insert(user3);
            userRepo.Insert(user4);
            userRepo.Insert(user5);

            // school to networks
            sToNRepo.Insert(GraphMatch.Relationships.NetworkRelationships.SchoolExistsInNetwork, networks[0], schools[0]);
            sToNRepo.Insert(GraphMatch.Relationships.NetworkRelationships.SchoolExistsInNetwork, networks[0], schools[1]);

            // users to schools
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user1);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user2);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[0], user3);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[1], user4);
            uToSRepo.Insert(GraphMatch.Relationships.SchoolRelationships.UserAttendsSchool, schools[1], user5);

            // users to attributes
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[0], user1, 3);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[1], user1, 4);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[2], user1, 5);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[3], user1, 5);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[3], user2, 7);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user2, 2);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[1], user2, 8);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[4], user2, 9);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user3, 10);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[3], user3, 1);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[4], user3, 4);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[0], user3, 5);
            
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[4], user4, 3);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user4, 3);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[1], user4, 7);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[3], user4, 2);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[1], user5, 1);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserHasAttribute, attributes[2], user5, 9);

            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[3], user5, 8);
            uToARepo.Insert(GraphMatch.Relationships.UserRelationships.UserLikesAttribute, attributes[4], user5, 8);
        }
    }
}
