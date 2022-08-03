using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.InMemory;

public record InMemoryOptions
{
    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string? DatabaseName { get; set; }

    /// <summary>
    /// Gets or sets the option to generate unique names on each request.
    /// </summary>
    public bool UseUniqueNames { get; set; }

    /// <summary>
    /// Gets or sets an optional action to allow additional in-memory specific configuration.
    /// </summary>
    public Action<InMemoryDbContextOptionsBuilder>? ConfigureProvider { get; set; }
}