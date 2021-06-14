using System;
using DomainAppApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DomainAppApi.Services
{
    public static class DomainServices
    {
        static List<Domain> Domains { get; }
        static int nextId = 3;
        static DomainServices()
        {
            Domains = new List<Domain>
            {
                new Domain { Id = 1, Name = "google", IpAddress = "" },
                new Domain { Id = 2, Name = "facebook", IpAddress = "" }
            };
        }

        public static List<Domain> GetAll() => Domains;

        public static Domain Get(int id) => Domains.FirstOrDefault(domain => domain.Id == id);

        public static Domain GetByName(string domain) => Domains.Find(x => x.Name == domain);

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

        public static void Add(Domain domain)
        {
            domain.Id = nextId++;
            Domains.Add(domain);
        }

        public static void Delete(int id)
        {
            var domain = Get(id);
            if (domain is null)
                return;

            Domains.Remove(domain);
        }

        public static void Update(Domain domain)
        {
            var index = Domains.FindIndex(domain => domain.Id == domain.Id);
            if (index == -1)
                return;

            Domains[index] = domain;
        }
    }
}

