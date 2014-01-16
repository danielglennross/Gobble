namespace Constraints
{
    public enum QuestionUserRelationship
    {
        Has, Likes, Dislikes
    }

    public enum UserOrientation
    {
        Straight,
        Gay,
        Bisexual,
        NotSet
    }

    public enum UserGender
    {
        Male,
        Female,
        NotSet
    }

    public enum BookMarkSource
    {
        Gobble,
        Facebook
    }

    public enum CommunityType
    {
        Course,
        Society,
        Social,
        NotSet
    }

    public enum Cities
    {
        Newcastle,
        Durham,
        Sunderland
    }

    public static class SchoolEmailPatterns
    {
        public const string NewcastleUniversity = "@ncl.ac.uk";
        public const string NorthumbriaUniversity = "";
        public const string SunderlandUniversity = "";
    }
}