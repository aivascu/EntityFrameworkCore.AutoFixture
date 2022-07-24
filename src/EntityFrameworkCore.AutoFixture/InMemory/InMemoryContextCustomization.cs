#nullable enable
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
public class InMemoryContextCustomization : DbContextCustomization
{
    /// <summary>
    /// Gets or sets the database name.<br />
    /// Default value is <see langword="TestDatabase" />.
    /// </summary>
    public string DatabaseName { get; set; } = "TestDatabase";

    /// <summary>
    /// Gets or sets the option to generate unique database names on each request.<br />
    /// When <see langword="true" /> the name will be suffixed by a random value.<br />
    /// When <see langword="false" /> the name will be same on each request.<br />
    /// Default value is <see langword="true" />.
    /// </summary>
    public bool UseUniqueNames { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional action to allow additional in-memory specific configuration.<br />
    /// Default value is <see langword="null" />.
    /// </summary>
    public Action<InMemoryDbContextOptionsBuilder>? Configure { get; set; } = null;

    /// <inheritdoc />
    public override void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        base.Customize(fixture);

        var options = new InMemoryOptions
        {
            DatabaseName = this.DatabaseName,
            UseUniqueNames = this.UseUniqueNames,
            Configure = this.Configure
        };
        
        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new InMemoryOptionsBuilder(
                new MethodInvoker(
                    new ModestConstructorQuery()),
                options),
            new ExactTypeSpecification(
                typeof(DbContextOptionsBuilder<>))));
    }
}
