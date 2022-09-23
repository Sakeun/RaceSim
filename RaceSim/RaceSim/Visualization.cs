using System.Net;
using System.Threading.Channels;
using Model;
namespace RaceSim;

public static class Visualization
{
    static void Initialize()
    {
        
    }

    public static void DrawTrack(Track track)
    {
        int[] xy = { 0 , 0 };
        int compass = 0;
        int lowestValx = 0;
        int lowestValy = 0;

        foreach (Section section in track.Sections)
        {
            xy = section.SetPosition(xy, compass);
            compass = SetCompass(compass, section);
            //Console.WriteLine($"X: {section.x} Y: {section.y} Type: {section.SectionType}");
            if (section.x < lowestValx)
            {
                lowestValx = section.x;
            }
            if (section.y < lowestValy)
            {
                lowestValy = section.y;
            }
        }

        //Console.WriteLine();

        //Console.WriteLine($"lowest X: {lowestValx}, Lowest Y: {lowestValy}");
        compass = 0;
        foreach (Section section in track.Sections)
        {
            int xdir = section.x + Math.Abs(lowestValx);
            int ydir = section.y + Math.Abs(lowestValy);
            //Console.WriteLine($"X: {xdir*5} Y: {ydir*5} Type: {section.SectionType}, Compass: {compass}");
            //Thread.Sleep(100);
            CurrentTrack(section.SectionType, (xdir * 5), (ydir * 5), compass);
            compass = SetCompass(compass, section);
        }
    }

    private static void CurrentTrack(SectionTypes type, int x, int y, int dir)
    {
        string[] printable;
        switch (type)
        {
            case SectionTypes.Straight:
                if (dir is 1 or 3)
                {
                    printable = _trackVertical;
                }
                else
                {
                    printable = _trackStraight;
                }
                printFunction(x, y, printable);
                break;
            case SectionTypes.LeftCorner:
                if (dir == 1)
                {
                    printable = _reverseLeftC;
                } else if (dir == 3)
                {
                    printable = _trackRightC;
                } else if (dir == 2)
                {
                    printable = _reverseRightC;
                }
                else
                {
                    printable = _trackLeftC;
                }
                printFunction(x, y, printable);
                break;
            case SectionTypes.RightCorner:
                if (dir == 3)
                {
                    printable = _reverseRightC;
                } else if (dir == 1)
                {
                    printable = _trackLeftC;
                } else if (dir == 2)
                {
                    printable = _reverseLeftC;
                }
                else
                {
                    printable = _trackRightC;
                }
                printFunction(x, y, printable);
                break;
            case SectionTypes.Finish:
                printFunction(x, y, _finishHorizontal);
                break;
            case SectionTypes.StartGrid:
                printFunction(x, y, _startingGrid);
                break;
        }
    }

    private static void printFunction(int xax, int yax, string[] track)
    {
        Console.SetCursorPosition(xax, yax);
        //Console.WriteLine(Console.CursorTop);
        foreach (string p in track)
        {
            Console.WriteLine(p);
            yax++;
            Console.SetCursorPosition(xax, yax);
        }
    }

    private static int SetCompass(int dir, Section sec)
    {
        switch (sec.SectionType)
        {
            case SectionTypes.RightCorner:
            {
                dir++;
                if (dir == 4)
                {
                    dir = 0;
                }

                break;
            }
            case SectionTypes.LeftCorner:
            {
                dir--;
                if (dir == -1)
                {
                    dir = 3;
                }

                break;
            }
        }

        return dir;
    }
    
    #region graphics
    private static string[] _finishHorizontal = { "-----", " FI  ", " FI  ", "-----" , "     "};
    private static string[] _startingGrid = { "-----", "# #  ", " # # ", "-----", "     " };
    
    private static string[] _trackStraight = { "-----", "     ", "     ", "-----", "     "};
    private static string[] _trackVertical = { "|   |", "|   |", "|   |", "|   |", "|   |" };
    
    private static string[] _trackRightC = { "----\\", "    |", "    |", "\\   |", "|   |" };
    private static string[] _reverseRightC = { "/----", "|     ", "|    ", "|   /-", "|   |   " };

    private static string[] _trackLeftC = { "/   |", "    |", "    |", "----/", "     " };
    private static string[] _reverseLeftC = { "|   \\-" , "|     ", "|    ", "\\----", "     "};

    #endregion
}