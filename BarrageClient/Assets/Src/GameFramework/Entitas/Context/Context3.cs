using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    
    /*
    public delegate void ContextEntityChanged<TEntity>(Context<TEntity> context, IEntity entity) where TEntity : Entity;
    public delegate void ContextGroupChanged<TEntity>(Context<TEntity> context, Group<TEntity> group) where TEntity : Entity;
    
    public class Context<TEntity> : IContext where TEntity : Entity
    {
        
    
        public ContextInfo ContextInfo
        { get { return m_ContextInfo; } }

   
        private readonly ContextInfo m_ContextInfo;
        

        readonly Stack<TEntity> m_ReusableEntities = new Stack<TEntity>();


        private readonly CObjectPool<List<GroupChanged<TEntity>>> m_GroupChangedListPool;

        IEntity[] m_EntitiesCache;

        readonly Dictionary<IMatcher<TEntity>, Group<TEntity>> m_Groups = new Dictionary<IMatcher<IEntity>, Group<IEntity>>();

        readonly List<Group<IEntity>>[] m_GroupsForIndex;


        /// Occurs when an entity gets created.
        public event ContextEntityChanged<IEntity> OnEntityCreated;

        /// Occurs when an entity got destroyed.
        public event ContextEntityChanged<IEntity> OnEntityDestroyed;


        /// Occurs when a group gets created for the first time.
        public event ContextGroupChanged<IEntity> OnGroupCreated;


        readonly EntityComponentChanged m_CachedEntityChanged;
        readonly EntityComponentReplaced m_CachedComponentReplaced;
        readonly EntityEvent m_CachedDestroyEntity;


        public HashSet<IEntity> Entities
        {
            get { return m_ContextData.m_Entities; }
        }
        public long CreationIndex
        {
            get
            {
                return m_ContextData.m_CreationIndex;
            }
            set
            {
                m_ContextData.m_CreationIndex = value;
            }
        }

        public HashSet<IEntity> m_Entities = new HashSet<IEntity>(EntityEqualityComparer<IEntity>.comparer);
        public long m_CreationIndex;

        IEntity m_SingleEntityCache;

        public IComponentPool ComponentPool()
        {
            return IComponentPool.Instance;
        }
        public Context(ContextInfo contextInfo):this(0, contextInfo)
        {
            
        }
        public Context(long startCreationIndex, ContextInfo contextInfo)
        {
            m_ContextData = new ContextData<IEntity>();
            m_ContextData.m_CreationIndex = startCreationIndex;

            m_ContextInfo = contextInfo;
            m_SingleEntityCache = null;

            m_GroupsForIndex = new List<Group<IEntity>>[m_ContextInfo.ComponentsLookup.TotalComponents()];
            m_GroupChangedListPool = new CObjectPool<List<GroupChanged<IEntity>>>();

            m_CachedEntityChanged = updateGroupsComponentAddedOrRemoved;
            m_CachedComponentReplaced = updateGroupsComponentReplaced;
            m_CachedDestroyEntity = DestroyEntity;
        }
        
        

        public IEntity CreateEntity()
        {
            IEntity entity;
            if(m_ReusableEntities.Count>0)
            {
                entity = m_ReusableEntities.Pop();
            }
            else
            {
                entity = Activator.CreateInstance<IEntity>();
                entity.Initialize(m_ContextInfo);
            }

            
            entity.Id =  CreationIndex++;
            entity.Init();
            Entities.Add(entity);

            m_EntitiesCache = null;

            entity.OnComponentAdded += m_CachedEntityChanged;
            entity.OnComponentRemoved += m_CachedEntityChanged;
            entity.OnComponentReplaced += m_CachedComponentReplaced;

            entity.OnDestroyEntity += m_CachedDestroyEntity;


            if (OnEntityCreated != null)
            {
                OnEntityCreated(this, entity);
            }

            return entity;
        }
        public IEntity[] GetEntities()
        {
            if (m_EntitiesCache == null)
            {
                m_EntitiesCache = new IEntity[Entities.Count];
                Entities.CopyTo(m_EntitiesCache);
            }

            return m_EntitiesCache;
        }
        /// <summary>
        /// 单例调用
        /// </summary>
        /// <returns></returns>
        public IEntity GetSingleEntity()
        {

            if (m_SingleEntityCache == null)
            {
                var c = Entities.Count;
                if (c == 1)
                {
                    using (var enumerator = Entities.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        m_SingleEntityCache = enumerator.Current;
                    }
                }
                else if (c == 0)
                {
                    m_SingleEntityCache = CreateEntity();
                }
                else
                {
                    throw new GameFrameworkException("more than one entity");
                }
            }

            return m_SingleEntityCache;
        }

        public void DestroyAllEntities()
        {
            var entities = GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                DestroyEntity(entities[i]);
            }
        }        /// Determines whether the context has the specified entity.
        public bool HasEntity(IEntity entity)
        {
            return Entities.Contains(entity);
        }


        /// Returns a group for the specified matcher.
        /// Calling context.GetGroup(matcher) with the same matcher will always
        /// return the same instance of the group.
        public Group<IEntity> GetGroup(IMatcher<IEntity> matcher)
        {
            Group<IEntity> group;
            if (!m_Groups.TryGetValue(matcher, out group))
            {
                group = new Group<IEntity>(matcher);
                var entities = GetEntities();
                for (int i = 0; i < entities.Length; i++)
                {
                    group.HandleEntitySilently(entities[i]);
                }

                m_Groups.Add(matcher, group);

                for (int i = 0; i < matcher.Indices.Length; i++)
                {
                    var index = matcher.Indices[i];
                    if (m_GroupsForIndex[index] == null)
                    {
                        m_GroupsForIndex[index] = new List<Group<IEntity>>();
                    }

                    m_GroupsForIndex[index].Add(group);
                }

                if (OnGroupCreated != null)
                {
                    OnGroupCreated(this, group);
                }
            }

            return group;
        }

        void updateGroupsComponentAddedOrRemoved(IEntity entity, int index, IComponent component)
        {
            var groups = m_GroupsForIndex[index];
            if (groups != null)
            {
                var events = m_GroupChangedListPool.Fetch< List<GroupChanged<IEntity>> >();

                var tEntity = (IEntity)entity;

                for (int i = 0; i < groups.Count; i++)
                {
                    events.Add(groups[i].HandleEntity(tEntity));
                }

                for (int i = 0; i < events.Count; i++)
                {
                    var groupChangedEvent = events[i];
                    if (groupChangedEvent != null)
                    {
                        groupChangedEvent(
                            groups[i], tEntity, index, component
                        );
                    }
                }
                events.Clear();
                m_GroupChangedListPool.Recycle(events);
            }
        }

        void updateGroupsComponentReplaced(IEntity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            var groups = m_GroupsForIndex[index];
            if (groups != null)
            {

                var tEntity = (IEntity)entity;

                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].UpdateEntity(
                        tEntity, index, previousComponent, newComponent
                    );
                }
            }
        }


        public void DestroyEntity(Entity entity)
        {
            var ientity = (IEntity)entity;
            var bremoved = Entities.Remove(ientity);
            if(!bremoved)
            {
                throw new GameFrameworkException($"DestroyEntity{entity.GetType()}Failed");
            }
            m_EntitiesCache = null;

            if (OnEntityDestroyed != null)
            {
                OnEntityDestroyed(this, ientity);
            }
            entity.Dispose();
            m_ReusableEntities.Push(ientity);

        }

        /// Removes all event handlers
        /// OnEntityCreated, OnEntityWillBeDestroyed,
        /// OnEntityDestroyed and OnGroupCreated
        public void RemoveAllEventHandlers()
        {
            OnEntityCreated = null;
            OnEntityDestroyed = null;
            OnGroupCreated = null;
        }





    }*/
}
