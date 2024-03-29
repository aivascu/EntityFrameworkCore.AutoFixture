using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities;

public class Item
{
    private Item()
    {
    }

    public Item(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price));
        }

        this.Name = name;
        this.Price = price;
        this.Orders = new List<Order>();
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public virtual ICollection<Order> Orders { get; private set; }
}
