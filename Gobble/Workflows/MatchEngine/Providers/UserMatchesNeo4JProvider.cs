using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using GraphMatch.Entities;
using Constraints;
using MatchEngine.Results;
using GraphMatch.Providers;

namespace MatchEngine.Providers
{
	public class UserMatchesNeo4JProvider
	{
		private const string CONNECTION_STRING = "http://localhost:7474/db/data";

		private const string SEARCH_USER_PARM = "MeDocumentUserID";
		private const double ZERO_APPROXIMATION = 0.1;

		protected GraphClientWrapper _graphClient;

		public UserMatchesNeo4JProvider()
		{
			_graphClient = new GraphClientWrapper(new Uri(CONNECTION_STRING));
			_graphClient.Connect();
		}

		private string GetNetworkUserGenderAndOrientationPredicate()
		{
			string matchOnGendersAndOrientation =
			  "me.DocumentUserID = {" + SEARCH_USER_PARM + "} and " +
			  "(sch-[:EXISTS]->n and theirsch-[:EXISTS]->n) and " +
			  "( " +
					"(me.Orientation = \"Straight\" AND person.Orientation IN [\"Bisexual\", \"Straight\"] AND me.Gender <> person.Gender) " +
				 "or (me.Orientation = \"Gay\" AND person.Orientation IN [\"Bisexual\", \"Gay\"] AND me.Gender = person.Gender) " +
				 "or (me.Orientation = \"Bisexual\" " +
					 "and ( " +
							"(person.Orientation IN [\"Bisexual\", \"Straight\"] AND me.Gender <> person.Gender) " +
						 "or (person.Orientation IN [\"Bisexual\", \"Gay\"] AND me.Gender = person.Gender) " +
						 ") " +
					 ") " +
			   ")";
			return matchOnGendersAndOrientation;
		}

		public List<UserToUserResults> GetGeneralUserMatches(User user, int? limit = null, int? skip = null)
		{
			string aggregationAndResults =
				"me, person, r1, r2, sch, theirsch, n, " +
				"count(attributes.DocumentAttributeID) as num_attributes, " +
				"COLLECT(attributes.DocumentAttributeID) as attribute_names, " +
				// if percent diff is 0, change to 0.001 for future divide by calcs
				"COALESCE(avg(abs(COALESCE(l1.Weight - h1.Weight, 0)) / (l1.Weight + h1.Weight) / 2), " + ZERO_APPROXIMATION + ") as avg_percent_diff_as_dec";

			var resutls = _graphClient.Cypher
				.Match("(me:User)-[r1:ATTENDS]->(sch:School)", "(person:User)-[r2:ATTENDS]->(theirsch:School)", "(n:Network)")
				.Where(GetNetworkUserGenderAndOrientationPredicate())
				.WithParam(SEARCH_USER_PARM, user.DocumentUserID)
				.With("me, person, r1, r2, sch, theirsch, n")
				.OptionalMatch("me-[l1:LIKES]->attributes<-[h1:HAS]-person")
				.With(aggregationAndResults)
				.Return(() => new UserToUserResults
				{
					DocumentNetworkID = Return.As<string>("n.DocumentNetworkID"),
					SearchUserDocumentUserID = Return.As<string>("me.DocumentUserID"),
					SearchUserDocumentSchoolID = Return.As<string>("sch.DocumentSchoolID"),
					MatchUserDocumentUserID = Return.As<string>("person.DocumentUserID"),
					MatchUserDocumentSchoolID = Return.As<string>("theirsch.DocumentSchoolID"),
					LikesDocumentAttributeID = Return.As<List<string>>("attribute_names"),
					LikesNumberOfMatchingAttributes = Return.As<int>("num_attributes"),
					LikesAverageVarience = Return.As<double>("avg_percent_diff_as_dec"),
					LikesMatchRatio = Return.As<double>("case when avg_percent_diff_as_dec = 0 then (num_attributes / " + ZERO_APPROXIMATION + ") else (num_attributes / avg_percent_diff_as_dec) end")
				})
				.OrderByDescending(new string[] { "MatchRatio" })
				.Skip(skip)
				.Limit(limit)
				.Results
				.ToList();

			return resutls;
		}

