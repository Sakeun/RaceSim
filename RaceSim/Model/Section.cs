using System;
namespace Model
{
    public class Section
    {
        public SectionTypes SectionType;
        public int x;
        public int y;
        public Section(SectionTypes type)
        {
            SectionType = type;
        }

        public int[] SetPosition(int[] xy, int direction)
        {
            switch (direction)
            {
                case 0:
                    x = xy[0] + 1;
                    y = xy[1];
                    break;
                case 1:
                    x = xy[0];
                    y = xy[1] + 1;
                    break;
                case 2:
                    x = xy[0] - 1;
                    y = xy[1];
                    break;
                case 3:
                    x = xy[0];
                    y = xy[1] - 1;
                    break;
            }

            return new[] { x, y };
        }
    }
}

public enum SectionTypes
{
    Straight,
    LeftCorner,
    RightCorner,
    StartGrid,
    Finish
}

