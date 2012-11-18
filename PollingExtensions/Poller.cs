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
        TimeSpan _interval = 0.Seconds();
        readonly List<Action<T>> _handlers = new List<Action<T>>();
        Func<bool> _predicate = () => true;
        bool _isRunning;

        int _times = 1;
        bool _isAsync = true;
        

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

        public PollSettings<T> While(Func<bool> predicate)
        {
            _predicate = predicate;
            return this;
        } 
        public PollSettings<T> For(Times times)
        {
            _times = times.Value;
            return this;
        } 
        public PollSettings<T> Start()
        {
            Action action = () => Enumerable.Repeat(0, _times).ToList().ForEach(x => Do());

            if (_isAsync)
            {
                Task.Factory.StartNew(action);
            }else
            {
                action();
            }
            return this;
        }

        private void Do()
        {
            if (!_predicate())
                return;

            Thread.Sleep(_interval);
            var result = Action();
            _handlers.ForEach(x => x(result));
        }

        public PollSettings<T> Blocking()
        {
            _isAsync = false;
            return this;
        } 
       
        public PollSettings<T> Stop()
        {
            _isRunning = false;
            return this;
        }
    }
}
