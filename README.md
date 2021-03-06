PollingExtensions
=================

This library makes it a breeze to schedule a repeated action with handlers

Expressions
-----------
    Poll(Func<TARGETCLASS, RETURNTYPE>) - extension method off of any object that sets up the method to poll
	Every(Timespan) - Sets the interval
	For(Times) - How many times to poll (default is 1)
	WithCallback(Action<RETURNTYPE>) - Add a callback handler
	Blocking() - Makes the polling expression blocking i.e. it doesn't run in its own thread
	Start() - starts the polling
	Stop() - stops the polling

For instance, if you have a Producer like so:

    class Consumer
    {
        int GetNext();
    }

You can schedule a blocking read on GetNext() to run 2 times with an interval of 10 seconds like so:

    var list = new List<int>();
    var consumer = new Consumer();
    consumer.Poll(x=>x.GetNext()).Blocking().Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).Start(); //blocking

The default amount of time that a statement will run is 1 time, unless the For(Times) statement is added.
The Start() method here is blocking. If you want to do the same thing asynchronously, simply remove the Blocking() and it will now longer block.

    consumer.Poll(x=>x.GetNext()).Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).Start();

Now if you have this polling occuring asynchronously, you will more than likely want a way to control it. To do this, use the While(Func<bool) statement:
    
    var shouldBeRunning = true;
    consumer.Poll(x=>x.GetNext()).Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).While(()=>shouldBeRunning).Start();
    
    Thread.Sleep(5.Seconds());
    var listCount = list.Count; // there will be 1 item in here as the interval is 5 seconds
    
    shouldBeRunning = false; // this stops the polling
    
    Thread.Sleep(5.Seconds());
    var listCount = list.Count; // this will still be 1

The While() statement takes precident over the For(Times) statement. In the event that the For(Times) is not specified and a While() statement is included, it will run indefinitely until the predicate passed into the While() is false.
    