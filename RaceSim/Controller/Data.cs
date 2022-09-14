using System;
using Model;
namespace Controller
{
    public static class Data
    {
        public static Competition Competition;
        public static Race CurrentRace;

        public static void Initialize(Competition competition)
        {
            Competition = competition;
            addParticipants();
            addTracks();
        }

        private static void addParticipants()
        {
            Driver d1 = new Driver("Strijders", TeamColors.Green);
            Driver d2 = new Driver("Helden", TeamColors.Red);
            Driver d3 = new Driver("Beesten", TeamColors.Yellow);
            Driver d4 = new Driver("Giganten", TeamColors.Blue);
            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
            Competition.Participants.Add(d4);
        }

        private static void addTracks()
        {
            SectionTypes[] types = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish };
            Track t1 = new Track("RaceTrack", types);
            Track t2 = new Track("DesertTrack", types);
            Track t3 = new Track("WaterTrack", types);
            Competition.Tracks.Enqueue(t1);
            Competition.Tracks.Enqueue(t2);
            Competition.Tracks.Enqueue(t3);
        }

        public static Track NextRace()
        {
            var CurrentTrack = Competition.NextTrack();
            if(CurrentTrack != null)
            {
                CurrentRace = new Race(CurrentTrack, Competition.Participants);
            }

            return CurrentTrack;
        }
    }
}

