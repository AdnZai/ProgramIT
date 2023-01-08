using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;

namespace ProgramIT.Controllers
{
    public class MySQL
    {
        MySqlConnection connection = null;
        MySqlDataReader datareader = null;
        MySqlCommand command = null;
        public Boolean transComit = false;
        public string strEx = "";

        public MySQL(string query, DataTable parametry, string strConnectionString)
        {
            connection = new MySqlConnection(strConnectionString);

            if (query != null)
            {
                NewQuery(query, parametry);
            }
        }

       
        public void NewQuery(string query, DataTable parametry)
        {
            ZamknijReadera();
            command = new MySqlCommand();
            command = connection.CreateCommand();
            command.Connection = connection;


            command.CommandText = query;

            if (parametry != null)
            {
                foreach (DataRow dtRow in parametry.Rows)
                {
                    if (dtRow[1] == DBNull.Value)
                    {
                        command.Parameters.AddWithValue((string)dtRow[0], DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue((string)dtRow[0], (string)dtRow[1]);
                    }
                }
            }
        }








        public DataTable ReaderToTable()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                datareader = command.ExecuteReader();
                DataTable tabela = new DataTable();
                tabela.Load(datareader);
                connection.Close();
                ZamknijReadera();
                return tabela;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DataTable tabela = new DataTable();
                tabela = null;
                return tabela;
            }

        }

        public DataSet ReaderToDataSet()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                datareader = command.ExecuteReader();

                DataSet tabela = new DataSet();

                int tablecount = -1;

                while (datareader.HasRows)
                {

                    tablecount++;

                    tabela.Tables.Add(tablecount.ToString());


                    for (int i = 0; i <= datareader.FieldCount - 1; i++)
                    {

                        tabela.Tables[tablecount].Columns.Add(datareader.GetName(i).ToString());

                    }
                    while (datareader.Read())
                    {
                        DataRow row = tabela.Tables[tablecount].NewRow();
                        for (int i = 0; i <= datareader.FieldCount - 1; i++)
                        {
                            row[i] = datareader.GetValue(i);
                        }
                        tabela.Tables[tablecount].Rows.Add(row);
                    }



                    datareader.NextResult();
                }

