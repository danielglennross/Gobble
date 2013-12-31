using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Constraints;

namespace DocumentMatch.Entities
{
    public class FacebookAccount
    {
        public string UserId { get; set; }
        public string OAuthToken { get; set; }
    }

    public class GobbleAccount
    {
        public string UserLogon { get; set; }
        public string UserPassword { get; set; }  
    }

    public class Account
    {
        public GobbleAccount GobbleAccount { get; set; }
        public FacebookAccount FacebookAccount { get; set; }   
    }

    public class QuestionAnswer
    {
        public QuestionUserRelationship QuestionUserRelationship { get; set; }
        public ObjectId Question_Id { get; set; }
        public int Weight { get; set; }
    }

    public class User : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsActive { get; set; }
        public string Email { get; set; }
        public List<string> School_Ids { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGender Gender { get; set; }
        public UserOrientation Orientation { get; set; }
        public List<string> Community_Ids { get; set; }
        public List<string> UserRole_Ids { get; set; }
        public List<Account> Accounts { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
