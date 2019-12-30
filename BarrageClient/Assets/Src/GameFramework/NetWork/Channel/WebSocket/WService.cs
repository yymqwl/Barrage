using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using Microsoft.IO;


namespace GameFramework
{
    public class WService : AService
    {
        private readonly HttpListener m_HttpListener;

        private readonly Dictionary<long, WChannel> m_Channels = new Dictionary<long, WChannel>();

        public RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        public WService(IEnumerable<string> prefixs, Action<AChannel> acceptCallback)
        {
            this.AcceptCallback += acceptCallback;

            this.m_HttpListener = new HttpListener();

            StartAccept(prefixs);
        }

        public WService()
        {
        }

        public override AChannel GetChannel(long id)
        {
            WChannel channel;
            this.m_Channels.TryGetValue(id, out channel);
            return channel;
        }

        public override AChannel ConnectChannel(IPEndPoint ipEndPoint)
        {
            throw new NotImplementedException();
        }

        public override AChannel ConnectChannel(string address)
        {
            ClientWebSocket webSocket = new ClientWebSocket();
            WChannel channel = new WChannel(webSocket, this);
            this.m_Channels[channel.Id] = channel;
            channel.ConnectAsync(address);
            return channel;
        }

        public override void Remove(long id)
        {
            WChannel channel;
            if (!this.m_Channels.TryGetValue(id, out channel))
            {
                return;
            }

            this.m_Channels.Remove(id);
            channel.Dispose();
        }

        public override void Update()
        {

        }

        public async void StartAccept(IEnumerable<string> prefixs)
        {
            try
            {
                foreach (string prefix in prefixs)
                {
                    this.m_HttpListener.Prefixes.Add(prefix);
                }

                m_HttpListener.Start();

                while (true)
                {
                    try
                    {
                        HttpListenerContext m_HttpListenerContext = await this.m_HttpListener.GetContextAsync();

                        HttpListenerWebSocketContext webSocketContext = await m_HttpListenerContext.AcceptWebSocketAsync(null);

                        WChannel channel = new WChannel(webSocketContext, this);

                        this.m_Channels[channel.Id] = channel;

                        this.OnAccept(channel);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode == 5)
                {
                    throw new Exception($"CMD管理员中输入: netsh http add urlacl url=http://*:8080/ user=Everyone", e);
                }

                Log.Error(e);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}