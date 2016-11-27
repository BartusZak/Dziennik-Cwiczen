using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace Dziennik
{
    [Activity(Label = "Cwiczenia", Icon = "@drawable/icon")]
    public class MainCwiczenia : Activity
    {
        private ListView mListView;
        private BaseAdapter<Cwiczenie> mAdapter;
        private List<Cwiczenie> mCwiczenia;
        private ImageView mSelectedPic;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.dziennik_dodaj);

            mListView = FindViewById<ListView>(Resource.Id.listView);
            mCwiczenia = new List<Cwiczenie>();

            Action<ImageView> action = PicSelected;

            mAdapter = new CwiczenieListAdapter(this, Resource.Layout.row_cwiczenie, mCwiczenia, action);
            mListView.Adapter = mAdapter;

           
        } 

        private void PicSelected (ImageView selectedPic)
        {
            mSelectedPic = selectedPic;
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Selecte a Photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedPic.SetImageBitmap(BitmapFactory.DecodeStream(stream));
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.add:

                    CreateCwiczenieDialog dialog = new CreateCwiczenieDialog();
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();

                    //Subscribe to event
                    dialog.OnDodajCwiczenie += dialog_OnDodajCwiczenie;
                    dialog.Show(transaction, "dodaj cwiczenie");
                    return true;    
                
                default:
                    return base.OnOptionsItemSelected(item);
            }
           
        }

        void dialog_OnDodajCwiczenie(object sender, DodajCwiczenieEventArgs e)
        {
            mCwiczenia.Add(new Cwiczenie() { Cwiczenie_v = e.Cwiczenie, IloscSerii_v = e.IloscSerii, IloscPowtorzen_v = e.IloscPowtorzen });
            
            mAdapter.NotifyDataSetChanged();
            
        }
    }
}

