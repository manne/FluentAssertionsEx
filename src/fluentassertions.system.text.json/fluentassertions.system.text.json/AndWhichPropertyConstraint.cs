using System.Text.Json;

namespace FluentAssertions.System.Text.Json
{
    public class AndWhichPropertyConstraint<TAssertion> : AndConstraint<TAssertion>
    {
        private readonly JsonElement _propertyValue;
        private readonly string _propertyName;

        public AndWhichPropertyConstraint(TAssertion parentConstraint, JsonElement propertyValue, string propertyName) : base(parentConstraint)
        {
            _propertyValue = propertyValue;
            _propertyName = propertyName;
        }

        public JsonPropertyAssertions Which => new (_propertyValue, _propertyName);
    }
}
