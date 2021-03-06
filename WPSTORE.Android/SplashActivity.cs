﻿using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Com.Airbnb.Lottie;
using System.Threading.Tasks;

namespace WPSTORE.Droid
{
    [Activity(Label = "TeaTalk",
        Icon = "@drawable/icon", 
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Splash);

            var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.AddAnimatorListener(this);
            //Task.Delay(500);
            //Finish();
            //StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }
    }
}
