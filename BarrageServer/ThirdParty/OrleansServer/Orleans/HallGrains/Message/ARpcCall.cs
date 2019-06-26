using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using Orleans;

namespace HallGrains
{
    public abstract class ARpcCall<TRequest,TRespone> : IRpcCall where TRequest : class, IMessage  where TRespone : class, IMessage
    {
        public Type GetRequestType()
        {
            return typeof(TRequest);
        }

        public Type GetResponeType()
        {
            return typeof(TRespone);
        }

        public async virtual Task<IMessage> Handle(long userid, IMessage message  , IGrainFactory grainfactory)
        {
            var trequest = message as TRequest;
            if(trequest == null)
            {
                Log.Error($"消息类型转换错误Requst: {GetRequestType().Name}");
                return null;
            }
            IMessage responce = await Run(userid,trequest,grainfactory) as IMessage;

            if(responce == null)
            {
                //Log.Error($"消息类型转换错误responce: {GetResponeType().Name}");
            }
            return  responce;
        }
        public  abstract  Task<TRespone> Run(long userid, TRequest requst, IGrainFactory grainfactory);

    }
}
