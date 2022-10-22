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

        public Driver(string name, TeamColors teamcolors, IEquipment equipment)
        {
            Name = name;
            Points = 0;
            TeamColors = teamcolors;
            Equipment = equipment;
        }
    }
}

