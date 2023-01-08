using System;
using System.Data;
using System.Linq;
using ProgramIT;
namespace ProgramIT.Controllers
{
    public class GenLoginAuth
    {
        
        private DataTable strtabela { get; set;}
      
      public GenLoginAuth()
        {
 
        }

        private string RandomString(int iloscZnakow)
        {
            Random random = new Random();

            string chars = "abcdefghijklmnoprstuwyzABCDEFGHIJKLMNOPRSTUWYZ1234567890!@$[]*{}";
            return new string(Enumerable.Repeat(chars, iloscZnakow)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


            public string GenerateAuth(int iloscZnakow)
        {
           
     

            string str = RandomString(iloscZnakow);

            return str;


           
        }


  
    }

}

