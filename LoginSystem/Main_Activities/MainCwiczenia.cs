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
        private List<Cwiczenie> mCwiczenia2;
        private ImageView mSelectedPic;
        private DBConnect_cwiczenia dbConnect_cwiczenia;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.dziennik_dodaj);

            mListView = FindViewById<ListView>(Resource.Id.listView);
            mCwiczenia = new List<Cwiczenie>();

            Action<ImageView> action = PicSelected;

            mAdapter = new CwiczenieListAdapter(this, Resource.Layout.row_cwiczenie, mCwiczenia, action);
            mListView.Adapter = mAdapter;

            dbConnect_cwiczenia = new DBConnect_cwiczenia();
            mCwiczenia2 = dbConnect_cwiczenia.Select_Cwiczenia_Wszystkie();

            mCwiczenia.AddRange(mCwiczenia2);

        }

        private void PicSelected(ImageView selectedPic)
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
                int cwiczenieID = mCwiczenia[(int)mSelectedPic.Tag].ID;
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedPic.SetImageBitmap(DecodeBitmapFromStream(data.Data, 150, 150));

                //przygotowania do wyslania bitmapy do sql
                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                MemoryStream memStream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memStream);
                byte[] picData = memStream.ToArray();

                //DBConnect_cwiczenia.Update_Image(Convert.ToBase64String(picData));
            }
        }

        private Bitmap DecodeBitmapFromStream(Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            Stream stream = ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            stream = ContentResolver.OpenInputStream(data);
            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);
            return bitmap;
        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int InSampleSize = 1;

            if (height > requestedWidth || width > requestedWidth)
            {
                // obrazek jest wiekszy niz powinien
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while ((halfHeight / InSampleSize) > requestedHight && (halfWidth / InSampleSize) > requestedWidth)
                {
                    InSampleSize *= 2;
                }
            }
            return InSampleSize;
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

