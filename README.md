# EntityFrameworkCore.AutoFixture

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/aivascu/EntityFrameworkCore.AutoFixture/release.yml?logo=github&style=flat-square)
[![Coveralls github](https://img.shields.io/coveralls/github/aivascu/EntityFrameworkCore.AutoFixture?logo=coveralls&style=flat-square)](https://coveralls.io/github/aivascu/EntityFrameworkCore.AutoFixture?branch=master)
[![NuGet](https://img.shields.io/nuget/v/EntityFrameworkCore.AutoFixture?logo=nuget&style=flat-square)](https://www.nuget.org/packages/EntityFrameworkCore.AutoFixture/)
[![NuGet downloads](https://img.shields.io/nuget/dt/EntityFrameworkCore.AutoFixture?logo=nuget&style=flat-square)](https://www.nuget.org/packages/EntityFrameworkCore.AutoFixture/)
[![GitHub](https://img.shields.io/github/license/aivascu/EntityFrameworkCore.AutoFixture?logo=MIT&style=flat-square)](https://licenses.nuget.org/MIT)

**EntityFrameworkCore.AutoFixture** is a library that helps with testing code that uses [Entity Framework](https://docs.microsoft.com/en-us/ef/core/), by reducing the boilerplate code necessary to set up database contexts (see [examples](#examples)), with the help of [AutoFixture](https://github.com/AutoFixture/AutoFixture).

Unlike other libraries for faking EF contexts, **EntityFrameworkCore.AutoFixture** does not use mocking frameworks or
dynamic proxies in to create database contexts, instead it uses the actual database [providers](https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/). This ensures the tests will behave a lot more similar to the code in the production environment, with little to no effort.

#### :warning: .NET Standard 2.0 in EF Core v3.0.x :warning:

Entity Framework Core `v3.0.0` - `v3.0.3` are targeting `netstandard2.1`, which means they are not compatible with
target frameworks that support at most `netstandard2.0` (`>= net47` and `netcoreapp2.1`).
Versions after `v3.1` are targeting `netstandard2.0`. If you've encountered this issue consider upgrading to a later
version of Entity Framework Core.

## Features

**EntityFrameworkCore.AutoFixture** offers three customizations to help your unit testing workflow:

- `InMemoryCustomization` - Customizes fixtures to create contexts using the In-Memory database provider
- `SqliteCustomization` - Customizes fixtures to create contexts using the SQLite database provider
- `DbContextCustomization` - A base class for database provider customizations. Can be used, in more advanced scenarios, for example, to extend the existing functionality or create customizations for other providers.

## Examples

The examples below demonstrate, the possible ways of using the library in [xUnit](https://xunit.net) test projects, both with `[Fact]` and `[Theory]` tests.

The library is not limited to `xUnit` and can be used with other testing frameworks like [NUnit](https://nunit.org) and [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest), since it only provides the customizations.

### Using In-Memory database provider

By default this customization will configure all contexts to use the [in-memory](https://docs.microsoft.com/en-us/ef/core/testing/testing-without-the-database#in-memory-provider) database provider for Enity Framework, and will create the database, giving you a ready to use context.

```csharp
[Fact]
public async Task CanSavesCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new InMemoryCustomization());
    var context = fixture.Create<TestDbContext>();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

With the default configuration, the custmization will set the store name to `TestDatabase` and will suffix it with a random string to ensure the name is unique. After the context is created it will run `Database.EnsureCreated()` to create the database.

This behavior can be changed by setting the corresponding values in the customizaiton initializer.

```csharp
[Fact]
public async Task CanSavesCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new InMemoryCustomization
    {
        DatabaseName = "MyCoolDatabase", // Sets the store name to "MyCoolDatabase"
        UseUniqueNames = false, // No suffix for store names. All contexts will connect to same store
        OnCreate = OnCreateAction.Migrate // Will run Database.Migrate()
                                          // Use OnCreateAction.None to skip creating the database
    });
    var context = fixture.Create<TestDbContext>();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

To encapsulate the configuration and remove even more of the boilerplate, use the `AutoData` attributes offered by AutoFixture.

```csharp
public class PersistenceDataAttribute : AutoDataAttribute
{
    public PersistenceDataAttribute()
        : base(() => new Fixture().Customize(new InMemoryCustomization {
            UseUniqueNames = false,
            OnCreate = OnCreateAction.Migrate
        }))
    {
    }
}
```

```csharp
[Theory, PersistenceData] // Notice the data attribute
public async Task CanUseGeneratedContext(TestDbContext context)
{
    // Arrange & Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

### Using SQLite database provider

By default this customization will configure all contexts to use the [SQLite](https://docs.microsoft.com/en-us/ef/core/testing/testing-without-the-database#sqlite-in-memory) database provider for Enity Framework, and will automatically create the database, giving you a ready to use context.

```csharp
[Fact]
public async Task CanUseGeneratedContext()
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

With the default configuration, the custmization will set the connection string to `Data Source=:memory:`, will open the connection and after the context is created it will run `Database.EnsureCreated()` to create the database.

This behavior can be changed by setting the corresponding values in the customizaiton initializer.

```csharp
[Fact]
public async Task CanSavesCustomers()
{
    // Arrange
    var fixture = new Fixture().Customize(new SqliteCustomization
    {
        ConnectionString = "Data Source=MyDatabase.sqlite;Cache=Shared;", // Sets the connection string to connect to a file
        AutoOpenConnection = false, // Disables opening the connection by default. Affects all SqliteConnection instances.
        OnCreate = OnCreateAction.None // Use OnCreateAction.None to skip creating the database.
                                       // Use OnCreateAction.EnsureCreated to run Database.EnsureCreated() automatically
                                       // Use OnCreateAction.Migrate to run Database.Migrate() automatically
    });
    var connection = fixture.Freeze<SqliteConnection>();
    var context = fixture.Create<TestDbContext>();
    connection.Open();
    context.Database.Migrate();

    // Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

To encapsulate the configuration and remove even more of the boilerplate, use the `AutoData` attributes offered by AutoFixture.

```csharp
public class PersistenceDataAttribute : AutoDataAttribute
{
    public PersistenceDataAttribute()
        : base(() => new Fixture().Customize(new SqliteCustomization {
            ConnectionString = "Data Source=MyDatabase;Mode=Memory;Cache=Shared;"
            OnCreate = OnCreateAction.Migrate
        }))
    {
    }
}
```

```csharp
[Theory, PersistenceData] // Notice the data attribute
public async Task CanUseGeneratedContext(TestDbContext context)
{
    // Arrange & Act
    context.Customers.Add(new Customer("Jane Smith"));
    await context.SaveChangesAsync();

    // Assert
    context.Customers.Should().Contain(x => x.Name == "Jane Smith");
}
```

## License

Copyright &copy; 2019 [Andrei Ivascu](https://github.com/aivascu).<br/>
This project is [MIT](https://github.com/aivascu/EntityFrameworkCore.AutoFixture/blob/master/LICENSE) licensed.
