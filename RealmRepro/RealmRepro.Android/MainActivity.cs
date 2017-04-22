using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Realms;

namespace RealmRepro.Droid
{
    [Activity(Label = "RealmRepro.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };

            //Realm.DeleteRealm(new RealmConfiguration());
            var db = Realm.GetInstance();
            using (var transaction = db.BeginWrite())
            {
                db.Add(new MyClass
                {
                    Time = DateTimeOffset.Now
                });
                transaction.Commit();
            }
            var time = db.All<MyClass>().ElementAt(0);
            button.Text = time.ToString();
        }
    }
}