using Xamarin.Forms;
using Android.Media;
using MemoryGame.App.Droid.Services;
using MemoryGame.App.Services;

[assembly: Dependency(typeof(SoundService))]

namespace MemoryGame.App.Droid.Services
{
    public class SoundService : ISound
    {
        public SoundService() { }

        private MediaPlayer _mediaPlayer;

        public bool PlayMp3File(string fileName)
        {
            _mediaPlayer = new MediaPlayer();
            var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
            _mediaPlayer.Prepared += (s, e) =>
            {
                _mediaPlayer.Start();
            };
            _mediaPlayer.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            _mediaPlayer.Prepare();
            return true;
        }

        public bool PlayWavFile(string fileName)
        {
            //TO DO: Own implementation here
            return true;
        }
    }
}