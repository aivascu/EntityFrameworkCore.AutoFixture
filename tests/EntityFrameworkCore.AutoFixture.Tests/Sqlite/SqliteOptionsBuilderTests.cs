using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Sqlite;
using EntityFrameworkCore.AutoFixture.Tests.Common;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests.Sqlite;

public class SqliteOptionsBuilderTests
{
    [Fact]
    public void CanCreateInstance()
    {
        var act = () => _ = new SqliteOptionsBuilder(new DelegatingBuilder());

        act.Should().NotThrow();
    }

    [Fact]
    public void ThrowsWhenBuilderNull()
    {
        var act = () => _ = new SqliteOptionsBuilder(null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void CanCreateInstanceWithOptionalParameter()
    {
        var act = () => _ = new SqliteOptionsBuilder(new DelegatingBuilder(), x => x.MaxBatchSize(10));

        act.Should().NotThrow();
    }

    [Fact]
    public void SetsPropertiesFromConstructor()
    {
        var specimenBuilder = new DelegatingBuilder();
        Action<SqliteDbContextOptionsBuilder> provider = x => x.MaxBatchSize(10);
        var builder = new SqliteOptionsBuilder(specimenBuilder, provider);

        using (new AssertionScope())
        {
            builder.Builder.Should().BeSameAs(specimenBuilder);
            builder.ConfigureProvider.Should().BeSameAs(provider);
        }
    }

    [Fact]
    public void ThrowsWhenContextNull()
    {
        var builder = new SqliteOptionsBuilder(new DelegatingBuilder());

        Action act = () => builder.Create(null!, null!);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void ReturnsNoSpecimenWhenDecoratedBuilderWhenNoResult()
    {
        var builder = new SqliteOptionsBuilder(new DelegatingBuilder
        {
            OnCreate = (_, _) => new NoSpecimen()
        });

        var actual = builder.Create(new object(), new DelegatingSpecimenContext());

        actual.Should().BeOfType<NoSpecimen>();
    }
}
