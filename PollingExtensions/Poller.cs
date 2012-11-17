using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PollExtensions
{
    public static class Poller
    {
        public static PollSettings<T> Poll<TF, T>(this TF obj, Func<TF, T> read)
        {
            return new PollSettings<T> { Action = () => read(obj) };
        }
    }

    public class PollSettings<T>
    {
        public Func<T> Action { get; set; }
        TimeSpan _interval;
        readonly List<Action<T>> _handlers = new List<Action<T>>();
        bool _isRunning;

        public PollSettings<T> Every(TimeSpan time)
        {
            _interval = time;
            return this;
        }
        public PollSettings<T> WithCallback(Action<T> handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public PollSettings<T> DoUntil(Func<bool> predicate)
        {
            _isRunning = true;
            Task.Factory.StartNew(() =>
            {
                while (!predicate() && _isRunning)
                {
                    Thread.Sleep(_interval);
                    Do();
                }
            });
            return this;
        }

        public PollSettings<T> Do(Times times)
        {
            Enumerable.Repeat(0,times.Value).ToList().ForEach(x=> Do());
            return this;
        }

        private PollSettings<T> Do()
        {
            var result = Action();

            _handlers.ForEach(x => x(result));
            return this;
        }
        public PollSettings<T> Stop()
        {
            _isRunning = false;
            return this;
        }
    }
}
