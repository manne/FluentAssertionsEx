using System.Text.Json;

namespace FluentAssertions.System.Text.Json
{
    public static class JsonElementExtensions
    {
        public static JsonElementAssertions Should(this JsonElement instance) => new(instance);
    }
}
