using System;
using AutoFixture;
using AutoFixture.Xunit2;
using EntityFrameworkCore.AutoFixture.Common;
using FluentAssertions;
using Xunit;

namespace EntityFrameworkCore.AutoFixture.Tests
{
    public class DbContextCustomizationTests
    {
        [Theory]
        [AutoData]
        public void Customize_ShouldAddContextBuilderToFixture(
            Fixture fixture,
            DbContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(DbContextSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ShouldAddOptionsBuilderToFixture(
            Fixture fixture,
            DbContextCustomization customization)
        {
            fixture.Customize(customization);

            fixture.Customizations.Should()
                .ContainSingle(x => x.GetType() == typeof(DbContextOptionsSpecimenBuilder));
        }

        [Theory]
        [AutoData]
        public void Customize_ForNullFixture_ShouldThrow(
            DbContextCustomization customization)
        {
            Action act = () => customization.Customize(default);

            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
