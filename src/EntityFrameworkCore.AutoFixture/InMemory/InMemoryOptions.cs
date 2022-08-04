using System;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.InMemory;

public record InMemoryOptions
{
    private string databaseName = "TestDatabase";

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when assigned <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">Thrown when assigned an empty value.</exception>
    public string DatabaseName
    {
        get => this.databaseName;
        set => this.SetDatabaseName(value);
    }

    /// <summary>
    /// Gets or sets the option to generate unique names on each request.
    /// </summary>
    public bool UseUniqueNames { get; set; }

    /// <summary>
    /// Gets or sets an optional action to allow additional in-memory specific configuration.
    /// </summary>
    public Action<InMemoryDbContextOptionsBuilder>? ConfigureProvider { get; set; }

    private void SetDatabaseName(string value)
    {
        Check.NotEmpty(value, nameof(value));

        this.databaseName = value;
    }
}
