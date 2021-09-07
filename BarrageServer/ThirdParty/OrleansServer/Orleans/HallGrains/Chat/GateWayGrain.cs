using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IHall;
using Orleans;
using GameFramework;
using System.Threading;
using System.Net;
using Orleans.Concurrency;

namespace HallGrains
{
    [Reentrant]
    public class GateWayGrain :   Grain , IGateWay
    {
        private ObserverSubscriptionManager<IGateWay_Obs> m_IGW_Obs = new ObserverSubscriptionManager<IGateWay_Obs>();
        RpcCallDispather m_RpcCallDispather;


        public override async Task OnActivateAsync()
        {
            m_IGW_Obs.Clear();
            m_RpcCallDispather = new RpcCallDispather();

            var opCodeTypeBv = new OpCodeTypeBv();
            opCodeTypeBv.Load(typeof(IHall.IHello).Assembly);
            m_RpcCallDispather.IOpCodeType = opCodeTypeBv;


            m_RpcCallDispather.Load(typeof(GateWayGrain).Assembly);

            /*
            m_RpcCallDispather.Load(GetType().Assembly);
            */

            await base.OnActivateAsync();
        }
        public async override Task OnDeactivateAsync()
        {

            await base.OnDeactivateAsync();
        }

        public async Task<IMessage> Call(long id, IMessage request)
        {


            return await m_RpcCallDispather.Call(id, request, GrainFactory);
            /*
            IMessage respone = await m_RpcCallDispather.Call(id, request, GrainFactory);
            return respone;
            //return Task.FromResult(msg);
            */
        }


        public Task SubscribeAsync(IGateWay_Obs view)
        {
            UnSubscribeAsync(view).Wait();
            m_IGW_Obs.Subscribe(view);
            return Task.CompletedTask;
        }

        public Task UnSubscribeAsync(IGateWay_Obs view)
        {
            if(m_IGW_Obs.IsSubscribed(view))
            {
                m_IGW_Obs.Unsubscribe(view);
            }
            return Task.CompletedTask;

        }

        public Task Ping(IGateWay_Obs view)
        {
            if (!m_IGW_Obs.IsSubscribed(view))
            {
                m_IGW_Obs.Clear();
            }
            m_IGW_Obs.Subscribe(view);
            return Task.CompletedTask;
        }

        public Task Reply(long id, IMessage msg)
        {
            m_IGW_Obs.Notify((IGateWay_Obs  gw_view) =>
            {
                gw_view.Reply(id,msg);
            });
            return Task.CompletedTask;
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }
        /*
public Task SubscribeAsync(string str_iep, IGateWay_Obs view)
{
if(m_Dict.ContainsKey(str_iep))
{
return Task.CompletedTask;
}
GateWayItem gateWayItem = new GateWayItem();
gateWayItem.GateWay_Viewer = view;
gateWayItem.IPEndPoint = NetHelper.ToIPEndPoint(str_iep);
gateWayItem.LastActiveTime = TimeHelper.ClientNow();
m_Dict.Add(str_iep, gateWayItem);

return Task.CompletedTask;
}

public Task UnSubscribeAsync(string str_iep)
{
GateWayItem gateWayItem;
if (!m_Dict.TryGetValue(str_iep,out gateWayItem))
{
return Task.CompletedTask;
}
gateWayItem.
}

public Task<bool> Ping(string iep)
{
throw new NotImplementedException();
}
*/

    }
}
