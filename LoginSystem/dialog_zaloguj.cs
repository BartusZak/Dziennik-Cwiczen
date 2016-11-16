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
using System.Data;
using Java.Lang;
using System.Threading.Tasks;

namespace LoginSystem
{
    #region Event #1
    //Tworze w³asn¹ klasê eventu potrzebna do onclick event
    public class OnZalogujEventArgs : EventArgs
    {
        private string txtEmailZaloguj;
        private string txtPasswordZaloguj;

        //Wykorzystuje properties, aby moc uzyc zmienne na zewnatrz klasy
        public string Email
        {
            get { return txtEmailZaloguj; }
            set { txtEmailZaloguj = value; }
        }
        public string Password
        {
            get { return txtPasswordZaloguj; }
            set { txtPasswordZaloguj = value; }
        }

        //Konstruktor ktory jest bazowym konstruktorem
        public OnZalogujEventArgs(string email, string password) : base()
        {
            //Wykorzystuje properties do przypisania (inicjalizacji) wartosci zmiennych
            Email = email;
            Password = password;
        }

    }
    #endregion
    public class dialog_zaloguj : DialogFragment
    {
        //Zmienne globalne
        private EditText txtEmailZaloguj;
        private EditText txtPasswordZaloguj;
        private Button btnZaloguj;
        private ProgressBar mprogressBarZaloguj;
        public static bool ZalogujSuccess;

        

        //mySQL database status variable
        private TextView txtSysLog;

       

        #region Tworze metodê nadpisuj¹c¹, ktora wyswietla dialog
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           base.OnCreateView(inflater, container, savedInstanceState);

            //Przypisuje plik .axml
            var view = inflater.Inflate(Resource.Layout.dialog_zaloguj, container, false);

            //Inicjuje zmienne globalne
            txtEmailZaloguj = view.FindViewById<EditText>(Resource.Id.txtEmailZaloguj);
            txtPasswordZaloguj = view.FindViewById<EditText>(Resource.Id.txtPasswordZaloguj);
            btnZaloguj = view.FindViewById<Button>(Resource.Id.btnZaloguj);
            txtSysLog = view.FindViewById<TextView>(Resource.Id.XtxtSysLog);
            mprogressBarZaloguj = view.FindViewById<ProgressBar>(Resource.Id.progressBarZaloguj);

            //Rejestruje onclick event (2x tab generuje metode) 
           btnZaloguj.Click += btnZaloguj_Click;

            return view;
        }
        #endregion
        void btnZaloguj_Click(object sender, EventArgs e)
        {
            //Co siê dzieje po wciœnieciu przycisku "Zaloguj!"
            //Widoczny progressbar
            mprogressBarZaloguj.Visibility = ViewStates.Visible;
            //Wywo³uje event
           OnZalogujComplete.Invoke(this, new OnZalogujEventArgs(txtEmailZaloguj.Text, txtPasswordZaloguj.Text));

            MySqlConnection con = new MySqlConnection("Server=bartuszak.pl;Port=3306;database=android;User Id=android;Password=pJ8AyURCxKqYRpeJ;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    #region Pirwszy sposob "wyluskwania" danych z bazy
                    // MySqlCommand cmd = new MySqlCommand("SELECT ID FROM Users WHERE User_Email=@email AND User_Password=@pass",con);
                    // cmd.Parameters.AddWithValue("@email", txtEmailZaloguj);
                    // cmd.Parameters.AddWithValue("@pass", txtPasswordZaloguj);
                    //  txtSysLog.Text = cmd.GetString(0);
                    #endregion

                    /////Drugi sposob
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = string.Format("SELECT ID,User_FirstName FROM Users WHERE User_Email ='"+txtEmailZaloguj.Text+"' AND User_Password='"+txtPasswordZaloguj.Text+"'"); 
                    cmd.Connection = con;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ZalogujSuccess = true;
                        int ID = int.Parse(reader.GetString(0));
                        string User_FirstName = reader.GetString(1);

                        //Wyswietlam komunikat o zalogowaniu
                        txtSysLog.Text = string.Format("Zalogowny jako: {0}\nID: {1}", User_FirstName, ID);                         
                    }
                    else
                    {
                        txtSysLog.Text = "B³êdne has³o lub email!";
                    }

                    reader.Close();                   
                }
            }
            catch(MySqlException ex)
            {
                txtSysLog.Text = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            //Ustawiam widocznosc progessbar na Invisible
            mprogressBarZaloguj.Visibility = ViewStates.Invisible;

            if (ZalogujSuccess)
            {
                txtSysLog.Text = "Zalogowano";
                Dismiss();

            }                      
        }

        //Tworze inicjalizator do eventu
        public event EventHandler<OnZalogujEventArgs> OnZalogujComplete;

        //Tworzê metodê nadpisuj¹c¹, która wykonujê sie przed wyœwietleniem dialogu
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            //Usuwam tytu³ okna (przypisuje wartosc invisible);
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            //Przypisuje animacje do dialogu
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }       
    }
}