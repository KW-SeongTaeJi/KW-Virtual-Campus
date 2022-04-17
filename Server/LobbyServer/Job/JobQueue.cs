using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public class JobQueue
    {
        Queue<IJob> _q = new Queue<IJob>();
        object _lock = new object();


        public void Push(IJob job)
        {
            lock (_lock)
            {
                _q.Enqueue(job);
            }
        }

        public void Flush()
        {
            while (true)
            {
                IJob job;

                lock (_lock)
                {
                    if (_q.Count == 0)
                        break;

                    job = _q.Dequeue();
                }

                job.Execute();
            }
        }
    }
}
