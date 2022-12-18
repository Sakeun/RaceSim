namespace Model
{
    public class Driver : IParticipant
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private int _points;
        public int Points { get { return _points; } set { _points = value; } }

        private IEquipment _equipment;
        public IEquipment Equipment { get { return _equipment; } set { _equipment = value; } }

        private TeamColors _teamColors;
        public TeamColors TeamColors { get { return _teamColors; } set { _teamColors = value; } }

        public int Rounds { get; set; }

        public int TimesBroken { get; set; }

        public Driver(string name, TeamColors teamcolors, IEquipment equipment)
        {
            Name = name;
            Points = 0;
            TeamColors = teamcolors;
            Equipment = equipment;
            Rounds = 0;
            TimesBroken = 0;
        }
    }
}

