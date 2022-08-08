using System.Collections;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace EntityFrameworkCore.AutoFixture.Core;

public class CommandExecutingNode : ISpecimenBuilderNode
{
    private readonly ISpecimenBuilder builder;
    private readonly ISpecimenCommand command;
    private readonly IRequestSpecification specification;

    public CommandExecutingNode(ISpecimenBuilder builder, IRequestSpecification specification, ISpecimenCommand command)
    {
        Check.NotNull(builder, nameof(builder));
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(command, nameof(command));

        this.builder = builder;
        this.specification = specification;
        this.command = command;
    }

    public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
    {
        var composedBuilder = new CompositeSpecimenBuilder(builders);
        return new CommandExecutingNode(composedBuilder, this.specification, this.command);
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (!this.specification.IsSatisfiedBy(request))
            return this.builder.Create(request, context);

        var result = this.builder.Create(request, context);
        this.command.Execute(result, context);

        return result;
    }

    public IEnumerator<ISpecimenBuilder> GetEnumerator() { yield return this.builder; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
