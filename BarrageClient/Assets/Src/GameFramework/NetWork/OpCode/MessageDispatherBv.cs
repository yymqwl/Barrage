using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class MessageDispatherBv :ABehaviour , IMessageDispatcher
    {
        protected readonly Dictionary<ushort, List<IMHandler>> m_Dict_Handlers = new Dictionary<ushort, List<IMHandler>>();


        public override bool Init()
        {
            return base.Init();
        }
        
        public void Load(Assembly assembly)
        {
            var types = AssemblyManager.Instance.GetAllTypesByAttribute(assembly,typeof(MessageHandlerAttribute));
            var iopCodeType = GetParent<NetWorkBs>().IOpCodeType;
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MessageHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                MessageHandlerAttribute messageHandlerAttribute = attrs[0] as MessageHandlerAttribute;
                if (messageHandlerAttribute == null)
                {
                    continue;
                }
                IMHandler iMHandler = Activator.CreateInstance(type) as IMHandler;
                if (iMHandler == null)
                {
                    Log.Error($"message handle {type.Name} 需要继承 IMHandler");
                    continue;
                }
                Type messageType = iMHandler.GetMessageType();
                ushort opcode = iopCodeType.GetOpcode(messageType);
                if (opcode == 0)
                {
                    Log.Error($"消息opcode为0: {messageType.Name}");
                    continue;
                }

                RegisterHandler(opcode, iMHandler);

            }
        }
        public void RegisterHandler(ushort opcode, IMHandler handler)
        {
            if(!m_Dict_Handlers.ContainsKey(opcode))
            {
                m_Dict_Handlers.Add(opcode,new List<IMHandler>());
            }
            m_Dict_Handlers[opcode].Add(handler);
        }

        public virtual  void Dispatch(Session session, MessageInfo messageInfo)
        {
            List<IMHandler> actions;
            if (!m_Dict_Handlers.TryGetValue(messageInfo.Opcode, out actions))
            {
                Log.Error($"消息没有处理: {messageInfo.Opcode}");
                return;
            }
            try
            {
                foreach (IMHandler ev in actions)
                {
                    ev.Handle(session, messageInfo.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public override bool ShutDown()
        {
            m_Dict_Handlers.Clear();
            return base.ShutDown();
        }


    }
}
