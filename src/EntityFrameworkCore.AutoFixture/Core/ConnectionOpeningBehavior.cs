using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class ConnectionOpeningBehavior : ISpecimenBuilderTransformation
    {
        private readonly IRequestSpecification specification;

        public ConnectionOpeningBehavior(IRequestSpecification specification)
        {
            this.specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            return new ConnectionOpeningNode(builder, this.specification);
        }
    }
}
