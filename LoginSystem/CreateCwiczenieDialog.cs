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
using System.Net;
using System.Collections.Specialized;

namespace Dziennik
{
    public class DodajCwiczenieEventArgs : EventArgs
    {
        public int ID { get; set; }
        public string Cwiczenie { get; set; }
        public string IloscSerii { get; set; }
        public string IloscPowtorzen { get; set; }


        public DodajCwiczenieEventArgs(int id, string cwiczenie, string iloscSerii, string iloscPowtorzen)
        {
            ID = id;
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
            
                WebClient client = new WebClient();
                Uri uri = new Uri("http://bartuszak.pl/android/CreateCwiczenie.php");
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("Cwiczenie", txtCwiczenie.Text);
                parameters.Add("IloscSerii", txtIloscSerii.Text);
                parameters.Add("IloscPowtorzen", txtIloscPowtorzen.Text);
                parameters.Add("User_ID", dialog_zaloguj.User_ID.ToString());

            client.UploadValuesCompleted += client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);


                //Broadcast event
                // dbConnect_cwiczenia = new DBConnect_cwiczenia();
                //int newID = dbConnect_cwiczenia.Insert_cwiczenie_return_cwicznie_id(txtCwiczenie.Text, txtIloscSerii.Text, txtIloscPowtorzen.Text);

                // OnDodajCwiczenie.Invoke(this, new DodajCwiczenieEventArgs(newID, txtCwiczenie.Text, txtIloscSerii.Text, txtIloscPowtorzen.Text));           
        }

        void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Activity.RunOnUiThread(() =>
            {
                string id = Encoding.UTF8.GetString(e.Result); //Get the data echo backed from PHP
                int newID = 0;

                int.TryParse(id, out newID); //Cast the id to an integer

                if (OnDodajCwiczenie != null)
                {
                    //Broadcast event
                    OnDodajCwiczenie.Invoke(this, new DodajCwiczenieEventArgs(newID, txtCwiczenie.Text, txtIloscSerii.Text, txtIloscPowtorzen.Text));
                }


                this.Dismiss();
            });

        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}