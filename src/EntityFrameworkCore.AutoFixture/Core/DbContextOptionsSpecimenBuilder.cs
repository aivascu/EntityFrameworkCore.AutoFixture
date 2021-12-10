using System;
using System.Linq;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Core
{
    public class DbContextOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public DbContextOptionsSpecimenBuilder()
            : this(new IsDbContextOptionsSpecification())
        {
        }

        public DbContextOptionsSpecimenBuilder(IRequestSpecification optionsSpecification)
        {
            this.OptionsSpecification = optionsSpecification
                ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        public IRequestSpecification OptionsSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (!this.OptionsSpecification.IsSatisfiedBy(request)) return new NoSpecimen();
            if (request is not Type type) return new NoSpecimen();

            var contextType = type.GetGenericArguments().Single();
            var optionsBuilderObj = context.Resolve(typeof(IOptionsBuilder));
            return optionsBuilderObj switch
            {
                NoSpecimen or OmitSpecimen or null => optionsBuilderObj,
                IOptionsBuilder optionsBuilder => optionsBuilder.Build(contextType),
                _ => new NoSpecimen()
            };
        }

        private class IsDbContextOptionsSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type
                {
                    IsAbstract: false,
                    IsGenericType: true
                } type
                && typeof(DbContextOptions<>) == type.GetGenericTypeDefinition();
            }
        }
    }
}
