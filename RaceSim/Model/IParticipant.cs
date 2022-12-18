using System;
namespace Model
{
    public interface IParticipant
    {
        string Name { get; set; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors TeamColors { get; set; }
        int Rounds { get; set; }
        int TimesBroken { get; set; }
    }


    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }
}