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


namespace Dziennik
{    
    public class dialog_zaloguj : DialogFragment
    {

        //Zmienne globalne statyczne
        public static bool ZalogujSuccess;
        public static int User_ID;
        public static string User_FirstName;
        public static string User_Email;

        //Zmienne prytwatne i dziedziczone
        protected internal EditText txtEmailZaloguj;
        protected internal EditText txtPasswordZaloguj;
        private Button btnZaloguj;

        //mySQL database status variable
        protected internal TextView txtSysLog;
        private DBConnect dbConnect;

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
            //mprogressBarZaloguj = view.FindViewById<ProgressBar>(Resource.Id.progressBarZaloguj);

            //Rejestruje onclick event (2x tab generuje metode) 
           btnZaloguj.Click += btnZaloguj_Click;

            return view;
        }
        #endregion
        void btnZaloguj_Click(object sender, EventArgs e)
        {
            //Co siê dzieje po wciœnieciu przycisku "Zaloguj!"
            dbConnect = new DBConnect();

            User_ID = dbConnect.Select_User_ID(txtEmailZaloguj.Text, txtPasswordZaloguj.Text);
            User_FirstName = dbConnect.Select_User_FirstName(User_ID);
            User_Email = dbConnect.Select_User_Email(User_ID);
            
            if (User_ID != 0)
            {
                txtSysLog.Text = User_ID.ToString();
                ZalogujSuccess = true;                
                Dismiss();

                

            }
            else
            {
                txtSysLog.Text = "U¿ytkownik nie istnieje!";
            }
            
        }
               
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