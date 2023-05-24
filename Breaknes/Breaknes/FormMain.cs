using SharpTools;
using System.Runtime.InteropServices;

namespace Breaknes
{
	public partial class FormMain : Form
	{
		[DllImport("kernel32")]
		static extern bool AllocConsole();

		private BoardControl board = new();
		private VideoRender vid_out = null;
		private AudioRender snd_out = null;
		private string original_title;
		private List<FormDebugger> debuggers = new();

		public FormMain()
		{
			InitializeComponent();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormAbout about = new FormAbout();
			about.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			board.Paused = true;
			board.EjectCartridge();
			board.DisposeBoard();
			Close();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			original_title = Text;
			var settings = FormSettings.LoadSettings();
			if (settings.AllocConsole)
			{
				AllocConsole();
			}
			board.onUpdateWaves += OnUpdateWaves;
			board.CreateBoard(BoardDescriptionLoader.Load(), settings.MainBoard);
			backgroundWorker1.RunWorkerAsync();
		}

		private void loadROMDumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string filename = openFileDialog1.FileName;
				board.Paused = true;
				board.EjectCartridge();
				if (board.InsertCartridge(filename) < 0)
				{
					MessageBox.Show("Unsupported or corrupted NES ROM image format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				BreaksCore.Reset();
				Text = original_title + " - " + filename;
				var settings = FormSettings.LoadSettings();
				var rom_name = Path.GetFileNameWithoutExtension(filename);
				vid_out = new(OnRenderField, settings.DumpVideo, settings.DumpVideoDir, rom_name);
				vid_out.SetOutputPictureBox(pictureBox1);
				snd_out = new(Handle, settings.DumpAudio, settings.DumpAudioDir, rom_name, settings.IIR, settings.CutoffFrequency);
				board.Paused = debuggers.Count != 0;

				foreach (var inst in debuggers)
				{
					inst.UpdateOnRomLoad();
					inst.ResetWaves();
				}
			}
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormSettings settings = new FormSettings();
			settings.FormClosed += Settings_FormClosed;
			settings.ShowDialog();
		}

		private void Settings_FormClosed(object? sender, FormClosedEventArgs e)
		{
			FormSettings form_settings = (FormSettings)sender;

			if (form_settings.PurgeBoard)
			{
				board.Paused = true;
				board.EjectCartridge();
				board.DisposeBoard();
				var settings = FormSettings.LoadSettings();
				board.CreateBoard(BoardDescriptionLoader.Load(), settings.MainBoard);
				Text = original_title;
			}
		}

		private void openDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormDebugger debugger = new(board);
			debugger.FormClosed += Debugger_FormClosed;
			debugger.Show();
			debuggers.Add(debugger);
		}

		private void Debugger_FormClosed(object? sender, FormClosedEventArgs e)
		{
			debuggers.Remove((FormDebugger)sender);
		}

		private void OnRenderField()
		{
			foreach (var inst in debuggers)
			{
				inst.UpdateOnRenderField();
			}
		}

		private void OnUpdateWaves()
		{
			foreach (var inst in debuggers)
			{
				inst.UpdateWaves();
			}
		}
	}
}
