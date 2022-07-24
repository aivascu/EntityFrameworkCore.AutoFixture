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
    /// properties, on <see cref="DbContext" /> derived types.
    /// </summary>
    public bool OmitDbSets { get; set; }

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

        fixture.Customizations.Add(new DbContextOptionsSpecimenBuilder());
    }
}
