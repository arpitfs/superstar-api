namespace ApiWorld.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class SuperStar
        {
            public const string GetAll = Base + "/superstar";
            public const string Get = Base + "/superstar/{superstarId}";
            public const string Create = Base + "/superstar";
            public const string Delete = Base + "/{superstartId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }

        public static class Manager
        {
            public const string GetAll = Base + "/manager";
            public const string Delete = Base + "/manager/{managerId}";
        }
    }
}