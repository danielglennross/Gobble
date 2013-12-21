using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraints;

namespace GraphMatch.Entities
{
    public interface ICommunity
    {
        string DocumentCommunityID { get; set; }
        CommunityType CommunityType { get; set; }
    }

    public class Community : Entity, ICommunity
    {
        public string DocumentCommunityID { get; set; }
        public CommunityType CommunityType { get; set; }
        public bool IsActive { get; set; }
    }
}
