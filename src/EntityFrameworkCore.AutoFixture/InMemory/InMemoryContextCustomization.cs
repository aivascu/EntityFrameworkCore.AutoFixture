using System;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.InMemory;

/// <summary>
/// Customizes AutoFixture to create in memory database contexts.
/// </summary>
[Obsolete(@"This customization will be removed in the next version. Use the InMemoryCustomization instead.
For details on migration visit https://github.com/aivascu/EntityFrameworkCore.AutoFixture .")]
public class InMemoryContextCustomization : ICustomization
{
    /// <summary>
    /// Gets or sets the configuration to omit <see cref="DbSet{TEntity}" />
    /// properties, on <see cref="DbContext" /> derived types.
    /// </summary>
    public bool OmitDbSets { get; set; }

    /// <summary>
    /// Automatically run <see cref="DatabaseFacade.EnsureCreated()" />, on
    /// <see cref="DbContext.Database" />, upon creating a database context instance. Default
    /// value is <see langword="false" />.
    /// </summary>
    public bool AutoCreateDatabase { get; set; }

    /// <inheritdoc />
    public void Customize(IFixture fixture)
    {
        Check.NotNull(fixture, nameof(fixture));

        var customization = new InMemoryCustomization
        {
            OmitDbSets = this.OmitDbSets,
            OnCreate = this.AutoCreateDatabase
                ? OnCreateAction.EnsureCreated
                : OnCreateAction.None,
        };

        fixture.Customize(customization);
    }
}
