using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture
{
    public class VirtualPropertySpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            if(request is not PropertyInfo propertyInfo)
                return false;

            return propertyInfo.GetAccessors()
                .All(x => x.IsVirtual && !x.IsFinal);
        }
    }
}
