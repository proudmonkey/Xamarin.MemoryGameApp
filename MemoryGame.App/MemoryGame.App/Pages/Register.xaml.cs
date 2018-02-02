using System;
using Xamarin.Forms;
using MemoryGame.App.Classes;
using System.Threading.Tasks;
using MemoryGame.App.Helper;

namespace MemoryGame.App.Pages
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        enum EntryOption
        {
            Register = 0,
            Returning = 1,
            Cancel = 2
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
        }
        async void CheckExistingProfileAndSave(string email)
        {
            if (Utils.IsConnectedToInternet())
            {
                try
                {
                    PlayerData player = await PlayerManager.CheckExistingPlayer(email);
                    if (string.IsNullOrEmpty(player.FirstName) && string.IsNullOrEmpty(player.LastName))
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Email does not exist.", "OK");
                    }
                    else
                    {
                        Settings.PlayerFirstName = player.FirstName.Trim();
                        Settings.PlayerLastName = player.LastName.Trim();
                        Settings.PlayerEmail = email.Trim();
                        Settings.TopScore = player.Best;
                        Settings.DateAchieved = player.DateAchieved;

                        await App._navPage.PopAsync();
                    }
                }
                catch
                {
                    await App.Current.MainPage.DisplayAlert("Oops", "An error occured while connecting to the server. Please check your connection.", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No internet connection.", "OK");
            }

            btnLogin.IsEnabled = true;
        }

        void Save()
        {
            Settings.PlayerFirstName = entryFirstName.Text.Trim();
            Settings.PlayerLastName = entryLastName.Text.Trim();
            Settings.PlayerEmail = entryEmail.Text.Trim();
            App._navPage.PopAsync();
        }

        void ToggleEntryView(EntryOption option)
        {
            switch (option)
            {
                case EntryOption.Register:
                    {
                        lblWelcome.IsVisible = false;
                        layoutChoose.IsVisible = false;
                        layoutLogin.IsVisible = false;
                        layoutRegister.IsVisible = true;
                        break;
                    }
                case EntryOption.Returning:
                    {
                        lblWelcome.IsVisible = false;
                        layoutChoose.IsVisible = false;
                        layoutRegister.IsVisible = false;
                        layoutLogin.IsVisible = true;
                        break;
                    }
                case EntryOption.Cancel:
                    {
                        lblWelcome.IsVisible = true;
                        layoutChoose.IsVisible = true;
                        layoutRegister.IsVisible = false;
                        layoutLogin.IsVisible = false;
                        break;
                    }
            }
        }
        void OnbtnNewClicked(object sender, EventArgs args)
        {
            ToggleEntryView(EntryOption.Register);
        }
        void OnbtnReturnClicked(object sender, EventArgs args)
        {
            ToggleEntryView(EntryOption.Returning);
        }
        void OnbtnCancelLoginClicked(object sender, EventArgs args)
        {
            ToggleEntryView(EntryOption.Cancel);
        }
        void OnbtnCancelRegisterClicked(object sender, EventArgs args)
        {
            ToggleEntryView(EntryOption.Cancel);
        }

        void OnbtnRegisterClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryFirstName.Text) || string.IsNullOrEmpty(entryLastName.Text) || string.IsNullOrEmpty(entryEmail.Text))
                App.Current.MainPage.DisplayAlert("Error", "Please supply the required fields.", "Got it");
            else
                Save();
        }

        void OnbtnLoginClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(entryExistingEmail.Text))
                App.Current.MainPage.DisplayAlert("Error", "Please supply your email.", "Got it");
            else
            {
                btnLogin.IsEnabled = false;
                CheckExistingProfileAndSave(entryExistingEmail.Text);
            }
        }
    }
}
