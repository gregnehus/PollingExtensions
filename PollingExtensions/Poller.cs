using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PollExtensions
{
    public static class Poller
    {
        public static PollSettings<RETURNTYPE> Poll<TARGETCLASS, RETURNTYPE>(this TARGETCLASS obj, Func<TARGETCLASS, RETURNTYPE> read)
        {
            return new PollSettings<RETURNTYPE> { Action = () => read(obj) };
        }
    }

    public class PollSettings<T>
    {
        public Func<T> Action { get; set; }
        TimeSpan _interval = 0.Seconds();
        readonly List<Action<T>> _handlers = new List<Action<T>>();
        Func<bool> _predicate = () => true;
        bool _wasStopped;

        int _times = 1;
        bool _useTimes;
        bool _usePredicate;
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
            _usePredicate = true;
            return this;
        } 
        public PollSettings<T> For(Times times)
        {
            _times = times.Value;
            _useTimes = true;
            return this;
        } 
        public PollSettings<T> Start()
        {
            _wasStopped = false;
            var action = (_useTimes || !_usePredicate)
                                ? () =>
                                  {
                                      for (var x = 0; x<_times; x++)
                                      {
                                          if (_wasStopped)
                                              break;
                                          Do();
                                      }
                                      
                                  }
                                : (Action) (() =>
                                            {
                                                while (_predicate())
                                                {
                                                    Do();
                                                }
                                            });

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

            
            var result = Action();
            _handlers.ForEach(x => x(result));
            Thread.Sleep(_interval);
        }

        public PollSettings<T> Blocking()
        {
            _isAsync = false;
            return this;
        } 
       
        public PollSettings<T> Stop()
        {
            _wasStopped = false;
            return this;
        }
    }
}
