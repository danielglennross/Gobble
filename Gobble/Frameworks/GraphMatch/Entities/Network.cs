using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMatch.Entities
{
    public interface INetwork
    {
        string DocumentNetworkID { get; set; }
        bool IsActive { get; set; }
    }

    public class Network : Entity, INetwork
    {
        public string DocumentNetworkID { get; set; }
        public bool IsActive { get; set; }
    }
}
