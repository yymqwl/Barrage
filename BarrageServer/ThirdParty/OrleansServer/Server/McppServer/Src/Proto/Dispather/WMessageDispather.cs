using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GameFramework;
using WebSocketSharp.Server;

namespace Mcpp
{
    //分发器
    public class WMessageDispather : ABehaviour
    {
        protected readonly Dictionary<ushort, List<IMWHandler>> m_Dict_Handlers = new Dictionary<ushort, List<IMWHandler>>();

        public override bool Init()
        {
            return base.Init();
        }
        public void Load(Assembly assembly)
        {
            var types = AssemblyManager.Instance.GetAllTypesByAttribute(assembly, typeof(MessageHandlerAttribute));
            var iopCodeType = GetParent<RootBehaviour<WebServerModule>>().Owner.OpCodeTypeBv;
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
                IMWHandler iMHandler = Activator.CreateInstance(type) as IMWHandler;
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
        public void RegisterHandler(ushort opcode, IMWHandler handler)
        {
            if (!m_Dict_Handlers.ContainsKey(opcode))
            {
                m_Dict_Handlers.Add(opcode, new List<IMWHandler>());
            }
            m_Dict_Handlers[opcode].Add(handler);
        }
        public virtual void Dispatch(IWebSocketSession session, MessageInfo_Json messageInfo)
        {
            List<IMWHandler> actions;
            if (!m_Dict_Handlers.TryGetValue(messageInfo.Opcode, out actions))
            {
                Log.Error($"消息没有处理: {messageInfo.Opcode}");
                return;
            }
            try
            {
                foreach (IMWHandler ev in actions)
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
