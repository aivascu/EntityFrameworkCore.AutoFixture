# Using the In-Memory provider

```csharp
[Fact]
public void SaveChanges_ShouldCreateCustomerRecord()
{
    var fixture = new Fixture().Customize(new InMemoryContextCustomization());
    using (var context = fixture.Create<TestDbContext>())
    {
        context.Database.EnsureCreated();

        context.Customers.Add(new Customer("John Doe"));
        context.SaveChanges();

        context.Customers.Should().Contain(x => x.Name == "John Doe");
    }
}
```
