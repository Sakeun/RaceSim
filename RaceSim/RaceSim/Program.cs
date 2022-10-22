using Model;
using Controller;
using RaceSim;

Competition c1 = new Competition();

Data.Initialize(c1);

Track data = Data.NextRace();
//Console.BackgroundColor = ConsoleColor.DarkGray;

IParticipant[] users = Data.CurrentRace.Participants.ToArray();

Visualization.DrawTrack(Data.CurrentRace.Track, users);
//Console.WriteLine(Data.CurrentRace.Track.Name);

Visualization.Initialize();

Data.CurrentRace.Start();

Visualization.OnDriversChanged(data, new DriversChangedEventArgs(Data.CurrentRace.Track, users));


for (; ; )
{
    Thread.Sleep(100);
}