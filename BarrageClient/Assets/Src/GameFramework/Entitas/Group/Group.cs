using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    
    public delegate void GroupChanged<TEntity>(
    Group<TEntity> group, TEntity entity, int index, IComponent component
) where TEntity : Entity;

    public delegate void GroupUpdated<TEntity>(
        Group<TEntity> group, TEntity entity, int index,
        IComponent previousComponent, IComponent newComponent
    ) where TEntity : Entity;

    
    public class Group<TEntity> where TEntity : Entity
    {
        public event GroupChanged<TEntity> OnEntityAdded;
        public event GroupChanged<TEntity> OnEntityRemoved;
        public event GroupUpdated<TEntity> OnEntityUpdated;


        readonly HashSet<TEntity> m_Entities = new HashSet<TEntity>();

        public int Count { get { return m_Entities.Count; } }
        public IMatcher<TEntity> Matcher { get { return m_Matcher; } }

        readonly IMatcher<TEntity> m_Matcher;

        //缓存
        TEntity[] m_EntitiesCache;
        TEntity m_SingleEntityCache;
        public Group(IMatcher<TEntity> matcher)
        {
            m_Matcher = matcher;
        }

        /// This is used by the context to manage the group.
        public void HandleEntitySilently(TEntity entity)
        {
            if (m_Matcher.Matches(entity))
            {
                AddEntitySilently(entity);
            }
            else
            {
                RemoveEntitySilently(entity);
            }
        }
        /// This is used by the context to manage the group.
        public void HandleEntity(TEntity entity, int index, IComponent component)
        {
            if (m_Matcher.Matches(entity))
            {
                AddEntity(entity, index, component);
            }
            else
            {
                RemoveEntity(entity, index, component);
            }
        }
 


        /// This is used by the context to manage the group.
        public void UpdateEntity(TEntity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            if (m_Entities.Contains(entity))
            {
                if (OnEntityRemoved != null)
                {
                    OnEntityRemoved(this, entity, index, previousComponent);
                }
                if (OnEntityAdded != null)
                {
                    OnEntityAdded(this, entity, index, newComponent);
                }
                if (OnEntityUpdated != null)
                {
                    OnEntityUpdated(
                        this, entity, index, previousComponent, newComponent
                    );
                }
            }
        }
        public GroupChanged<TEntity> HandleEntity(TEntity entity)
        {
            return m_Matcher.Matches(entity)
                ? (AddEntitySilently(entity) ? OnEntityAdded : null)
                : (RemoveEntitySilently(entity) ? OnEntityRemoved : null);
        }


        bool AddEntitySilently(TEntity entity)
        {
          
            var added = m_Entities.Add(entity);
            if (added)
            {
                m_EntitiesCache = null;
                m_SingleEntityCache = null;
            }
            return added;
        }

        void AddEntity(TEntity entity, int index, IComponent component)
        {
            if (AddEntitySilently(entity) && OnEntityAdded != null)
            {
                OnEntityAdded(this, entity, index, component);
            }
        }
        bool RemoveEntitySilently(TEntity entity)
        {
            var removed = m_Entities.Remove(entity);
            if (removed)
            {
                m_EntitiesCache = null;
                m_SingleEntityCache = null;
            }

            return removed;
        }
        void RemoveEntity(TEntity entity, int index, IComponent component)
        {
            if(RemoveEntitySilently(entity)&& OnEntityRemoved!=null)
            {
                OnEntityRemoved(this,entity,index,component);
            }
        }
        public bool ContainsEntity(TEntity entity)
        {
            return m_Entities.Contains(entity);
        }
        public TEntity[] GetEntities()
        {
            if (m_EntitiesCache == null)
            {
                m_EntitiesCache = new TEntity[m_Entities.Count];
                m_Entities.CopyTo(m_EntitiesCache);
            }

            return m_EntitiesCache;
        }

        /// Fills the buffer with all entities which are currently in this group.
        public List<TEntity> GetEntities(List<TEntity> buffer)
        {
            buffer.Clear();
            buffer.AddRange(m_Entities);
            return buffer;
        }
        public IEnumerable<TEntity> AsEnumerable()
        {
            return m_Entities;
        }
        public HashSet<TEntity>.Enumerator GetEnumerator()
        {
            return m_Entities.GetEnumerator();
        }
        /// Returns the only entity in this group. It will return null
        /// if the group is empty. It will throw an exception if the group
        /// has more than one entity.
        public TEntity GetSingleEntity()
        {
            if (m_SingleEntityCache == null)
            {
                var c = m_Entities.Count;
                if (c == 1)
                {
                    using (var enumerator = m_Entities.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        m_SingleEntityCache = enumerator.Current;
                    }
                }
                else if (c == 0)
                {
                    return null;
                }
                else
                {
                    throw new GameFrameworkException("more than one entity");
                }
            }

            return m_SingleEntityCache;
        }

    }
}
