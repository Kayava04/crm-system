namespace CRMSystem.WebAPI.Auth
{
    public static class AuthorizationPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string UserOnly = "UserOnly";
        public const string UserOrAdmin = "UserOrAdmin";
    }
}