using AutoFixture.AutoEFCore.Tests.Common.SpecimenBuilders;

namespace AutoFixture.AutoEFCore.Tests.Common.Customizations
{
    public class IgnoredVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoredVirtualMembersSpecimenBuilder());
        }
    }
}