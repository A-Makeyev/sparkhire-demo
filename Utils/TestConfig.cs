using System;
using dotenv.net;

namespace ComeetDemoTests.Config
{
    public static class EnvConfig
    {
        static EnvConfig()
        {
            // Load .env automatically
            DotEnv.Load();
        }

        public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.comeet.com";
        public static bool Headless => Environment.GetEnvironmentVariable("HEADLESS") == "true";
        public static string FirstName => Environment.GetEnvironmentVariable("FIRST_NAME") ?? "Anatoly";
        public static string LastName => Environment.GetEnvironmentVariable("LAST_NAME") ?? "Makeyev";
        public static string Company => Environment.GetEnvironmentVariable("COMPANY") ?? "Cloudbeat";
        public static string Email => Environment.GetEnvironmentVariable("EMAIL") ?? "anatoly.makeyev@cloudbeat.io";
    }
}
