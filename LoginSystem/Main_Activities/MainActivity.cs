using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Java.Lang;
using System.Net;
using Android.Content;
using Android.Runtime;
using Android.Views;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using System.Collections.Specialized;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Dziennik
{
    [Activity(Label = "Dziennik Treningowy", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class MainActivity : Activity
    {
        //Deklaracja zmiennych
        private Button mBtSignUp;
        private Button mBtZaloguj;

        public static int User_ID;
        public static string User_FirstName;
        public static string User_Email;

        protected internal EditText txtEmailZaloguj;
        protected internal EditText txtPasswordZaloguj;
        private DBConnect dbConnect;
        protected internal TextView txtSysLog;

        //Główny kod api
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Ustawiam ekran głowny na:

            SetContentView (Resource.Layout.Main);



            //------------------------------------------------------------------------------------------------------
            //Przypisuje event do przycisku "Zaloguj się" (referecnja do .axml)
            txtEmailZaloguj = FindViewById<EditText>(Resource.Id.txtEmailZalogujMain);
            txtPasswordZaloguj = FindViewById<EditText>(Resource.Id.txtPasswordZalogujMain);
            txtSysLog = FindViewById<TextView>(Resource.Id.XtxtSysLogMain);
            mBtZaloguj = FindViewById<Button>(Resource.Id.btnZaloguj);

            



            //Tworze Click Event dla przyciusku "Zaloguj sie" (wykorzystuje metode anonimową)
            mBtZaloguj.Click += (object sender, EventArgs args) =>
            {
                //Co się dzieje po wciśnieciu przycisku "Zaloguj!"
                dbConnect = new DBConnect();

                User_ID = dbConnect.Select_User_ID(txtEmailZaloguj.Text, txtPasswordZaloguj.Text);
                User_FirstName = dbConnect.Select_User_FirstName(User_ID);
                User_Email = dbConnect.Select_User_Email(User_ID);

                if (User_ID != 0)
                {
                    StartActivity(typeof(otworz_dziennik_activity));
                }
                else
                {
                    txtSysLog.Text = "Użytkownik nie istnieje!";
                }
                /* Tutaj korzystałem z dialogu do logowania, ale nie potrafilem rozwiązać problemu z uruchomienime nowej activity a dialogform
                if (dialog_zaloguj.ZalogujSuccess)
                 {
                     StartActivity(typeof(otworz_dziennik_activity));
                 }
                else
                {
                     //Wyswietlanie dilogu do logowania
                     FragmentTransaction zaloguj_tran = FragmentManager.BeginTransaction();
                     dialog_zaloguj zalogujDialog = new dialog_zaloguj();
                     zalogujDialog.Show(zaloguj_tran, "dialog logowania");
                 }
                 */
            };
          

            //------------------------------------------------------------------------------------------------------
            //Przypisuje event do przycisku "Zarejestruj się" (referencja do .axml)
            mBtSignUp = FindViewById<Button>(Resource.Id.btnSignUp);

            //Tworze Click Event dla przyciusku "Zarejestruj się" (wykorzystuje metode anonimową)
            mBtSignUp.Click += (object sender, EventArgs args) =>
            {
                //Wyswietlenie dialogu do tworzenia konta
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialog_SignUp signUpDialog = new dialog_SignUp();
                signUpDialog.Show(transaction, "dialog fragment");

               signUpDialog.mOnSignUpComplete +=  signUpDialog_mOnSignUpComplete;
               
            };
        }

       
        // Co się dzieje po zamknieciu okna dialogowego
        void signUpDialog_mOnSignUpComplete(object sender, OnSignUpEventArgs e)
        {
         

        }

        
    }
}

