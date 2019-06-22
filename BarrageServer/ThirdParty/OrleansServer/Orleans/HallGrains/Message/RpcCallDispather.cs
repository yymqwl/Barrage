using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HallGrains
{
    public class RpcCallDispather
    {
        protected readonly Dictionary<ushort, List<IRpcCall>> m_Dict_Handlers = new Dictionary<ushort, List<IRpcCall>>();

        protected OpCodeTypeBv m_OpCodeTypeBv = new OpCodeTypeBv();

        public void Load(Assembly assembly)
        {
            m_OpCodeTypeBv.Load(assembly);

            var types = AssemblyManager.Instance.GetAllTypesByAttribute(assembly, typeof(MessageHandlerAttribute));
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
                IRpcCall iMHandler = Activator.CreateInstance(type) as IRpcCall;
                if (iMHandler == null)
                {
                    Log.Error($"message handle {type.Name} 需要继承 IMHandler");
                    continue;
                }
                Type messageType = iMHandler.GetMessageType();
                ushort opcode = m_OpCodeTypeBv.GetOpcode(messageType);
                if (opcode == 0)
                {
                    Log.Error($"消息opcode为0: {messageType.Name}");
                    continue;
                }

                RegisterHandler(opcode, iMHandler);

            }
        }
        public void RegisterHandler(ushort opcode, IRpcCall handler)
        {
            if (!m_Dict_Handlers.ContainsKey(opcode))
            {
                m_Dict_Handlers.Add(opcode, new List<IRpcCall>());
            }
            m_Dict_Handlers[opcode].Add(handler);
        }
        public  void Call(long uid, MessageInfo messageInfo)
        {
            List<IRpcCall> actions;
            if (!m_Dict_Handlers.TryGetValue(messageInfo.Opcode, out actions))
            {
                Log.Error($"消息没有处理: {messageInfo.Opcode}");
                return;
            }
            try
            {
                foreach (IRpcCall ev in actions)
                {
                    ev.Handle(uid, messageInfo.Message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


        }


    }
}
