using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Orleans;

namespace HallGrains
{
    public class RpcCallDispather
    {
        protected readonly Dictionary<ushort, IRpcCall> m_Dict_Handlers = new Dictionary<ushort, IRpcCall>();

        public IOpCodeType IOpCodeType
        {
            get;set;
        }


        public void Load(Assembly assembly)
        {
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
                Type messageType = iMHandler.GetRequestType();
                ushort opcode = IOpCodeType.GetOpcode(messageType);
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
            m_Dict_Handlers[opcode] = handler;
        }



        public async Task<IMessage> Call(long uid, IMessage request, IGrainFactory grainfactory)
        {
            ushort opcode = IOpCodeType.GetOpcode(request.GetType());
            IRpcCall action;
            if (!m_Dict_Handlers.TryGetValue(opcode, out action))
            {
                Log.Error($"消息没有处理: {opcode}");
                return null;
            }
            try
            {
                return await action.Handle(uid,request,grainfactory);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return null;
        }

            /*
        public async Task<IMessage> Call(long uid, MessageInfo messageInfo, IGrainFactory grainfactory )
        {
            List<IRpcCall> actions;
            if (!m_Dict_Handlers.TryGetValue(messageInfo.Opcode, out actions))
            {
                Log.Error($"消息没有处理: {messageInfo.Opcode}");
                
                return ;
            }
            try
            {
                
                if()
                
                foreach (IRpcCall ev in actions)
                {
                     ev.Handle(uid, messageInfo.Message , grainfactory);
                }
                
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


        }*/


        }
    }
