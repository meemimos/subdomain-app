using System;
using DomainAppApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DomainAppApi.Services
{
    public static class DomainServices
    {
        public static IPAddress[] GetIpFromHost(String host)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(host);

            if (hostInfo == null || hostInfo.AddressList.Length == 0)
                throw new ArgumentException($"Host not found: {host}");

            Console.WriteLine(hostInfo);
            Console.WriteLine(hostInfo.AddressList[0].ToString());
            return hostInfo.AddressList;
        }

        public static void FindSubdomains(string url)
        {
            Console.WriteLine(url);
        }

        
    }
}

