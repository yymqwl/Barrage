using System;
using System.Reflection;

namespace GameFramework
{
    public class AssemblyItem
    {
        Assembly m_Assembly;
        private readonly UnOrderMultiMap<Type, Type> m_Map_Attribute = new UnOrderMultiMap<Type, Type>();

        public UnOrderMultiMap<Type, Type> Map_Attribute { get { return m_Map_Attribute; } }

        public AssemblyItem(Assembly assembly)
        {
            m_Assembly = assembly;
            Load();
        }
        private void Load()
        {
            foreach (Type tp in m_Assembly.GetTypes())
            {
                object[] objects = tp.GetCustomAttributes(typeof(BaseAttribute), false);
                if (objects.Length == 0)
                {
                    continue;
                }

                BaseAttribute baseAttribute = (BaseAttribute)objects[0];
                m_Map_Attribute.Add(baseAttribute.AttributeType, tp);
            }
        }
        

        public Type[] GetAllTypesByAttribute(Type tp)
        {
            return m_Map_Attribute.GetAll(tp);
        }

        public void Clear()
        {
            m_Map_Attribute.Clear();
        }
    }
}
