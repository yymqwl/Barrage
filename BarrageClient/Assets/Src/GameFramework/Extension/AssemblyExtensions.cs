using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework
{


    public static class AssemblyExtensions
    {
        
        public static Type[] Find(this Assembly assembly,Predicate<Type> predicate)
        {
            List<Type> Temp_Types = new List<Type>();
            foreach(Type tp in assembly.GetTypes())
            {
                if(predicate(tp))
                {
                    Temp_Types.Add(tp);
                }
            }
            return Temp_Types.ToArray();
        }
    }
}