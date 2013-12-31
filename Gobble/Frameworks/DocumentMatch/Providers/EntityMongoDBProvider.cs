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

        public abstract string Insert(T user);

        public abstract bool Update(T user);

        public abstract T Get(string id);

        public abstract bool Delete(T user);
    }
}
