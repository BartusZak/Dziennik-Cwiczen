using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace Dziennik
{
    class DBConnect_cwiczenia : DBConnect
    {
        //Insert statement
        public int Insert_cwiczenie_return_cwicznie_id(string Cwiczenie_Nazwa, string Cwiczenie_IloscSerii, string Cwiczenie_IloscPowtorzen)
        {
            int Cwiczenie_ID = 0;
            string query = "INSERT INTO Cwiczenia (User_ID,Cwiczenie_Nazwa,Cwiczenie_IloscSerii,Cwiczenie_IloscPowtorzen) VALUES(" + User_ID + ",'" + Cwiczenie_Nazwa + "','" + Cwiczenie_IloscSerii + "','" + Cwiczenie_IloscPowtorzen + "')";
            string query2 = "SELECT IDENT_CURRENT('Cwiczenia') AS Cwiczenie_ID";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader = cmd2.ExecuteReader();

                Cwiczenie_ID = (int)dataReader["Cwiczenie_ID"];

                //close Data Reader
                dataReader.Close();
                //close connection
                this.CloseConnection();
            }
            return Cwiczenie_ID;
        }

        //Select statement
        public List<Cwiczenie> Select_Cwiczenia_Wszystkie()
        {
            string query = "SELECT Cwiczenie_Nazwa,Cwiczenie_IloscSerii,Cwiczenie_IloscPowtorzen FROM Cwiczenia WHERE User_ID=" + User_ID;
            Initialize();

            //Create a list to store the result
            List<Cwiczenie> list = new List<Cwiczenie>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                Cwiczenie record;
                while (dataReader.Read())
                {
                    record = new Cwiczenie()
                    {
                        Cwiczenie_v = dataReader["Cwiczenie_Nazwa"] as String,
                        IloscSerii_v = dataReader["Cwiczenie_IloscSerii"] as String,
                        IloscPowtorzen_v = dataReader["Cwiczenie_IloscPowtorzen"] as String,
                       // Image = dataReader["Ikona"] as byte []
                       
                    };
                    list.Add(record);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Update statement
         public  void Update_Image(byte[] img, int cwiczenie_ID)
        {
            string query = "UPDATE Cwiczenia SET Cwiczenie_Nazwa='bartek' WHERE Cwiczenie_ID='65'" ;
            Initialize();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
    }
}