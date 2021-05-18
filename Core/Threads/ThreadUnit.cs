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
        private readonly Thread _thread;
        private readonly ManualResetEventSlim _waitEvent;
        private Job _job;

        public ThreadUnit(string name)
        {
            _waitEvent = new ManualResetEventSlim(true, 1000);
            _thread = new Thread(DoWork);
            _thread.Name = name;
            _thread.IsBackground = true;
            _thread.Start();
        }
        public Job Job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
                if (!_job.IsCompleted) _waitEvent.Set();
            }
        }

        private void DoWork()
        {
            while(true)
            {
                if (Job == null || Job.IsCompleted)
                {
                    _waitEvent.Reset();
                    _waitEvent.Wait();
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
