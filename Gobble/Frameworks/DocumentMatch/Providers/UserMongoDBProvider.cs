﻿using System;
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
    public class UserMongoDBProvider : EntityMongoDBProvider<User>
    {
        public const string COLLECTION_NAME = "user";

        public UserMongoDBProvider() 
            : base(COLLECTION_NAME)
        {
            
        }
    }
}
