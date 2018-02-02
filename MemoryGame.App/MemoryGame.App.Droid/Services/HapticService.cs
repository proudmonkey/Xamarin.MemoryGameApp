using Android.Content;
using Android.OS;
using Xamarin.Forms;
using MemoryGame.App.Droid.Services;
using MemoryGame.App.Services;

[assembly: Dependency(typeof(HapticService))]
namespace MemoryGame.App.Droid.Services
{
    public class HapticService : IHaptic
    {
        public HapticService() { }
        public void ActivateHaptic()
        {
            Vibrator vibrator = (Vibrator)global::Android.App.Application.Context.GetSystemService(Context.VibratorService);
            vibrator.Vibrate(100);
        }
    }
}