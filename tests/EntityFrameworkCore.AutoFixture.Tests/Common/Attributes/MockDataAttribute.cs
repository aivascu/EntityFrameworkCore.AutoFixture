using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Attributes;

public class MockDataAttribute : AutoDataAttribute
{
    public MockDataAttribute()
        : base(() => new Fixture()
            .Customize(new AutoMoqCustomization()))
    {
    }
}
