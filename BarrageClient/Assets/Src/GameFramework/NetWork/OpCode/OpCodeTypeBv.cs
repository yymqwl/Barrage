
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework
{
    public class OpCodeTypeBv : ABehaviour
    {
        private readonly DoubleMap<ushort, Type> m_OpCodeTypes = new DoubleMap<ushort, Type>();

        private readonly Dictionary<ushort, object> m_TypeMessages = new Dictionary<ushort, object>();

        public override bool Init()
        {
            m_OpCodeTypes.Clear();
            m_TypeMessages.Clear();

            return base.Init();
        }
        public void Load(Assembly assembly)
        {
            
            
            foreach (Type type in assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(MessageAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                MessageAttribute messageAttribute = attrs[0] as MessageAttribute;
                if (messageAttribute == null)
                {
                    continue;
                }
                this.m_OpCodeTypes.Add(messageAttribute.Opcode, type);
                this.m_TypeMessages.Add(messageAttribute.Opcode, Activator.CreateInstance(type));

            }
            
        }
        public ushort GetOpcode(Type type)
        {
            return this.m_OpCodeTypes.GetKeyByValue(type);
        }
        public Type GetType(ushort opcode)
        {
            return this.m_OpCodeTypes.GetValueByKey(opcode);
        }
        public object GetInstance(ushort opcode)
        {
            return this.m_TypeMessages[opcode];
        }


        public override bool Shut()
        {
            return base.Shut();
        }


    }
}