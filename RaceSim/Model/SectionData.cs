using System;
namespace Model
{
    public class SectionData
    {
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }

        public SectionData(IParticipant participant1, IParticipant participant2, int disleft, int disright)
        {
            Left = participant1;
            Right = participant2;

            DistanceLeft = disleft;
            DistanceRight = disright;
        }

        public SectionData(IParticipant participant, int distance, bool isRight)
        {
            if (!isRight)
            {
                Left = participant;
                DistanceLeft = distance;
                Right = null;
            }
            else
            {
                Right = participant;
                DistanceRight = distance;
                Left = null;
            }
        }

        public void AddSecondPlayer(IParticipant p, int distance)
        {
            Right = p;
            DistanceRight = distance;
        }
    }
}

