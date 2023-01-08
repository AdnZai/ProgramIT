using System;
using System.Data;
using System.Web;
using Microsoft.AspNetCore;
using ProgramIT;
using Microsoft.AspNetCore.Html;
using System.IO;

namespace ProgramIT.Controllers
{
    public class WeryfikacjaLogowania
    {
        public string strlogin { get; set; }
        public string strhaslo { get; set; }
        public string strKluczLogin { get; set; }
        public int idUzytkownika = 0;
        public string adresEmailUzytkownika = "";
        public bool WeryfikacjaczyAdmin = false;
        public bool WeryfikacjaczyDostep= false;
        public string komunikat = "";
         string nazwaHosta = "185.22.9.9:8484";

        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string mail { get; set; }

        public string kluczDoAktywacjiKonta { get; set; }

        MailSerwer wyslijWiadomosc = new MailSerwer();
        MySQL MySQLconn = new MySQL(null, null, "Database=progit;Data Source = 192.168.50.177; User Id = programit; Password=iN5@sCXao); Connection Timeout = 30; charset=utf8;SslMode=none;");


        SzyfrowanieTekstu szyfrowanieTekstu = new SzyfrowanieTekstu();
        GenLoginAuth genLoginAuth = new GenLoginAuth();

        DataTable tabela = new DataTable();


