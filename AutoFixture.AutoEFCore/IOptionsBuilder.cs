using System;
using Microsoft.EntityFrameworkCore;

namespace AutoFixture.AutoEFCore
{
    public interface IOptionsBuilder
    {
        object Build(Type type);

        DbContextOptions<TContext> Build<TContext>() where TContext : DbContext;
    }
}
