using System;

namespace EntityFrameworkCore.AutoFixture.Common
{
    public interface IOptionsBuilder
    {
        object Build(Type type);
    }
}
