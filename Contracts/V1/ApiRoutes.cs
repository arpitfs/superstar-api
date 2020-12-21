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
        }
    }
}