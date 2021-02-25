using System;
using System.Linq;
using System.Text.Json;

namespace FluentAssertions.System.Text.Json.UnitTests.Helpers
{
    internal static class JsonElementExtensions
    {
        public static JsonProperty GetTypedProperty(this JsonElement element, string propertyName)
        {
            using var objectEnumerator = element.EnumerateObject();
            foreach (var jsonProperty in objectEnumerator.Where(jsonProperty => jsonProperty.Name == propertyName))
            {
                return jsonProperty;
            }

            throw new InvalidOperationException($"The JSON element does not contain property named {propertyName}");
        }
    }
}
