using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var e = new ManualResetEvent(false);
            var waiter = new EventWaiter(e, TimeSpan.FromMilliseconds(1000));
            var targetService = new Someervice();
            InitializeSystem(e, TimeSpan.FromMilliseconds(5000));
            InitializeSystem(e, TimeSpan.FromMilliseconds(5000));
            var service = new ConditionalHolder(targetService, () => waiter.Wait());
            var tasks = Enumerable
                .Range(1, 100)
                .Select(i => service.DoSomeWork());
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Done all!");
        }

        private static async Task InitializeSystem(EventWaitHandle @event, TimeSpan duration)
        {
            await Task
                .Delay(duration)
                .ContinueWith(t => @event.Set());
        }
    }
    class EventWaiter
    {
        private readonly EventWaitHandle @event;
        private readonly TimeSpan timeout;

        public EventWaiter(EventWaitHandle @event, TimeSpan timeout)
        {
            this.@event = @event;
            this.timeout = timeout;
        }

        public void Wait()
        {
            if (!@event.WaitOne(timeout))
            {
                throw new SystemUnititializedException();
            }
        }
    }

    class ConditionalHolder : IService
    {
        private readonly IService target;
        private readonly Action wait;

        public ConditionalHolder(IService target, Action wait)
        {
            this.target = target;
            this.wait = wait;
        }

        public async Task DoSomeWork()
        {
            wait();
            target.DoSomeWork();
        }
    }

    interface IService
    {
        Task DoSomeWork();
    }

    class Someervice : IService
    {
        public async Task DoSomeWork()
        {
            Console.WriteLine("Did some work here...");
        }
    }
}
