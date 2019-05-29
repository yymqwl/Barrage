using System;
using System.Collections.Generic;
using System.Reflection;
namespace GameFramework
{
    public class AssemblyManager : TInstance<AssemblyManager>
    {
        private Dictionary<Assembly, AssemblyItem> m_Dict = new Dictionary<Assembly, AssemblyItem>();

        public void Add(Assembly assembly)
        {
            if(m_Dict.ContainsKey(assembly))
            {
                return;
            }
            AssemblyItem assemblyItem = new AssemblyItem(assembly);
            m_Dict.Add(assembly, assemblyItem);
        }
        public AssemblyItem GetAssemblyItem(Assembly  assembly)
        {
            AssemblyItem assemblyItem;
            m_Dict.TryGetValue(assembly, out assemblyItem);
            return assemblyItem;
        }
        public Type[] GetAllTypesByAttribute(Assembly assembly,Type tp)
        {
            var assemblyItem = GetAssemblyItem(assembly);
            return assemblyItem.GetAllTypesByAttribute(tp);
        }

    }
}
