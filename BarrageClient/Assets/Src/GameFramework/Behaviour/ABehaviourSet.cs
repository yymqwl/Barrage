using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class ABehaviourSet :  IBehaviourSet
    {
        protected LinkedList<IBehaviour> m_IBehaviours = new LinkedList<IBehaviour>();
        
        public virtual int Priority { get { return 0; } }

        public IBehaviour Parent { get; set; }

        public virtual bool Init()
        {

            for (LinkedListNode<IBehaviour> current = m_IBehaviours.Last; current != null; current = current.Previous)
            {
                current.Value.Init();
            }
            return true;
        }

        public virtual bool ShutDown()
        {

            for (LinkedListNode<IBehaviour> current = m_IBehaviours.Last; current != null; current = current.Previous)
            {
                current.Value.ShutDown();
            }
            return true;
        }

        public virtual void Update()
        {
            foreach(IBehaviour ib in m_IBehaviours)
            {
                ib.Update();
            }
        }

        public T GetIBehaviour<T>() where T : class, IBehaviour
        {
            return GetIBehaviour(typeof(T)) as T;
        }

        public IBehaviour GetIBehaviour(Type tp)
        {
            foreach (IBehaviour ib in m_IBehaviours)
            {
                if(ib.GetType() == tp)
                {
                    return ib;
                }
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
            foreach (IBehaviour ib in m_IBehaviours)
            {
                if (ib.GetType() == tp)
                {
                    Temp_List.Add(ib);
                }
            }
            return Temp_List.ToArray();
        }
        public T AddIBehaviour<T>(T ib) where T : class, IBehaviour
        {
            if(ib==null)
            {
                throw new GameFrameworkException("ib Null");
            }
            LinkedListNode<IBehaviour> current = m_IBehaviours.First;
            while (current != null)
            {
                if (ib.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_IBehaviours.AddBefore(current, ib);
            }
            else
            {
                m_IBehaviours.AddLast(ib);
            }
            ib.Parent = this;
            return ib;
        }

        public void RemoveIBehaviour<T>(T ib) where T :class, IBehaviour
        {
            if( !m_IBehaviours.Contains(ib) )
            {
                throw new GameFrameworkException("Not Contains Ib");
            }
            m_IBehaviours.Remove(ib);
            ib.Parent = null;
        }

        public T GetParent<T>() where T : class, IBehaviour
        {
            return Parent as T;
        }
    }
}
