using System;
using System.Reflection;

namespace EntityFrameworkCore.AutoFixture.Core
{
    internal class DeclaringMemberCriterion : IEquatable<ParameterInfo>
    {
        public DeclaringMemberCriterion(IEquatable<MemberInfo> criterion)
        {
            this.Criterion = criterion ?? throw new ArgumentNullException(nameof(criterion));
        }

        public IEquatable<MemberInfo> Criterion { get; }

        public bool Equals(ParameterInfo other)
        {
            return other is not null
                   && this.Criterion.Equals(other.Member);
        }
    }
}
