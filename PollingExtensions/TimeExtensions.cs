using System;

namespace PollExtensions
{
    public static class TimeExtensions
    {
        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, 0, seconds);
        }

        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        public static Times Times(this int times)
        {
            return new Times(times);
        }
    }
    public class Times
    {
        public int Value { get; private set; }
        
        public Times(int value)
        {
            Value = value;
        }
    }
}
