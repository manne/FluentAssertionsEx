# Fluent Assertions Extensions

This repo contains extensions for [Fluent Assertions](https://fluentassertions.com).

Currently solely for `System.Text.Json`.

## Installation

Currently not available.

## System.Text.Json

There are basic assertions for:

- JsonElement and
- JsonProperty

### JsonElement

There are the following assertions for JSON elements:

- HaveProperty, to verify whether the element contains a property with a specific name, provides chaining the property assertions
- HaveValueKind, to verify the value kind
- HaveEqualValue, to verify the value and
- BeAString, to verify whether the value is of kind `String`, provides chaining the property assertions

#### JsonElement.Should().HaveProperty(string)

Asserts whether the element has got a property name `fullName`.

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""fullName"": ""Bobby"" }");

documentBase.RootElement.Should().HaveProperty("fullName");
```

Asserts whether the element has got a property name `fullName` and its value is the string `Bobby`.

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""fullName"": ""Bobby"" }");

documentBase.RootElement
    .Should().HaveProperty("fullName")
    .Which.SubjectValue.Should().BeAString()
    .Subject.Should().Be("Bobby");
```

#### JsonElement.Should().HaveValueKind(JsonValueKind)

Asserts whether the value kind of the element is the expected one.

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""fullName"": ""Bobby"" }");
using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

documentBase.RootElement.Should().HaveValueKind(JsonValueKind.Object);
```

#### JsonElement.Should().HaveEqualValue(JsonElement)

Asserts whether the element is equal to the expected element.

Simple equal objects

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"", ""secondName"":""Foo"" }");
using var expected = JsonDocument.Parse(@" {  ""secondName"":""Foo"", ""firstName"": ""Bobby"" }");

var actualProperty = documentBase.RootElement;
var expectedProperty = expected.RootElement;

actualProperty.Should().HaveEqualValue(expectedProperty);
```

Simple unequal objects

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"", ""secondName"":""Foo"" }");
using var expected = JsonDocument.Parse(@" {  ""secondName"":""Bar"", ""firstName"": ""Bobby"" }");

var actualProperty = documentBase.RootElement;
var expectedProperty = expected.RootElement;

Action act = () => actualProperty.Should().HaveEqualValue(expectedProperty);

act.Should().Throw<XunitException>();
```

#### JsonElement.Should().BeAString()

Asserts whether the value of the element is of kind `String`.

### JsonProperty

There are two assertions for JSON properties:

- Be, to verify the whole property with the expected property and
- BeOfValueKind, to verify the value kind

### JsonProperty.Should().Be(JsonProperty)

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");
using var expected = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

var property = documentBase.RootElement.GetTypedProperty("firstName");
var expectedProperty = expected.RootElement.GetTypedProperty("firstName");

property.Should().Be(expectedProperty);
```

### JsonProperty.Should().BeOfValueKind(JsonProperty)

```CSharp
using var documentBase = JsonDocument.Parse(@" { ""firstName"": ""Bobby"" }");

var property = documentBase.RootElement.GetTypedProperty("firstName");

property.Should().BeOfValueKind(JsonValueKind.String);
```
