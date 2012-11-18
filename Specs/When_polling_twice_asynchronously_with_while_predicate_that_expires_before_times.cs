using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject("Asynchronous")]
    public class When_polling_twice_with_while_predicate_that_expires_before_times
    {
        Because of = () =>
                     {
                         Producer.Poll(x => x.GetNext()).Every(20.Milliseconds()).While(() => _shouldRun).WithCallback(x => Results.Add(x)).For(2.Times()).Start();

                         Thread.Sleep(20.Milliseconds());
                         _shouldRun = false;
                     };

        It should_have_one_result = () => Results.Count.ShouldEqual(1);
        It should_have_first_value = () => Results.First().ShouldEqual(0);
        
        static readonly Producer Producer = new Producer();
        static bool _shouldRun = true;
        static readonly List<int> Results = new List<int>();
    }

    [Subject("Asynchronous")]
    public class When_polling_and_stopping_before_times_elapsed{
        Because of = () =>
        {
            var settings = Producer.Poll(x => x.GetNext()).Every(200.Milliseconds()).WithCallback(x => Results.Add(x)).For(20.Times()).Start();

            Thread.Sleep(2000.Milliseconds());
            settings.Stop();
        };

        It should_have_10_results = () => Results.Count.ShouldEqual(10);

        static readonly Producer Producer = new Producer();
        static readonly List<int> Results = new List<int>();
    }
}