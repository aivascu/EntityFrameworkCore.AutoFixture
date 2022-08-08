using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class CommandExecutingBehavior : ISpecimenBuilderTransformation
{
    private readonly IRequestSpecification specification;
    private readonly ISpecimenCommand command;

    public CommandExecutingBehavior(IRequestSpecification specification, ISpecimenCommand command)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(command, nameof(command));

        this.specification = specification;
        this.command = command;
    }

    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        return new CommandExecutingNode(builder, this.specification, this.command);
    }
}
