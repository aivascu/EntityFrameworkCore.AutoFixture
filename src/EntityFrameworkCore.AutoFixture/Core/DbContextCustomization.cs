using System;
using System.Reflection;
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
    /// Gets or sets the configuration to omit <see cref="DbSet{TEntity}" />
    /// properties, on <see cref="DbContext" /> derived types. <br />
    /// Default value is <see langword="true"/>.
    /// </summary>
    public bool OmitDbSets { get; set; } = true;

    /// <summary>
    /// Gets or sets the postprocessing action for <see cref="DbContext" />.<br/>
    /// Default value is <see cref="OnCreateAction.None"/>.
    /// </summary>
    public OnCreateAction OnCreate { get; set; } = OnCreateAction.EnsureCreated;

    /// <inheritdoc />
    public virtual void Customize(IFixture fixture)
    {
        if (fixture is null) throw new ArgumentNullException(nameof(fixture));

        if (this.OmitDbSets)
        {
            fixture.Customizations.Add(new Omitter(
                new AndRequestSpecification(
                    new PropertySpecification(
                        new GenericPropertyTypeCriterion(typeof(DbSet<>))),
                    new PropertySpecification(
                        new DeclaringTypeCriterion<PropertyInfo>(
                            new BaseTypeCriterion(typeof(DbContext)))))));
        }

        ISpecimenCommand onCreate = this.OnCreate switch
        {
            OnCreateAction.None => new EmptyCommand(),
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
