using System.Collections.Generic;
using System.Threading;
using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    [Subject("Asynchronous")]
    public class When_polling_with_while_predicate
    {
        Because of = () =>
                     {
                         Producer.Poll(x => x.GetNext()).Every(10.Milliseconds()).While(() => _shouldRun).WithCallback(x => Results.Add(x)).Start();

                         Thread.Sleep(80.Milliseconds());
                         _shouldRun = false;
                     };

        It should_have_eight_results = () => Results.Count.ShouldEqual(8);

        static readonly Producer Producer = new Producer();
        static bool _shouldRun = true;
        static readonly List<int> Results = new List<int>();
    }
}