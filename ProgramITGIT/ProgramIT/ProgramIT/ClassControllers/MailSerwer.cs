using Microsoft.AspNetCore.Html;
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Encodings.Web;

namespace ProgramIT.Controllers
{
    public class MailSerwer
    {
        public bool czyBladPodczasWysylania = false;

        private string email = "itprog@zmb.pl";
        private string passemail = "NpDtmnRY4i!nQ";
        private string mailHost = "mail.zmb.pl";

        private string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public void WyslijMaila(string adresat, string temat, HtmlContentBuilder tekst)
        {
         
           
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.Credentials = new System.Net.NetworkCredential(email, passemail);
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.Timeout = 5000;


                SmtpServer.Host = mailHost;
                mail = new MailMessage();
                mail.From = new MailAddress(email);
             
                    mail.To.Add("" + adresat + "");


                mail.Subject = temat;



                mail.IsBodyHtml = true;
              
                mail.Body = GetString(tekst);
          
                
             
                SmtpServer.Send(mail);
 
                Console.WriteLine("Wiadomość aktywacyjna została wyslana");

                mail.Attachments.Dispose();
                czyBladPodczasWysylania = false;
            }
            catch (Exception ex)
            {
                czyBladPodczasWysylania = true;
                Console.Write(ex.Message);
            }
        }

    }
}
