using Controller;
using Model;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;

namespace WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RaceStats _raceStatsScreen;
        private PlayerStats _playerStatsScreen;
        public MainWindow()
        {
            InitializeComponent();
            InitializeController();

            this.DataContext = new DataContextBinding();
        }

        private void InitializeController()
        {
            Competition comp = new Competition();
            Data.Initialize(comp);
            Track data = Data.NextRace();
            Data.CurrentRace.DriversChanged += OnDriverChanged;
			Data.CurrentRace.NextRaceStart += OnRaceDone;
		}

        public void OnDriverChanged(object? sender, DriversChangedEventArgs e)
        {
            this.ImageDraw.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() =>
            {
                this.ImageDraw.Source = null;
                this.ImageDraw.Source = WPFVisualization.DrawTrack(e.Track);
            }));

			if (Data.CurrentRace.RaceDone)
			{
                ImageCreator.ClearCache();
				Data.CurrentRace.DriversChanged -= OnDriverChanged;
                Data.CurrentRace.NextRaceStart -= OnRaceDone;
                if (Data.Competition.Tracks.Count != 0)
                {
                    Data.CurrentRace.NextRaceStart += OnRaceDone;
                    Data.CurrentRace.DriversChanged += OnDriverChanged;
                    e.Track = Data.CurrentRace.Track;
                }
			}
		}

		private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
		{
            Application.Current.Shutdown();
		}

		private void MenuItem_Players_Click(object sender, RoutedEventArgs e)
		{
            _playerStatsScreen= new PlayerStats();
            _playerStatsScreen.Show();
		}

		private void MenuItem_Race_Click(object sender, RoutedEventArgs e)
		{
            _raceStatsScreen = new RaceStats();
            _raceStatsScreen.Show();
		}

		public static void OnRaceDone(object? sender, EventArgs e)
		{
		    if (!Data.CurrentRace.RaceDone) return;

			Data.CurrentRace.Track = Data.NextRace();
		    foreach (var participant in Data.Competition.Participants)
		    {
		        participant.Rounds = 0;
                participant.RoundsDone = false;
		    }
            Data.CurrentRace.Start();
        }
	}
}
