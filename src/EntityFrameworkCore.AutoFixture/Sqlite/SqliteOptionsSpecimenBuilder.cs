using System;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    public class SqliteOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public SqliteOptionsSpecimenBuilder(IRequestSpecification optionsBuilderSpecification)
        {
            this.OptionsBuilderSpecification = optionsBuilderSpecification
                ?? throw new ArgumentNullException(nameof(optionsBuilderSpecification));
        }

        public SqliteOptionsSpecimenBuilder()
            : this(new IsOptionsBuilder())
        {
        }

        public IRequestSpecification OptionsBuilderSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (!this.OptionsBuilderSpecification.IsSatisfiedBy(request)) return new NoSpecimen();

            var sqliteConnectionObj = context.Resolve(typeof(SqliteConnection));
            return sqliteConnectionObj switch
            {
                NoSpecimen or OmitSpecimen or null => sqliteConnectionObj,
                SqliteConnection sqliteConnection => new SqliteOptionsBuilder(sqliteConnection),
                _ => new NoSpecimen()
            };
        }

        private class IsOptionsBuilder : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type { IsInterface: true } type
                    && type == typeof(IOptionsBuilder);
            }
        }
    }
}
