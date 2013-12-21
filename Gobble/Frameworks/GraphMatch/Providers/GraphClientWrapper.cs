using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace GraphMatch.Providers
{
    public class GraphClientWrapper
    {
        private GraphClient _graphClient;

        public ICypherFluentQuery Cypher { get { return _graphClient.Cypher; } }

        public GraphClientWrapper(Uri connectionString)
        {
            _graphClient = new GraphClient(connectionString);
        }

        public void Connect()
        {
            int maxRetry = 3;
            for (int i = 0; i < maxRetry; i++)
            {
                try
                {
                    TryConnect();
                    break;
                }
                catch (TimeoutException)
                {
                    if (i < maxRetry - 1)
                        System.Threading.Thread.Sleep(1000);
                    else
                        throw;
                }
            }
        }

        private void TryConnect()
        {
            System.Threading.Thread t = new System.Threading.Thread(EstablishConnection);
            t.Start();
            if (!t.Join(TimeSpan.FromMilliseconds(5000)))
            {
                t.Abort();
                throw new TimeoutException("Taking too long");
            }
        }

        private void EstablishConnection()
        {
            try
            {
                _graphClient.Connect();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // log
            }
        }
    }
}
