﻿using System.Timers;
using Model;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public event EventHandler<DriversChangedEventArgs> DriversChanged;
        public event EventHandler NextRaceStart;
        public Track Track { get; set; }
        public IList<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private Random _random { get; set; }
        private Dictionary<Section, SectionData> _positions { get; set; }

        private Timer _timer { get; set; } = new(500);

        private Dictionary<IParticipant, Stack<Section>> _playerStack { get; set; } = new ();
        
        public bool RaceDone { get; set; }

        public Race(Track track, IList<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = new DateTime();
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            SetStartPosition(Track);
            RandomizeEquipment();

            foreach (IParticipant p in participants)
            {
                _playerStack.Add(p, CreateStack());
                if (_playerStack.Count > 2)
                {
                    _playerStack[p].Pop();
                }
            }

            _timer.Elapsed += OnTimedEvent;
            
            Start();
            
            OnTimedEvent(this, null);
        }

        //Randomize the equipment of the participants in the race
        private void RandomizeEquipment()
        {
            foreach(IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(80, 200);
                participant.Equipment.Performance = _random.Next(1, 3);
                participant.Equipment.Speed = _random.Next(10, 25);
            }
        }

        //GetSectionData used to add users to a new section. If the section already exists the function will return the existing section
        public SectionData GetSectionData(Section section, IParticipant participant1, IParticipant participant2, int disleft, int disright)
        {
            if (_positions.ContainsKey(section)) {
                return _positions[section];
            }
            _positions.Add(section, new SectionData(participant1, participant2, disleft, disright)); 
            return _positions[section];
        }

        // GetSectionData used to either return a section
        public SectionData GetSectionData(Section section)
        {
            SectionData data = _positions.ContainsKey(section) ? _positions[section] : null;
            return data;
        }

        //Removes the player from the section they're on. If the left one is the current user that'll be removed, otherwise the other will get removed
        public void RemovePlayerFromSection(Section section, IParticipant participant)
        {
            if (_positions[section].Left == participant)
            {
                _positions[section].Left = null;
            }
            else if (_positions[section].Right == participant)
            {
                _positions[section].Right = null;
            }
        }

        /*
         * Moves a player to the next section:
         * first the player will get removed from the section they're currently on
         * then if the positions library has the current section in it, the player will get added to said section
         * if not, a new position will get created and the section will get added to it
         */
        private Section MovePlayerNextSection(Section currentSection, Section nextSection, IParticipant participant,
            int distance, bool isRight)
        {
            if (NextTrackContainsPlayer(nextSection, isRight)) 
                return currentSection;
            
            RemovePlayerFromSection(currentSection, participant);
            if (_positions.ContainsKey(nextSection) && isRight)
            {
                _positions[nextSection].Right = participant;
                _positions[nextSection].DistanceRight = distance - 100;
                return nextSection;
            } 
            if(_positions.ContainsKey(nextSection))
            {
                _positions[nextSection].Left = participant;
                _positions[nextSection].DistanceLeft = distance - 100;
                return nextSection;
            }

            _positions.Add(nextSection, new SectionData(participant, distance - 100, isRight));
            return nextSection;
            
        }

        public void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track, Participants.ToArray()));
            NextRaceStart?.Invoke(this, EventArgs.Empty);
        }

        // Calculates the speed a user has to move
        private int GetSpeed(IParticipant participant)
        {
            return participant.Equipment.Speed * participant.Equipment.Performance;
        }

        /*
         * full function to move a player:
         * if the positions 
         */
        public void MovePlayer(IParticipant[] participants)
        {
            foreach (IParticipant participant in participants)
            {
                Section current = _playerStack[participant].Pop();;
                Section next;
                int distance = 0;
                bool isRight = false;
                
                FixBreakCar(participant);

                if (participant.Equipment.IsBroken)
                {
                    continue;
                }
                
                if (participant.Rounds == 2 && current.SectionType == SectionTypes.Finish)
                {
                    if (_positions.ContainsKey(current) && (_positions[current].Left == participant ||
                                                            _positions[current].Right == participant))
                    {
                        RemovePlayerFromSection(current, participant);
                    }

                    participant.Rounds++;
                    CheckRaceDone(participants);
                    _playerStack[participant].Push(current);
                    continue;
                }
                
                if (_playerStack[participant].Count > 1)
                {
                    next = _playerStack[participant].Peek();
                }
                else
                {
                    _playerStack[participant] = CreateStack();
                    next = _playerStack[participant].Peek();
                    participant.Rounds++;
                }

                if (_positions.ContainsKey(current))
                {
                    if (_positions[current].Left == participant)
                    {
                        distance = _positions[current].DistanceLeft += GetSpeed(participant);
                        isRight = false;
                    }
                    else if (_positions[current].Right == participant)
                    {
                        distance = _positions[current].DistanceRight += GetSpeed(participant);
                        isRight = true;
                    }
                }
                
                if (distance >= 200)
                {
                    _playerStack[participant].Pop();
                    next = _playerStack[participant].Peek();
                    distance -= 100;
                }
                if (distance >= 100)
                {
                    Section s = MovePlayerNextSection(current, next, participant, distance, isRight);
                    if (s == current)
                    {
                        _playerStack[participant].Push(current);
                    }
                }
                else
                {
                    _playerStack[participant].Push(current);
                }
            }
        }

        private Stack<Section> CreateStack()
        {
            Stack<Section> temp1 = new Stack<Section>();
            foreach (Section s in Track.Sections)
            {
                temp1.Push(s);
            }
            Stack<Section> temp = new Stack<Section>();
            while (temp1.Count > 0)
            {
                temp.Push(temp1.Pop());
            }
            return temp;
        }

        public void Start()
        {
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void SetStartPosition(Track track)
        {
            int currentP = 0;
            foreach(Section section in track.Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                {
                    //GetSectionData(section, Participants[currentP], Participants[currentP + 1], 40, 80);
                    _positions.Add(section, new SectionData(Participants[currentP], Participants[currentP + 1], 40, 80));
                    Console.WriteLine($"P1: {Participants[currentP].Name} P2: {Participants[currentP + 1].Name}");
                    currentP += 2;
                }
            }
        }

        private bool NextTrackContainsPlayer(Section next, bool isRight)
        {
            if (_positions.ContainsKey(next))
            {
                if (isRight)
                {
                    return _positions[next].Right != null;
                }

                return _positions[next].Left != null;
            }

            return false;
        }

        private void CheckRaceDone(IParticipant[] players)
        {
            int check = 0;

            foreach (IParticipant p in players)
            {
                if(p.Rounds == 3)
                {
                    check++;
                }
            }

            if (check >= _playerStack.Count)
            {
                RaceDone = true;
            }
        }

        private void FixBreakCar(IParticipant participant)
        {
            var number = _random.Next(0, 10);

            if ((participant.Equipment.Quality < (number * 10)) && !participant.Equipment.IsBroken)
            {
                participant.Equipment.IsBroken = true;
            }

            if (participant.Equipment.Quality >= (number * 10) && participant.Equipment.IsBroken)
            {
                participant.Equipment.IsBroken = false;
                participant.Equipment.Quality -= 5;
                if (participant.Equipment.Quality <= 10)
                {
                    participant.Equipment.Quality = 10;
                }
            }
        }
    }
    
}