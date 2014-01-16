using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraints;

namespace GraphMatch.Entities
{
    public interface IBookMark
    {
        string DocumentBookMarkID { get; set; }
        bool IsActive { get; set; }
    }

    public class BookMark : Entity, IBookMark
    {
        public const string INDEX_NAME = "BookMark";

        public string DocumentBookMarkID { get; set; }
        public bool IsActive { get; set; }
        public BookMarkSource Source { get; set; }
    }
}
