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
    public class UserMongoDBProvider : EntityMongoDBProvider<User>
    {
        public UserMongoDBProvider() : base("user")
        {
            
        }

        public override string Insert(User user)
        {
            collection.Insert(user);
            return user.Id;
        }

        public override bool Update(User user)
        {
            collection.Save(user);
            return true;
        }

        public override User Get(string id)
        {
            return collection.AsQueryable<User>().SingleOrDefault(x => x.Id == id);
        }

        public override bool Delete(User user)
        {
            var query = Query<User>.EQ(e => e.Id, user.Id);
            collection.Remove(query);
            return true;
        }
    }
}
