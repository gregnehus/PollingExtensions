using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject(typeof(Poller))]
    public class When_polling_twice_asynchronously_with_while_predicate_that_expires_before_times
    {
        Because of = () =>
                     {
                         Producer.Poll(x => x.GetNext()).Every(2.Seconds()).While(() => _shouldRun).WithCallback(x => Results.Add(x)).For(2.Times()).Start();
 
                         Thread.Sleep(2.Seconds());
                         _shouldRun = false;
                     };

        It should_have_one_result = () => Results.Count.ShouldEqual(1);
        It should_have_first_value = () => Results.First().ShouldEqual(0);
        
        static readonly Producer Producer = new Producer();
        static bool _shouldRun = true;
        static readonly List<int> Results = new List<int>();
    }
}