using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

/// <summary>
/// Checks whether a property is <see langword="virtual"/>.
/// </summary>
public class VirtualPropertySpecification : IRequestSpecification
{
    /// <inheritdoc />
    public bool IsSatisfiedBy(object request)
    {
        if (request is not PropertyInfo propertyInfo)
            return false;

        return propertyInfo.GetAccessors()
            .All(x => x.IsVirtual && !x.IsFinal);
    }
}
