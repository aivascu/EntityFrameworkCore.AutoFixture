using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Tests.Common;

public class DelegatingFixture : IFixture
{
    private readonly List<ISpecimenBuilderTransformation> behaviors = new();
    private readonly List<ISpecimenBuilder> customizations = new();
    private readonly List<ISpecimenBuilder> residueCollectors = new();

    public IList<ISpecimenBuilderTransformation> Behaviors => this.behaviors;

    public IList<ISpecimenBuilder> Customizations => this.customizations;

    public bool OmitAutoProperties { get; set; }
    public int RepeatCount { get; set; }

    public IList<ISpecimenBuilder> ResidueCollectors => this.residueCollectors;

    public Func<object> OnBuild { get; set; }
    public Func<object, ISpecimenContext, object> OnCreate { get; set; }
    public Func<ICustomization, IFixture> OnCustomize { get; set; }
    public Action<Func<object, ISpecimenBuilder>> OnCustomizeType { get; set; }

    public ICustomizationComposer<T> Build<T>() => (ICustomizationComposer<T>)this.OnBuild?.Invoke();

    public object Create(object request, ISpecimenContext context) => this.OnCreate?.Invoke(request, context);

    public IFixture Customize(ICustomization customization) => this.OnCustomize?.Invoke(customization);

    public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
    {
        throw new NotImplementedException();
    }
}