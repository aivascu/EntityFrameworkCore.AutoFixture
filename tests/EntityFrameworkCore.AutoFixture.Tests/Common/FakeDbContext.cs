using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.AutoFixture.Tests.Common;

public class FakeDbContext : DbContext
{
    public DatabaseFacade ActualFacade { get; set; }
    public override DatabaseFacade Database => this.ActualFacade;
}