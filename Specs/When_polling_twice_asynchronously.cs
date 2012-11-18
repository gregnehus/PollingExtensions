using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject("Asynchronous")]
    public class When_polling_twice_asynchronously
    {
        Because of = () =>
                     {
                         var start = DateTime.Now;
                         Producer.Poll(x => x.GetNext()).Every(2.Milliseconds()).WithCallback(x => Results.Add(x)).For(2.Times()).Start();
                         _hasItemsAfterCall = Results.Count > 0;
                         _time = DateTime.Now - start;
                         Thread.Sleep(4.Milliseconds());
                     };

        It should_not_block = () => _time.ShouldBeLessThan(1.Seconds());
        It should_not_do_anything_during_initial_call = () => _hasItemsAfterCall.ShouldBeFalse();
        It should_have_two_results = () => Results.Count.ShouldEqual(2);
        It should_have_first_value = () => Results.First().ShouldEqual(0);
        It should_have_second_value = () => Results.Last().ShouldEqual(1);

        static readonly Producer Producer = new Producer();
        static TimeSpan _time;
        static bool _hasItemsAfterCall;
        static readonly List<int> Results = new List<int>();
    }
}