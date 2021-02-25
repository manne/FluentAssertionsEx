using System.Text.Json;

namespace FluentAssertions.System.Text.Json
{
    public static class JsonPropertyExtensions
    {
        public static JsonPropertyAssertions Should(this JsonProperty instance) => new(instance);
    }
}
