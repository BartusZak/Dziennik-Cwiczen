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
using MySql.Data.MySqlClient;

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
        public  byte[] picData;

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
                int contactID = mCwiczenia[(int)mSelectedPic.Tag].ID;

                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mSelectedPic.SetImageBitmap(DecodeBitmapFromStream(data.Data, 150, 150));

                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                MemoryStream memStream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memStream);
                picData = memStream.ToArray();
           
                //Probowalem z przeslanie obrazka do bazy MySql - nie wyszlo
               // DBConnect_cwiczenia img = new DBConnect_cwiczenia();
               //img.Update_Image(picData,contactID);

                // DBConnect_cwiczenia.Update_Image(picData);
                // WebClient client = new WebClient();
                // Uri uri = new Uri("http://bartuszak.pl/android/UpdateCwiczeniePhoto.php");

                // NameValueCollection parameters = new NameValueCollection();
                //// parameters.Add("Image", Convert.ToBase64String(picData));
                // //parameters.Add("ContactID", contactID.ToString());

                //client.UploadValuesAsync(uri, parameters);
                // client.UploadValuesCompleted += client_UploadValuesCompleted;
            }
        }
        

        void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.Result));
            });

        }

        private Bitmap DecodeBitmapFromStream(Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            //Decode with InJustDecodeBounds = true to check dimensions
            Stream stream = ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            //Calculate InSamplesize
            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

            //Decode bitmap with InSampleSize set
            stream = ContentResolver.OpenInputStream(data); //Must read again
            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);
            return bitmap;
        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            //Raw height and widht of image
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > requestedHeight || width > requestedWidth)
            {
                //the image is bigger than we want it to be
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while ((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
                {
                    inSampleSize *= 2;
                }

            }

            return inSampleSize;
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

                    CreateCwiczenieDialog dialog_dodaj = new CreateCwiczenieDialog();
                    FragmentTransaction transaction_dodaj = FragmentManager.BeginTransaction();

                    //Subscribe to event
                    dialog_dodaj.OnDodajCwiczenie += dialog_OnDodajCwiczenie;
                    dialog_dodaj.Show(transaction_dodaj, "dodaj cwiczenie");
                    return true;

                case Resource.Id.delete:

                    DeleteCwiczenieDialog dialog_usun = new DeleteCwiczenieDialog();
                    FragmentTransaction transaction_usun = FragmentManager.BeginTransaction();

                    //Subscribe to event
                    dialog_usun.OnUsunCwiczenie += dialog_OnUsunCwiczenie;
                    dialog_usun.Show(transaction_usun, "usun cwiczenie");
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

        void dialog_OnUsunCwiczenie(object sender, UsunCwiczenieEventArgs e)
        {
            //mCwiczenia.Remove(Cwiczenie() { Cwiczenie_v = e.Cwiczenie });
            mAdapter.NotifyDataSetChanged();
        }
    }
}

