namespace SimpleWebApp
{
    public enum Routes { Default, Index, Login, Info, NotFound, InvalidLogin };
    public static class Route
    {
        public static string Get(Routes target)
        {
            switch(target)
            {
                case Routes.Index: return "../index.jhtm";
                case Routes.Login: return "login.jhtm";
                case Routes.Info: return "info.jhtm";
                case Routes.NotFound: return "notFound.jhtm";
                case Routes.InvalidLogin: return "invalid.jhtm";
                default: return "default.jhtm";
            }
        }
    }
}