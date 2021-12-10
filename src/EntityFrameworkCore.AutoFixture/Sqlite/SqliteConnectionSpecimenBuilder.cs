using System;
using AutoFixture.Kernel;
using Microsoft.Data.Sqlite;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    public class SqliteConnectionSpecimenBuilder : ISpecimenBuilder
    {
        public SqliteConnectionSpecimenBuilder(IRequestSpecification connectionSpecification)
        {
            this.ConnectionSpecification = connectionSpecification
                ?? throw new ArgumentNullException(nameof(connectionSpecification));
        }

        public SqliteConnectionSpecimenBuilder()
            : this(new IsSqliteConnectionSpecification())
        {
        }

        public IRequestSpecification ConnectionSpecification { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!this.ConnectionSpecification.IsSatisfiedBy(request)) return new NoSpecimen();

            return new SqliteConnection("DataSource=:memory:");
        }

        private class IsSqliteConnectionSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                return request is Type { IsAbstract: false } type
                    && type == typeof(SqliteConnection);
            }
        }
    }
}
