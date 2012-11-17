using System.Collections.Generic;

namespace PollExtensions
{
    public class Consumer
    {
        readonly IProducer _producer;
        List<int> pl = new List<int>();
        public Consumer(IProducer producer)
        {
            _producer = producer;
           

           
        }

        public void Add(int val)
        {
            pl.Add(val);
        }
    }
}
