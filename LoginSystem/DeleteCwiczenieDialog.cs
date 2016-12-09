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
    public class UsunCwiczenieEventArgs : EventArgs
    {
        public int ID { get; set; }
        public string Cwiczenie { get; set; }


        public UsunCwiczenieEventArgs(string cwiczenie)
        {
            Cwiczenie = cwiczenie;
           
        }
    }

    class DeleteCwiczenieDialog : DialogFragment
    {
        private Button mButtOnUsunCwiczenie;
        private EditText txtCwiczenieUsun;
        private DBConnect_cwiczenia dbConnect_cwiczenia;

        public event EventHandler<UsunCwiczenieEventArgs> OnUsunCwiczenie;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_usun_cwiczenie, container, false);
            mButtOnUsunCwiczenie = view.FindViewById<Button>(Resource.Id.btnDialogUsunCwiczenie);
            txtCwiczenieUsun = view.FindViewById<EditText>(Resource.Id.txtCwiczenieUsun);
            

            mButtOnUsunCwiczenie.Click += mButtOnUsunCwiczenie_Click;
            return view;

        }

        void mButtOnUsunCwiczenie_Click(object sender, EventArgs e)
        {
            //Broadcast event
            dbConnect_cwiczenia = new DBConnect_cwiczenia();
            dbConnect_cwiczenia.Remove_Cwiczenie_by_Cwicznie_Name(txtCwiczenieUsun.Text);

           // OnUsunCwiczenie.Invoke(this, new UsunCwiczenieEventArgs(txtCwiczenieUsun.Text));
            this.Dismiss();
        }

        void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Activity.RunOnUiThread(() =>
            {
                //string id = Encoding.UTF8.GetString(e.Result); //Get the data echo backed from PHP
                //int newID = 0;

                //int.TryParse(id, out newID); //Cast the id to an integer

                if (OnUsunCwiczenie != null)
                {
                    //Broadcast event
                  //  OnUsunCwiczenie.Invoke(this, new UsunCwiczenieEventArgs(txtCwiczenieUsun.Text));

                }


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