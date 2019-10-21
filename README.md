# AutoFixture.AutoEFCore

AutoEFCore is the logical product of the new [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) in-memory [providers](https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/) and the [AutoFixture](https://github.com/AutoFixture/AutoFixture) library. 

The goal of this project is to minimize the boilerplate work, necessary to test the code that uses Entity Framework Core.

## Examples

The examples below demonstrate, the possible ways of using the library, both with a `[Fact]` and a `[Theory]`.

The library is not however limited to `xUnit` and can be used with other testing frameworks like `NUnit` and `MSTest`, since it only provides a few `Customization` implementations.

I will provide more examples, using different framework, as time goes.

Mainly there are three available customizations at the moment:

- `InMemoryContextCustomization` - by default uses the In-Memory database provider
- `SqliteContextCustomization` - by default uses the SQLite database provider, with an in-memory *connection string* (i.e. `DataSource=:memory:`).
- `DbContextCustomization` - serves as the base customization for the other two implementations.
  Can be used if you'd like to have more flexibility in the way you configure your `Fixture`.

### Using In-Memory database provider

```csharp
[Fact]
public void SaveChanges_ShouldCreateCustomerRecord()
{
    var fixture = new Fixture().Customize(new InMemoryContextCustomization());
    using var context = fixture.Create<TestDbContext>();
    context.Database.EnsureCreated();

    context.Customers.Add(new Customer("John Doe"));
    context.SaveChanges();

    context.Customers.Should().Contain(x => x.Name == "John Doe");

    context.Database.EnsureDeleted();
}

[Theory]
[AutoDomainDataWithInMemoryContext]
public async Task SaveChangesAsync_ShouldCreateCustomerRecord(TestDbContext context)
{
    await using (context)
    {
        await context.Database.EnsureCreatedAsync();

        context.Customers.Add(new Customer("Jane Smith"));
        await context.SaveChangesAsync();

        context.Customers.Should().Contain(x => x.Name == "Jane Smith");

        await context.Database.EnsureDeletedAsync();
    }
}
```

### Using SQLite database provider

When using the SQLite database provider be sure to also *freeze* / *inject* the `SqliteConnection` instance, in order to be able to control its lifetime.
Otherwise the connection might close, which might in its turn fail your tests.

```csharp
[Theory]
[AutoDomainDataWithSqliteContext]
public void Customize_ShouldProvideSqliteContext([Frozen] SqliteConnection connection,
  TestDbContext context, Item item, Customer customer)
{
    using (connection)
    using (context)
    {
        connection.Open();
        context.Database.EnsureCreated();
        context.Items.Add(item);

        context.Customers.Add(customer);
        context.SaveChanges();

        customer.Order(item, 5);
        context.SaveChanges();

        context.Orders.Should().Contain(x => x.CustomerId == customer.Id && x.ItemId == item.Id);

        context.Database.EnsureDeleted();
    }
}
```
