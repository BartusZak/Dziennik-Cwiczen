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
        private TextView txtUser_FirstName;
        private TextView txtUser_Email;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dziennik_main);
            
           
           txtUser_FirstName = FindViewById<TextView>(Resource.Id.txtUser_FirstName);
            txtUser_Email = FindViewById<TextView>(Resource.Id.txtUser_Email);


          //txtUser_FirstName.Text = DBConnect.User_FirstName;
           
         
        }

       
    }
}