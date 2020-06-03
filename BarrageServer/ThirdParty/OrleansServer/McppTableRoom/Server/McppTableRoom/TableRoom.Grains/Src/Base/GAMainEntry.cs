using GameFramework;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    [StatelessWorker]
    public class GAMainEntry :GEntry , IAMainEntry
    {
        LinkedList<IEntry> m_Entrys = new LinkedList<IEntry>();


        public LinkedList<IEntry> Entrys
        {
            get
            {
                return this.m_Entrys;
            }
        }

        public async override Task<bool> Init()
        {
            var bret = true;
            for (LinkedListNode<IEntry> current = m_Entrys.First; current != null; current = current.Next)
            {
                await current.Value.Init();
            }
            return bret;
        }
        public async override Task<bool> ShutDown()
        {
            var bret = true;
            for (LinkedListNode<IEntry> current = m_Entrys.Last; current != null; current = current.Previous)
            {
                await current.Value.ShutDown();
            }
            return bret;
        }

        protected async Task<IEntry> AddIEntry(IEntry  entry)
        {
            LinkedListNode<IEntry> current = m_Entrys.First;
            while (current != null)
            {
                if (await entry.GetPriority() <= await current.Value.GetPriority() )
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                m_Entrys.AddBefore(current, entry);
            }
            else
            {
                m_Entrys.AddLast(entry);
            }

            return (entry);
        }
        public override async Task Update(float dt)
        {
            foreach (IEntry entry in m_Entrys)
            {
                await entry.Update(dt);
            }
        }
    }
}
