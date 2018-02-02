using System;
using Xamarin.Forms;
using MemoryGame.App.Classes;

namespace MemoryGame.App.Pages
{
    public partial class Result : ContentPage
    {
        public static bool _answered = false;
        public Result()
        {
            InitializeComponent();
            ClearResult();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ClearResult();
            NavigationPage.SetHasBackButton(this, false);
        }
        void ClearResult()
        {
            lblText.Text = string.Empty;
            lblBlinkCount.Text = string.Empty;
            lblBeepCount.Text = string.Empty;
            lblHapticCount.Text = string.Empty;
            pickerLight.SelectedIndex = 0;
            pickerSpeaker.SelectedIndex = 0;
            pickerHaptic.SelectedIndex = 0;
            btnSubmit.IsVisible = true;
            btnRetry.IsVisible = false;
            _answered = false;
        }

        bool CheckAnswer(int actualAnswer, int selectedAnswer)
        {
            if (selectedAnswer == actualAnswer)
                return true;
            else
                return false;
        }

        void Retry()
        {
            btnSubmit.IsVisible = false;
            btnRetry.IsVisible = true;
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            if (pickerLight.SelectedIndex >= 0 && pickerSpeaker.SelectedIndex >= 0 && pickerHaptic.SelectedIndex >= 0)
            {
                lblText.Text = "The actual answers are:";
                lblBlinkCount.Text = Home.CurrentGameBlinkCount.ToString();
                lblBeepCount.Text = Home.CurrentGameSoundCount.ToString();
                lblHapticCount.Text = Home.CurrentGameHapticCount.ToString();

                if (CheckAnswer(Home.CurrentGameBlinkCount, Convert.ToInt32(pickerLight.Items[pickerLight.SelectedIndex])))
                    if (CheckAnswer(Home.CurrentGameSoundCount, Convert.ToInt32(pickerSpeaker.Items[pickerSpeaker.SelectedIndex])))
                        if (CheckAnswer(Home.CurrentGameHapticCount, Convert.ToInt32(pickerHaptic.Items[pickerHaptic.SelectedIndex])))
                        {
                            _answered = true;
                            Home.IncrementGameLevel();

                            var isSynced = PlayerManager.CheckScoreAndSync(Home.CurrentGameLevel);

                            var answer = await App.Current.MainPage.DisplayAlert("Congrats!", $"You've got it all right and made it to level {Home.CurrentGameLevel}. Continue?", "Yes", "No");

                            if (answer)
                                await App._navPage.PopAsync();
                            else
                                Retry();
                        }

                if (!_answered)
                {
                    var isSynced = PlayerManager.CheckScoreAndSync(Home.CurrentGameLevel);

                    var answer = await App.Current.MainPage.DisplayAlert("Game Over!", $"Your current best is at level {Home.CurrentGameLevel}. Retry?", "Yes", "No");
                    if (answer)
                        await App._navPage.PopAsync();
                    else
                        Retry();
                }
            }
        }

        void OnRetryButtonClicked(object sender, EventArgs args)
        {
            App._navPage.PopAsync();
        }
    }
}
