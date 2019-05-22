using System;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf;

namespace GameFramework
{

    [ProtoContract]
    public class  ContextData<TEntity>
    {
        [ProtoMember(1)]
        public HashSet<TEntity> m_Entities = new HashSet<TEntity>();
        [ProtoMember(2)]
        public long m_CreationIndex;
    }


    public class Context<TEntity> :IContext  where TEntity:Entity
    {

        protected ContextInfo m_ContextInfo;
        protected ContextData<TEntity> m_ContextData;
        Protobuf_Context m_Protobuf_Context;

        readonly Stack<TEntity> m_ReusableEntities = new Stack<TEntity>();
        private readonly CObjectPool<List<GroupChanged<TEntity>>> m_GroupChangedListPool=new CObjectPool<List<GroupChanged<TEntity>>>();
        TEntity[] m_EntitiesCache;
        readonly Dictionary<IMatcher<TEntity>, Group<TEntity>> m_Groups = new Dictionary<IMatcher<TEntity>, Group<TEntity>>();
        readonly Dictionary<int, List<Group<TEntity>>>  m_GroupsForIndex =new Dictionary<int, List<Group<TEntity>>>();


        /// Occurs when an entity gets created.
        public event ContextEntityChanged<TEntity> OnEntityCreated;

        /// Occurs when an entity got destroyed.
        public event ContextEntityChanged<TEntity> OnEntityDestroyed;

        public event ContextGroupChanged<TEntity> OnGroupCreated;

        readonly EntityComponentChanged m_CachedEntityChanged;
        readonly EntityComponentReplaced m_CachedComponentReplaced;

        TEntity m_SingleEntityCache;

        public ContextInfo ContextInfo => m_ContextInfo;
        public Protobuf_Context Protobuf_Context => m_Protobuf_Context;
        public HashSet<TEntity> Entities=> m_ContextData.m_Entities;
        public long CreationIndex
        {
            get
            {
               return  m_ContextData.m_CreationIndex;
            }
            set
            {
                m_ContextData.m_CreationIndex = value;
            }
        }
        public Context():this(0)
        {
            m_CachedEntityChanged = updateGroupsComponentAddedOrRemoved;
            m_CachedComponentReplaced = updateGroupsComponentReplaced;
        }
        public Context(long startCreationIndex)
        {
            m_ContextData = new ContextData<TEntity>();
            m_ContextData.m_CreationIndex = 0;
        }

        public IComponentPool ComponentPool()
        {
            return IComponentPool.Instance;
        }


        public virtual void Init()
        {
            m_Protobuf_Context = new Protobuf_Context();
            m_SingleEntityCache = null;
        }
        #region 序列化PbMap
        protected void CreateMap(Assembly assembly)
        {
            m_ContextInfo = new ContextInfo(GetType().Name);
            
            foreach(Type type in assembly.GetTypes())
            {
                IComponentAttribute[] Icomps = (IComponentAttribute[])type.GetCustomAttributes(typeof(IComponentAttribute), false);
                if(Icomps.Length == 0)
                {
                    continue;
                }
                else if(Icomps.Length>1)
                {
                    throw new GameFrameworkException("Icomps > 0");
                }
                if(!type.IsSubclassOf(typeof(IComponent)))
                {
                    continue;
                }
                if(Icomps[0].ContextType != GetType() )
                {
                    continue;
                }
                var mt = m_Protobuf_Context.RuntimeTypeModel[typeof(IComponent)];
                mt.AddSubType(Icomps[0].PbId ,type);

                m_ContextInfo.DMap.Add(type,Icomps[0].PbId);
            }
            foreach (Type type in assembly.GetTypes())
            {
                EntityAttribute[] entities = (EntityAttribute[])type.GetCustomAttributes(typeof(EntityAttribute), false);
                if (entities.Length == 0)
                {
                    continue;
                }
                else if (entities.Length > 1)
                {
                    throw new GameFrameworkException("entities > 0");
                }
                if (!type.IsSubclassOf(typeof(Entity)))
                {
                    continue;
                }
                if (entities[0].ContextType != GetType())
                {
                    continue;
                }
                var mt = m_Protobuf_Context.RuntimeTypeModel[typeof(Entity)];
                mt.AddSubType(entities[0].PbId, type);
            }
        }
        #endregion
        public TEntity CreateEntity()
        {
            TEntity entity;
            if(m_ReusableEntities.Count>0)
            {
                entity = m_ReusableEntities.Pop();
            }
            else
            {
                entity = Activator.CreateInstance<TEntity>();
                entity.Init(this);
            }


            entity.Id = CreationIndex++;
            Entities.Add(entity);
            entity.Create();

            m_EntitiesCache = null;
            m_SingleEntityCache = null;


            entity.OnComponentAdded += m_CachedEntityChanged;
            entity.OnComponentRemoved += m_CachedEntityChanged;
            entity.OnComponentReplaced += m_CachedComponentReplaced;

            if (OnEntityCreated != null)
            {
                OnEntityCreated(this, entity);
            }


            return entity;
        }
        public TEntity[] GetEntities()
        {
            if (m_EntitiesCache == null)
            {
                m_EntitiesCache = new TEntity[Entities.Count];
                Entities.CopyTo(m_EntitiesCache);
            }

            return m_EntitiesCache;
        }
        #region GetSingleEntity
        public TEntity GetSingleEntity()
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
        #endregion

