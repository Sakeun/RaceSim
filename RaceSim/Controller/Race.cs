using System;
using Model;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public IList<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        public Race(Track track, IList<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = new DateTime();
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
        }

        public void RandomizeEquipment(IList<IParticipant> participants)
        {
            foreach(IParticipant participant in participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }

        public SectionData GetSectionData(Section section)
        {
            if (_positions.ContainsKey(section)) {
                return _positions[section];
            } else
            {
                _positions.Add(section, new SectionData());
                return _positions[section];
            }
        }

        public void SetStartPosition(Track track, IList<IParticipant> participants)
        {
            
        }
    }
}

