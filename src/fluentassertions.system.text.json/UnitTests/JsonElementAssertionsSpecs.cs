using System;
using System.Text.Json;
using FluentAssertions.System.Text.Json.UnitTests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.System.Text.Json.UnitTests
{
    public class JsonElementAssertionsSpecs
    {
        [Fact]
        public void ElementHasProperty()
        {
            using var documentBase = JsonDocument.Parse(@" { ""fullName"": ""Bobby"" }");
            documentBase.RootElement.Should().HaveProperty("fullName");
        }

        [Fact]
        public void ElementDoesNotHaveProperty()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            Action act = () => documentBase.RootElement.Should().HaveProperty("fullName");

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementValueKindDoesNotHaveProperty()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

            documentBase.RootElement.Should().HaveValueKind(JsonValueKind.Object);
        }

        [Fact]
        public void ElementValueKindDoesNotHaveProperty2()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            Action act = () => documentBase.RootElement.Should().HaveValueKind(JsonValueKind.Number);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementBeString()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

            documentBase.RootElement.GetProperty("firstName").Should().BeAString().Which.Should().Be("Bobby");
        }

        [Fact]
        public void ElementCompare()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            using var expected = JsonDocument.Parse(@" { ""test"": ""Bobby"", ""second"":""test"" }");

            Action act = () => documentBase.RootElement.Should().HaveEqualValue(expected.RootElement);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementValueString()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            var actualProperty = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("firstName");
            actualProperty.Should().Be(expectedProperty);
        }

        [Fact]
        public void ElementValueStringFail()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": ""Bobby The second"" }");
            var actualProperty = documentBase.RootElement.GetProperty("firstName");
            var expectedProperty = expected.RootElement.GetProperty("firstName");
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementValueNumber()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": 123 }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": 123 }");
            var actualProperty = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("firstName");
            actualProperty.Should().Be(expectedProperty);
        }

        [Fact]
        public void ElementValueNumberFail()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": 123 }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": 456 }");
            var actualProperty = documentBase.RootElement.GetProperty("firstName");
            var expectedProperty = expected.RootElement.GetProperty("firstName");
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementValueFalse()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": false }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": false }");
            var actualProperty = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("firstName");
            actualProperty.Should().Be(expectedProperty);
        }

        [Fact]
        public void ElementValueFalseFail()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": true }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": false }");
            var actualProperty = documentBase.RootElement.GetProperty("firstName");
            var expectedProperty = expected.RootElement.GetProperty("firstName");
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementValueTrue()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": true }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": true }");
            var actualProperty = documentBase.RootElement.GetTypedProperty("firstName");
            var expectedProperty = expected.RootElement.GetTypedProperty("firstName");
            actualProperty.Should().Be(expectedProperty);
        }

        [Fact]
        public void ElementValueTrueFail()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": false }");
            using var expected = JsonDocument.Parse(@" { ""firstName"": true }");
            var actualProperty = documentBase.RootElement.GetProperty("firstName");
            var expectedProperty = expected.RootElement.GetProperty("firstName");
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementObject()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"", ""secondName"":""Foo"" }");
            using var expected = JsonDocument.Parse(@" {  ""secondName"":""Foo"", ""firstName"": ""Bobby"" }");
            var actualProperty = documentBase.RootElement;
            var expectedProperty = expected.RootElement;
            actualProperty.Should().HaveEqualValue(expectedProperty);
        }

        [Fact]
        public void ElementObjectFail()
        {
            using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"", ""secondName"":""Foo"" }");
            using var expected = JsonDocument.Parse(@" {  ""secondName"":""Bar"", ""firstName"": ""Bobby"" }");
            var actualProperty = documentBase.RootElement;
            var expectedProperty = expected.RootElement;
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void ElementArray()
        {
            using var documentBase = JsonDocument.Parse(@" [{ ""firstName"": ""Bobby""}]");
            using var expected = JsonDocument.Parse(@"[{ ""firstName"": ""Bobby""}]");
            var actualProperty = documentBase.RootElement;
            var expectedProperty = expected.RootElement;
            actualProperty.Should().HaveEqualValue(expectedProperty);
        }

        [Fact]
        public void ElementArrayFail()
        {
            using var documentBase = JsonDocument.Parse(@" [{ ""firstName"": ""Bobby""}]");
            using var expected = JsonDocument.Parse(@" [{ ""secondName"": ""Bobby""}]");
            var actualProperty = documentBase.RootElement;
            var expectedProperty = expected.RootElement;
            Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

            act.Should().Throw<XunitException>();
        }
    }
}
