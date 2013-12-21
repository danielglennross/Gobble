using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Providers;

namespace GraphMatch.Repositories
{
    public class SchoolRepository : EntityRepository<School, SchoolNeo4JProvider>
    {
        public School CreateSchool()
        {
            School s = new School
            {
                DocumentSchoolID = null,
                IsActive = true
            };
            return s;
        }

        public void InitalizeProvider(List<string> documentNetworkIDs)
        {
            List<School> Schools = PopulateSchools(documentNetworkIDs);
            foreach (School s in Schools)
            {
                Insert(s);
            }
        }

        public override bool Insert(School School)
        {
            if (School.DocumentSchoolID == null)
                throw new InvalidOperationException("This School does not exist");

            return base.Insert(School);
        }

        public override bool Update(School School)
        {
            if (School.DocumentSchoolID == null)
                throw new InvalidOperationException("This School does not exist");

            return base.Update(School);
        }

        public List<School> PopulateSchools(List<string> documentSchoolIDs)
        {
            return documentSchoolIDs.Select(x => new School() { DocumentSchoolID = x, IsActive = true }).ToList();
        }
    }
}
