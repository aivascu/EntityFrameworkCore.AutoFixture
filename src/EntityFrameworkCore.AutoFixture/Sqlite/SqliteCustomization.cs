using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

/// <summary>
/// Customizes AutoFixture to create sqlite Entity Framework database contexts.
/// </summary>
public class SqliteCustomization : DbContextCustomization
{
    private const string ConnectionStringParameter = "connectionString";
    private const string DefaultConnectionString = "DataSource=:memory:";

    private string connectionString = DefaultConnectionString;

    /// <summary>
    /// Configures the database connections to open immidiately after creation. Default value is <see langword="true"/>.
    /// </summary>
    public bool AutoOpenConnection { get; set; } = true;

    /// <summary>
    /// Gets or sets the connection string used to create database connections. Default is "DataSource=:memory:".
    /// </summary>
    public string ConnectionString
    {
        get => this.connectionString;
        set => this.SetConnectionString(value);
    }

    /// <summary>
    /// Provides additional in-memory specific configuration. Default value is <see langword="null"/>.
    /// </summary>
    public Action<SqliteDbContextOptionsBuilder> ConfigureProvider { get; set; }

    /// <inheritdoc/>
    public override void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        base.Customize(fixture);

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new FixedBuilder(this.ConnectionString),
            new AndRequestSpecification(
                new ParameterSpecification(typeof(string), ConnectionStringParameter),
                new ParameterSpecification(
                    new DeclaringMemberCriterion(
                        new DeclaringTypeCriterion<MemberInfo>(typeof(SqliteConnection)))))));

        ISpecimenCommand onCreateConnection = this.AutoOpenConnection
            ? new OpenDatabaseConnection()
            : new EmptyCommand();

        fixture.Customizations.Insert(0, new FilteringSpecimenBuilder(
            new Postprocessor(
                new MethodInvoker(
                    new GreedyConstructorQuery()),
                onCreateConnection),
            new ExactTypeSpecification(typeof(SqliteConnection))));

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new OptionsBuilderConfigurator(
                new SqliteOptionsBuilder(
                    new MethodInvoker(
                        new ModestConstructorQuery()),
                    this.ConfigureProvider),
                this.Configure),
            new ExactTypeSpecification(
                typeof(DbContextOptionsBuilder<>))));
    }

    private void SetConnectionString(string value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty or whitespace.", nameof(value));

        this.connectionString = value;
    }
}
