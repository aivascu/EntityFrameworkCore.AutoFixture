using System;
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
    /// Configures the database connections to open immediately after creation. Default value is <see langword="true"/>.
    /// </summary>
    public bool AutoOpenConnection { get; set; } = true;

    /// <summary>
    /// Gets or sets the connection string used to create database connections. Default is "DataSource=:memory:".
    /// </summary>
    /// <example>
    /// This shows how to use connection string that defines the cache mode as shared.
    /// <code>
    /// var fixture = new Fixture().Customize(new SqliteCustomization
    /// {
    ///     ConnectionString = "Data Source=:memory:;Mode=Memory;Cache=Shared;"
    /// });
    /// </code>
    /// </example>
    public string ConnectionString
    {
        get => this.connectionString;
        set => this.connectionString = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Provides additional in-memory specific configuration. Default value is <see langword="null"/>.
    /// </summary>
    public Action<SqliteDbContextOptionsBuilder>? ConfigureProvider { get; set; } = null;

    /// <inheritdoc/>
    public override void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        base.Customize(fixture);

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new FixedBuilder(this.ConnectionString),
            new AndRequestSpecification(
                new ParameterSpecification(typeof(string), ConnectionStringParameter),
                new ParameterMemberSpecification(
                    new DeclaringTypeSpecification(typeof(SqliteConnection))))));

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
}
