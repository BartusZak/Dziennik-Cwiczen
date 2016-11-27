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
using Android.Graphics;

namespace Dziennik
{
    class CwiczenieListAdapter : BaseAdapter<Cwiczenie>
    {
        private Context mContext;
        private int mLayout;
        private List<Cwiczenie> mCwiczenia;
        private Action<ImageView> mActionPicSelected;

        public CwiczenieListAdapter (Context context, int layout, List<Cwiczenie> contacts, Action<ImageView> picSelected)
        {
            mContext = context;
            mLayout = layout;
            mCwiczenia = contacts;
            mActionPicSelected = picSelected;
        }

        public override Cwiczenie this[int position]
        {
            get { return mCwiczenia[position]; }
        }

        public override int Count
        {
            get { return mCwiczenia.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mLayout, parent, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtCwiczenie).Text = mCwiczenia[position].Cwiczenie_v;
            row.FindViewById<TextView>(Resource.Id.txtIloscSerii).Text = mCwiczenia[position].IloscSerii_v;
            row.FindViewById<TextView>(Resource.Id.txtIloscPowtorzen).Text = mCwiczenia[position].IloscPowtorzen_v;

            ImageView pic = row.FindViewById<ImageView>(Resource.Id.imgPic);

            if (mCwiczenia[position].Image != null)
            {
                pic.SetImageBitmap(BitmapFactory.DecodeByteArray(mCwiczenia[position].Image, 0, mCwiczenia[position].Image.Length));
            }

            pic.Click -= pic_Click;
            pic.Click += pic_Click;
            return row;
        }

        void pic_Click(object sender, EventArgs e)
        {
            //Picture has been clicked
            mActionPicSelected.Invoke((ImageView)sender);
        }
    }
}