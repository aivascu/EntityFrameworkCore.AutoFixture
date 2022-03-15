using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Tests.Common
{
    public class DelegatingDatabaseFacade : DatabaseFacade
    {
        public DelegatingDatabaseFacade(DbContext context)
            : base(context)
        {
        }

        public Func<bool> OnEnusreCreated { get; set; }

        public override bool EnsureCreated()
        {
            return this.OnEnusreCreated?.Invoke() == true;
        }
    }
}
