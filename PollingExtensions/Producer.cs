namespace PollExtensions
{
    public class Producer : IProducer
    {
        int _value;
        
        public int GetNext()
        {
            return _value ++;
        }
    }

    public interface IProducer
    {
        int GetNext();
    }
}