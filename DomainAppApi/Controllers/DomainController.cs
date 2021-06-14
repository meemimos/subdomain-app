using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DomainAppApi.Models;
using DomainAppApi.Services;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Text;


namespace DomainAppApi.Controllers
{
    //[ApiController]
    //[Route("subdomain/[controller]")]
    //public class DomainController : ControllerBase
    //{
    //    public DomainController() { }

    //    [Route("enumerate"), HttpGet]
    //    public ActionResult<List<Domain>> GetAll() =>
    //    DomainServices.GetAll();

    //    [HttpGet("{id}")]
    //    public ActionResult<Domain> Get(int id)
    //    {
    //        var domain = DomainServices.Get(id);

    //        if (domain == null)
    //            return NotFound();

    //        return domain;
    //    }

        

    //    [HttpPost]
    //    public IActionResult Create(Domain domain)
    //    {
    //        DomainServices.Add(domain);
    //        return CreatedAtAction(nameof(Create), new { id = domain.Id }, domain);
    //    }



    //    [HttpPut("{id}")]
    //    public IActionResult Update(int id, Domain domain)
    //    {
    //        if (id != domain.Id)
    //            return BadRequest();

    //        var existingDomain = DomainServices.Get(id);
    //        if (existingDomain is null)
    //            return NotFound();

    //        DomainServices.Update(domain);

    //        return NoContent();
    //    }

    //    [HttpDelete("{id}")]
    //    public IActionResult Delete(int id)
    //    {
    //        var domain = DomainServices.Get(id);

    //        if (domain is null)
    //            return NotFound();

    //        DomainServices.Delete(id);

    //        return NoContent();
    //    }


    //}

    [ApiController]
    [Route("subdomain/[controller]")]
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

            if (words.Length == 3)
            {
                domain_name = words[1];
            } else if (words.Length == 2)
            {
                domain_name = words[0];
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

                subdo.Add(text + "." + domain_name + ".com");
            }

            String[] subdomains = subdo.ToArray<String>();

            IEnumerable<string> UniqueSubdomain = subdomains.Distinct<string>();
            Console.WriteLine(UniqueSubdomain);
            return Ok(UniqueSubdomain);
            
        }

        [HttpPost("/subdomain/findipaddresses")] //subdomain/findipaddresses
        public async Task<IActionResult> PostSubdomainToGetIPAddresses(
            [FromBody]String domain_name)
        {
            try
            {
                IPAddress[] addresses = DomainServices.GetIpFromHost(domain_name);

                foreach (IPAddress add in addresses)
                {
                    Console.WriteLine(add);
                }
                return Ok(addresses[0].ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(e.Message);
            }
        }


    }
}