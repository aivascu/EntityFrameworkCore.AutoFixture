using System;

namespace EntityFrameworkCore.AutoFixture.Core;

internal class BaseTypeCriterion : IEquatable<Type>
{
    public BaseTypeCriterion(Type type)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public Type Type { get; }

    public bool Equals(Type other)
    {
        return other is not null
               && this.Type.IsAssignableFrom(other);
    }
}
