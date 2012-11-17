PollingExtensions
=================

This library makes it a breeze to schedule a repeated action with handlers

For instance, if you have a Producer like so:

    class Consumer
    {
        int GetNext();
    }

You can schedule a read on GetNext() to run 2 times with an interval of 10 seconds like so:

    var list = new List<int>();
    var consumer = new Consumer();
    consumer.Poll(x=>x.GetNext()).Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).Start(); //blocking

The Do() method here is blocking. If you want to do the same thing asynchronously, simply add Async() and it will now longer block.

    consumer.Poll(x=>x.GetNext()).Async().Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).Start();

Now if you have this polling occuring asynchronously, you will more than likely want a way to control it. To do this, use the While(Func<bool) statement:
    
    var shouldBeRunning = true;
    consumer.Poll(x=>x.GetNext()).Async().Every(5.Seconds()).For(2.Times()).WithCallback(x=>list.Add(x)).While(()=>shouldBeRunning).Start();
    Thread.Sleep(5.Seconds());

    var listCount = list.Count; // there will be 1 item in here as the interval is 5 seconds
    
    shouldBeRunning = false; // this stops the polling
    Thread.Sleep(5.Seconds());

    var listCount = list.Count; // this will still be 1
    