using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    /// <summary>
    /// Creates SQLite options builders.
    /// </summary>
    public class SqliteOptionsSpecimenBuilder : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a <see cref="SqliteConnectionSpecimenBuilder" />.
        /// </summary>
        /// <param name="optionsBuilderSpecification">The specification for options builder.</param>
        /// <exception cref="ArgumentNullException">When specification is <see langword="null"/>.</exception>
        public SqliteOptionsSpecimenBuilder(IRequestSpecification optionsBuilderSpecification)
        {
            this.OptionsBuilderSpecification = optionsBuilderSpecification
                ?? throw new ArgumentNullException(nameof(optionsBuilderSpecification));
        }

        /// <summary>
        /// Creates a <see cref="SqliteConnectionSpecimenBuilder" />.
        /// </summary>
        public SqliteOptionsSpecimenBuilder()
            : this(new ExactTypeSpecification(typeof(IOptionsBuilder)))
        {
        }

        /// <summary>
        /// Gets the options builder specification.
        /// </summary>
        public IRequestSpecification OptionsBuilderSpecification { get; }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (!this.OptionsBuilderSpecification.IsSatisfiedBy(request)) return new NoSpecimen();

            var sqliteConnectionObj = context.Resolve(typeof(SqliteConnection));
            return sqliteConnectionObj switch
            {
                NoSpecimen or OmitSpecimen or null => sqliteConnectionObj,
                SqliteConnection sqliteConnection => new SqliteOptionsBuilder(sqliteConnection),
                _ => new NoSpecimen()
            };
        }
    }
}
