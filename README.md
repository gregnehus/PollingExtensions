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
    consumer.Poll(x=>x.GetNext()).Every(5.Seconds()).WithHandler(x=>list.Add(x)).Do(2.Times());
    // no blocking
    Thread.Sleep(10.Seconds());
