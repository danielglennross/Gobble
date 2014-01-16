using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentMatch.Entities
{
    public class Network : Entity
    {
        public string Name { get; set; }
        public List<string> SchoolIds { get; set; }
    }
}
