using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Providers;

namespace GraphMatch.Repositories
{
    public class CommunityRepository : EntityRepository<Community, CommunityNeo4JProvider>
    {
        public Community CreateSchool()
        {
            Community c = new Community
            {
                DocumentCommunityID = null,
                CommunityType = Constraints.CommunityType.NotSet,
                IsActive = true
            };
            return c;
        }

        public void InitalizeProvider(Dictionary<string, Constraints.CommunityType> documentCommunityIDsAndTypes)
        {
            List<Community> Communities = PopulateCommunity(documentCommunityIDsAndTypes);
            foreach (Community c in Communities)
            {
                Insert(c);
            }
        }

        private bool IsCommunityValid(Community community, out string error)
        {
            error = "";
            if (community.CommunityType == Constraints.CommunityType.NotSet)
                error += "CommunityType Not Set";
            
            return error == "";
        }

        public override bool Insert(Community Community)
        {
            if (Community.DocumentCommunityID == null)
                throw new InvalidOperationException("This Community does not exist");

            string error;
            if (!IsCommunityValid(Community, out error))
                throw new InvalidOperationException(error);

            return base.Insert(Community);
        }

        public override bool Update(Community Community)
        {
            if (Community.DocumentCommunityID == null)
                throw new InvalidOperationException("This Community does not exist");

            string error;
            if (!IsCommunityValid(Community, out error))
                throw new InvalidOperationException(error);

            return base.Update(Community);
        }

        public List<Community> PopulateCommunity(Dictionary<string, Constraints.CommunityType> documentCommunityIDsAndTypes)
        {
            return documentCommunityIDsAndTypes.Select(x => new Community() 
            { 
                DocumentCommunityID = x.Key,
                CommunityType = x.Value,
                IsActive = true
            }).ToList();
        }
    }
}
