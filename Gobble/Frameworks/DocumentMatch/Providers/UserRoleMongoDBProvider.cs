using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using DocumentMatch.Entities;

namespace DocumentMatch.Providers
{
    public class UserRoleMongoDBProvider : EntityMongoDBProvider<UserRole>
    {
        public const string COLLECTION_NAME = "userRole";

        public UserRoleMongoDBProvider()
            : base(COLLECTION_NAME)
        {

        }
    }
}
