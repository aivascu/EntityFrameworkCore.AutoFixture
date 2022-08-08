using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class ConnectionOpeningBehavior : CommandExecutingBehavior
{
    public ConnectionOpeningBehavior(IRequestSpecification specification)
        : base(specification, new OpenDatabaseConnection())
    {
    }
}
