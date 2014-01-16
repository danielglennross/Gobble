﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphMatch.Entities;
using GraphMatch.Providers;
using GraphMatch.Relationships;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace GraphMatch.Repositories
{
    public class UserToSchoolRepository : RelationshipRepository<School, User, UserToSchool, UserToSchoolRelNeo4JProvider, UserSchoolRelationships>
    {
    }
}
