using System;
using System.Data.Common;
using AutoFixture;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Sqlite;

/// <summary>
/// Customizes AutoFixture to create sqlite Entity Framework database contexts.
/// </summary>
[Obsolete(@"Use the InMemoryCustomization instead. This customization will be removed in the next version.
For details on migration visit https://github.com/aivascu/EntityFrameworkCore.AutoFixture .")]
public class SqliteContextCustomization : ICustomization
{
    /// <summary>
    /// Gets or sets the configuration to omit <see cref="DbSet{TEntity}" />
    /// properties, on <see cref="DbContext" /> derived types.
    /// </summary>
    public bool OmitDbSets { get; set; }

    /// <summary>
    /// Automatically open <see cref="DbConnection" /> database connections upon creation.
    /// Default value is <see langword="false" />.
    /// </summary>
    public bool AutoOpenConnection { get; set; }

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

        var customization = new SqliteCustomization
        {
            OmitDbSets = this.OmitDbSets,
            AutoOpenConnection = this.AutoOpenConnection,
            OnCreate = this.AutoCreateDatabase
                ? OnCreateAction.EnsureCreated
                : OnCreateAction.None
        };

        fixture.Customize(customization);
    }
}
