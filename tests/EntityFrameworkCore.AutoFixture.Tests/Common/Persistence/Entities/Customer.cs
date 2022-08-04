using System;
using System.Collections.Generic;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

public class Customer
{
    public Customer(string name)
    {
        Check.NotEmpty(name, nameof(name));

        this.Name = name;
        this.Orders = new List<Order>();
    }

    private Customer()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public virtual ICollection<Order> Orders { get; private set; }

    public void Order(Item item, int count = 1)
    {
        var order = new Order(item, count, this);
        this.Orders.Add(order);
    }
}
