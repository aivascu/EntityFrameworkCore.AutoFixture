using System;
using System.Data.Common;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

/// <summary>
/// Customizes AutoFixture to create sqlite Entity Framework database contexts.
/// </summary>
public class SqliteContextCustomization : DbContextCustomization
{
    private const string ConnectionStringParameter = "connectionString";
    private const string DefaultConnectionString = "DataSource=:memory:";

    private string connectionString = DefaultConnectionString;

    /// <summary>
    /// Automatically open <see cref="DbConnection" /> database connections upon creation.<br />
    /// Default value is <see langword="true" />.
    /// </summary>
    public bool AutoOpenConnection { get; set; } = true;

    /// <summary>
    /// Gets or sets the connection string used to create database connections.<br />
    /// Default is &quot;DataSource=:memory:&quot;.
    /// </summary>
    public string ConnectionString
    {
        get => this.connectionString;
        set => this.SetConnectionString(value);
    }

    /// <inheritdoc />
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
