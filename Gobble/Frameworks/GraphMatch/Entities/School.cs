using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMatch.Entities
{
    public interface ISchool
    {
        string DocumentSchoolID { get; set; }
    }
    
    public class School : Entity, ISchool
    {
        public string DocumentSchoolID { get; set; }
        public bool IsActive { get; set; }
    }
}
