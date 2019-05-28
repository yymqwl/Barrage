using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class ABehaviourSet :  IBehaviourSet
    {
        private Dictionary<Type, List<IBehaviour>> m_Dict_IBehaviour = new Dictionary<Type, List<IBehaviour>>();

        public IBehaviour Parent { get; set; }

        public virtual bool Init()
        {
            return true;
        }

        public virtual bool Shut()
        {
            return true;
        }

        public virtual void Update()
        {
            foreach(var vk in m_Dict_IBehaviour)
            {
                foreach(var ib in vk.Value)
                {
                    ib.Update();
                }
            }
        }

        public T GetIBehaviour<T>() where T : class, IBehaviour
        {
            return GetIBehaviour(typeof(T)) as T;
        }

        public IBehaviour GetIBehaviour(Type tp)
        {
            List<IBehaviour> Temp_List = null;
            if(m_Dict_IBehaviour.TryGetValue(tp,out Temp_List))
            {
                return Temp_List[0];
            }
            return null;
        }

        public T[] GetIBehaviours<T>() where T : class, IBehaviour
        {
            return GetIBehaviour(typeof(T)) as T[];
        }

        public IBehaviour[] GetIBehaviours(Type tp)
        {
            List<IBehaviour> Temp_List = null;
            if (m_Dict_IBehaviour.TryGetValue(tp, out Temp_List))
            {
                return Temp_List.ToArray();
            }
            return null;
        }
        public T AddIBehaviour<T>(T ib) where T : class, IBehaviour
        {
            
            Type tp = typeof(T);
            List<IBehaviour> Temp_List = null;
            if(!m_Dict_IBehaviour.TryGetValue(tp, out Temp_List))
            {
                Temp_List = new List<IBehaviour>();
                m_Dict_IBehaviour.Add(tp, Temp_List);
            }
            if(Temp_List.Contains(ib))
            {
                throw new GameFrameworkException($"{tp.Name}Already Add");
            }
            Temp_List.Add(ib);
            ib.Init();
            ib.Parent = this;
            return ib;
        }

        public void RemoveIBehaviour<T>(T ib) where T :class, IBehaviour
        {
            Type tp = typeof(T);
            List<IBehaviour> Temp_List = null;
            if (m_Dict_IBehaviour.TryGetValue(tp, out Temp_List))
            {
                if(Temp_List.Remove(ib))
                {
                    ib.Shut();
                }
                else
                {
                    Log.Debug($"Dont Have {tp.Name}");
                }
            }
            else
            {
                Log.Debug($"Dont Have {tp.Name}");
            }

        }

        public T GetParent<T>() where T : class, IBehaviour
        {
            return Parent as T;
        }
    }
}
