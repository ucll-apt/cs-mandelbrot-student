using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public interface IScheduler
    {
        /// <summary>
        /// Run all jobs.
        /// </summary>
        /// <param name="planner">Planner that provides all jobs.</param>
        void Schedule( IPlanner planner );
    }

    public class SingleThreadScheduler : IScheduler
    {
        public void Schedule( IPlanner planner )
        {
            // Run all jobs on current thread
        }
    }

    public class ManualThreadingScheduler : IScheduler
    {
        public void Schedule( IPlanner planner )
        {
            /*
                Use the System.Threading.Thread class to create threads. Creates as many threads as there are processors available.

                Each thread looks for the next available job and runs it. A thread dies when there's no jobs left.

                You will need shared state (i.e., a variable read/written to by each thread) in order to keep track of which job is next.
                Make sure to use a robust solution so that no two threads perform the same jobs and no jobs are skipped.
            */
        }
    }

    public class ThreadproolScheduler : IScheduler
    {
        public void Schedule( IPlanner planner )
        {
            /*
                This scheduler should rely on the ThreadPool class, more specifically its QueueUserWorkItem method.
                Create a work item for each individual job; don't try to assign multiple jobs at once.

                The difficulty lies in finding a way to wait for all work items to be finished.
                You are prohibited from using busy waiting: the waiting itself must use a minimal amount of CPU.

                Hint: What you actually need is a condition variable. However, they are not directly supported by C#, but they can be easily faked.
                ====
            */
        }
    }

    public class ParallelScheduler : IScheduler
    {
        public void Schedule( IPlanner planner )
        {
            /*
                Rely on the class `System.Threading.Tasks.Parallel` to run all jobs in parallel.
            */
        }
    }

    public class TaskScheduler : IScheduler
    {
        public void Schedule( IPlanner planner )
        {
            /*
                Rely on `System.Threading.Tasks.Task`. Create one `Task` for each job.
            */
        }
    }    
}
