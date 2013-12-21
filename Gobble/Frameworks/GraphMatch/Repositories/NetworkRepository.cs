using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Providers;
using GraphMatch.Entities;

namespace GraphMatch.Repositories
{
    public class NetworkRepository : EntityRepository<Network, NetworkNeo4JProvider>
    {
        public Network CreateNetwork()
        {
            Network a = new Network
            {
                DocumentNetworkID = null,
                IsActive = true
            };
            return a;
        }

        public void InitalizeProvider(List<string> documentNetworkIDs)
        {
            List<Network> Networks = PopulateNetworks(documentNetworkIDs);
            foreach (Network n in Networks)
            {
                Insert(n);
            }
        }

        public override bool Insert(Network Network)
        {
            if (Network.DocumentNetworkID == null)
                throw new InvalidOperationException("This Network does not exist");

            return base.Insert(Network);
        }

        public override bool Update(Network Network)
        {
            if (Network.DocumentNetworkID == null)
                throw new InvalidOperationException("This Network does not exist");

            return base.Update(Network);
        }

        public List<Network> PopulateNetworks(List<string> documentNetworkIDs)
        {
            return documentNetworkIDs.Select(x => new Network() { DocumentNetworkID = x, IsActive = true }).ToList();
        }
    }
}
