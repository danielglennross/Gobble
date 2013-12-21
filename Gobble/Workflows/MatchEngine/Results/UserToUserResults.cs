using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchEngine.Results
{
    public class UserToUserResults
    {
        public string DocumentNetworkID { get; set; }

        public string SearchUserDocumentUserID { get; set; }
        public string SearchUserDocumentSchoolID { get; set; }

        public string MatchUserDocumentUserID { get; set; }
        public string MatchUserDocumentSchoolID { get; set; }

        public List<string> LikesDocumentAttributeID { get; set; }
        public int LikesNumberOfMatchingAttributes { get; set; }
        public double LikesAverageVarience { get; set; }
        public double LikesMatchRatio { get; set; }

        public List<string> DislikesDocumentAttributeID { get; set; }
        public int DislikesNumberOfMatchingAttributes { get; set; }
        public double DislikesAverageVarience { get; set; }
        public double DislikesMatchRatio { get; set; }

        public double MatchRatio { get; set; }
    }
}
