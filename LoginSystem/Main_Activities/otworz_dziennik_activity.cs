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

namespace Dziennik
{
    [Activity(Label = "Twój Dziennik | bartuszak.pl")]
    public class otworz_dziennik_activity : Activity
    {
        //Deklaracja zmiennych prytawnych TextView's
        private TextView txtUser_FirstName;
        private TextView txtUser_Email;

        //przyciski
        private Button btnWylogujSie;
        private Button btnTwojeCwiczenia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dziennik_main);
            
           //pola uzytkownika
            txtUser_FirstName = FindViewById<TextView>(Resource.Id.txtUser_FirstName);
            txtUser_Email = FindViewById<TextView>(Resource.Id.txtUser_Email);

            //inicjalizcja pol uzytkonika
            txtUser_FirstName.Text = dialog_zaloguj.User_FirstName;
            txtUser_Email.Text = dialog_zaloguj.User_Email;

            //buttons
            btnWylogujSie = FindViewById<Button>(Resource.Id.btnWylogujSie);
            btnTwojeCwiczenia = FindViewById<Button>(Resource.Id.btnTwojeCwiczenia);

            //inicjalizcja buttonow
            btnWylogujSie.Click += (object sender, EventArgs args) =>
            {
                dialog_zaloguj.ZalogujSuccess = false;
                Finish();
                StartActivity(typeof(MainActivity));
            };

            btnTwojeCwiczenia.Click += (object sender, EventArgs args) =>
            {
                StartActivity(typeof(MainCwiczenia));
            };



        }

    
    }
}