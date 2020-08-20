using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FeedMe.Models;

namespace FeedMe
{
    [Activity(Label = "RegisterActivitycs")]
    public class RegisterActivitycs : Activity
    {
        private EditText usernameEditText;
        private EditText passwordEditText;
        private EditText firstNameEditText;
        private EditText lastNameEditText;
        private EditText emailEditText;
        private Button registerButton;
        private Button backButton;
        private HttpClient httpClient = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_register);

            InitializeViews();
        }

        private void InitializeViews()
        {
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            firstNameEditText = FindViewById<EditText>(Resource.Id.firstNameEditText);
            lastNameEditText = FindViewById<EditText>(Resource.Id.lastNameEditText);
            emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);
            backButton = FindViewById<Button>(Resource.Id.backButton);

            registerButton.Click += RegisterButton_Click;
            backButton.Click += BackButton_Click;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            var registerRequest = await httpClient.GetAsync($"http://10.0.2.2:59671/Users/RegisterUser?firstname={firstNameEditText.Text}&lastname={lastNameEditText.Text}&email={emailEditText.Text}&username={usernameEditText.Text}&password={passwordEditText.Text}");
            if (registerRequest.IsSuccessStatusCode)
            {
                Toast.MakeText(this, "Registration Successful", ToastLength.Short).Show();
                // immediately login user
            }
            else
            {
                Toast.MakeText(this, "Error!", ToastLength.Short).Show();
            }
        }
    }
}