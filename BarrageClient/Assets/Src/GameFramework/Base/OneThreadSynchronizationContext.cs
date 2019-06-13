using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GameFramework
{
    public class OneThreadSynchronizationContext : SynchronizationContext
    {

        public static OneThreadSynchronizationContext Instance { get; } = new OneThreadSynchronizationContext();

        private readonly int m_MainThreadId = Thread.CurrentThread.ManagedThreadId;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private readonly ConcurrentQueue<Action> m_Queue = new ConcurrentQueue<Action>();

        private Action m_Act;

        public void Update()
        {
            while (true)
            {
                if (!this.m_Queue.TryDequeue(out m_Act))
                {
                    return;
                }
                m_Act();
            }
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            // 如果是主线程Post则直接执行回调,不需要进入队列,进入线程之后会在下一帧处理消息
            if (Thread.CurrentThread.ManagedThreadId == this.m_MainThreadId)
            {
                callback(state);
                return;
            }
            this.m_Queue.Enqueue(() => { callback(state); });
        }
    }
}
