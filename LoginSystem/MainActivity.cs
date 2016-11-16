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
using SlidingTabLayoutTutorial;

namespace LoginSystem
{
    [Activity(Label = "Dziennik Treningowy", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //Deklaracja zmiennych
        private Button mBtSignUp;
        private ProgressBar mProgressBar;
        private Button mBtZaloguj;
        
        //Główny kod api
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Ustawiam ekran głowny na:
            SetContentView (Resource.Layout.Main);



            //------------------------------------------------------------------------------------------------------
            //Przypisuje event do przycisku "Zaloguj się" (referecnja do .axml)
            mBtZaloguj = FindViewById<Button>(Resource.Id.btnZaloguj);

            //Tworze Click Event dla przyciusku "Zaloguj sie" (wykorzystuje metode anonimową)
            mBtZaloguj.Click += (object sender, EventArgs args) =>
            {
                //Wyswietlanie dilogu do logowania
                FragmentTransaction zaloguj_tran = FragmentManager.BeginTransaction();
                dialog_zaloguj zalogujDialog = new dialog_zaloguj();
                zalogujDialog.Show(zaloguj_tran, "dialog logowania");

                zalogujDialog.OnZalogujComplete += ZalogujDialog_OnZalogujComplete;
            };

            //------------------------------------------------------------------------------------------------------
            //Przypisuje event do przycisku "Zarejestruj się" (referencja do .axml)
            mBtSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

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

        //Logowanie zakonczono poprawnie
        private void ZalogujDialog_OnZalogujComplete(object sender, OnZalogujEventArgs e)
        {
            if (dialog_zaloguj.ZalogujSuccess)
            {

                //SetContentView(Resource.Layout.dziennik_main);
                //----------------------------------------------------------------------------------------------
                //DZIENNIK
            SetContentView(Resource.Layout.dziennik2);
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            SlidingTabsFragment fragment = new SlidingTabsFragment();
            transaction.Replace(Resource.Id.sample_content_fragment, fragment);
            transaction.Commit();
            }

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        // Co się dzieje po zamknieciu okna dialogowego
        void signUpDialog_mOnSignUpComplete(object sender, OnSignUpEventArgs e)
        {
            // Progress Bar
            mProgressBar.Visibility = ViewStates.Invisible;

        }

       
    }
}

