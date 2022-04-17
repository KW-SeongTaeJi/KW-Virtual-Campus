using NetworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    class JobTimerElement : IComparable<JobTimerElement>
    {
        public IJob _job;
        public int _execTick;

        public int CompareTo(JobTimerElement other)
        {
            return other._execTick - _execTick;
        }
    }

    public class JobTimer
    {
        PriorityQueue<JobTimerElement> _pq = new PriorityQueue<JobTimerElement>();

        object _lock = new object();


        public void Push(IJob job, int tickAfter = 0)
        {
            JobTimerElement jobElement = new JobTimerElement();
            jobElement._execTick = System.Environment.TickCount + tickAfter;
            jobElement._job = job;

            lock (_lock)
            {
                _pq.Push(jobElement);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now = System.Environment.TickCount;

                JobTimerElement jobElement;

                lock (_lock)
                {
                    if (_pq.Count == 0)
                        break;

                    jobElement = _pq.Peek();
                    if (jobElement._execTick > now)
                        break;

                    _pq.Pop();
                }

                jobElement._job.Execute();
            }
        }
    }
}
