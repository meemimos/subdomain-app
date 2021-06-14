using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DomainAppApi.Services;
using System;
using System.Threading.Tasks;
using System.Net;

using System.Text;


namespace DomainAppApi.Controllers
{
    [Route("subdomain/[controller]")]
    [ApiController]
    public class enumerateController : ControllerBase
    {
        public enumerateController() { }

        public static string GenerateRandomText()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[2];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var firstString = new String(stringChars);

            return firstString;
        }

        [HttpGet("{domain_name}")] //subdomain/enumerate/{domain_name}
        public async Task<IActionResult> GetSubdomain(
            [FromRoute] String domain_name)
        {
            string[] words = domain_name.Split('.');
            string dotWhat;

            if (words.Length == 3)
            {
                dotWhat = words[2];
                domain_name = words[1] + "." + dotWhat;
            } else if (words.Length == 2)
            {
                dotWhat = words[1];
                domain_name = words[0] + "." + dotWhat;
            }


            List<String> subdo = new List<String>();

            for (int i = 0; i < 10; i ++)
            {
                var size = 2;
                var builder = new StringBuilder(size);
                bool lowerCase = false;
                var _random = new Random();

                // Unicode/ASCII Letters are divided into two blocks
                // (Letters 65–90 / 97–122):   
                // The first group containing the uppercase letters and
                // the second group containing the lowercase.  

                // char is a single Unicode character  
                char offset = lowerCase ? 'a' : 'A';
                const int lettersOffset = 26; // A...Z or a..z: length = 26  

                for (int j = 0; j < size; j++)
                {
                    var @char = (char)_random.Next(offset, offset + lettersOffset);
                    builder.Append(@char);
                }

                var text = lowerCase ? builder.ToString().ToLower() : builder.ToString().ToLower();

                subdo.Add(text + "." + domain_name);
            }

            String[] subdomains = subdo.ToArray<String>();

            IEnumerable<string> UniqueSubdomain = subdomains.Distinct<string>();
            Console.WriteLine(UniqueSubdomain);
            return Ok(UniqueSubdomain);
            
        }


        [HttpPost("/subdomain/findipaddresses")] //subdomain/findipaddresses
        public async Task<IActionResult> PostSubdomainToGetIPAddresses(
            [FromBody]string domain_name)
        {
            try
            {
                IPAddress[] addresses = DomainServices.GetIpFromHost(domain_name);
                List<string> ips = new List<string>();
                foreach (IPAddress ip in addresses)
                {
                    Console.WriteLine(ip);
                    ips.Add(ip.ToString());
                }
                return Ok(ips.ToArray());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok("IP not found");
            }
        }


    }
}