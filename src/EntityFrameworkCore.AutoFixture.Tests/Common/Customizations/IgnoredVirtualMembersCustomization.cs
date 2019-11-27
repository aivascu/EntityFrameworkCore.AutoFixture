using AutoFixture;
using EntityFrameworkCore.AutoFixture.Tests.Common.SpecimenBuilders;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class IgnoredVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoredVirtualMembersSpecimenBuilder());
        }
    }
}
