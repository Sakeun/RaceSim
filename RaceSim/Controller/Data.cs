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
            Driver d1 = new Driver("Anakin", TeamColors.Yellow, new Pod());
            Driver d2 = new Driver("Quadinaros", TeamColors.Red, new Pod());
            Driver d3 = new Driver("Sebulba", TeamColors.Yellow, new Pod());
            Driver d4 = new Driver("Nemesso", TeamColors.Blue, new Pod());
            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
            Competition.Participants.Add(d4);
        }

        private static void addTracks()
        {
            SectionTypes[] types = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish };
            SectionTypes[] raceTrack =
            {
                SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner
            };
            SectionTypes[] raceTrack2 =
            {
                SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.RightCorner
            };
            SectionTypes[] raceTrack3 =
            {
                SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Straight,
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner
            };
            SectionTypes[] raceTrack4 =
            {
                SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.RightCorner, 
            };
            Track t1 = new Track("Aleen", raceTrack4);
            Track t2 = new Track("Mon Gazza", types);
            Track t3 = new Track("Baroonda", types);
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

