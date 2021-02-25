using System;
using System.Linq;
using System.Text.Json;
using FluentAssertions.Execution;

namespace FluentAssertions.System.Text.Json
{
    public class JsonElementAssertions : JsonElementAssertions<JsonElementAssertions>
    {
        public JsonElementAssertions(JsonElement subject)
            : base(subject)
        {
        }
    }

    public class JsonElementAssertions<TAssertions> where TAssertions : JsonElementAssertions<TAssertions>
    {
        public JsonElementAssertions(JsonElement subject)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public JsonElement Subject { get; }

        public AndWhichPropertyConstraint<TAssertions> HaveProperty(string propertyName, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.TryGetProperty(propertyName, out var value))
                .BecauseOf(because, becauseArgs)
                .FailWith($"Expected element to have property with name '{propertyName}, but did not find it.");

            return new AndWhichPropertyConstraint<TAssertions>((TAssertions)this, value, propertyName);
        }

        public AndConstraint<TAssertions> HaveValueKind(JsonValueKind valueKind, string because = "", params object[] becauseArgs)
        {
            StartValueKindContinuation(valueKind, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        public AndConstraint<TAssertions> HaveEqualValue(JsonElement expected, string because = "", params object[] becauseArgs)
        {
            StartValueKindContinuation(expected.ValueKind, because, becauseArgs);

            switch (expected.ValueKind)
            {
                case JsonValueKind.String:
                {
                    var actualValue = Subject.GetString();
                    var expectedValue = expected.GetString();
                    actualValue.Should().Be(expectedValue, because, becauseArgs);
                    break;
                }
                case JsonValueKind.Number:
                {
                    var actualValue = Subject.GetDecimal();
                    var expectedValue = expected.GetDecimal();
                    actualValue.Should().Be(expectedValue, because, becauseArgs);
                    break;
                }
                case JsonValueKind.False:
                {
                    var actualValue = Subject.GetBoolean();
                    actualValue.Should().BeFalse(because, becauseArgs);
                    break;
                }
                case JsonValueKind.True:
                {
                    var actualValue = Subject.GetBoolean();
                    actualValue.Should().BeTrue(because, becauseArgs);
                    break;
                }
                case JsonValueKind.Object:
                    using (new AssertionScope())
                    {
                        using var objectEnumerator = expected.EnumerateObject();
                        foreach (var expectedProperty in objectEnumerator)
                        {
                            var name = expectedProperty.Name;
                            var subjectHasPropertyName = Execute.Assertion
                                .ForCondition(Subject.TryGetProperty(name, out var subjectProperty))
                                .FailWith($"Expected JSON element to have a property named '{name}'.");
                            if (subjectHasPropertyName.SourceSucceeded)
                            {
                                var jsonPropertyAssertions = new JsonPropertyAssertions(subjectProperty, name);
                                jsonPropertyAssertions.Be(expectedProperty);
                            }
                        }
                    }

                    break;
                case JsonValueKind.Array:
                    using (new AssertionScope())
                    {
                        using var expectedArray = expected.EnumerateArray();
                        using var subjectArray = Subject.EnumerateArray();

                        var objectPairs = expectedArray.Zip(subjectArray, (e, s) => (Expected: e, Subject: s));
                        foreach (var pair in objectPairs)
                        {

                            var elementAssertions = new JsonElementAssertions(pair.Subject);
                            elementAssertions.HaveEqualValue(pair.Expected);
                        }
                    }

                    break;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    // already verified by the value kind check
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The value kind {expected.ValueKind} is not supported.");
            }

            return new AndConstraint<TAssertions>((TAssertions) this);
        }
        
        private Continuation StartValueKindContinuation(JsonValueKind expected, string because = "", params object[] becauseArgs)
        => Execute.Assertion
            .ForCondition(Subject.ValueKind == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected element to have value kind '{0}'{reason}, but found '{1}'", expected, Subject.ValueKind);

        public AndWhichConstraint<TAssertions, string?> BeAString(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.ValueKind == JsonValueKind.String)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to be a string{reason}, but found '{0}'", Subject.ValueKind);

            var value = Subject.GetString();

            return new AndWhichConstraint<TAssertions, string?>((TAssertions)this, value);
        }
    }
}
