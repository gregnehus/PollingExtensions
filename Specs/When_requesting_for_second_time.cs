using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject(typeof(Producer))]
    public class When_requesting_for_second_time : With<Producer>
    {
        Establish context = () => Subject.GetNext();

        Because of = () => _result = Subject.GetNext();
        
        It should_return_1 = () => _result.ShouldEqual(1);
        
        static int _result;
    }
}