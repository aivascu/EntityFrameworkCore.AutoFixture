using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using EntityFrameworkCore.AutoFixture.Core;

namespace EntityFrameworkCore.AutoFixture.Tests.Common.Customizations
{
    public class VirtualPropertyOmitterCustomization : ICustomization
    {
        public VirtualPropertyOmitterCustomization(IRequestSpecification typeSpecification)
        {
            this.TypeSpecification = typeSpecification;
        }

        public IRequestSpecification TypeSpecification { get; }

        public void Customize(IFixture fixture)
        {
            var omitter = CreateOmitter(this.TypeSpecification);
            fixture.Customizations.Add(omitter);
        }

        public static VirtualPropertyOmitterCustomization ForTypes(params Type[] types)
            => ForTypes(types.AsEnumerable());

        public static VirtualPropertyOmitterCustomization ForTypes(IEnumerable<Type> types)
        {
            var typeSpecifications = types
                .Select(x => new DeclaringTypeSpecification(
                    new ExactTypeSpecification(x)))
                .ToList();

            return new VirtualPropertyOmitterCustomization(
                new OrRequestSpecification(typeSpecifications));
        }

        public static VirtualPropertyOmitterCustomization ForTypesInNamespaces(params Type[] markerTypes)
            => ForTypesInNamespaces(markerTypes.AsEnumerable());

        public static VirtualPropertyOmitterCustomization ForTypesInNamespaces(IEnumerable<Type> markerTypes)
        {
            var typeSpecifications = markerTypes
                .Select(x => new DeclaringTypeSpecification(
                    new TypeNamespaceSpecification(x.Namespace)))
                .ToList();

            return new VirtualPropertyOmitterCustomization(
                new OrRequestSpecification(typeSpecifications));
        }

        private static Omitter CreateOmitter(IRequestSpecification specification)
        {
            return new Omitter(
                new AndRequestSpecification(
                    new VirtualPropertySpecification(),
                    specification));
        }
    }
}
