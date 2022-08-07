# Using the SQLite provider

Below is the most basic usage of the library using the SQLite provider for EF Core.

```csharp
[Fact]
public async Task CanAddCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new SqliteCustomization());
    var context = fixture.Create<TestDbContext>();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

This test is equivalent to the following test written using the EF Core API.

```csharp
[Fact]
public async Task CanAddCustomers()
{
    // Arrange
    var connection = new SqliteConnection("Data Source=:memory:");
    var builder = new DbContextOptionsBuilder<TestDbContext>()
        .UseSqlite(connection);
    var options = builder.Options;
    var context = new TestDbContext(options);
    connection.Open();
    context.Database.EnsureCreated();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

## Cheat sheet

### Using custom connection string

In some scenarios it might be required to use a different conection string than the default `"Data Source=:memory:"`.
To change the connection string used by the customization, simply set the `ConnectionString` property to the required value.
The connection string set in the customization, will be used by all `SqliteConnection` instances created by AutoFixture.

```csharp
[Fact]
public async Task CanAddCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new SqliteCustomization {
        ConnectionString = "Data Source=AcmeDb;Mode=Memory;Cache=Shared;"
    });
    var context = fixture.Create<TestDbContext>();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

For more information about SQLite connection strings visit this [page](https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings) on Microsoft's documentation website.

### Manually opening connections

By default `SqliteCustomization` will open the database connection automatically before the database context is created. To change this behavior set the `AutoOpenConnections` to `false`.

```csharp
[Fact]
public async Task CanAddCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new SqliteCustomization {
        AutoOpenConnection = false,
        OnCreate = OnCreateAction.None
    });
    var connection = fixture.Freeze<SqliteConnection>();
    var context = fixture.Create<TestDbContext>();
    // do things before opening the connection
    connection.Open();
    context.Database.Migrate();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

Note that migrating the database require the connection to be open. To avoid failing tests make sure the `OnCreate` action is not set to `OnCreateAction.Migrate`.

### Using custom migrations assembly

The library offers the ability to additionaly configure the provider by setting the `ConfigureProvider` property with a `Action<SqliteDbContextOptionsBuilder>` delegate in the customization options.

```csharp
[Fact]
public async Task CanAddCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new SqliteCustomization {
        OnCreate = OnCreateAction.Migrate,
        ConfigureProvider = builder => builder
            // sets the migations assembly to "Acme.Persistence.SqliteMigrations"
            .MigrationsAssembly("Acme.Persistence.SqliteMigrations")
    });
    var context = fixture.Create<TestDbContext>();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```
