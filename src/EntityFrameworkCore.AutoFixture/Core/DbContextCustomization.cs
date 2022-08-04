using System;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Customizes AutoFixture to resolve database context options from fixture.
/// </summary>
public class DbContextCustomization : ICustomization
{
    /// <summary>
    /// Configures whether <see cref="DbSet{TEntity}" /> properties on <see cref="DbContext" /> will be omitted by AutoFixture.
    /// Default value is <see langword="true" />.
    /// </summary>
    public bool OmitDbSets { get; set; } = true;

    /// <summary>
    /// Gets or sets the postprocessing action for <see cref="DbContext" />.
    /// Default value is <see cref="OnCreateAction.EnsureCreated" />.
    /// </summary>
    public OnCreateAction OnCreate { get; set; } = OnCreateAction.EnsureCreated;

    /// <summary>
    /// Gets or sets the DbContext options builder configuration delegate,
    /// which runs after configuring the database provider.
    /// </summary>
    public Func<DbContextOptionsBuilder, DbContextOptionsBuilder>? Configure { get; set; }

    /// <inheritdoc />
    public virtual void Customize(IFixture fixture)
    {
        Check.NotNull(fixture, nameof(fixture));

        if (this.OmitDbSets)
        {
            fixture.Customizations.Add(new Omitter(
                new AndRequestSpecification(
                    new PropertyTypeSpecification(typeof(DbSet<>)),
                    new DeclaringTypeSpecification(
                        new BaseTypeSpecification(typeof(DbContext))))));
        }

        ISpecimenCommand onCreate = this.OnCreate switch
        {
            OnCreateAction.EnsureCreated => new EnsureCreatedCommand(),
            OnCreateAction.Migrate => new MigrateCommand(),
            _ => new EmptyCommand()
        };

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new ContextOptionsBuilder(),
            new ExactTypeSpecification(typeof(DbContextOptions<>))));

        fixture.Customizations.Add(new FilteringSpecimenBuilder(
            new Postprocessor(
                new MethodInvoker(
                    new GreedyConstructorQuery()),
                onCreate),
            new AndRequestSpecification(
                new BaseTypeSpecification(typeof(DbContext)),
                new InverseRequestSpecification(
                    new AbstractTypeSpecification()))));
    }
}
