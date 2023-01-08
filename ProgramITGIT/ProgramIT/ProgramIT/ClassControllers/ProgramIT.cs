using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Text;
using ProgramIT.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace ProgramIT.Controllers
{
    public class ProgramIT
    {
        public bool poprawnieZalogowany = false;
        public string keyAuth { get; set; }
        public string strJakaBaza { get; set; }
        public bool bladDanych { get; set; } = false;
        public string CzegoSzukac { get; set; } = "";

        public string strNazwa { get; set; } = "";
        public string strHaslo { get; set; } = "";
        public string strOpis { get; set; } = "";
        public string strGrupa { get; set; } = "";
        public string strGrupaDod { get; set; } = "";
        public bool statusZapisu { get; set; } = false;


        public bool EdycjaDanych { get; set; } = false;
        public string passUzyt { get; set; }

        public string komunikat = "";

        public string idEdycja { get; set; } = "";

        private int idZalogowanego = 0;




        public string WyborID { get; set; }
        public DataTable TableToReturn = new DataTable();   
        WeryfikacjaLogowania weryfikacjaLogowania = new WeryfikacjaLogowania();
        public SzyfrowanieTekstu SzyfrowanieTekstu = new SzyfrowanieTekstu();
        public MailSerwer wyslijWiadomosc = new MailSerwer();
        MySQL MySQLconn = new MySQL(null, null, "Database=progit;Data Source = 192.168.50.177; User Id = programit; Password=iN5@sCXao); Connection Timeout = 30; charset=utf8;SslMode=none;");
        public ProgramIT(){
        
        }


        public bool SprawdzLogowanie()
        {
            try
            {
                if (keyAuth != null)
                { 
                poprawnieZalogowany = weryfikacjaLogowania.SprawdzKluczLogowania(keyAuth);
                
                 if(poprawnieZalogowany)idZalogowanego = weryfikacjaLogowania.idUzytkownika;
                }
            else
            {
                poprawnieZalogowany = false;
            }
            return poprawnieZalogowany;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                poprawnieZalogowany = false;
                return poprawnieZalogowany;
               
            }
        }








        public bool ZmianaHasla()
        {
           
            try
            {
                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("Paratetr");
                dtParam.Columns.Add("Wartosc");
                dtParam.Rows.Add(new object[] { "@keyAuth", keyAuth });
                dtParam.Rows.Add(new object[] { "@Haslo", SzyfrowanieTekstu.Encrypt(passUzyt, "programit") });


                MySQLconn.RunSqlTransaction("UPDATE Users SET Haslo = @Haslo WHERE KluczLogin = @keyAuth", dtParam, true);

                return MySQLconn.transComit;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                komunikat = "Błąd podczas zmiany hasła";
                return false;
                
            }

        }









        public void WczytajTabele(string coWczytujemy)
        {
            
            if (SprawdzLogowanie())
            {
                try
                {
                    DataTable dtParam = new DataTable();
                    dtParam.Columns.Add("Paratetr");
                    dtParam.Columns.Add("Wartosc");
                    dtParam.Rows.Add(new object[] { "@idEdycja", "" + idEdycja.Trim() + "" });
                    dtParam.Rows.Add(new object[] { "@szukaj", "%" + CzegoSzukac + "%" });

                    if (coWczytujemy == "standard") //wczytywanie danych z bazy danaych
                    { 
                    string strwhere = $" WHERE (IdUser = '{idZalogowanego}' AND (Public = '0' OR Public IS NULL)) OR Public = '1'";

                    if (CzegoSzukac != "")
                    {

                        MySQLconn.NewQuery($"SELECT d.Id, d.Nazwa, d.Grupa, d.Opis, CONCAT(u.Imie,' ',u.Nazwisko) `Właściciel`, if(d.Public = 1,'Publiczny','Prywatny') `Status` from Dane `d` " +
                            $"JOIN Users `u` ON (u.Id = d.IdUser) WHERE ((Public = '1') OR (IdUser = '{idZalogowanego}' AND (Public = '0' OR Public IS NULL))) AND (Nazwa LIKE @szukaj OR Grupa LIKE @szukaj OR Opis LIKE @szukaj)", dtParam);
                    }
                    else
                    {
                        if (EdycjaDanych)
                        {

                        MySQLconn.NewQuery($"SELECT d.Id, d.Nazwa, d.Grupa, d.Opis, d.Haslo, d.Public from Dane `d` " +
                                                  $"WHERE ((d.IdUser = '{idZalogowanego}' AND (d.Public = '0' OR d.Public IS NULL)) OR d.Public = '1') AND d.Id = @idEdycja", dtParam);

                        }
                        else
                        {
                            MySQLconn.NewQuery($"SELECT d.Id, d.Nazwa, d.Grupa, d.Opis, CONCAT(u.Imie,' ',u.Nazwisko) `Właściciel`, if(d.Public = 1,'Publiczny','Prywatny') `Status` from Dane `d` " +
                                $"JOIN Users `u` ON (u.Id = d.IdUser) WHERE (IdUser = '{idZalogowanego}' AND (Public = '0' OR Public IS NULL)) OR Public = '1'", null);
                        }
                    }
                }
                    else //// Wczytywanie danych dla pola ComboBox

                    {
                        MySQLconn.NewQuery($"SELECT ROW_NUMBER() OVER(ORDER BY Grupa) AS `id`, Grupa `nazwa` FROM Dane WHERE (IdUser = '{idZalogowanego}' AND (Public = '0' OR Public IS NULL)) OR Public = '1' GROUP BY Grupa", null);

           
                    }


                    bladDanych = false;
                    TableToReturn = MySQLconn.ReaderToTable();

                }
                catch (Exception ex)
                {

                    bladDanych = true;
                    Console.WriteLine(ex.Message + " Bląd Wczytywania tabeli");
                }


            }
        
        }

        public bool UsunZBazy()
        {
            DataTable dtParam = new DataTable();
            dtParam.Columns.Add("Paratetr");
            dtParam.Columns.Add("Wartosc");
            dtParam.Rows.Add(new object[] { "@szukaj", "%" + CzegoSzukac + "%" });
            dtParam.Rows.Add(new object[] { "@idEdycja", "" + idEdycja + "" });
            dtParam.Rows.Add(new object[] { "@idZalogowanego", "" + idZalogowanego + "" });
            try
            {
                MySQLconn.RunSqlTransaction("DELETE FROM Dane " +
                                  "WHERE ((IdUser = @idZalogowanego AND (Public = '0' OR Public IS NULL)) OR Public = '1') AND Id = @idEdycja ", dtParam, false);

                return MySQLconn.transComit;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
          
               
       
        }


        public bool ZapiszDoBazy()
        {
            DataTable dtParam = new DataTable();
            dtParam.Columns.Add("Paratetr");
            dtParam.Columns.Add("Wartosc");
            dtParam.Rows.Add(new object[] { "@szukaj", "%" + CzegoSzukac + "%" });
            dtParam.Rows.Add(new object[] { "@idEdycja", "" + idEdycja + "" });
            dtParam.Rows.Add(new object[] { "@idZalogowanego", "" + idZalogowanego + "" });
            dtParam.Rows.Add(new object[] { "@strNazwa", "" + strNazwa + "" });

            if (strGrupaDod == "" || strGrupaDod == null)
            {
                dtParam.Rows.Add(new object[] { "@strGrupa", "" + strGrupa + "" });
            }
            else
            {
                dtParam.Rows.Add(new object[] { "@strGrupa", "" + strGrupaDod + "" });
            }
          
            dtParam.Rows.Add(new object[] { "@strHaslo", "" + SzyfrowanieTekstu.Encrypt(strHaslo, "programit") + "" });
            dtParam.Rows.Add(new object[] { "@Opis", "" + strOpis + "" });
            if(statusZapisu)
            {
                dtParam.Rows.Add(new object[] { "@status", "" + 0 + "" });
            }
            else
            {
                dtParam.Rows.Add(new object[] { "@status", "" + 1 + "" });
            }

            


         

            if (idEdycja == null || idEdycja == "")
                {
                MySQLconn.RunSqlTransaction("INSERT INTO Dane SET " +
                        "Nazwa = @strNazwa, " +
                        "Grupa = @strGrupa, " +
                        "Haslo = @strHaslo, " +
                        "Opis = @Opis, " +
                        "Public = @status," +
                        "IdUser = @idZalogowanego",
                       dtParam, false);

                return MySQLconn.transComit;

            }
                else
                {
      
                MySQLconn.RunSqlTransaction("UPDATE Dane SET " +
                        "Nazwa = @strNazwa, " +
                        "Grupa = @strGrupa, " +
                        "Haslo = @strHaslo, " +
                        "Opis = @Opis, " +
                        "Public = @status" +
                      $" WHERE ((IdUser = @idZalogowanego AND (Public = '0' OR Public IS NULL)) OR Public = '1') AND Id = @idEdycja", dtParam, false);

           return MySQLconn.transComit;
            }



        }



    }
}
