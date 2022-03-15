using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class DatabaseInitializingNode : ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;
        private readonly IRequestSpecification specification;

        public DatabaseInitializingNode(ISpecimenBuilder builder, IRequestSpecification specification)
        {
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder = new CompositeSpecimenBuilder(builders);
            return new DatabaseInitializingNode(composedBuilder, this.specification);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!this.specification.IsSatisfiedBy(request))
                return this.builder.Create(request, context);

            var result = this.builder.Create(request, context);

            if (result is DbContext dbContext)
                dbContext.Database.EnsureCreated();

            return result;
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
