using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraints;
using Neo4jClient;

namespace GraphMatch.Entities
{
    public interface IUser
    {
        string DocumentUserID { get; set; }
        UserOrientation Orientation { get; set; }
        int DateOfBirth { get; set; }
        UserGender Gender { get; set; }
        bool IsActive { get; set; }
    }

    public class User : Entity, IUser
    {
        public const string INDEX_NAME = "User";

        public string DocumentUserID { get; set; }
        public UserOrientation Orientation { get; set; }
        public int DateOfBirth { get; set; }
        public UserGender Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
