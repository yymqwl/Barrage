using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TestWebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            //GameMainEntry.Instance.Entry(args);
            
            
            var wss = new WebSocketServer("ws://192.168.0.90:8800");
            wss.Start();
            wss.AddWebSocketService<HelloBv> ("/");
            Console.ReadLine();
            wss.Stop();
            
            
            //if (wss.Start()
            /*
            using (var ws = new WebSocket("ws://192.168.0.90:8800"))
            {
                
                ws.OnOpen += (sender,e)=>
                {
                    Console.WriteLine("receive");
                };
            }*/

            //Console.ReadLine();
        }
    }
}
