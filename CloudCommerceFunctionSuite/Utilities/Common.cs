using Newtonsoft.Json;

namespace CloudCommerceFunctionSuite.Utilities
{
    public static class JsonHelper
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Formatting = Formatting.None // Change to Formatting.Indented for better readability in logs
        };
    }
}
