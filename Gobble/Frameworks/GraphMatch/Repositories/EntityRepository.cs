using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Providers;
using GraphMatch.Entities;

namespace GraphMatch.Repositories
{
    public abstract class EntityRepository<T, G> where T : Entity
                                                 where G : EntityNeo4JProvider<T>, new()
    {
        protected G _provider;

        public EntityRepository()
        {
            _provider = new G();
        }

        public virtual bool Insert(T t)
        {
            return _provider.Insert(t);
        }

        public virtual bool Update(T t)
        {
            return _provider.Update(t);
        }

        public virtual bool Delete(T t)
        {
            return _provider.Delete(t);
        }

        public virtual bool DeleteWithInboundRelationships(T t)
        {
            return _provider.DeleteWithInboundRelationships(t);
        }

        public virtual T Get(string documentNetworkID)
        {
            return (T)_provider.Get(documentNetworkID);
        }
    }
}