		public List<UserToUserResults> GetGeneralUserMatches2(User user, int? limit = null, int? skip = null)
		{
			string aggregationAndResults =
				"me, person, r1, r2, sch, theirsch, n, " +
				"count(attributes.DocumentAttributeID) as num_attributes, " +
				"COLLECT(attributes.DocumentAttributeID) as attribute_names, " +
				// if percent diff is 0, change to 0.001 for future divide by calcs
				"COALESCE(avg(abs(COALESCE(l1.Weight - h1.Weight, 0)) / (l1.Weight + h1.Weight) / 2), " + ZERO_APPROXIMATION + ") as avg_percent_diff_as_dec";

			var resutls = _graphClient.Cypher
				.Match("(me:User)-[r1:ATTENDS]->(sch:School)", "(person:User)-[r2:ATTENDS]->(theirsch:School)", "(n:Network)")
				.Where("(sch-[:EXISTS]->n and theirsch-[:EXISTS]->n)")
				.AndWhere((User me, User person) =>
					me.DocumentUserID == user.DocumentUserID &&
				   (
					  (me.Orientation.ToString() == UserOrientation.Straight.ToString() && (person.Orientation.ToString() == UserOrientation.Straight.ToString() || person.Orientation.ToString() == UserOrientation.Bisexual.ToString()) && me.Gender.ToString() != person.Gender.ToString())
				   || (me.Orientation.ToString() == UserOrientation.Gay.ToString() && (person.Orientation.ToString() == UserOrientation.Gay.ToString() || person.Orientation.ToString() == UserOrientation.Bisexual.ToString()) && me.Gender.ToString() == person.Gender.ToString())
				   || (me.Orientation.ToString() == UserOrientation.Bisexual.ToString() && 
						   (((person.Orientation.ToString() == UserOrientation.Bisexual.ToString() || person.Orientation.ToString() == UserOrientation.Straight.ToString()) && me.Gender.ToString() != person.Gender.ToString())
						|| ((person.Orientation.ToString() == UserOrientation.Bisexual.ToString() || person.Orientation.ToString() == UserOrientation.Gay.ToString()) && me.Gender.ToString() == person.Gender.ToString())))
					)
				 )
				.With("me, person, r1, r2, sch, theirsch, n")
				.OptionalMatch("me-[l1:LIKES]->attributes<-[h1:HAS]-person")
				.With(aggregationAndResults)
				.Return(() => new UserToUserResults
				{
					DocumentNetworkID = Return.As<string>("n.DocumentNetworkID"),
					SearchUserDocumentUserID = Return.As<string>("me.DocumentUserID"),
					SearchUserDocumentSchoolID = Return.As<string>("sch.DocumentSchoolID"),
					MatchUserDocumentUserID = Return.As<string>("person.DocumentUserID"),
					MatchUserDocumentSchoolID = Return.As<string>("theirsch.DocumentSchoolID"),
					LikesDocumentAttributeID = Return.As<List<string>>("attribute_names"),
					LikesNumberOfMatchingAttributes = Return.As<int>("num_attributes"),
					LikesAverageVarience = Return.As<double>("avg_percent_diff_as_dec"),
					LikesMatchRatio = Return.As<double>("case when avg_percent_diff_as_dec = 0 then (num_attributes / " + ZERO_APPROXIMATION + ") else (num_attributes / avg_percent_diff_as_dec) end")
				})
				.OrderByDescending(new string[] { "MatchRatio" })
				.Skip(skip)
				.Limit(limit)
				.Results
				.ToList();

			return resutls;
		}

