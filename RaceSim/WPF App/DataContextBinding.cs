using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_App
{
	public class DataContextBinding : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public string CurrentTrackName => Data.CurrentRace.Track?.Name;

		public List<string> PlayerEquipment => Data.Competition.Participants.Select(i => $"Quality: {i.Equipment.Quality}, Speed: {i.Equipment.Speed}").ToList();
		public List<string> PlayerTotalPoints => Data.Competition.Participants.Select(i => $"{i.Name} has {i.Points} Points").ToList();
		public List<string> TimesBroken => Data.Competition.Participants.Select(i => $"{i.Name} has been broken: {i.TimesBroken}").ToList();
		public string CurrentFirstPlace => Data.CurrentRace.FirstPlace;
		public string UpcomingTrack => Data.Competition.Tracks.Count == 0 ? "Final Track" : Data.Competition.Tracks.Peek().Name;


		public DataContextBinding()
		{
			if(Data.CurrentRace != null)
			{
				Data.CurrentRace.DriversChanged += DriverPropertyChanged;
			}
		}

		public void DriverPropertyChanged(object sender, DriversChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
	}
	}
}
