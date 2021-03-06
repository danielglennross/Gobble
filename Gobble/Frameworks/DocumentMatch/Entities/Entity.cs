﻿using System;
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
using Common.Security;

namespace DocumentMatch.Entities
{
    // add all inheritors of entity here...
    [BsonKnownTypes(
        typeof(User), 
        typeof(UserRole),
        typeof(Attribute),
        typeof(BookMark),
        typeof(MessageThread),
        typeof(Network),
        typeof(Question),
        typeof(Community)
    )]
    public abstract class Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
