﻿using System.Collections.Generic;
using System.Net;

namespace GameFramework
{
    public static class NetHelper
    {
        public static string[] GetAddressIPs()
        {
            //获取本地的IP地址
            List<string> addressIPs = new List<string>();
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    addressIPs.Add(address.ToString());
                }
            }
            return addressIPs.ToArray();
        }
        public static IPEndPoint ToIPEndPoint(string host, int port)
        {
            return new IPEndPoint(IPAddress.Parse(host), port);
        }

        public static string GetIp(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            return host;
        }
        public static int GetPort(string address)
        {
            int index = address.LastIndexOf(':');
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return port;
        }
        public static IPEndPoint ToIPEndPoint(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return ToIPEndPoint(host, port);
        }
    }
}
