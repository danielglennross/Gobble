using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Providers;
using GraphMatch.Relationships;
using Constraints;

namespace GraphMatch.Repositories
{
    public class BookMarkRepository : EntityRepository<BookMark, BookMarkNeo4JProvider>
    {
        public BookMark CreateBookMark(BookMarkSource source)
        {
            BookMark b = new BookMark
            {
                DocumentBookMarkID = null,
                IsActive = true,
                Source = source
            };
            return b;
        }

        public List<BookMark> GetBookMarksForUser(User user, UserAttributeRelationships relationship)
        {
            return _provider.GetBookMarksForUser(user, relationship);
        }

        public void InitalizeProvider(Dictionary<string, BookMarkSource> documentBookMarkIDs)
        {
            List<BookMark> bookMarks = PopulateBookMarks(documentBookMarkIDs);
            foreach (BookMark bMark in bookMarks)
            {
                Insert(bMark);
            }
        }

        public override bool Insert(BookMark bookMark)
        {
            if (bookMark.DocumentBookMarkID == null)
                throw new InvalidOperationException("This BookMark does not exist");

            return base.Insert(bookMark);
        }

        public override bool Update(BookMark bookMark)
        {
            if (bookMark.DocumentBookMarkID == null)
                throw new InvalidOperationException("This BookMark does not exist");

            return base.Update(bookMark);
        }

        public List<BookMark> PopulateBookMarks(Dictionary<string, BookMarkSource> documentBookMarkIDs)
        {
            return documentBookMarkIDs.Select(x => new BookMark() { DocumentBookMarkID = x.Key, Source = x.Value, IsActive = true }).ToList();
        }
    }
}
