using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileCore.Threads
{
    public class ThreadManager
    {
        private readonly ConcurrentDictionary<string, ThreadUnit> _threads;

        public ThreadManager()
        {
            _threads = new ConcurrentDictionary<string, ThreadUnit>();
        }

        public bool AddOrUpdateJob(Job job)
        {
            return AddOrUpdateJob(job.Name, job);
        }

        public bool AddOrUpdateJob(string name, Job job)
        {
            if (!_threads.ContainsKey(name))
            {
                var threadUnit = new ThreadUnit(name);
                _threads.AddOrUpdate(name, threadUnit, (key, oldValue) => threadUnit);
            }
            if (_threads[name].Job != null && !_threads[name].Job.IsCompleted) return false;

            _threads[name].Job = job;
            return true;
        }

        public void AbortLongRunningThreads()
        {
            foreach (var thread in _threads)
            {
                var job = thread.Value?.Job;
                if (job == null) continue;
                if (job.ElapsedMs < job.TimeoutMs) continue;

                thread.Value.Abort();
                DebugWindow.LogError($"ThreadManager -> Thread aborted: {thread.Key}, timeout: {job.TimeoutMs}ms, elapsed: {job.ElapsedMs}ms");
            }
        }
    }
}
