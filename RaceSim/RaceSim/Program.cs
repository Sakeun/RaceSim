using Model;
using Controller;

Competition c1 = new Competition();

Data.Initialize(c1);

Data.NextRace();

Console.WriteLine(Data.CurrentRace.Track.Name);
for (; ; )
{
    Thread.Sleep(100);
}