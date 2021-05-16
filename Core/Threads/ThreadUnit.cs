using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExileCore.Threads
{
    public class ThreadUnit
    {
        public Job Job { get; set; }
        private readonly Thread _thread;
        public ThreadUnit(string name)
        {
            _thread = new Thread(DoWork);
            _thread.Name = name;
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void DoWork()
        {
            while(true)
            {
                if (Job == null || Job.IsCompleted)
                {
                    Thread.Sleep(10);
                    continue;
                }
                Job.Run();
            }
        }

        public void Abort()
        {
            try
            {
                Job.IsFailed = true;
                Job.IsCompleted = true;
            }
            finally
            {
                _thread.Abort();
            }
        }
    }
}
