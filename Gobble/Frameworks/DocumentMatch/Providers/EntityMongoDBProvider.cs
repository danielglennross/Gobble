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
    public abstract class EntityMongoDBProvider<T> where T : Entity
    {
        private const string CONNECTION_STRING = "mongodb://localhost";
        private const string DATABASE = "gobble";

        protected MongoCollection collection;

        public EntityMongoDBProvider(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("param cannot be empty", "collectionName");

            MongoClient client = new MongoClient(CONNECTION_STRING);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase(DATABASE);

            collection = database.GetCollection<T>(collectionName);
        }

        public virtual string Insert(T t)
        {
            collection.Insert(t);
            return t.Id;
        }

        public virtual bool Update(T t)
        {
            collection.Save(t);
            return true;
        }

        public virtual T Get(string id)
        {
            return collection.AsQueryable<T>().SingleOrDefault(x => x.Id == id);
        }

        public virtual bool Delete(T t)
        {
            var query = Query<T>.EQ(e => e.Id, t.Id);
            collection.Remove(query);
            return true;
        }
    }
}
