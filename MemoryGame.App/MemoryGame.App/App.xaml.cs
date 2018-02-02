using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MemoryGame.App.Pages;

namespace MemoryGame.App
{
    public partial class App : Application
    {
        public static NavigationPage _navPage;
        public static Home _homePage;
        public static Result _resultPage;
        public static Register _registerPage;

        public App()
        {
           
            _homePage = new Home();
            _resultPage = new Result();
            _registerPage = new Register();
            _navPage = new NavigationPage(_homePage);

            MainPage = _navPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
