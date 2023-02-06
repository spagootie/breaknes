using Be.Windows.Forms;
using NSFPlayerCustomClass;
using System.Runtime.InteropServices;

namespace NSFPlayer
{
	public partial class FormMain : Form
	{
		[DllImport("kernel32")]
		static extern bool AllocConsole();

		private DSound? audio_backend;
		private NSFLoader nsf = new();
		private bool nsf_loaded = false;
		private byte current_song = 0;
		private bool Paused = true;			// atomic

		private int SourceSampleRate = 48000;
		private List<float> SampleBuf = new();

		private string DefaultTitle = "";

		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
#if DEBUG
			AllocConsole();
#endif

			audio_backend = new DSound(Handle);

			DefaultTitle = this.Text;

			comboBox2.SelectedIndex = 0;
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormSettings formSettings = new();
			formSettings.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormAbout dlg = new FormAbout();
			dlg.ShowDialog();
		}


		#region "NSF Controls"

		private void InitBoard(string nsf_filename)
		{
			byte[] data = File.ReadAllBytes(nsf_filename);
			nsf.LoadNSF(data);
			nsf_loaded = true;
			SetSong(nsf.GetHead().StartingSong);
			this.Text = DefaultTitle + " - " + nsf_filename;

			var settings = FormSettings.LoadSettings();
			NSFPlayerInterop.CreateBoard("NSFPlayer", settings.APU_Revision, "", "");

			// Autoplay
			SetPaused(false);
		}

		private void DisposeBoard()
		{
			SetPaused(true);
			NSFPlayerInterop.DestroyBoard();
			nsf_loaded = false;
			nsf = new();
			this.Text = DefaultTitle;
			UpdateTrackStat();
		}

		private void loadNSFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string nsf_filename = openFileDialog1.FileName;
				InitBoard(nsf_filename);
			}
		}

		private void playToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetPaused(false);
		}

		private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetPaused(true);
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DisposeBoard();
		}

		private void previousTrackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PrevSong();
		}

		private void nextTrackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NextSong();
		}

		private void nSFInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormNSFInfo info = new FormNSFInfo(nsf_loaded ? nsf.GetHead() : null);
			info.ShowDialog();
		}

		private void SetSong(byte n)
		{
			current_song = n;
			UpdateTrackStat();
		}

		private void PrevSong()
		{
			if (!nsf_loaded)
				return;
			byte total = nsf.GetHead().TotalSongs;
			if (current_song <= 1)
				return;
			current_song--;
			UpdateTrackStat();
		}

		private void NextSong()
		{
			if (!nsf_loaded)
				return;
			byte total = nsf.GetHead().TotalSongs;
			if (current_song >= total)
				return;
			current_song++;
			UpdateTrackStat();
		}

		private void UpdateTrackStat()
		{
			if (nsf_loaded)
				toolStripStatusLabel4.Text = current_song.ToString() + " / " + nsf.GetHead().TotalSongs.ToString();
			else
				toolStripStatusLabel4.Text = "Not loaded";
		}

		private void SetPaused(bool paused)
		{
			Paused = paused;
			toolStripStatusLabelState.Text = paused ? "Paused" : "Running";
		}

		#endregion "NSF Controls"


		#region "Sample Buffer Playback Controls"

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			if (audio_backend != null)
				audio_backend.PlaySampleBuf(SourceSampleRate, SampleBuf);
		}

		private void toolStripButtonDiscard_Click(object sender, EventArgs e)
		{
			if (audio_backend != null)
				audio_backend.StopSampleBuf();
			SampleBuf.Clear();
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			if (audio_backend != null)
				audio_backend.StopSampleBuf();
		}

		#endregion "Sample Buffer Playback Controls"


		#region "APU Debug"

		private void loadSaveLogisimHexAsBinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialogHEX.ShowDialog() == DialogResult.OK)
			{
				string hex_filename = openFileDialogHEX.FileName;
				byte[] arr = LogisimHEXConv.HEXToByteArray(File.ReadAllText(hex_filename));

				if (saveFileDialogBin.ShowDialog() == DialogResult.OK)
				{
					string bin_filename = saveFileDialogBin.FileName;
					File.WriteAllBytes(bin_filename, arr);
				}
			}
		}

		List<BreaksCore.MemDesciptor> mem = new();

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			Button2Click();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (UpdateMemLayoutInProgress)
				return;

			Button1Click();
		}

		/// <summary>
		/// Update DebugInfo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			Button2Click();
		}

		/// <summary>
		/// Dump Mem
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			Button1Click();
		}

		void Button2Click()
		{
			List<BreaksCore.DebugInfoEntry> entries = BreaksCore.GetDebugInfo(ComboBoxToDebugInfoType());
			UpdateDebugInfo(entries);
		}

		BreaksCore.DebugInfoType ComboBoxToDebugInfoType()
		{
			switch (comboBox2.SelectedIndex)
			{
				case 0:
					return BreaksCore.DebugInfoType.DebugInfoType_Core;
				case 1:
					return BreaksCore.DebugInfoType.DebugInfoType_CoreRegs;
				case 2:
					return BreaksCore.DebugInfoType.DebugInfoType_APU;
				case 3:
					return BreaksCore.DebugInfoType.DebugInfoType_APURegs;
				case 4:
					return BreaksCore.DebugInfoType.DebugInfoType_Board;
			}

			return BreaksCore.DebugInfoType.DebugInfoType_Test;
		}

		void UpdateDebugInfo(List<BreaksCore.DebugInfoEntry> entries)
		{
			// Construct an object to show in PropertyGrid

			CustomClass myProperties = new CustomClass();

			foreach (BreaksCore.DebugInfoEntry entry in entries)
			{
				CustomProperty myProp = new CustomProperty();
				myProp.Name = entry.name;
				myProp.Category = entry.category;
				myProp.Value = entry.value;
				myProperties.Add(myProp);
			}

			propertyGrid1.SelectedObject = myProperties;
		}

		void Button1Click()
		{
			if (mem.Count == 0)
			{
				hexBox1.ByteProvider = new DynamicByteProvider(new byte[0]);
				hexBox1.Refresh();
				return;
			}

			int descrID = comboBox1.SelectedIndex;

			{
				byte[] buf = new byte[mem[descrID].size];

				BreaksCore.DumpMem(descrID, buf);
				hexBox1.ByteProvider = new DynamicByteProvider(buf);
				hexBox1.Refresh();
			}
		}

		bool UpdateMemLayoutInProgress = false;

		/// <summary>
		/// Get a set of memory regions from the debugger and fill the ComboBox.
		/// </summary>
		void UpdateMemLayout()
		{
			UpdateMemLayoutInProgress = true;

			comboBox1.Items.Clear();

			mem = BreaksCore.GetMemoryLayout();

			foreach (var descr in mem)
			{
				comboBox1.Items.Add(descr.name);
			}

			if (mem.Count != 0)
			{
				comboBox1.SelectedIndex = 0;
			}

			UpdateMemLayoutInProgress = false;
		}

		#endregion "APU Debug"

		private void sendFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenUrl("https://github.com/emu-russia/breaknes/issues");
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenUrl("https://github.com/emu-russia/breaknes/releases");
		}

		private void OpenUrl(string url)
		{
			// https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp

			try
			{
				System.Diagnostics.Process.Start(url);
			}
			catch
			{
				// hack because of this: https://github.com/dotnet/corefx/issues/10361
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					url = url.Replace("&", "^&");
					System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					System.Diagnostics.Process.Start("xdg-open", url);
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					System.Diagnostics.Process.Start("open", url);
				}
				else
				{
					throw;
				}
			}
		}
	}
}
