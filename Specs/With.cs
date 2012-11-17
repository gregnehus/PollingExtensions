using Machine.Specifications;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace Specs
{
    public class With<T> where T: class
    {
        static protected T Subject { get { return Mocks.ClassUnderTest; } }
        static AutoMocker<T> Mocks;
        Establish context = () =>
        {
            Mocks = (AutoMocker<T>)NSubstituteAutoMockerBuilder.Build<T>();
        };
    }
}
