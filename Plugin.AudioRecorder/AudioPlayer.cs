using System;
using System.IO;

namespace Plugin.AudioRecorder
{
	public partial class AudioPlayer
    {
		public void Play (string pathToAudioFile) => throw new NotImplementedException ();
		public void Play (Stream stream, string contentType = "audio/wav") => throw new NotImplementedException();

		public void Pause () => throw new NotImplementedException ();

		public void Play () => throw new NotImplementedException ();
	}
}
