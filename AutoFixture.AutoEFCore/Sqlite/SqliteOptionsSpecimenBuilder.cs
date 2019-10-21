using System;
using AutoFixture.AutoEFCore.Common;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;

namespace AutoFixture.AutoEFCore.Sqlite
{
    public class SqliteOptionsSpecimenBuilder : ISpecimenBuilder
    {
        public SqliteOptionsSpecimenBuilder(IRequestSpecification optionsBuilderSpecification)
        {
            OptionsBuilderSpecification = optionsBuilderSpecification;
        }

        public SqliteOptionsSpecimenBuilder()
            : this(new IsOptionsBuilder())
        {
        }

        public IRequestSpecification OptionsBuilderSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!OptionsBuilderSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            var sqliteConnectionObj = context.Resolve(typeof(SqliteConnection));

            if (sqliteConnectionObj is NoSpecimen || sqliteConnectionObj is OmitSpecimen || sqliteConnectionObj is null)
                return sqliteConnectionObj;

            if (!(sqliteConnectionObj is SqliteConnection sqliteConnection))
                return new NoSpecimen();

            return new SqliteOptionsBuilder(sqliteConnection);
        }

        private class IsOptionsBuilder : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type type
                       && type.IsInterface
                       && type == typeof(IOptionsBuilder);
            }
        }
    }
}