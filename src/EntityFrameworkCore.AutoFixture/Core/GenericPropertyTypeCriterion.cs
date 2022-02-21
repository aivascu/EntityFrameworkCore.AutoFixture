using System;
using System.Reflection;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class GenericPropertyTypeCriterion : IEquatable<PropertyInfo>
    {
        public GenericPropertyTypeCriterion(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException("Argument should be a generic type definition", nameof(type));

            this.Type = type;
        }

        public Type Type { get; }

        public bool Equals(PropertyInfo other)
        {
            if (other?.PropertyType.IsGenericType != true)
                return false;

            var definition = other.PropertyType.GetGenericTypeDefinition();
            return definition == this.Type;
        }
    }
}
