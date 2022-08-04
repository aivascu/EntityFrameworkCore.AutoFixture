using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Core;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Core;

public class PropertyTypeSpecificationTests
{
    [Fact]
    public void IsSpecification()
    {
        typeof(PropertyTypeSpecification)
            .Should().BeAssignableTo<IRequestSpecification>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        _ = new PropertyTypeSpecification(new DelegatingSpecification());
    }

    [Theory]
    [AutoData]
    public void GuardsConstructorParameters(
        Fixture fixture, GuardClauseAssertion assertion)
    {
        fixture.Inject<IRequestSpecification>(new DelegateSpecification());
        assertion.Verify(typeof(PropertyTypeSpecification).GetConstructors());
    }

    [Theory]
    [AutoData]
    public void SetsPropertiesFromConstructor(
        Fixture fixture, ConstructorInitializedMemberAssertion assertion)
    {
        fixture.Inject<IRequestSpecification>(new DelegateSpecification());
        assertion.Verify(typeof(PropertyTypeSpecification)
            .GetConstructor(new[] { typeof(IRequestSpecification) }));
    }

    [Fact]
    public void ReturnsFalseWhenRequestNotProperty()
    {
        var specification = new PropertyTypeSpecification(
            new DelegatingSpecification());

        var actual = specification.IsSatisfiedBy(new object());

        actual.Should().BeFalse();
    }

    [Fact]
    public void CallsDecoratedSpecificationWithPropertyType()
    {
        object request = default;
        var next = new DelegatingSpecification
        {
            OnIsSatisfiedBy = x =>
            {
                request = x;
                return false;
            }
        };
        var specification = new PropertyTypeSpecification(next);
        var property = typeof(PropertyTypeSpecification)
            .GetProperties().Single();

        _ = specification.IsSatisfiedBy(property);

        request.Should().BeSameAs(property.PropertyType);
    }

    [Theory]
    [InlineData(typeof(TrueRequestSpecification), true)]
    [InlineData(typeof(FalseRequestSpecification), false)]
    public void ReturnsResultFromDecoratedSpecification(
        Type type, bool expected)
    {
        var next = (IRequestSpecification)Activator.CreateInstance(type);
        var specification = new PropertyTypeSpecification(next!);
        var property = typeof(PropertyTypeSpecification)
            .GetProperties().Single();

        var actual = specification.IsSatisfiedBy(property);

        actual.Should().Be(expected);
    }
}
