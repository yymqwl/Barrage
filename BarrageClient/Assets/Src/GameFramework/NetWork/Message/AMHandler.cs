using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class AMHandler<T> : IMHandler where T:class,IMessage
    {
        public Type GetMessageType()
        {
            return typeof(T);
        }
        protected abstract void Run(Session session, T message);
        public  void Handle(Session session, IMessage message)
        {
            T tmessage = message as T;
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {GetType().Name} to {GetMessageType().Name}");
                return;
            }
            this.Run(session, tmessage);
        }
    }
}
