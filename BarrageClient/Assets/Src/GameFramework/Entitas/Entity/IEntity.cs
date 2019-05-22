using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public interface IEntity
    {
        event EntityComponentChanged OnComponentAdded;
        event EntityComponentChanged OnComponentRemoved;
        event EntityComponentReplaced OnComponentReplaced;
        event EntityEvent OnCreateEntity;
        event EntityEvent OnDestroyEntity;

        event EntityEvent OnInitEntity;
        event EntityEvent OnDisposeEntity;
        long Id { get; set; }
    }
}
