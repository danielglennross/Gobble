using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Relationships;
using GraphMatch.Providers;
using Constraints;

namespace GraphMatch.Repositories
{
    public class UserToBookMarkRepository : RelationshipRepository<BookMark, User, UserToBookMark, UserToBookMarkRelNeo4JProvider, UserBookMarkRelationships>
    {
    }
}
