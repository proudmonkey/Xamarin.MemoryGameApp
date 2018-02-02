using MemoryGame.App.Pages;
using MemoryGame.App.Services;
using System;
using Xamarin.Forms;

namespace MemoryGame.App.Classes
{
    public class MemoryGame
    {
        public static int _blinkCount = 0;
        public static int _soundCount = 0;
        public static int _hapticCount = 0;
        public static byte _level = 1;
        public int _cycleStart = 7;

        Home _homePage = new Home();

        public void StartRandomPlay()
        {

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int choice = rnd.Next(0, 3);

            switch (choice)
            {
                case 0:
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                            _homePage.FindByName<Image>("imgLight").IsVisible = false;
                            _homePage.FindByName<Image>("imgLightOn").IsVisible = true;
                        });
                        _blinkCount++;
                        break;
                    }
                case 1:
                    {
                        DependencyService.Get<ISound>().PlayMp3File("beep.mp3");
                        _soundCount++;
                        break;
                    }
                case 2:
                    {
                        DependencyService.Get<IHaptic>().ActivateHaptic();
                        _hapticCount++;
                        break;
                    }
            }
        }

        public void ResetGameCount()
        {
            _blinkCount = 0;
            _soundCount = 0;
            _hapticCount = 0;
        }

        public void LevelUp()
        {
            _cycleStart = _cycleStart + 2;
        }
        public void ResetLevel()
        {
            _level = 1;
            _cycleStart = 7;
        }
    }
}
