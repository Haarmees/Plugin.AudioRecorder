using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Plugin.AudioRecorder
{
	public partial class AudioPlayer
	{
		MediaPlayer audioPlayer;

		public AudioPlayer ()
		{
			audioPlayer = new MediaPlayer ();
			audioPlayer.MediaEnded += AudioPlayer_MediaEnded;
		}

		public void Play (string pathToAudioFile)
		{
			_ = PlayAudio (pathToAudioFile);
		}

		public void Play(Stream stream)
		{
			_ = PlayAudio (stream);
		}

		async Task PlayAudio (string pathToAudioFile)
		{
			StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;

			var storageFile = await StorageFile.GetFileFromPathAsync (pathToAudioFile);

			IRandomAccessStream stream = await storageFile.OpenAsync (FileAccessMode.Read);
			audioPlayer.Source = MediaSource.CreateFromStream (stream, storageFile.FileType);
			audioPlayer.Play ();
		}
		
		async Task PlayAudio (Stream stream, string contentType = "audio/wav")
		{
			audioPlayer.Source = MediaSource.CreateFromStream(stream.AsRandomAccessStream(), contentType);
			audioPlayer.Play();
		}

		async void AudioPlayer_MediaEnded (MediaPlayer sender, object args)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync (CoreDispatcherPriority.Normal, () =>
			{
				FinishedPlaying?.Invoke (this, EventArgs.Empty);
			});
		}

		public void Pause ()
		{
			audioPlayer?.Pause ();
		}

		public void Play ()
		{
			audioPlayer?.Play ();
		}
	}
}
