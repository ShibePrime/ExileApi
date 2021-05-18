using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore
{
    [DebuggerDisplay("Name: {Name}, Elapsed: {ElapsedMs}, Completed: {IsCompleted}, Failed: {IsFailed}")]
    public class Job
    {
        public Action Work { get; set; }
        public string Name { get; set; }
        public double ElapsedMs { get; set; }
        public double TimeoutMs { get; set; }

        public volatile bool IsCompleted;
        public volatile bool IsFailed;
        public volatile bool IsStarted;

        private readonly Stopwatch _stopwatch;
        public Job(string name, Action work, double timeoutMs = 500)
        {
            Name = name;
            Work = work;
            TimeoutMs = timeoutMs;
            _stopwatch = new Stopwatch();
        }       

        public void Run()
        {
            if (IsCompleted) return;

            try
            {
                _stopwatch.Restart();
                Work?.Invoke();
            }
            catch (Exception e)
            {
                DebugWindow.LogError(e.ToString());
                IsFailed = true;
            }
            finally
            {
                ElapsedMs = _stopwatch.Elapsed.TotalMilliseconds;
                IsCompleted = true;
            }
        }
    }
}
