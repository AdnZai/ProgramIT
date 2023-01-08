using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using ProgramIT.Controllers;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using ProgramIT;


namespace ProgramIT.Controllers
{
    
    


    [ApiController, Route("[controller]")]
    public class DataController : Controller
    {
        PobierzTabele pobierzTabele = new PobierzTabele();

        [HttpPost, Route("[action]")]
        public IActionResult LoginWeryfikacja([FromBody] WeryfikacjaLogowania post)
        {
           
            dynamic obj = new ExpandoObject();
   
            obj.zalogowany = post.SprawdzDane();
            obj.keyLogin = post.strKluczLogin;
         obj.komunikat = post.komunikat;

            return new JsonResult(obj);

        }

        [HttpPost, Route("[action]")]
        public IActionResult ResetujHaslo([FromBody] WeryfikacjaLogowania post)
        {

            dynamic obj = new ExpandoObject();

            obj.czyReset = post.ResetujHaslo();
            obj.komunikat = post.komunikat;
            return new JsonResult(obj);

        }

        [HttpPost, Route("[action]")]
        public IActionResult RejestrujUzytkownika([FromBody] WeryfikacjaLogowania post)
        {

            dynamic obj = new ExpandoObject();
    

            obj.rejestracja = post.RejestracjaUzytkownika();

            obj.komunikat = post.komunikat;

            return new JsonResult(obj);

        }

        [HttpPost, Route("[action]")]
        public IActionResult AktywujKonto([FromBody] WeryfikacjaLogowania post)
        {

            dynamic obj = new ExpandoObject();

            obj.aktywowane = post.AktywujKonto();
            obj.komunikat = post.komunikat;
            return new JsonResult(obj);

        }


        [HttpPost, Route("[action]")]
        public IActionResult AktywujDostep([FromBody] WeryfikacjaLogowania post)
        {

            dynamic obj = new ExpandoObject();

            obj.aktywowane = post.AktywujDostep();
            obj.komunikat = post.komunikat;
            return new JsonResult(obj);

        }


        

        [HttpPost, Route("[action]")]
        public IActionResult ProgITPobierzTabele([FromBody] ProgramIT post)
        {
            dynamic obj = new ExpandoObject();

            
            post.WczytajTabele("standard");

        
 

            try
            {



                if (post.poprawnieZalogowany)
                {

                    obj.zalogowany = post.poprawnieZalogowany;
                    obj.komunikat = post.komunikat;

                    obj.tabela = pobierzTabele.GetTable(post.TableToReturn);
                    return new JsonResult(obj);
                }
                else
                {
                    obj.zalogowany = post.poprawnieZalogowany;
                    return new JsonResult(obj);
                }



            }
            catch (Exception)
            {
                obj.zalogowany = false;
                return new JsonResult(obj);
            }

        }

        [HttpPost, Route("[action]")]
        public IActionResult ZmianaHasla([FromBody] ProgramIT post)
        {

            dynamic obj = new ExpandoObject();


            if (post.SprawdzLogowanie())
            {

                obj.zalogowany = post.poprawnieZalogowany;
                obj.czyZmieniono = post.ZmianaHasla();
                obj.komunikat = post.komunikat;
                obj.bladDanych = post.bladDanych;

                return new JsonResult(obj);
            }
            else
            {

                obj.zalogowany = false;
                obj.komunikat = post.komunikat;
                obj.bladDanych = post.bladDanych;
                return new JsonResult(obj);
            }
        }


        [HttpPost, Route("[action]")]
        public IActionResult ITEdycja([FromBody] ProgramIT post)
        {
            dynamic obj = new ExpandoObject();
            
            post.WczytajTabele("standard");
            //    post.SprawdzLogowanie();
           
            try
            {



                if (post.poprawnieZalogowany)
                {

                  
                    obj.zalogowany = post.poprawnieZalogowany;
        
              
                    obj.nazwa = post.TableToReturn.Rows[0][1];
                      obj.grupa = post.TableToReturn.Rows[0][2];
                       obj.opis = post.TableToReturn.Rows[0][3];
           
                        obj.haslo = post.SzyfrowanieTekstu.Decrypt(post.TableToReturn.Rows[0][4].ToString(), "programit");
             
                    if (post.TableToReturn.Rows[0][5].ToString() == "1")
                    {
                        obj.status = false;
                    }


                        else

                            {
                        obj.status = true;
                    }
                      



                    return new JsonResult(obj);
                }
                else
                {
                    obj.zalogowany = post.poprawnieZalogowany;
                    obj.komunikat = post.komunikat;
                    return new JsonResult(obj);
                }



            }
            catch (Exception)
            {
                obj.zalogowany = false;
                return new JsonResult(obj);

            }

        }


        [HttpPost, Route("[action]")]
        public IActionResult WczytajComboBox([FromBody] ProgramIT post) 
        {

            dynamic obj = new ExpandoObject();
            post.WczytajTabele("combobox");

            if (post.poprawnieZalogowany)
            {

                obj.zalogowany = true;
               
                obj.komunikat = post.komunikat;

                obj.tabela = pobierzTabele.GetTable(post.TableToReturn);


                return new JsonResult(obj);
            }
            else
            {
                obj.zalogowany = false;
                return new JsonResult(obj);
            }
        }

        [HttpPost, Route("[action]")]
        public IActionResult ProgramITZapisz([FromBody] ProgramIT post)
        {
            dynamic obj = new ExpandoObject();


            post.SprawdzLogowanie();

            try
            {



                if (post.poprawnieZalogowany)
                {


                    obj.zalogowany = post.poprawnieZalogowany;


                    obj.zapisano = post.ZapiszDoBazy();



                    return new JsonResult(obj);
                }
                else
                {
                    obj.zalogowany = post.poprawnieZalogowany;
                    obj.komunikat = post.komunikat;
                    return new JsonResult(obj);
                }



            }
            catch (Exception)
            {
                obj.zalogowany = false;
                return new JsonResult(obj);
            }

        }


        [HttpPost, Route("[action]")]
        public IActionResult ProgramITUsun([FromBody] ProgramIT post)
        {
            dynamic obj = new ExpandoObject();


            post.SprawdzLogowanie();

            try
            {



                if (post.poprawnieZalogowany || post.bladDanych == false)
                {


                    obj.zalogowany = post.poprawnieZalogowany;


                    obj.zapisano = post.UsunZBazy();



                    return new JsonResult(obj);
                }
                else
                {
                    obj.zalogowany = post.poprawnieZalogowany;
                    return new JsonResult(obj);
                }



            }
            catch (Exception)
            {
                obj.zalogowany = false;
                return new JsonResult(obj);
                //   Console.WriteLine(ex.Message);
            }

        }


        [HttpGet, Route("[action]")]
        public IActionResult Htmls()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "www", "web", "htmls");
            var files = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories).Select(i => Path.Combine(Path.GetRelativePath(path, i)).Replace('\\', '/'));
            return new JsonResult(files);
        }
    }
}