		public List<UserToUserResults> GetGeneralUserMatchesWithDislikes(User user, int? limit = null, int? skip = null)
		{
			string aggregationAndResults =
				"me, person, r1, r2, sch, theirsch, n, " +

				"COUNT(attributesLikes.DocumentAttributeID) as num_attribute_likes, " +
				"COUNT(attributesDislikes.DocumentAttributeID) as num_attribute_dislikes, " +

				"COLLECT(attributesLikes.DocumentAttributeID) as attribute_names_likes, " +
				"COLLECT(attributesDislikes.DocumentAttributeID) as attribute_names_dislikes, " +
				
				// if percent diff is 0, change to 0.001 for future divide by calcs
				"COALESCE(avg(abs(COALESCE(l1.Weight - h1.Weight, 0)) / (l1.Weight + h1.Weight) / 2) * 100, " + ZERO_APPROXIMATION + ") as avg_percent_diff_as_dec_likes, " +
				"COALESCE(avg(abs(COALESCE(d1.Weight - h2.Weight, 0)) / (d1.Weight + h2.Weight) / 2) * 100, " + ZERO_APPROXIMATION + ") as avg_percent_diff_as_dec_dislikes";

			var resutls = _graphClient.Cypher
				.Match("(me:User)-[r1:ATTENDS]->(sch:School)", "(person:User)-[r2:ATTENDS]->(theirsch:School)", "(n:Network)")
				.Where("(sch-[:EXISTS]->n and theirsch-[:EXISTS]->n)")
				.AndWhere((User me, User person) =>
					me.DocumentUserID == user.DocumentUserID &&
				   (
					  (me.Orientation.ToString() == UserOrientation.Straight.ToString() && (person.Orientation.ToString() == UserOrientation.Straight.ToString() || person.Orientation.ToString() == UserOrientation.Bisexual.ToString()) && me.Gender.ToString() != person.Gender.ToString())
				   || (me.Orientation.ToString() == UserOrientation.Gay.ToString() && (person.Orientation.ToString() == UserOrientation.Gay.ToString() || person.Orientation.ToString() == UserOrientation.Bisexual.ToString()) && me.Gender.ToString() == person.Gender.ToString())
				   || (me.Orientation.ToString() == UserOrientation.Bisexual.ToString() &&
						   (((person.Orientation.ToString() == UserOrientation.Bisexual.ToString() || person.Orientation.ToString() == UserOrientation.Straight.ToString()) && me.Gender.ToString() != person.Gender.ToString())
						|| ((person.Orientation.ToString() == UserOrientation.Bisexual.ToString() || person.Orientation.ToString() == UserOrientation.Gay.ToString()) && me.Gender.ToString() == person.Gender.ToString())))
				   )
				 )
				.With("me, person, r1, r2, sch, theirsch, n")
				.OptionalMatch("me-[l1:LIKES]->attributesLikes<-[h1:HAS]-person")
				.OptionalMatch("me-[d1:DISLIKES]->attributesDislikes<-[h2:HAS]-person")
				.With(aggregationAndResults)
				.Return(() => new UserToUserResults
				{
					DocumentNetworkID = Return.As<string>("n.DocumentNetworkID"),
					SearchUserDocumentUserID = Return.As<string>("me.DocumentUserID"),
					SearchUserDocumentSchoolID = Return.As<string>("sch.DocumentSchoolID"),
					MatchUserDocumentUserID = Return.As<string>("person.DocumentUserID"),
					MatchUserDocumentSchoolID = Return.As<string>("theirsch.DocumentSchoolID"),
					
					LikesDocumentAttributeID = Return.As<List<string>>("attribute_names_likes"),
					LikesNumberOfMatchingAttributes = Return.As<int>("num_attribute_likes"),
					LikesAverageVarience = Return.As<double>("avg_percent_diff_as_dec_likes"),
					LikesMatchRatio = Return.As<double>("case when avg_percent_diff_as_dec_likes = 0 then (num_attribute_likes / " + ZERO_APPROXIMATION + ") else (num_attribute_likes / avg_percent_diff_as_dec_likes) end"),

					DislikesDocumentAttributeID = Return.As<List<string>>("attribute_names_dislikes"),
					DislikesNumberOfMatchingAttributes = Return.As<int>("num_attribute_dislikes"),
					DislikesAverageVarience = Return.As<double>("avg_percent_diff_as_dec_dislikes"),
					DislikesMatchRatio = Return.As<double>("case when avg_percent_diff_as_dec_dislikes = 0 then (num_attribute_dislikes / " + ZERO_APPROXIMATION + ") else (num_attribute_dislikes / avg_percent_diff_as_dec_dislikes) end"),

					MatchRatio = Return.As<double>("case when avg_percent_diff_as_dec_likes = 0 then (num_attribute_likes / " + ZERO_APPROXIMATION + ") else (num_attribute_likes / avg_percent_diff_as_dec_likes) end - case when avg_percent_diff_as_dec_dislikes = 0 then (num_attribute_dislikes / " + ZERO_APPROXIMATION + ") else (num_attribute_dislikes / avg_percent_diff_as_dec_dislikes) end")
				})
				.OrderByDescending(new string[] { "MatchRatio" })
				.Skip(skip)
				.Limit(limit)
				.Results
				.ToList();

			return resutls;
		}
	}
}
