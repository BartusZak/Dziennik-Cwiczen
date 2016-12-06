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
        MySqlConnection connection;

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

                Update_Image(picData, contactID);

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
        //Update statement
        public void Update_Image(byte[] img, int cwiczenie_ID)
        {
            string query = "UPDATE Cwiczenia SET Cwiczenie_Nazwa='bartek' WHERE Cwiczenie_ID='65'";
            
            string server = "bartuszak.pl";
            string database = "android";
           string  uid = "android";
           string password = "hbsUu6yRdx6Xa4vx";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        protected internal bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
               // txtSysLog.Text = ex.Message;
                return false;
            }
        }
        protected internal bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                       // txtSysLog.Text = "Cannot connect to server.  Contact administrator";
                        break;

                    case 1045:
                       // txtSysLog.Text = "Invalid username/password, please try again";
                        break;
                }
                return false;
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

