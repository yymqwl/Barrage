using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{

    public delegate void ContextEntityChanged<TEntity>(Context<TEntity> context, TEntity entity) where TEntity : Entity;
    public delegate void ContextGroupChanged<TEntity>(Context<TEntity> context, Group<TEntity> group) where TEntity : Entity;
    public interface IContext : ISerialize
    {

        ContextInfo ContextInfo { get; }

        void Init();
        void Dispose();
        
        IComponentPool ComponentPool();
        void DestroyAllEntities();


    }
}
