using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.InMemory
{
    /// <summary>
    /// Creates in-memory options builders.
    /// </summary>
    public class InMemoryOptionsSpecimenBuilder : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a <see cref="InMemoryOptionsSpecimenBuilder"/> instance.
        /// </summary>
        /// <param name="optionsSpecification">The options builder specification.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="optionsSpecification"/> is <see langword="null" />.</exception>
        public InMemoryOptionsSpecimenBuilder(IRequestSpecification optionsSpecification)
        {
            this.OptionsSpecification = optionsSpecification
                ?? throw new ArgumentNullException(nameof(optionsSpecification));
        }

        /// <summary>
        /// Creates a <see cref="InMemoryOptionsSpecimenBuilder"/> instance.
        /// </summary>
        public InMemoryOptionsSpecimenBuilder()
            : this(new ExactTypeSpecification(typeof(IOptionsBuilder)))
        {
        }

        /// <summary>
        /// Gets the options specification.
        /// </summary>
        public IRequestSpecification OptionsSpecification { get; }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (!this.OptionsSpecification.IsSatisfiedBy(request)) return new NoSpecimen();

            return new InMemoryOptionsBuilder();
        }
    }
}
