using System;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.InMemory;

/// <summary>
/// Customizes AutoFixture to create in memory database contexts.
/// </summary>
public class InMemoryCustomization : DbContextCustomization
{
    private string databaseName = "TestDatabase";

    /// <summary>
    /// Gets or sets the database name. <br/> Default value is <see langword="TestDatabase"/>.
    /// </summary>
    public string DatabaseName
    {
        get => this.databaseName;
        set => this.databaseName = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Configures whether the database names should contain a random suffix.
    /// Default value is <see langword="true"/>.
    /// </summary>
    public bool UseUniqueNames { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional action to allow additional in-memory specific configuration. <br/>
    /// Default value is <see langword="null"/>.
    /// </summary>
    public Action<InMemoryDbContextOptionsBuilder>? ConfigureProvider { get; set; } = null;

    /// <inheritdoc/>
    public override void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        base.Customize(fixture);

        var options = new InMemoryOptions
        {
            DatabaseName = this.DatabaseName,
            UseUniqueNames = this.UseUniqueNames,
            ConfigureProvider = this.ConfigureProvider
        };

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new OptionsBuilderConfigurator(
                new InMemoryOptionsBuilder(
                    new MethodInvoker(
                        new ModestConstructorQuery()),
                    options),
                this.Configure),
            new ExactTypeSpecification(
                typeof(DbContextOptionsBuilder<>))));
    }
}
