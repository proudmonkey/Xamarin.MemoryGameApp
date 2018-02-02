using System;
using System.Threading.Tasks;
using MemoryGame.App.Classes;
using Xamarin.Forms;
using MemoryGame.App.Services;
using MemoryGame.App.Helper;

namespace MemoryGame.App.Pages
{
    public partial class Home : ContentPage
    {
        private static int _blinkCount = 0;
        private static int _soundCount = 0;
        private static int _hapticCount = 0;
        private static int _level = 1;

        public int _cycleStartInMS = 0;
        public int _cycleMaxInMS = 7000; // 7 seconds
        private const int CycleIntervalInMS = 2000; // 2 seconds
        private const int PlayTimeCount = 3; // 3 types default

        enum PlayType
        {
            Blink = 0,
            Sound = 1,
            Haptic = 2
        }

        public static int CurrentGameBlinkCount
        {
            get { return _blinkCount; }
        }

        public static int CurrentGameSoundCount
        {
            get { return _soundCount; }
        }

        public static int CurrentGameHapticCount
        {
            get { return _hapticCount; }
        }

        public static int CurrentGameLevel
        {
            get { return _level; }
        }

        public Home()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (string.IsNullOrEmpty(Settings.PlayerFirstName))
                await App._navPage.PushAsync(App._registerPage);
            else
            {

                PlayerManager.UpdateBest(_level);

                if (Result._answered)
                    LevelUp();
                else
                    ResetLevel();

                lblBest.Text = $"Best: Level {PlayerManager.GetBestScore(_level)}";
                lblLevel.Text = $"Level {_level}";
            }
        }

        static void IncrementPlayCount(PlayType play)
        {
            switch (play)
            {
                case PlayType.Blink:
                    {
                        _blinkCount++;
                        break;
                    }
                case PlayType.Sound:
                    {
                        _soundCount++;
                        break;
                    }
                case PlayType.Haptic:
                    {
                        _hapticCount++;
                        break;
                    }
            }
        }

        public static void IncrementGameLevel()
        {
            _level++;
        }

        void ResetLevel()
        {
            _level = 1;
            _cycleStartInMS = CycleIntervalInMS;
            lblTime.Text = string.Empty;
        }

        async void StartRandomPlay()
        {
            await Task.Run(() =>
            {

                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int choice = rnd.Next(0, PlayTimeCount);

                switch (choice)
                {
                    case (int)PlayType.Blink:
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {

                                await imgLightOff.FadeTo(0, 200);
                                imgLightOff2.IsVisible = false;
                                imgLightOff.IsVisible = true;
                                imgLightOff.Source = ImageSource.FromFile("lighton.png");
                                await imgLightOff.FadeTo(1, 200);

                            });
                            IncrementPlayCount(PlayType.Blink);
                            break;
                        }
                    case (int)PlayType.Sound:
                        {
                            DependencyService.Get<ISound>().PlayMp3File("beep.mp3");
                            IncrementPlayCount(PlayType.Sound);
                            break;
                        }
                    case (int)PlayType.Haptic:
                        {
                            DependencyService.Get<IHaptic>().ActivateHaptic();
                            IncrementPlayCount(PlayType.Haptic);
                            break;
                        }
                }
            });
        }

        void ResetGameCount()
        {
            _blinkCount = 0;
            _soundCount = 0;
            _hapticCount = 0;
        }

        void LevelUp()
        {
            _cycleStartInMS = _cycleStartInMS - 200; //minus 200 ms
        }

        void Play()
        {
            int timeLapsed = 0;
            int duration = 0;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                duration++;
                lblTime.Text = $"Timer: { TimeSpan.FromSeconds(duration).ToString("ss")}";

                if (duration < 7)
                    return true;
                else
                    return false;
            });

            Device.StartTimer(TimeSpan.FromMilliseconds(_cycleStartInMS), () => {
                timeLapsed = timeLapsed + _cycleStartInMS;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    imgLightOff2.IsVisible = true;
                    imgLightOff.IsVisible = false;
                    await Task.Delay(200);

                    //imgLightOn.IsVisible = false;
                    //imgLightOff.IsVisible = true;
                    //await Task.Delay(200);

                });

                if (timeLapsed <= _cycleMaxInMS)
                {
                    StartRandomPlay();
                    return true; //continue
                }

                btnStart.Text = "Start";
                btnStart.IsEnabled = true;

                App._navPage.PushAsync(App._resultPage);
                return false; //not continue
            });
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            btnStart.Text = "Game Started...";
            btnStart.IsEnabled = false;

            ResetGameCount();
            Play();
        }

        async void OnbtnSyncClicked(object sender, EventArgs args)
        {

            if (Utils.IsConnectedToInternet())
            {
                btnSync.Text = "Syncing...";
                btnSync.IsEnabled = false;
                btnStart.IsEnabled = false;

                var response = await PlayerManager.Sync();
                if (!response)
                    await App.Current.MainPage.DisplayAlert("Oops", "An error occured while connecting to the server. Please check your connection.", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Sync", "Data synced!", "OK");

                btnSync.Text = "Sync";
                btnSync.IsEnabled = true;
                btnStart.IsEnabled = true;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No internet connection.", "OK");
            }
        }
    }
}
