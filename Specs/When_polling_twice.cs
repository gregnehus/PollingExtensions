using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using PollExtensions;

namespace Specs
{
    public class When_polling_twice
    {
        Because of = () => Producer.Poll(x => x.GetNext()).WithCallback(x => Results.Add(x)).Do(2.Times());

        It should_have_two_results = () => Results.Count.ShouldEqual(2);
        It should_have_first_value = () => Results.First().ShouldEqual(0);
        It should_have_second_value = () => Results.Last().ShouldEqual(1);

        static readonly Producer Producer = new Producer();
        static readonly List<int> Results = new List<int>();
    }

    public class When_polling_once_asynchronously
    {
        Because of = () => Producer.Poll(x => x.GetNext()).WithCallback(x => Results.Add(x)).Do(2.Times());

        It should_have_two_results = () => Results.Count.ShouldEqual(2);
        It should_have_first_value = () => Results.First().ShouldEqual(0);
        It should_have_second_value = () => Results.Last().ShouldEqual(1);

        static readonly Producer Producer = new Producer();
        static readonly List<int> Results = new List<int>();
    }
}