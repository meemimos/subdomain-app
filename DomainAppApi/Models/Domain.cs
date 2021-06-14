using System;
namespace DomainAppApi.Models
{
    public class Domain
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public Array subdomains { get; set; }

    }
}
