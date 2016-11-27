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
    class DBConnect : dialog_zaloguj
    {
        protected internal MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        protected internal void Initialize()
        {
            server = "bartuszak.pl";
            database = "android";
            uid = "android";
            password = "hbsUu6yRdx6Xa4vx";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        protected internal bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        txtSysLog.Text = "Cannot connect to server.  Contact administrator";
                        break;

                    case 1045:
                        txtSysLog.Text = "Invalid username/password, please try again";
                        break;
                }
                return false;
            }
        }

        //Close connection
        protected internal bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                txtSysLog.Text = ex.Message;
                return false;
            }
        }

        //Select statement
        public int Select_User_ID(string User_Email,string User_Password)
        {
       
            string query = "SELECT ID FROM Users WHERE User_Email='"+User_Email+"' AND User_Password='"+User_Password+"'";
            Initialize();
            //Create a list to store the result
           int User_ID=0;
            

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.Read())
                {
                    User_ID = int.Parse(dataReader.GetString(0));
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return User_ID;
            }
            else
            {
                return User_ID;
            }
        }

        public string Select_User_FirstName(int ID)
        {
          
            string query = "SELECT User_FirstName FROM Users WHERE ID="+ID;
            Initialize();
            //Create a list to store the result
            string User_FirstName="nothing";


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.Read())
                {
                    User_FirstName = dataReader.GetString(0);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return User_FirstName;
            }
            else
            {
                return User_FirstName;
            }
        }

        public string Select_User_Email(int ID)
        {

            string query = "SELECT User_Email FROM Users WHERE ID=" + ID;
            Initialize();
            //Create a list to store the result
            string User_Email = "nothing";


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.Read())
                {
                    User_Email = dataReader.GetString(0);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return User_Email;
            }
            else
            {
                return User_Email;
            }
        }
    }
}