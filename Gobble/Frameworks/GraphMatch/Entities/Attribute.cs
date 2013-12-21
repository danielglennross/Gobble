using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMatch.Entities
{
    public interface IAttribute
    {
        string DocumentAttributeID { get; set; }
        bool IsActive { get; set; }
    }

    public class Attribute : Entity, IAttribute
    {
        public const string INDEX_NAME = "Attribute";

        public string DocumentAttributeID { get; set; }
        public bool IsActive { get; set; }
    }
}
