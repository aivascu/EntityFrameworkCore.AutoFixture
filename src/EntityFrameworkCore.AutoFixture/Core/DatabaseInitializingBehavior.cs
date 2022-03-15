using System;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class DatabaseInitializingBehavior : ISpecimenBuilderTransformation
    {
        private readonly IRequestSpecification specification;

        public DatabaseInitializingBehavior(IRequestSpecification specification)
        {
            this.specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            return new DatabaseInitializingNode(builder, this.specification);
        }
    }
}
