using System.Text.Json;
using FluentAssertions.Execution;

namespace FluentAssertions.System.Text.Json
{
    public class JsonPropertyAssertions : JsonPropertyAssertions<JsonPropertyAssertions>
    {
        public JsonPropertyAssertions(JsonElement subjectValue, string subjectName)
            : base(subjectValue, subjectName)
        {
        }

        public JsonPropertyAssertions(JsonProperty subject)
            : base(subject)
        {
        }
    }

    public class JsonPropertyAssertions<TAssertions> where TAssertions : JsonPropertyAssertions<TAssertions>
    {
        protected JsonPropertyAssertions(JsonElement subjectValue, string subjectName)
        {
            SubjectValue = subjectValue;
            SubjectName = subjectName;
        }

        protected JsonPropertyAssertions(JsonProperty subject)
            : this(subject.Value, subject.Name)
        {

        }

        public JsonElement SubjectValue { get; }

        public string SubjectName { get; }

        public JsonPropertyAssertions<TAssertions> BeOfValueKind(JsonValueKind valueKind, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(SubjectValue.ValueKind == valueKind)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected element to have value kind '{0}'{reason}, but found '{1}'", valueKind, SubjectValue.ValueKind);

            return this;
        }

        public JsonPropertyAssertions<TAssertions> Be(JsonProperty expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(SubjectName == expected.Name)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property to have the name '{0}'{reason}, but found '{1}'", expected.Name, SubjectName);
            SubjectValue.Should().HaveEqualValue(expected.Value, because, becauseArgs);

            return this;
        }
    }
}
