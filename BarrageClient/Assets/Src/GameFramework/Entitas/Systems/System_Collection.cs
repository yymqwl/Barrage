using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    //
    public class System_Collection<T> where T: ISystem
    {
        private LinkedList<T> m_Systems = new LinkedList<T>();

        public LinkedList<T> Systems
        {
            get
            {
                return m_Systems;
            }
        }

        public T AddSystem(T sys)
        {
            if(sys == null)
            {
                throw new GameFrameworkException("Null syss");
            }
            LinkedListNode<T> current = m_Systems.First;
            while (current != null)
            {
                if (sys.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_Systems.AddBefore(current, sys);
            }
            else
            {
                m_Systems.AddLast(sys);
            }
            return sys;
        }
        public void Exect_Order(Action<T> act)
        {
            foreach(T sys in m_Systems)
            {
                act(sys);
            }
        }
        public void Exect_ReverseOrder(Action<T> act)
        {
            for (LinkedListNode<T> current = m_Systems.Last; current != null; current = current.Previous)
            {
                act(current.Value);
            }
        }

    }
}
