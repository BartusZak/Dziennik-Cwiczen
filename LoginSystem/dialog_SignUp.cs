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
    public class OnSignUpEventArgs : EventArgs
    {
        //private string mFirstName;
        //private string mEmail;
        //private string mPassword;

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public OnSignUpEventArgs(int id,string firstName, string email, string password) : base()
        {
            ID = id;
            FirstName = firstName;
            Email = email;
            Password = password;
        }
    }
    class dialog_SignUp : DialogFragment
    {
        public EditText txtFirstName;
        private EditText txtEmail;
        private EditText txtPassword;
        private Button mBtnSignUp;
        private ProgressBar mProgressBar_Sign_up;

        public event EventHandler<OnSignUpEventArgs> mOnSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);
            txtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            txtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            txtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);
            mProgressBar_Sign_up = view.FindViewById<ProgressBar>(Resource.Id.progressBar_sign_up);

            mBtnSignUp.Click += mBtnSignUp_Click;
            return view;           
        }

         void mBtnSignUp_Click(object sender, EventArgs e)
        {
            mProgressBar_Sign_up.Visibility = ViewStates.Visible;   

            WebClient client = new WebClient();
            Uri uri = new Uri("http://bartuszak.pl/android/CreateUser.php");
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("FirstName", txtFirstName.Text);
            parameters.Add("email", txtEmail.Text);
            parameters.Add("password", txtPassword.Text);

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameters); 
            
        }

        void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Activity.RunOnUiThread(() =>
            {
                string id = Encoding.UTF8.GetString(e.Result);
                int newID = 0;

                int.TryParse(id, out newID);

               if (mOnSignUpComplete != null)
                {
                   // User has clicked the sign up buttoon
                    mOnSignUpComplete.Invoke(this, new OnSignUpEventArgs(newID,txtFirstName.Text, txtEmail.Text, txtPassword.Text));
                }


               this.Dismiss();
            });
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); // Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;// Set the animiation
        }
    }

}