        public bool ResetujHaslo()
        {
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            tabela.Rows.Add(new object[] { "@Mail", mail });

         
            MySQLconn.NewQuery("SELECT Count(*) FROM Users WHERE Mail = @Mail", tabela);
            if (MySQLconn.ReaderGetInt64() == 0 || MySQLconn.ReaderGetInt64() == -1)
            {
                komunikat = "Brak użytkownika o podanym adresem E-mail";
                return false;
            }
            else
            {
                try
                {
                    MySQLconn.NewQuery("SELECT Login FROM Users WHERE Mail = @Mail", tabela);
                    string login = MySQLconn.ReaderGetString();
                    string newpass = genLoginAuth.GenerateAuth(8);

                    MySQLconn.RunSqlTransaction($"UPDATE Users SET Haslo = '{szyfrowanieTekstu.Encrypt(newpass, "programit")}' WHERE Mail = @Mail", tabela, true);
                    if (MySQLconn.transComit)
                    {
                        var builder = new HtmlContentBuilder();



                        string link = $"<p>Login: {login}</a>";

                        builder.AppendHtml(link);
                        link = $"<p>Hasło: {newpass}</a>";

                        builder.AppendHtml(link);

                        wyslijWiadomosc.WyslijMaila(mail, "Reset hasła Program IT", builder);
                        komunikat = "Na adres e-mail zostalo wysłane nowe haslo";
                        return true;
                    }
                    else
                    {
                        komunikat = "Błąd podczas resetowania hasła";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    komunikat = "Błąd podczas resetowania hasła";
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public bool AktywujDostep()
        {
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            string kluczAktywacjaDost = kluczDoAktywacjiKonta + DateTime.Now.ToString();
            tabela.Rows.Add(new object[] { "@Aktywacja", kluczDoAktywacjiKonta });
            tabela.Rows.Add(new object[] { "@Aktywacja2", kluczAktywacjaDost });
            Console.WriteLine(kluczDoAktywacjiKonta);

            MySQLconn.NewQuery("SELECT Count(*) FROM Users WHERE Aktywacja = @Aktywacja", tabela);
            if (MySQLconn.ReaderGetInt64() == 0 || MySQLconn.ReaderGetInt64() == -1)
            {
                komunikat = "Użytkownik ma już nadany dostęp, lub kod nadania dostępu jest błędny";
                return false;
            }
            else
            {


                try
                {


                    MySQLconn.RunSqlTransaction("UPDATE Users SET Dostep = 1, Aktywacja = @Aktywacja2 WHERE Aktywacja = @Aktywacja", tabela, true);
                    if (MySQLconn.transComit)
                    {
                        try
                        {
                            MySQLconn.NewQuery("SELECT Mail FROM Users WHERE Aktywacja = @Aktywacja2", tabela);
                            var builder = new HtmlContentBuilder();

                            builder.AppendHtml($"<a>Dostęp do Programu IT został przyznany</a><br>");

                            string link = $"<br><a href=\"http://{nazwaHosta}\">Kliknij tutaj, aby otworzyć stronę</a>";

                            builder.AppendHtml(link);


                            wyslijWiadomosc.WyslijMaila(MySQLconn.ReaderGetString(), "Dostęp przyznany Program IT", builder);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex.Message);
                        }
                      

                        komunikat = "Nadanie dostępu przebiegło pomyślnie";
                        return true;
                    }
                    else
                    {
                        komunikat = "Błąd podczas nadawania dostępu";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    komunikat = "Błąd podczas nadawania dostępu";
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

        }


        public bool AktywujKonto()
        {
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            string kluczAktywacjaWer = kluczDoAktywacjiKonta + DateTime.Now.ToString();
            tabela.Rows.Add(new object[] { "@Aktywacja", kluczDoAktywacjiKonta });
            tabela.Rows.Add(new object[] { "@Aktywacja2", kluczAktywacjaWer });

            MySQLconn.NewQuery("SELECT Count(*) FROM Users WHERE Aktywacja = @Aktywacja", tabela);
            if (MySQLconn.ReaderGetInt64() == 0 || MySQLconn.ReaderGetInt64() == -1)
            {
                komunikat = "Użytkownik został już aktywowany, lub kod aktywacyjny jest błędny";
                return false;
            }
            else
            {


                try
                {


                    MySQLconn.RunSqlTransaction("UPDATE Users SET Aktywny = 1, Aktywacja = @Aktywacja2 WHERE Aktywacja = @Aktywacja", tabela, true);

                    MySQLconn.NewQuery("SELECT CONCAT('Użytkownik:',' ',CONCAT(CONCAT(Imie,' ',Nazwisko),' ',CONCAT(CONCAT('Login:',' ',Login),' ',CONCAT('Email:',' ',Mail))))\r\n FROM Users WHERE Aktywacja = @Aktywacja2 ", tabela);



                    if (MySQLconn.transComit)
                    {
                   
                            var builder = new HtmlContentBuilder();

                        builder.AppendHtml($"<a>{MySQLconn.ReaderGetString()} </a><br><a>Prosi o przyznanie dostępu do programu, kliknij w link, aby zezwolić na dostęp.</a>");

                        string link = $"<br><a href=\"http://{nazwaHosta}/INadanieDostepu?KluczAktywacyjny={kluczAktywacjaWer}\">Kliknij tutaj, aby aktywować konto</a>";

                            builder.AppendHtml(link);

                  
                    


                        try
                        {
                         
                                string str;

                                using (StreamReader sr = new StreamReader("AdresyMailAdmin.txt"))
                                {
                                    str = sr.ReadToEnd();

                                }


                                wyslijWiadomosc.WyslijMaila(str, "Prośba o dostęp do konta IT", builder);
                        
                        }
                        catch (Exception e)
                        {
                            // Let the user know what went wrong.
                            Console.WriteLine("Problem z odczytaniem pliku.");
                            Console.WriteLine(e.Message);
                        }


                        
                      

                        komunikat = "Aktywacja przebiegła pomyślnie. Teraz poczekaj aż Administrator przydzieli dostęp.";
                        return true;
                    }
                    else
                    {
                        komunikat = "Błąd podczas aktywacji";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    komunikat = "Błąd podczas aktywacji";
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
       
        }

        public bool RejestracjaUzytkownika()
        {

            string aktywacja = genLoginAuth.GenerateAuth(35);
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            tabela.Rows.Add(new object[] { "@Login", strlogin });
            tabela.Rows.Add(new object[] { "@Haslo", szyfrowanieTekstu.Encrypt(strhaslo, "programit") });
            tabela.Rows.Add(new object[] { "@Mail", mail });
            tabela.Rows.Add(new object[] { "@Imie", imie });
            tabela.Rows.Add(new object[] { "@Nazwisko", nazwisko });
            tabela.Rows.Add(new object[] { "@Aktywacja", aktywacja });
            tabela.Rows.Add(new object[] { "@KluczLogin", genLoginAuth.GenerateAuth(35) });
            if (mail.Trim() == "")
            {
                komunikat = "Podaj poprawny adres e-mail";
                return false;
            }
            if (strlogin.Trim() == "")
            {
                komunikat = "Login nie może być pusty";
                return false;
            }
            if (strhaslo.Trim() == "")
            {
                komunikat = "Hasło nie może być puste";
                return false;
            }


         
            try
            {
                MySQLconn.RunSqlTransaction("INSERT INTO Users SET Imie = @Imie, Nazwisko = @Nazwisko, Mail = @Mail, Login = @Login, Haslo = @Haslo, KluczLogin = @KluczLogin, Aktywacja = @Aktywacja", tabela, true);


                if (MySQLconn.transComit)
                {
                    var builder = new HtmlContentBuilder();



                    string link = $"<a href=\"http://{nazwaHosta}/iaktywacja?KluczAktywacyjny={aktywacja}\">Kliknij tutaj, aby aktywować konto</a>";

                    builder.AppendHtml(link);

                    wyslijWiadomosc.WyslijMaila(mail, "Aktywacja konta IT", builder);
                    return true;
                }
                else
                {
                    if (MySQLconn.strEx.Contains("for key 'Login'"))
                    {
                        komunikat = "Użytkownik o takim loginie już istnieje w bazie";
                    }
                    else if (MySQLconn.strEx.Contains("for key 'Mail'"))
                    {
                        komunikat = "Użytkownik o takim adresie E-mail już istnieje w bazie";
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }

    
           
        }


        public bool SprawdzDane()
        {
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            tabela.Rows.Add(new object[] { "@login", strlogin });
            tabela.Rows.Add(new object[] { "@haslo", szyfrowanieTekstu.Encrypt(strhaslo, "programit") });
            tabela.Rows.Add(new object[] { "@DataLogowania", DateTime.Now });

            Console.WriteLine("Próba logowania: " + strlogin.ToString() + " " + DateTime.Now.ToString());
       
    
                MySQLconn.NewQuery("SELECT COUNT(*) FROM Users WHERE Login = @login AND Haslo = @haslo AND Aktywny = 1 AND Dostep = 1", tabela);

                if (MySQLconn.ReaderGetInt64() == 0 || MySQLconn.ReaderGetInt64() == -1)
                {
                komunikat = "Brak aktywnego konta lub Administrator nie przydzielił jeszcze dostępu";
                Console.WriteLine($"Błędne logowanie" + strlogin.ToString() + " " + DateTime.Now.ToString() + " " + DateTime.Now.ToString());
                    return false;
                }
                else
                {
                    strKluczLogin = genLoginAuth.GenerateAuth(35);
                    Console.WriteLine($"Poprawne logowanie " + strlogin.ToString() + " " + DateTime.Now.ToString());
                    MySQLconn.RunSqlTransaction($"UPDATE Users SET KluczLogin = '{strKluczLogin}', DataLogowania = @DataLogowania WHERE Login = @login AND Haslo = @haslo", tabela, false);
                    return true;

                }
          
        }

        

        public bool SprawdzKluczLogowania(string kluczL)
        {
            try
            {

       
            if(kluczL == null || kluczL == "")
            {
                Console.WriteLine($"Błędny kod weryfikacji  " + kluczL + " " + DateTime.Now.ToString());
                return false;
            }
            WeryfikacjaczyAdmin = false;
            WeryfikacjaczyDostep = false;
            tabela.Clear();
            tabela.Columns.Add("Paratetr");
            tabela.Columns.Add("Wartosc");
            tabela.Rows.Add(new object[] { "@kluczLogin", kluczL });

       
                MySQLconn.NewQuery("SELECT Id FROM Users WHERE KluczLogin = @kluczLogin AND Dostep = 1", tabela);
                idUzytkownika = MySQLconn.ReaderGetInt();
                // if (mysqlconnTabeleDaneRaportyOW.ReaderGetInt64() == 0 || mysqlconnTabeleDaneRaportyOW.ReaderGetInt64() == -1)
                if (idUzytkownika == -1)
                {
                    Console.WriteLine($"Błędny kod weryfikacji " + kluczL + " " + DateTime.Now.ToString());
                    komunikat = "Brak aktywnego konta lub Administrator nie przydzielił jeszcze dostępu";
                    return false;

                }
                else
                {
           
                    return true;

                }

            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
