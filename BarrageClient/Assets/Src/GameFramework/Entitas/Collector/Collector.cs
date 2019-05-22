using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    
    public class Collector<IEntity>where IEntity: Entity
    {
        /// Returns all collected entities.
        /// Call collector.ClearCollectedEntities()
        /// once you processed all entities.
        public HashSet<IEntity> CollectedEntities { get { return m_CollectedEntities; } }

        /// Returns the number of all collected entities.
        public int Count { get { return m_CollectedEntities.Count; } }

        readonly HashSet<IEntity> m_CollectedEntities;
        readonly Group<IEntity>[] m_Groups;
        readonly GroupEvent[] m_GroupEvents;
        readonly GroupChanged<IEntity> m_AddEntityCache;

        public Collector(Group<IEntity> group, GroupEvent groupEvent):this(new[] {group},new[] { groupEvent })
        {

        }
        public Collector(Group<IEntity>[] groups, GroupEvent[] groupEvents) 
        {
            m_Groups = groups;
            m_CollectedEntities= new HashSet<IEntity>();
            m_GroupEvents = groupEvents;
            if (groupEvents.Length != groupEvents.Length)
            {
                throw new GameFrameworkException("Unbalanced count with groups and group events");
            }
            m_AddEntityCache = AddEntity;
        }


        void AddEntity(Group<IEntity> group, IEntity entity, int index, IComponent component)
        {
            var added = m_CollectedEntities.Add(entity);
        }


        public void Activate()
        {
            for(int i=0;i<m_Groups.Length;++i)
            {
                var group = m_Groups[i];
                var groupevt = m_GroupEvents[i];
                switch(groupevt)
                {
                    case GroupEvent.Added:
                        group.OnEntityAdded -= m_AddEntityCache;
                        group.OnEntityAdded += m_AddEntityCache;
                        break;
                    case GroupEvent.Removed:
                        group.OnEntityRemoved -= m_AddEntityCache;
                        group.OnEntityRemoved += m_AddEntityCache;
                        break;
                    case GroupEvent.AddedOrRemoved:
                        group.OnEntityAdded -= m_AddEntityCache;
                        group.OnEntityAdded += m_AddEntityCache;
                        group.OnEntityRemoved -= m_AddEntityCache;
                        group.OnEntityRemoved += m_AddEntityCache;
                        break;
                    default:
                        Log.Error("Error evt");
                        break;
                }
            }
        }
        public void Deactivate()
        {
            for(int i=0;i<m_Groups.Length;++i)
            {
                var group = m_Groups[i];
                group.OnEntityAdded -= m_AddEntityCache;
                group.OnEntityRemoved -= m_AddEntityCache;
            }
            ClearCollectedEntities();
        }
        public void ClearCollectedEntities()
        {
            m_CollectedEntities.Clear();
        }
        public IEnumerable<TCast> GetCollectedEntities<TCast>() where TCast : IEntity
        {
            return m_CollectedEntities.Cast<TCast>();
        }
    }
    
}
