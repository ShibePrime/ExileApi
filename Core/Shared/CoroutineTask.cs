namespace ExileCore.Shared
{
    public class CoroutineTask : ICoroutine
    {
        public void Continue()
        {
        }

        public bool IsCompleted { get; private set; }
        public ICoroutine[] Children { get; }
    }

    public interface ICoroutine
    {
        bool IsCompleted { get; }
        ICoroutine[] Children { get; }
        void Continue();
    }
}