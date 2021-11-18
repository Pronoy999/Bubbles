namespace BubblesAPI.Exceptions
{
    public static class ErrorCodes
    {
        public const string BadRequest = "BadRequest";
        public const string FirstNameMissing = "missing_first_name";
        public const string LastNameMissing = "missing_last_name";
        public const string EmailMissing = "missing_email";
        public const string PasswordMissing = "missing_password";

        public const string DbNameMissing = "missing_db_name";
        public const string NodeIdMissing = "missing_node_id";
        public const string RelationshipTypeMissing = "missing_relationship_type";
        public const string GraphNameMissing = "missing_graph_name";
        public const string RelationshipIdMissing = "missing_relationship_id";
    }
}