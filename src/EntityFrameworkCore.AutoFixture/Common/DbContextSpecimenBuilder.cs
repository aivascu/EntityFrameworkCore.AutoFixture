using System;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public class DbContextSpecimenBuilder : ISpecimenBuilder
    {
        public DbContextSpecimenBuilder()
            : this(new IsDbContextSpecification())
        {
        }

        public DbContextSpecimenBuilder(IRequestSpecification contextSpecification)
        {
            ContextSpecification = contextSpecification
                ?? throw new ArgumentNullException(nameof(contextSpecification));
        }

        public IRequestSpecification ContextSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!ContextSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            if (!(request is Type type))
            {
                return new NoSpecimen();
            }

            var options = context.Resolve(typeof(DbContextOptions<>).MakeGenericType(type));

            if (options is NoSpecimen || options is OmitSpecimen || options is null)
            {
                return options;
            }

            return Activator.CreateInstance(type, options);
        }

        private class IsDbContextSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type type
                    && !type.IsAbstract
                    && typeof(DbContext).IsAssignableFrom(type);
            }
        }
    }
}