                connection.Close();
                ZamknijReadera();
                return tabela;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                DataSet tabela = new DataSet();
                tabela = null;
                return tabela;
            }

        }

        public DataSet ReaderToDataSet2()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                datareader = command.ExecuteReader();
                int tablecount = -1;
                DataSet tabela = new DataSet();



                while (datareader.HasRows)
                {

                    tablecount++;

                    tabela.Tables.Add(tablecount.ToString());


                    for (int i = 0; i <= datareader.FieldCount - 1; i++)
                    {

                        tabela.Tables[tablecount].Columns.Add(datareader.GetName(i).ToString());

                    }
                    while (datareader.Read())
                    {
                        DataRow row = tabela.Tables[tablecount].NewRow();
                        for (int i = 0; i <= datareader.FieldCount - 1; i++)
                        {
                            row[i] = datareader.GetValue(i);
                        }
                        tabela.Tables[tablecount].Rows.Add(row);
                    }

                    datareader.NextResult();
                }
                connection.Close();
                ZamknijReadera();
                return tabela;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                DataSet tabela = new DataSet();
                tabela = null;
                return tabela;
            }

        }



        public int ReaderGetInt64()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                int zmienna = -1;
                datareader = command.ExecuteReader();
                if (datareader.Read())
                    zmienna = (int)(Int64)datareader.GetValue(0);
                connection.Close();
                ZamknijReadera();
                return zmienna;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReaderGetInt " + ex.Message);
                int zmienna;
                zmienna = -1;
                return zmienna;
            }

        }

        public int ReaderGetInt()
        {
          try
           {
                if (connection.State == ConnectionState.Closed)
                {

                   connection.Open();

                }
                int zmienna = -1;
                datareader = command.ExecuteReader();
                datareader.Read();
                if (datareader.HasRows)
                {
                    if (datareader.GetValue(0) != DBNull.Value)
                    {
                        zmienna = (int)datareader.GetValue(0);
                    }
                }
                connection.Close();
                ZamknijReadera();
                return zmienna;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReaderGetInt " + ex.Message);
               int zmienna;
               zmienna = -1;
                return zmienna;
            }

        }

        public string ReaderGetString()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                string zmienna = "";
                datareader = command.ExecuteReader();
                if (datareader.Read())
                    zmienna = Convert.ToString(datareader.GetValue(0));
                connection.Close();
                ZamknijReadera();
                return zmienna;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReaderGetString" + ex.Message);
                string zmienna;
                zmienna = "";
                return zmienna;
            }

        }




        public DateTime ReaderGetDateScalar()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            DateTime dt = (DateTime)command.ExecuteScalar();
            connection.Close();
            ZamknijReadera();
            return dt;

        }





        private void ZamknijReadera()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            if (datareader != null)
            {
                datareader.Close();
                datareader = null;

            }
        }


        public void RunSqlTransaction(string query, DataTable parametry, Boolean info)
        {

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            MySqlCommand myCommand = connection.CreateCommand();
            MySqlTransaction myTrans;


            myTrans = connection.BeginTransaction();

            myCommand.Connection = connection;
            myCommand.Transaction = myTrans;

            try
            {

                myCommand.CommandText = query;
                if (parametry != null)
                {
                    foreach (DataRow dtRow in parametry.Rows)
                    {
                        if (dtRow[1] == DBNull.Value)
                        {
                            myCommand.Parameters.AddWithValue((string)dtRow[0], DBNull.Value);
                        }
                        else
                        {
                            if (dtRow[0].ToString().Contains("@File") && dtRow[1].ToString().Contains("\\"))
                            {
                                myCommand.Parameters.AddWithValue((string)dtRow[0], ReadBytesFile((string)dtRow[1]));
                            }
                            else
                            {
                                
                                myCommand.Parameters.AddWithValue((string)dtRow[0], (string)dtRow[1]);
                            }

                        }

                    }
                }
             
                   




               
                myCommand.ExecuteNonQuery();
                 
                myTrans.Commit();
                transComit = true;
                if (info)
                    Console.WriteLine("Zapisano");
                connection.Close();
            }
            catch (Exception e)
            {

                 Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                strEx = e.Message;
                transComit = false;


            }
            finally
            {

                connection.Close();
            }
        }

        MySqlCommand myCommand;
        MySqlTransaction myTrans;

        public void RunSqlTransaction2(Boolean koniec, string query, DataTable parametry)
        {
            if (koniec == false && query == "")
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                myCommand = connection.CreateCommand();


                myTrans = connection.BeginTransaction();

                myCommand.Connection = connection;
                myCommand.Transaction = myTrans;
            }

            if (query != "")
            {

                myCommand.CommandText = query;
                if (parametry != null)
                {
                    foreach (DataRow dtRow in parametry.Rows)
                    {
                        if (dtRow[1] == DBNull.Value)
                        {
                            myCommand.Parameters.AddWithValue((string)dtRow[0], DBNull.Value);
                        }
                        else
                        {
                            if (dtRow[0].ToString().Contains("@File") && dtRow[1].ToString().Contains("\\"))
                            {
                                myCommand.Parameters.AddWithValue((string)dtRow[0], ReadBytesFile((string)dtRow[1]));
                            }
                            else
                            {
                                myCommand.Parameters.AddWithValue((string)dtRow[0], (string)dtRow[1]);
                            }

                        }

                    }
                }

                myCommand.ExecuteNonQuery();
            }



            if (koniec == true && query == "")
            {
                try
                {

                    myTrans.Commit();

                    connection.Close();
                }
                catch (Exception e)
                {
                    try
                    {
                        myTrans.Rollback();
                    }
                    catch (MySqlException ex)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("Wyjątek typu " + ex +
                            " napotkano podczas próby wycofania transakcji.");
                            transComit = false;
                        }
                    }
                    Console.WriteLine("Wyjątek typu " + e +
                    " napotkano podczas wstawiania danych.");
                    Console.WriteLine("Żaden rekord nie został zapisany w bazie danych.");
                    transComit = false;


                }
                finally
                {

                    connection.Close();
                }
            }
        }

        private static byte[] ReadBytesFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] filebytes = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return filebytes;
        }





        public void Dispose()
        {
            connection?.Dispose();
            command?.Dispose();
        }

        public static string ConnectionString { get; set; } = null;
    }
}
