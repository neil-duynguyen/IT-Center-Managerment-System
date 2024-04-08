using AutoFixture.Kernel;
using System.Reflection;

namespace Domain.Tests
{
    public class TypePropertyOmitter : ISpecimenBuilder
    {
        private readonly IEnumerable<string> _properties;

        public TypePropertyOmitter(params string[] properties)
        {
            _properties = properties;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfor = request as PropertyInfo;
            if (propInfor != null && _properties.Contains(propInfor!.Name))
            {
                return new OmitSpecimen();
            }
            return new NoSpecimen();
        }
    }
}
