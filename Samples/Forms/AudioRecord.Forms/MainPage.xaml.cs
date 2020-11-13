using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.AudioRecorder;

namespace AudioRecord.Forms
{
	public partial class MainPage : ContentPage
	{
		AudioRecorderService recorder;
		AudioPlayer player;
		private Stream stream;

		public MainPage ()
		{
			InitializeComponent ();

			recorder = new AudioRecorderService
			{
				StopRecordingAfterTimeout = true,
				TotalAudioTimeout = TimeSpan.FromSeconds (15),
				AudioSilenceTimeout = TimeSpan.FromSeconds (2)
			};

			player = new AudioPlayer ();
			player.FinishedPlaying += Player_FinishedPlaying;
		}

		async void Record_Clicked (object sender, EventArgs e)
		{
			await RecordAudio ();
		}

		async Task RecordAudio ()
		{
			try
			{
				if (!recorder.IsRecording) //Record button clicked
				{
					recorder.StopRecordingOnSilence = TimeoutSwitch.IsToggled;

					RecordButton.IsEnabled = false;
					PlayButton.IsEnabled = false;
					
					stream?.Dispose();
					stream = new MemoryStream();

					//start recording audio
					var audioRecordTask = await recorder.StartRecording(stream);

					RecordButton.Text = "Stop Recording";
					RecordButton.IsEnabled = true;

					await audioRecordTask;

					RecordButton.Text = "Record";
					PlayButton.IsEnabled = true;
				}
				else //Stop button clicked
				{
					RecordButton.IsEnabled = false;

					//stop the recording...
					await recorder.StopRecording ();

					RecordButton.IsEnabled = true;
				}
			}
			catch (Exception ex)
			{
				//blow up the app!
				throw ex;
			}
		}

		void Play_Clicked (object sender, EventArgs e)
		{
			PlayAudio ();
		}

		void PlayAudio ()
		{
			try
			{
				var filePath = recorder.GetAudioFilePath ();

				if (filePath != null)
				{
					PlayButton.IsEnabled = false;
					RecordButton.IsEnabled = false;

					player.Play (filePath);
				} else if (stream != null)
				{
					player.Play(stream);
				}
			}
			catch (Exception ex)
			{
				//blow up the app!
				throw ex;
			}
		}

		void Player_FinishedPlaying (object sender, EventArgs e)
		{
			PlayButton.IsEnabled = true;
			RecordButton.IsEnabled = true;
		}
	}
}