        public void DestroyAllEntities()
        {
            var entities = GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                DestroyEntity(entities[i]);
            }
        }
        public bool HasEntity(TEntity entity)
        {
            return Entities.Contains(entity);
        }
        public void DestroyEntity(TEntity entity)
        {
            var bremoved = Entities.Remove(entity);
            if (!bremoved)
            {
                throw new GameFrameworkException($"DestroyEntity{entity.GetType()}Failed");
            }
            m_EntitiesCache = null;
            m_SingleEntityCache = null;

            if (OnEntityDestroyed != null)
            {
                OnEntityDestroyed(this, entity);
            }
            entity.Destroy();
            m_ReusableEntities.Push(entity);
        }


        /// Returns a group for the specified matcher.
        /// Calling context.GetGroup(matcher) with the same matcher will always
        /// return the same instance of the group.
        public Group<TEntity> GetGroup(IMatcher<TEntity> matcher)
        {
            Group<TEntity> group;
            if (!m_Groups.TryGetValue(matcher, out group))
            {
                group = new Group<TEntity>(matcher);
                var entities = GetEntities();
                for (int i = 0; i < entities.Length; i++)
                {
                    group.HandleEntitySilently(entities[i]);
                }

                m_Groups.Add(matcher, group);

                for (int i = 0; i < matcher.Indices.Length; i++)
                {
                    var index = matcher.Indices[i];
                    if( !m_GroupsForIndex.ContainsKey(index) )
                    {
                        m_GroupsForIndex.Add(index, new List<Group<TEntity>>());
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

        void updateGroupsComponentAddedOrRemoved(Entity entity, int index, IComponent component)
        {
            List<Group<TEntity>>  groups;
            m_GroupsForIndex.TryGetValue(index,out groups);
            if (groups != null)
            {
                var events = m_GroupChangedListPool.Fetch<List<GroupChanged<TEntity>>>();

                var tEntity = (TEntity)entity;

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
        void updateGroupsComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            List<Group<TEntity>> groups;
            m_GroupsForIndex.TryGetValue(index, out groups);
            if (groups != null)
            {

                var tEntity = (TEntity)entity;

                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].UpdateEntity(
                        tEntity, index, previousComponent, newComponent
                    );
                }
            }
        }

        public virtual void Dispose()
        {
            OnEntityCreated = null;
            OnEntityDestroyed = null;
            OnGroupCreated = null;
        }

        public byte[] Serialize()
        {
            return Protobuf_Context.Serialize(m_ContextData);
        }

        public void Deserialize(byte[] bytes)
        {
           m_ContextData =  Protobuf_Context.Deserialize<ContextData<TEntity>>(bytes);
        }
    }

}
