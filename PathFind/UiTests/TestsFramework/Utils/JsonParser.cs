using Newtonsoft.Json;

namespace TestsFramework.Utils
{
    internal class JsonParser<T>
    {
        internal static T Parse(string text) => JsonConvert.DeserializeObject<T>(text);
    }
}
