using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class JobSerializer
    {
        // Jobs with time delay
        JobTimer _jobTimer = new JobTimer();

        // Jobs with no time delay
        JobQueue _jobQueue = new JobQueue();


        public IJob PushTimer(int tickAfter, IJob job)
        {
            _jobTimer.Push(job, tickAfter);
            return job;
        }
        public IJob PushTimer(int tickAfter, Action action) { return PushTimer(tickAfter, new Job(action)); }
        public IJob PushTimer<T1>(int tickAfter, Action<T1> action, T1 t1) { return PushTimer(tickAfter, new Job<T1>(action, t1)); }
        public IJob PushTimer<T1, T2>(int tickAfter, Action<T1, T2> action, T1 t1, T2 t2) { return PushTimer(tickAfter, new Job<T1, T2>(action, t1, t2)); }
        public IJob PushTimer<T1, T2, T3>(int tickAfter, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { return PushTimer(tickAfter, new Job<T1, T2, T3>(action, t1, t2, t3)); }
        
        public void PushQueue(IJob job)
        {
            _jobQueue.Push(job);
        }
        public void PushQueue(Action action) { PushQueue(new Job(action)); }
        public void PushQueue<T1>(Action<T1> action, T1 t1) { PushQueue(new Job<T1>(action, t1)); }
        public void PushQueue<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2) { PushQueue(new Job<T1, T2>(action, t1, t2)); }
        public void PushQueue<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { PushQueue(new Job<T1, T2, T3>(action, t1, t2, t3)); }

        // flush and execute all jobs
        public void Flush()
        {
            _jobTimer.Flush();
            _jobQueue.Flush();
        }
    }
}

