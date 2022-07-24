using System;
using System.Data.Common;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Sqlite
{
    /// <summary>
    /// Customizes AutoFixture to create sqlite Entity Framework database contexts.
    /// </summary>
    public class SqliteContextCustomization : DbContextCustomization
    {
        private string connectionString = DefaultConnectionString;
        private const string ConnectionStringParameter = "connectionString";
        private const string DefaultConnectionString = "DataSource=:memory:";

        /// <summary>
        /// Automatically open <see cref="DbConnection"/> database connections upon creation.
        /// Default value is <see langword="false"/>.
        /// </summary>
        public bool AutoOpenConnection { get; set; }

        /// <summary>
        /// Automatically run <see cref="DatabaseFacade.EnsureCreated()"/>, on
        /// <see cref="DbContext.Database"/>, upon creating a database context instance. Default
        /// value is <see langword="false"/>.
        /// </summary>
        public bool AutoCreateDatabase { get; set; }

        /// <summary>
        /// Gets or sets the default connection string used when creating <see cref="SqliteConnection"/> instances.
        /// Default is &quot;DataSource=:memory:&quot;.
        /// </summary>
        public string ConnectionString
        {
            get => this.connectionString;
            set => this.SetConnectionString(value);
        }

        private void SetConnectionString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(value));

            this.connectionString = value;
        }

        /// <inheritdoc/>
        public override void Customize(IFixture fixture)
        {
            if (fixture is null) throw new ArgumentNullException(nameof(fixture));

            base.Customize(fixture);

            fixture.Customizations.Add(new FilteringSpecimenBuilder(
                new FixedBuilder(this.ConnectionString),
                new AndRequestSpecification(
                    new ParameterSpecification(typeof(string), ConnectionStringParameter),
                    new ParameterSpecification(
                        new DeclaringMemberCriterion(
                            new DeclaringTypeCriterion<MemberInfo>(typeof(SqliteConnection)))))));
            
            fixture.Customizations.Add(new TypeRelay(typeof(IOptionsBuilder), typeof(SqliteOptionsBuilder)));
            
            var connectionBuilder = SpecimenBuilderNodeFactory.CreateTypedNode(
                typeof(SqliteConnection),
                new MethodInvoker(new GreedyConstructorQuery()));
            fixture.Customizations.Insert(0, connectionBuilder);
            
            if (this.AutoOpenConnection)
            {
                fixture.Behaviors.Add(new ConnectionOpeningBehavior(
                    new BaseTypeSpecification(typeof(DbConnection))));
            }

            if (this.AutoCreateDatabase)
            {
                fixture.Behaviors.Add(new DatabaseInitializingBehavior(
                    new BaseTypeSpecification(typeof(DbContext))));
            }
        }
    }
}
