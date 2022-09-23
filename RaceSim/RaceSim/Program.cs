using Model;
using Controller;
using RaceSim;

Competition c1 = new Competition();

Data.Initialize(c1);

Data.NextRace();
//Console.BackgroundColor = ConsoleColor.DarkGray;

//Visualization.DrawTrack(Data.CurrentRace.Track);
Console.WriteLine(Data.CurrentRace.Track.Name);
for (; ; )
{
    Thread.Sleep(100);
}