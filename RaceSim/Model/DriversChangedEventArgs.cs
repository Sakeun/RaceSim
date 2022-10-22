namespace Model;

public class DriversChangedEventArgs : EventArgs
{
    public Track Track { get; set; }
    public IParticipant[] Users { get; set; }

    public DriversChangedEventArgs(Track track, IParticipant[] users)
    {
        Track = track;
        Users = users;
    }
}