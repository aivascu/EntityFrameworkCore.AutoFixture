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

public class ParameterMemberSpecificationTests
{
    [Fact]
    public void IsSpecification()
    {
        typeof(ParameterMemberSpecification)
            .Should().BeAssignableTo<IRequestSpecification>();
    }

    [Fact]
    public void CanCreateInstance()
    {
        var act = () => _ = new ParameterMemberSpecification(new DelegatingSpecification());

        act.Should().NotThrow();
    }

    [Theory]
    [AutoData]
    public void GuardsConstructorParameters(Fixture fixture, GuardClauseAssertion assertion)
    {
        fixture.Inject<IRequestSpecification>(new DelegateSpecification());
        assertion.Verify(typeof(ParameterMemberSpecification).GetConstructors());
    }

    [Theory]
    [AutoData]
    public void SetsPropertiesFromConstructor(Fixture fixture, ConstructorInitializedMemberAssertion assertion)
    {
        fixture.Inject<IRequestSpecification>(new DelegateSpecification());
        assertion.Verify(typeof(ParameterMemberSpecification));
    }

    [Fact]
    public void ReturnsFalseWhenRequestNotParameter()
    {
        var specification = new ParameterMemberSpecification(new DelegatingSpecification());

        var actual = specification.IsSatisfiedBy(new object());

        actual.Should().BeFalse();
    }

    [Fact]
    public void CallsDecoratedSpecificationWithMember()
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
        var specification = new ParameterMemberSpecification(next);
        var constructor = typeof(ParameterMemberSpecification).GetConstructors().Single();
        var parameter = constructor.GetParameters().Single();

        _ = specification.IsSatisfiedBy(parameter);

        request.Should().BeSameAs(constructor);
    }

    [Theory]
    [InlineData(typeof(TrueRequestSpecification), true)]
    [InlineData(typeof(FalseRequestSpecification), false)]
    public void ReturnsResultFromDecoratedSpecification(Type type, bool expected)
    {
        var next = (IRequestSpecification)Activator.CreateInstance(type);
        var specification = new ParameterMemberSpecification(next!);
        var constructor = typeof(ParameterMemberSpecification).GetConstructors().Single();
        var parameter = constructor.GetParameters().Single();

        var actual = specification.IsSatisfiedBy(parameter);

        actual.Should().Be(expected);
    }
}
