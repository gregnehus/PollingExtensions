using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject(typeof(Producer))]
    public class When_requesting_for_first_time : With<Producer>
    {
        Because of = () => _result = Subject.GetNext();
        
        It should_return_0 = () => _result.ShouldEqual(0);
        
        static int _result;
    }
}