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
    public class DodajCwiczenieEventArgs : EventArgs
    {
        public string Cwiczenie { get; set; }
        public string IloscSerii { get; set; }
        public string IloscPowtorzen { get; set; }
        

        public DodajCwiczenieEventArgs(string cwiczenie, string iloscSerii, string iloscPowtorzen)
        {
            Cwiczenie = cwiczenie;
            IloscSerii = iloscSerii;
            IloscPowtorzen = iloscPowtorzen;
        }
    }

    class CreateCwiczenieDialog : DialogFragment
    {
        private Button mButtOnDodajCwiczenie;
        private EditText txtCwiczenie;
        private EditText txtIloscSerii;
        private EditText txtIloscPowtorzen;
        private DBConnect_cwiczenia dbConnect_cwiczenia;

        public event EventHandler<DodajCwiczenieEventArgs> OnDodajCwiczenie;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_dodaj_cwiczenie, container, false);
            mButtOnDodajCwiczenie = view.FindViewById<Button>(Resource.Id.btnDialogDodajCwiczenie);
            txtCwiczenie = view.FindViewById<EditText>(Resource.Id.txtCwiczenie);
            txtIloscSerii = view.FindViewById<EditText>(Resource.Id.txtIloscSerii);
            txtIloscPowtorzen = view.FindViewById<EditText>(Resource.Id.txtIloscPowtorzen);

            mButtOnDodajCwiczenie.Click += mButtOnDodajCwiczenie_Click;
            return view;
           
        }

        void mButtOnDodajCwiczenie_Click(object sender, EventArgs e)
        {
           if (OnDodajCwiczenie != null)
           {
                //Broadcast event
                dbConnect_cwiczenia = new DBConnect_cwiczenia();
                dbConnect_cwiczenia.Insert_cwiczenie(txtCwiczenie.Text, txtIloscSerii.Text, txtIloscPowtorzen.Text);

                OnDodajCwiczenie.Invoke(this, new DodajCwiczenieEventArgs(txtCwiczenie.Text, txtIloscSerii.Text, txtIloscPowtorzen.Text));              
           }

           this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}