using System;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Persistence.Entities
{
    public class Order
    {
        public Order(Item item, int count, Customer customer)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.Count = count;

            this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        private Order()
        {
        }

        public Guid Id { get; private set; }
        public int Count { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid ItemId { get; private set; }

        public virtual Customer Customer { get; private set; }
        public virtual Item Item { get; private set; }
    }
}
