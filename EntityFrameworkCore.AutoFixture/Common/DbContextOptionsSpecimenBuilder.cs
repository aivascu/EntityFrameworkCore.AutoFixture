using System;
using System.Linq;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public class DbContextOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public DbContextOptionsSpecimenBuilder()
            : this(new IsDbContextOptionsSpecification())
        {
        }

        public DbContextOptionsSpecimenBuilder(IRequestSpecification optionsSpecification)
        {
            OptionsSpecification = optionsSpecification;
        }

        public IRequestSpecification OptionsSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!OptionsSpecification.IsSatisfiedBy(request))
            {
                return new NoSpecimen();
            }

            if (!(request is Type type))
            {
                return new NoSpecimen();
            }

            var contextType = type.GetGenericArguments().SingleOrDefault();

            if (contextType is null)
            {
                return new NoSpecimen();
            }

            var optionsBuilderObj = context.Resolve(typeof(IOptionsBuilder));

            if (optionsBuilderObj is NoSpecimen
                || optionsBuilderObj is OmitSpecimen
                || optionsBuilderObj is null)
            {
                return optionsBuilderObj;
            }

            if (!(optionsBuilderObj is IOptionsBuilder optionsBuilder))
            {
                return new NoSpecimen();
            }

            return optionsBuilder.Build(contextType);
        }

        private class IsDbContextOptionsSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type type
                    && !type.IsAbstract
                    && type.IsGenericType
                    && typeof(DbContextOptions<>) == type.GetGenericTypeDefinition();
            }
        }
    }
}
