using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraints;

namespace DocumentMatch.Entities
{
    public class BookMark : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BookMarkSource Source { get; set; }
    }
}
