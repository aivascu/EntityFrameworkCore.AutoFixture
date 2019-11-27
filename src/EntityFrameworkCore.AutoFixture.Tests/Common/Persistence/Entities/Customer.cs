using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities
{
    public class Customer
    {
        public Customer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

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
}