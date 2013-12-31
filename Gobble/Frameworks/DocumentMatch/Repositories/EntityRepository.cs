using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentMatch.Entities;
using DocumentMatch.Providers;

namespace DocumentMatch.Repositories
{
    public class EntityRepository<T, G> where T : Entity
                                        where G : EntityMongoDBProvider<T>, new()
    {
        protected G _provider;

        public EntityRepository()
        {
            _provider = new G();
        }

        public virtual string Insert(T t)
        {
            return _provider.Insert(t);
        }

        public virtual T Get(string id)
        {
            return _provider.Get(id);
        }

        public virtual bool Update(T t)
        {
            return _provider.Update(t);
        }

        public virtual bool Delete(T t)
        {
            return _provider.Delete(t);
        }
    }
}
