using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core
{
    internal class DeclaringTypeCriterion<T> : IEquatable<T>
        where T : MemberInfo
    {
        public DeclaringTypeCriterion(IEquatable<Type> criterion)
        {
            this.Criterion = criterion ?? throw new ArgumentNullException(nameof(criterion));
        }

        public DeclaringTypeCriterion(Type type)
            : this(new Criterion<Type>(type, EqualityComparer<Type>.Default))
        {
        }

        public IEquatable<Type> Criterion { get; }

        public bool Equals(T other)
        {
            return other is not null
                   && this.Criterion.Equals(other.DeclaringType);
        }
    }
}
