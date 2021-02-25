using System;
using System.Text.Json;
using FluentAssertions.System.Text.Json.UnitTests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.System.Text.Json.UnitTests
{
    public class JsonPropertyAssertionsSpecs
    {
        [Fact]
        public void ElementValueKindDoesNotHaveProperty4()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

            documentBase.RootElement.Should().HaveProperty("firstName").Which.BeOfValueKind(JsonValueKind.String);
        }

        [Fact]
        public void ElementValueKindDoesNotHaveProperty4f()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

            Action act = () => documentBase.RootElement.Should().HaveProperty("firstName").Which.BeOfValueKind(JsonValueKind.Number);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeEqual_ShouldNot_DifferentNames()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            using var expected = JsonDocument.Parse(@" { ""secondName"": ""Bobby"" }");

            var property = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("secondName");

            Action act = () => property.Should().Be(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeEqual()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": ""Bobby22"" }");

            var property = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("firstName");

            property.Should().Be(expectedProperty);
        }
    }
}
